using Basics.Interfaces;
using System.Collections.Generic;

namespace Basics.Models
{
    public class Groupchat : ChatRoom
    {
        public List<User> Participants { get; set; }
        public long RoomId { get; }

        public Groupchat(long roomId, string name, string picture, User me, ISender sender) : base(name, picture, me, sender)
        {
            RoomId = roomId;
            Participants = new();
        }
    }
}
