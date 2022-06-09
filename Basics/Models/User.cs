// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using System.Net;

namespace Basics.Models
{
    public class User : BaseNotifyPropertyChanged
    {
        private string userName;
        private IPAddress ip;
        private string picture;

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IPAddress Ip
        {
            get
            {
                return ip;
            }
            set
            {
                if (value != ip)
                {
                    ip = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Picture
        {
            get
            {
                return picture;
            }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged();
                }
            }
        }

        public long UserId { get; }

        public User(IPAddress ip, string userName, string picture, long id)
        {
            Ip = ip;
            UserName = userName;
            Picture = picture;
            UserId = id;
        }
    }
}