// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Basics.Models
{
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}