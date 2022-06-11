// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Commands;
using Basics.Interfaces;
using System;


namespace Basics.Viewmodels
{
    public class WelcomeViewModel : BaseViewModel, ICloseWindow
    {
        static string[] simaMaleRules = new string[] 
        { 
            "Isolation is a way to know ourselves.",
            "If you’re lonely when you are alone, you’re in bad company.",
            "You cannot be lonely if you like the person you’re alone with.",
            "Whosoever is delighted in their own solitude is either a wild beast or a god.",
            "To be left alone is the most precious thing one can ask of the modern world.",
            "The more powerful and original a mind, the more it will incline towards the religion of solitude.",
            "But there are no loners. No man lives in a void. His every act is conditioned by his time and his society.",
            "Be a loner. That gives you time to wonder, to search for truth. Have holy curiosity. Make your life worth living.",
            "A man can be himself alone so long as he is alone. If he does not love solitude, he will not love freedom; for it is only when he is alone that he is really free."
        };

        private string name;
        private string sigmaRule;

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

        public string SigmaRule
        {
            get { return sigmaRule; }
            set 
            {
                sigmaRule = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ClickCommand { get; set; }

        public Action CloseAction { get; set; }

        public WelcomeViewModel()
        {
            Random rnd = new Random();
            SigmaRule = simaMaleRules[rnd.Next(0, simaMaleRules.Length)];
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