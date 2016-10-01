using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json;
using ExternalLogins_CoreMVC.Models;
using Microsoft.AspNetCore.Http;

namespace ExternalLogins_CoreMVC.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ExternalLoginFailure()
        {
            return View();
        }
        public IActionResult ExternalLoginConfirmation()
        {
            return View();
        }


        private const string RequestTokenEndpoint = "https://api.twitter.com/oauth/request_token";
        [HttpPost]
        public async void ExternalLogin(string provider, string returnUrl = null)
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", "Home", new { ReturnUrl = returnUrl, provider = provider }) };
            await HttpContext.Authentication.ChallengeAsync(provider, properties);
        }



        [HttpGet]
        public async Task<ActionResult> ExternalLoginCallback(string provider, string returnUrl = null)
        {
            var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(provider);
            var info1 = await HttpContext.Authentication.AuthenticateAsync(provider);
            
            ViewBag.LoginProvider = provider;

            #region Facebook
            if (provider == "Facebook")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string email = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:email").Value;
                string name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:name").Value;
                string first_name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name").Value;
                string last_name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;
                string birthday = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:birthday").Value;
                string locale = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:locale").Value;
                string gender = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:gender").Value;
                string relationship_status = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:relationship_status").Value;
                string link = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:link").Value;

                string education = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:education").Value;
                List<EducationDetails> educationDetails = GetFacebookSchoolDetails(education);

                string picture = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:facebook:picture").Value;
                string imageUrl = GetFacebookImageUrl(picture);

                FacebookViewModel model = new FacebookViewModel() { BirthDate = birthday, EducationDetails = educationDetails, Email = email, FirstName = first_name, Gender = gender, ImageUrl = imageUrl, LastName = last_name, Link = link, Locale = locale, Name = name, RelationshipStatus = relationship_status };

                //Scopes and Permissions https://developers.facebook.com/docs/facebook-login/permissions#reference-public_profile
                //Fields https://developers.facebook.com/docs/graph-api/reference/user

                return View("Facebook", model);
            }
            #endregion Facebook
            #region Google
            else if (provider == "Google")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string emailaddress = info.Principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                string name = info.Principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                string displayName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:displayName").Value;
                string givenname = info.Principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
                string surname = info.Principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
                string profile = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:profile").Value;
                string id = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:id").Value;
                string kind = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:kind").Value;
                string objectType = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:objectType").Value;
                string isPlusUser = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:isPlusUser").Value;
                string language = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:language").Value;
                string circledByCount = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:circledByCount").Value;
                string verified = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:verified").Value;
                string emails = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:emails").Value;
                List<GoogleEmails> emailList = GetGoogleEmailList(emails);

                string image = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:google:image").Value;
                var imageUrl = GetGoogleImageUrl(image);

                GoogleViewModel model = new GoogleViewModel() { CircledByCount = circledByCount, DisplayName = displayName, Email = emailaddress, Emails = emailList, FirstName = givenname, Id = id, ImageUrl = imageUrl, IsPlusUser = isPlusUser, Kind = kind, Language = language, LastName = surname, Link = profile, Name = name, ObjectType = objectType, Verified = verified };

                //Scopes https://developers.google.com/identity/protocols/googlescopes

                return View("Google", model);
            }
            #endregion Google
            #region Microsoft
            else if (provider == "Microsoft")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string id = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:id").Value;
                string name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoftaccount:name").Value;
                string displayName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:displayName").Value;
                string first_name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoftaccount:givenname").Value;
                string last_name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoftaccount:surname").Value;
                string email = info.Principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                string userPrincipalName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:userPrincipalName").Value;
                string businessPhones = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:businessPhones").Value;
                string jobTitle = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:jobTitle").Value;
                string mail = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:mail").Value;
                string mobilePhone = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:mobilePhone").Value;
                string officeLocation = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:officeLocation").Value;
                string preferredLanguage = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:preferredLanguage").Value;

                //Scopes and Permissions http://graph.microsoft.io/en-us/docs/authorization/permission_scopes

                MicrosoftViewModel model = new MicrosoftViewModel() { DisplayName = displayName, Email = email, FirstName = first_name, Id = id, LastName = last_name, Name = name, UserPrincipalName = userPrincipalName };

                return View("Microsoft", model);
            }
            #endregion Microsoft
            #region LinkedIn
            else if (provider == "LinkedIn")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string id = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:id").Value;
                string formattedName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:formattedName").Value;
                string firstName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:firstName").Value;
                string lastName = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:lastName").Value;
                string emailAddress = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:emailAddress").Value;
                string imageUrl = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:pictureUrl").Value;
                string headline = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:headline").Value;
                string publicProfileUrl = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:publicProfileUrl").Value;

                string location = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:linkedin:location").Value;
                LinkedInCountry country = GetLinkedInCountryInfo(location);

                //You can find other fields at https://developer.linkedin.com/docs/fields/basic-profile

                LinkedInViewModel model = new LinkedInViewModel() { Email = emailAddress, FirstName = firstName, FormattedName = formattedName, Headline = headline, Id = id, ImageUrl = imageUrl, LastName = lastName, Location = country, PublicProfileUrl = publicProfileUrl };

                return View("LinkedIn", model);
            }
            #endregion LinkedIn
            else if (provider == "Twitter")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string userid = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:twitter:userid").Value;
                string screenname = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:twitter:screenname").Value;

            }
            #region GitHub
            else if (provider == "GitHub")
            {
                var list1 = info1.Claims.ToList();
                var list2 = info.Principal.Claims.ToList();

                string url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:url").Value;
                string login = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:login").Value;
                string id = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:id").Value;
                string avatar_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:avatar_url").Value;
                string gravatar_id = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:gravatar_id").Value;
                string html_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:html_url").Value;
                string followers_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:followers_url").Value;
                string following_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:following_url").Value;
                string gists_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:gists_url").Value;
                string starred_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:starred_url").Value;
                string subscriptions_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:subscriptions_url").Value;
                string organizations_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:organizations_url").Value;
                string repos_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:repos_url").Value;
                string events_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:events_url").Value;
                string received_events_url = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:received_events_url").Value;
                string type = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:type").Value;
                string site_admin = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:site_admin").Value;
                string name = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:name").Value;
                string company = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:company").Value;
                string blog = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:blog").Value;
                string location = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:location").Value;
                string email = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:email").Value;
                string hireable = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:hireable").Value;
                string bio = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:bio").Value;
                string public_repos = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:public_repos").Value;
                string public_gists = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:public_gists").Value;
                string followers = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:followers").Value;
                string following = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:following").Value;
                string created_at = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:created_at").Value;
                string updated_at = info.Principal.Claims.FirstOrDefault(c => c.Type == "urn:github:updated_at").Value;

                GitHubViewModel model = new GitHubViewModel() { AvatarUrl = avatar_url, Bio = bio, Blog = blog, Company = company, CreatedAt = created_at, Email = email, EventsUrl = events_url, Followers = followers, FollowersUrl = followers_url, Following = following, FollowingUrl = following_url, GistsUrl = gists_url, GravatarId = gravatar_id, Hireable = hireable, HtmlUrl = html_url, Id = id, Location = location, Login = login, Name = name, OrganizationsUrl = organizations_url, PublicGists = public_gists, PublicRepos = public_repos, ReceivedEventsUrl = received_events_url, ReposUrl = repos_url, SiteAdmin = site_admin, StarredUrl = starred_url, SubscriptionsUrl = subscriptions_url, Type = type, UpdatedAt = updated_at, Url = url };

                return View("GitHub", model);
            }
            #endregion GitHub
            return View("ExternalLoginFailure");
        }


        public List<EducationDetails> GetFacebookSchoolDetails(string education)
        {
            List<EducationDetails> educationDetails = new List<EducationDetails>();
            dynamic educationData = JsonConvert.DeserializeObject(education);
            foreach (var ed in educationData.Root)
            {
                var schoolDetails = JsonConvert.DeserializeObject<Education>(ed.ToString());
                educationDetails.Add(new EducationDetails { Name = schoolDetails.school.name, SchoolType = schoolDetails.type });
            }
            return educationDetails;
        }
        public string GetFacebookImageUrl(string picture)
        {
            var imageData = JsonConvert.DeserializeObject<FacebookImageData>(picture);
            var imageUrl = imageData.data.url;
            return imageUrl;
        }
        public string GetGoogleImageUrl(string image)
        {
            var imageData = JsonConvert.DeserializeObject<GoogleImage>(image);
            var imageUrl = imageData.url;
            return imageUrl;
        }
        public List<GoogleEmails> GetGoogleEmailList(string emails)
        {
            List<GoogleEmails> googleEmailList = new List<GoogleEmails>();
            dynamic googleEmails = JsonConvert.DeserializeObject(emails);
            foreach (var email in googleEmails.Root)
            {
                var emailDetails = JsonConvert.DeserializeObject<GoogleEmails>(email.ToString());
                googleEmailList.Add(new GoogleEmails { Email = email.value, EmailType = email.type });
            }
            return googleEmailList;
        }
        public LinkedInCountry GetLinkedInCountryInfo(string location)
        {
            dynamic country = JsonConvert.DeserializeObject(location);
            var countryDetails = JsonConvert.DeserializeObject<LinkedInCountryInfo>(country.ToString());
            return new LinkedInCountry { CountryCode = countryDetails.Country.Code, CountryName = countryDetails.Name };
        }


    }

    
}
