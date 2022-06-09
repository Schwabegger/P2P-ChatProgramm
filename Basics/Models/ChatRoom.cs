using Basics.Interfaces;
using System.Collections.ObjectModel;

namespace Basics.Models
{
    public class ChatRoom
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public ObservableCollection<Message> ChatHistory { get; set; }
        public User Me { get; init; }

        public ISender Sender { get; set; }

        public ChatRoom(string name, string picture, User me, ISender sender)
        {
            Me = me;
            Name = name;
            Picture = picture;
            ChatHistory = new();
            Sender = sender;
        }
    }
}