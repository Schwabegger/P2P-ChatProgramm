// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Interfaces;
using System.Collections.ObjectModel;

namespace Basics.Models
{
    public class ChatRoom : BaseNotifyPropertyChanged
    {
        private string name;
        private string picture;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != Name)
                {
                    name = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Picture
        {
            get { return picture; }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged();
                }
            }
        }
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