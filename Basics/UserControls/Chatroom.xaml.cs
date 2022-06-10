// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace Basics.UserControls
{
    /// <summary>
    /// Interaction logic for Chatroom.xaml
    /// </summary>
    public partial class Chatroom : UserControl
    {
        public ChatRoomViewModel ChatRoomViewModel
        {
            get { return (ChatRoomViewModel)GetValue(ChatRoomViewModelProperty); }
            set { SetValue(ChatRoomViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChatRoomViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChatRoomViewModelProperty =
            DependencyProperty.Register("ChatRoomViewModel", typeof(ChatRoomViewModel), typeof(Chatroom), new PropertyMetadata(null));

        public Chatroom()
        {
            InitializeComponent();
            //ChatRoomViewModel = new ChatRoomViewModel();
            //ChatRoomViewModel.ScrollOnSendHandler += (_, _) => ScrollDown();
        }

        private void ScrollDown()
        {
            lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
        }

        private void Button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ScrollDown();
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return || e.Key == System.Windows.Input.Key.Enter)
                ScrollDown();
        }
    }
}