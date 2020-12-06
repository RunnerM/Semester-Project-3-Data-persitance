using System.Collections.Generic;
using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class GetUsersResponse : Request
    {
        [JsonPropertyName("users")] 
        public List<User> Users { get; set; }

        public GetUsersResponse(List<User> users) : base(RequestType.GetUsers)
        {
            this.Users = users;
        }
    }
}