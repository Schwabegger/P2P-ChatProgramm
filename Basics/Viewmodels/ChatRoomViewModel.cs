using Basics.Commands;
using Basics.Interfaces;
using Basics.Models;
using Basics.Windows;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Basics.Viewmodels
{
    public class ChatRoomViewModel : BaseViewModel
    {
        public ChatRoom ChatRoom { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand AddUserToGroupchatCommand { get; }
        public DelegateCommand LeavGroupChatCommand { get; }
        public DelegateCommand UploadCommand { get; }
        public DelegateCommand ListMembersCommand { get; }

        public event EventHandler MessageSentBringChatToTopHandler;

        public event EventHandler LeavGroupChatEventHandler;


        string currentMessage;
        public string CurrentMessage
        {
            get => this.currentMessage;
            set
            {
                if (value != this.currentMessage)
                {
                    this.currentMessage = value;
                    this.RaisePropertyChanged();
                    this.AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ChatRoomViewModel(ChatRoom chatRoom)
        {
            this.ChatRoom = chatRoom;
            this.AddCommand = new DelegateCommand(_ => CanAddMessage(), _ => AddMessage());
            this.AddUserToGroupchatCommand = new DelegateCommand(
                _ =>
                {
                    // prüfen ob admin
                    return true;
                },
                _ =>
                {
                    var addUserVm = new AddUserToGroupChatViewModel();
                    AddUserToGroupChat addUser = new() { AddUserToGroupChatViewModel = addUserVm };
                    var result = addUser.ShowDialog();
                    if (result == true)
                    {
                        AddUserToChatroom(IPAddress.Parse(addUserVm.IpField));
                    }
                });
            this.LeavGroupChatCommand = new DelegateCommand(
                _ =>
                {

                    var result = MessageBox.Show(Application.Current.FindResource("StrLeaveGroupMsg").ToString(), Application.Current.FindResource("StrLeaveGroupMsgTitle").ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel);
                    if (result == MessageBoxResult.Yes)
                        LeavGroupChatEventHandler?.Invoke(this, EventArgs.Empty);
                });
            this.UploadCommand = new DelegateCommand(
                _ =>
                {
                    MessageBox.Show("Sus ඞ", "Amogus", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                });
            this.ListMembersCommand = new DelegateCommand(
                _ =>
                {
                    var userListVm = new GroupMemberListViewModel(((Groupchat)ChatRoom).Participants, ChatRoom.Name);
                    GroupMemberList memberList = new() { GroupMemberListViewModel = userListVm };
                    memberList.Resources = MainWindow.Instance.Resources;
                    memberList.ShowDialog();
                });
        }

        private async void AddUserToChatroom(IPAddress addedUserIp)
        {
            await ChatRoom.Sender.AddToGroupchat(addedUserIp, ((Groupchat)ChatRoom).RoomId, ((Groupchat)ChatRoom).Name, ((Groupchat)ChatRoom).Picture);
            User addedUser = GetUser(addedUserIp);
            // (string name, string pfp) = AddUserToChat(AddIp, ((Groupchat)this.ChatRoom).RoomId, ((Groupchat)this.ChatRoom).Picture)
            //User addedUser = new User() { UserName = name, Picture = pfp, Ip = AddIp};
            foreach (User user in ((Groupchat)this.ChatRoom).Participants)
                if (user.UserId == addedUser.UserId)
                    return;

            foreach (User user in ((Groupchat)this.ChatRoom).Participants)
            {
                ChatRoom.Sender.TransmitChatroomParticipantsToAddedUser(addedUser.Ip, ((Groupchat)this.ChatRoom).RoomId, user.Ip, user.UserId, user.UserName, user.Picture);
                ChatRoom.Sender.TellOthersANewUserWasAddedToChatroomAsync(user.Ip, ((Groupchat)this.ChatRoom).RoomId, addedUserIp, addedUser.UserId, addedUser.UserName, addedUser.Picture);
            }
            ((Groupchat)this.ChatRoom).Participants.Add(addedUser);
        }

        private User GetUser(IPAddress ip)
        {
            foreach (User user in MainWindowViewModel.Contacts)
                if (user.Ip == ip)
                    return user;
            MainWindowViewModel.Contacts.Add(new User(IPAddress.Parse("0.0.0.0"), "username über GrPc", Viewmodels.BaseViewModel.Pfps[0], -1));
            return MainWindowViewModel.Contacts[MainWindowViewModel.Contacts.Count - 1]; // name und pfp über grpc hoin
        }

        private bool CanAddMessage()
        {
            return !string.IsNullOrEmpty(this.CurrentMessage);
        }

        /// <summary>
        /// Adds the message the user sent
        /// and sends it to the others
        /// </summary>
        private async void AddMessage()
        {
            this.ChatRoom.ChatHistory.Add(new Message(ChatRoom.Me, currentMessage));
            MessageSentBringChatToTopHandler?.Invoke(this, EventArgs.Empty);
            if (this.ChatRoom is PrivateChat)
            {
                // call async SendPrivate
                try
                {
                    await ChatRoom.Sender.SendPrivateMessage(((PrivateChat)ChatRoom).OtherUser.Ip, ChatRoom.Me.UserId, CurrentMessage);
                }
                catch
                {
                    MessageBox.Show($"Could not send message to {((PrivateChat)ChatRoom).Name}", "Could not send message");
                }
            }
            else
            {
                Groupchat groupchat = (Groupchat)ChatRoom;
                for (int i = 0; i < groupchat.Participants.Count; i++)
                {
                    try
                    {
                        await ChatRoom.Sender.SendGroupMessage(groupchat.Participants[i].Ip, ChatRoom.Me.UserId, CurrentMessage, groupchat.RoomId);
                    }
                    catch
                    {
                        MessageBox.Show($"Could not send {((Groupchat)ChatRoom).Name} message to {groupchat.Participants[i].UserName}", "Could not send message");
                    }
                }
            }
            this.CurrentMessage = "";
        }
    }
}