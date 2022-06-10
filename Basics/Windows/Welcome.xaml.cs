// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Basics.Windows
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
            WelcomeViewModel vm = new WelcomeViewModel();
            this.DataContext = vm;
            vm.CloseAction = new Action(this.Close);
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}