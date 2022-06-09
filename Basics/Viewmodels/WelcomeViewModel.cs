// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Commands;
using Basics.Interfaces;
using System;


namespace Basics.Viewmodels
{
    public class WelcomeViewModel : BaseViewModel, ICloseWindow
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    this.RaisePropertyChanged();
                    this.ClickCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ClickCommand { get; set; }

        public Action CloseAction { get; set; }

        public WelcomeViewModel()
        {
            ClickCommand = new DelegateCommand(
            _ =>
            {
                return !string.IsNullOrEmpty(this.Name);
            },
            _ =>
            {
                Properties.Settings.Default.Name = this.Name;
                Properties.Settings.Default.Save();
                CloseAction();
            });
        }
    }
}

