using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Model.Entities
{
    [Table(nameof(User))]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }

        public IList<Friendship> FriendsTo { get; set; }
        public IList<Friendship> FriendsFrom { get; set; }
    }

    public class Friendship
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }

        public User User1 { get; set; }
        public User User2 { get; set; }
    }
}