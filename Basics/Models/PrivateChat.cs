// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Interfaces;

namespace Basics.Models
{
    public class PrivateChat : ChatRoom
    {
        public User OtherUser { get; set; }

        public PrivateChat(User otherUser, string picture, User me, ISender sender) : base(otherUser.UserName, picture, me, sender)
        {
            OtherUser = otherUser;
        }
    }
}
