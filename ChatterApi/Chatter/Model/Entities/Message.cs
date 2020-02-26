using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Model.Entities
{
    [Table(nameof(Message))]
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset SentAt { get; set; }
        public DateTimeOffset DeliveredAt { get; set; }
        public DateTimeOffset ReadAt { get; set; }

        public User FromUser { get; set; }
        public User ToUser { get; set; }
    }
}