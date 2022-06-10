// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System.Windows;

namespace Basics.Windows
{
    /// <summary>
    /// Interaction logic for AddContacts.xaml
    /// </summary>
    public partial class AddUserToGroupChat : Window
    {
        public AddUserToGroupChatViewModel AddUserToGroupChatViewModel
        {
            get { return (AddUserToGroupChatViewModel)GetValue(AddUserToGroupChatViewModelProperty); }
            set { SetValue(AddUserToGroupChatViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddUserToGroupChatViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddUserToGroupChatViewModelProperty =
            DependencyProperty.Register("AddUserToGroupChatViewModel", typeof(AddUserToGroupChatViewModel), typeof(AddUserToGroupChat), new PropertyMetadata(null));

        public AddUserToGroupChat()
        {
            InitializeComponent();
        }

        private void btn_addIP_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}