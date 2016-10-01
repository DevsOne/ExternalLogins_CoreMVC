using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExternalLogins_CoreMVC.Models
{

    #region Facebook
    public class FacebookImage
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }
    public class FacebookImageData
    {
        public FacebookImage data { get; set; }
    }
    public class School
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class Education
    {
        public School school { get; set; }
        public string type { get; set; }
        public string id { get; set; }
    }
    public class EducationDetails
    {

        [Display(Name = "School Type")]
        public string SchoolType { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
    public class FacebookViewModel
    {

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "BirthDate")]
        public string BirthDate { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Locale")]
        public string Locale { get; set; }

        [Display(Name = "Relationship Status")]
        public string RelationshipStatus { get; set; }

        [Display(Name = "Education")]
        public List<EducationDetails> EducationDetails { get; set; }

        [Display(Name = "Profile Picture")]
        public string ImageUrl { get; set; }
    }

    #endregion Facebook
    
    #region Google

    public class GoogleImage
    {
        public bool isDefault { get; set; }
        public string url { get; set; }
    }
    
    public class GoogleEmails
    {

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Type")]
        public string EmailType { get; set; }
    }
    public class GoogleViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Emails")]
        public List<GoogleEmails> Emails { get; set; }

        [Display(Name = "Profile")]
        public string Link { get; set; }
        
        [Display(Name = "Id")]
        public string Id { get; set; }
        
        [Display(Name = "Kind")]
        public string Kind { get; set; }
        
        [Display(Name = "Object Type")]
        public string ObjectType { get; set; }
        
        [Display(Name = "Is PlusUser?")]
        public string IsPlusUser { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }
        
        [Display(Name = "Circled By Count")]
        public string CircledByCount { get; set; }

        [Display(Name = "Verified?")]
        public string Verified { get; set; }

        [Display(Name = "Profile Picture")]
        public string ImageUrl { get; set; }
    }

    #endregion Google

    #region Microsoft

    public class MicrosoftViewModel
    {

        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "User Principal Name")]
        public string UserPrincipalName { get; set; }
    }

    #endregion Microsoft

    #region LinkedIn

    public class LinkedInCountryDetails
    {
        [Display(Name ="Country Code")]
        public string Code { get; set; }
    }

    public class LinkedInCountryInfo
    {
        public LinkedInCountryDetails Country { get; set; }

        [Display(Name = "Country Name")]
        public string Name { get; set; }
    }

    public class LinkedInCountry
    {
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Display(Name = "Country Name")]
        public string CountryName { get; set; }
    }

    public class LinkedInViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string FormattedName { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public string ImageUrl { get; set; }

        [Display(Name = "Headline")]
        public string Headline { get; set; }

        [Display(Name = "Location")]
        public LinkedInCountry Location { get; set; }

        [Display(Name = "Public Profile Url")]
        public string PublicProfileUrl { get; set; }
    }

    #endregion LinkedIn

    #region GitHub

    public class GitHubViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Avatar Url")]
        public string AvatarUrl { get; set; }

        [Display(Name = "Gravatar Id")]
        public string GravatarId { get; set; }

        [Display(Name = "Html Url")]
        public string HtmlUrl { get; set; }
        
        [Display(Name = "Followers Url")]
        public string FollowersUrl { get; set; }

        [Display(Name = "Following Url")]
        public string FollowingUrl { get; set; }

        [Display(Name = "Gists Url")]
        public string GistsUrl { get; set; }

        [Display(Name = "Starred Url")]
        public string StarredUrl { get; set; }

        [Display(Name = "Subscriptions Url")]
        public string SubscriptionsUrl { get; set; }

        [Display(Name = "Organizations Url")]
        public string OrganizationsUrl { get; set; }

        [Display(Name = "Repos Url")]
        public string ReposUrl { get; set; }

        [Display(Name = "Events Url")]
        public string EventsUrl { get; set; }

        [Display(Name = "Received Events Url")]
        public string ReceivedEventsUrl { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Site Admin")]
        public string SiteAdmin { get; set; }
        
        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Blog")]
        public string Blog { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
       
        [Display(Name = "Hireable")]
        public string Hireable { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Public Repos")]
        public string PublicRepos { get; set; }

        [Display(Name = "Public Gists")]
        public string PublicGists { get; set; }

        [Display(Name = "Followers")]
        public string Followers { get; set; }

        [Display(Name = "Following")]
        public string Following { get; set; }

        [Display(Name = "Created At")]
        public string CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public string UpdatedAt { get; set; }

    }

    #endregion GitHub

    
}
