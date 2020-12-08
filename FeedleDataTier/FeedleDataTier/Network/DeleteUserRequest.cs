using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class DeleteUserRequest : Request
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set;}

        public DeleteUserRequest(int userId) : base(RequestType.DeleteUser)
        {
            this.UserId = userId;
        }
    }
}