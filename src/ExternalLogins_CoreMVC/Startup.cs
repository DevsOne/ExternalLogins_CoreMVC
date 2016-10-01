using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExternalLogins_CoreMVC
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });

            #region Facebook

            var facebookOptions = new FacebookOptions()
            {
                AppId = Configuration["Facebook:AppId"],
                AppSecret = Configuration["Facebook:AppSecret"],
                Scope = { "public_profile", "email", "user_birthday", "user_relationships", "user_education_history" },
                Fields = { "birthday", "locale", "picture", "education", "link", "gender", "relationship_status" },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        await AddClaims(context, null, "Facebook");
                    }
                }
            };
            app.UseFacebookAuthentication(facebookOptions);

            #endregion Facebook

            #region Google

            var googleOptions = new GoogleOptions()
            {
                ClientId = Configuration["Google:ClientId"],
                ClientSecret = Configuration["Google:ClientSecret"],
                Scope = { "https://www.googleapis.com/auth/plus.login", "email" },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        await AddClaims(context, null, "Google");
                    }
                }
            };
            app.UseGoogleAuthentication(googleOptions);

            #endregion Google

            #region Microsoft

            var microsoftOptions = new MicrosoftAccountOptions()
            {
                ClientId = Configuration["Microsoft:ClientId"],
                ClientSecret = Configuration["Microsoft:ClientSecret"],
                Scope = { "User.ReadBasic.All" },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        await AddClaims(context, null, "Microsoft");
                    }
                }
            };
            app.UseMicrosoftAccountAuthentication(microsoftOptions);

            #endregion Microsoft

            #region Twitter

            //var twitterOptions = new TwitterOptions()
            //{
            //    ConsumerKey = Configuration["Twitter:ConsumerKey"],
            //    ConsumerSecret = Configuration["Twitter:ConsumerSecret"],
            //    //Events = new TwitterEvents
            //    //{
            //    //    OnCreatingTicket = async context =>
            //    //    {
            //    //        context.Principal.AddIdentity(new ClaimsIdentity(new[] { new Claim("TwitterAccessToken", context.AccessToken) }));
            //    //        foreach (var claim in context.User)
            //    //        {
            //    //            var claimType = string.Format("urn:twitter:{1}", claim.Key);
            //    //            string claimValue = claim.Value.ToString();
            //    //            if (!context.Principal.HasClaim(claimType, claimValue))
            //    //            {
            //    //                context.Principal.AddIdentity(new ClaimsIdentity(new[] { new Claim(claimType, claimValue, claim.GetType().ToString(), context.Options.ClaimsIssuer) }));
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //};
            //app.UseTwitterAuthentication(twitterOptions);

            #endregion Twitter


            #region GitHub

            app.UseOAuthAuthentication(new OAuthOptions()
            {
                AuthenticationScheme = "GitHub",
                ClientId = Configuration["GitHub:ClientId"],
                ClientSecret = Configuration["GitHub:ClientSecret"],
                CallbackPath = new PathString("/signin-github"),
                AuthorizationEndpoint = "https://github.com/login/oauth/authorize",
                TokenEndpoint = "https://github.com/login/oauth/access_token",
                UserInformationEndpoint = "https://api.github.com/user",
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        await GetUser(context, "GitHub");
                    }
                }
            });

            #endregion GitHub

            #region LinkedIn

            var linkedinOptions = new OAuthOptions()
            {
                AuthenticationScheme = "LinkedIn",

                ClientId = Configuration["LinkedIn:ClientId"],
                ClientSecret = Configuration["LinkedIn:ClientSecret"],
                CallbackPath = new PathString("/signin-linkedin"),
                AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization",
                TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken",
                UserInformationEndpoint = "https://api.linkedin.com/v1/people/~:(id,formatted-name,email-address,picture-url,firstName,lastName,headline,location,public-profile-url)",
                Scope = { "r_basicprofile", "r_emailaddress" },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        await GetUser(context, "LinkedIn");
                    }
                }
            };
            app.UseOAuthAuthentication(linkedinOptions);

            #endregion LinkedIn

            //var twitterOptions = new OAuthOptions()
            //{
            //    AuthenticationScheme = "Twitter",
            //    ClientId = Configuration["Twitter:ConsumerKey"],
            //    ClientSecret = Configuration["Twitter:ConsumerSecret"],
            //    CallbackPath = new PathString("/signin-twitter"),
            //    BackchannelHttpHandler = CreateHandler(),
            //    SignInScheme = "Authenticated",
            //    AuthorizationEndpoint = "https://api.twitter.com/oauth/authorize",
            //    TokenEndpoint = "https://api.twitter.com/oauth/access_token",
            //};
            //app.UseOAuthAuthentication(twitterOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public async Task GetUser(OAuthCreatingTicketContext context, string provider)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();
            var user = JObject.Parse(await response.Content.ReadAsStringAsync());
            AddClaims(context, user, provider);
        }
        public async Task AddClaims(OAuthCreatingTicketContext context, JObject user, string provider)
        {
            if (user != null)
            {
                foreach (var claim in user)
                {
                    var claimType = string.Format("urn:{0}:{1}", provider.ToLower(), claim.Key);
                    string claimValue = claim.Value.ToString();
                    if (!context.Identity.HasClaim(claimType, claimValue))
                        context.Identity.AddClaim(new Claim(claimType, claimValue, claim.GetType().ToString(), context.Options.ClaimsIssuer));
                }
            }
            else
            {
                foreach (var claim in context.User)
                {
                    var claimType = string.Format("urn:{0}:{1}", provider.ToLower(), claim.Key);
                    string claimValue = claim.Value.ToString();
                    if (!context.Identity.HasClaim(claimType, claimValue))
                        context.Identity.AddClaim(new Claim(claimType, claimValue, claim.GetType().ToString(), context.Options.ClaimsIssuer));
                }
            }
        }
      

    }
}