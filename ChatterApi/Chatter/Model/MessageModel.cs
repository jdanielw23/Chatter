using System;

namespace Chatter.Model
{
    public class MessageModel
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset ClientSentAt { get; set; }
    }
}