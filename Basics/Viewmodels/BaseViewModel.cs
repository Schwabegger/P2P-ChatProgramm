using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Basics.Viewmodels
{

    public class BaseViewModel : INotifyPropertyChanged
    {

        //INotifyPropertyChanged Method and Event
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyname = "")
        {
            if (!string.IsNullOrEmpty(propertyname))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        public static string[] Pfps { get => pfpSelection; }

        #region Profile Pictures
        protected static string[] pfpSelection = new string[] { "https://image.geo.de/30126332/t/-U/v4/w1440/r0/-/alice-im-wunderland-disney-zeichentrick-png--71570-.jpg",
            "https://static.wikia.nocookie.net/disney/images/8/8e/DisneyCheshireCat.jpg/revision/latest?cb=20190228152714",
            "https://i.pinimg.com/originals/98/f9/b6/98f9b64d03a15c7c7fe6951aaab8abf8.jpg",
            "https://static.wikia.nocookie.net/disney/images/0/0f/Profile_-_White_Rabbit.jpeg/revision/latest?cb=20190314055534",
            "https://mickeyblog.com/wp-content/uploads/2018/06/Mad-Hatter-Alice.jpg",
            "https://i.pinimg.com/564x/3a/ef/af/3aefafb971d8294a5d68944a0937ef1a--disney-costumes-halloween-costumes.jpg" };
        protected static int selectedPfp = Array.IndexOf(pfpSelection, Properties.Settings.Default.Pfp);
        #endregion
    }
}