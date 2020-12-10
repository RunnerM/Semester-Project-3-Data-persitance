using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class SendMessageRequest : Request
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
        public SendMessageRequest(Message message) : base(RequestType.SendMessage)
        {
            this.Message = message;
        }
    }
}