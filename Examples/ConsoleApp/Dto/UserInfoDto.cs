using ConsoleApp.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp.Dto
{
    internal class UserInfoDto
    {
        public string Login { get; set; }

        public int Id { get; set; }

        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty(PropertyName = "gravatar_id")]
        public string GravatarId { get; set; }

        public string Url { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "followers_url")]
        public string FollowersUrl { get; set; }

        [JsonProperty(PropertyName = "following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty(PropertyName = "gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty(PropertyName = "starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty(PropertyName = "subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonProperty(PropertyName = "organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty(PropertyName = "repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty(PropertyName = "events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty(PropertyName = "received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        public string Type { get; set; }

        [JsonProperty(PropertyName = "site_admin")]
        public bool SiteAdmin { get; set; }

        public string Name { get; set; }

        public string Blog { get; set; }

        [JsonProperty(PropertyName = "public_repos")]
        public int PublicRepos { get; set; }

        [JsonProperty(PropertyName = "public_gists")]
        public int PublicGists { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        public static UserInfo MapToModel(UserInfoDto dto)
            => new UserInfo(dto.Login, dto.Id, dto.AvatarUrl, dto.Url, dto.Name, dto.CreatedAt, dto.UpdatedAt);
    }
}