using Basics.Commands;
using Basics.Interfaces;
using Basics.Models;
using Grpc.Core;
using GrpcServer;
using GrpcShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace Basics.Viewmodels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static List<Message> UnsentMessages = new();
        public static List<User> Contacts = new();
        //public static User MeAsUser = new User(IPAddress.Parse("69.69.69.69"), Properties.Settings.Default.Name, Properties.Settings.Default.Pfp);
        //public static User MeAsUser;

        public event EventHandler ThemeChanged;
        public event EventHandler LeftGroup;
        #region Field
        private ICollectionView chatroomCollectionView;
        public string _ActiveWindow = "";
        private string filter;
        ObservableCollection<ChatRoomViewModel> chatrooms;
        ChatRoomViewModel selectedChatRoom;
        ChatRoomViewModel newChatroom;
        #endregion

        #region Commands
        public DelegateCommand SaveCommand { get; }

        public DelegateCommand ThemeCommand { get; }

        public DelegateCommand SettingsCommand { get; }

        public DelegateCommand AddChatroomCommand { get; }

        //public DelegateCommand CloseCommand { get; set; }
        #endregion     

        public event EventHandler SwaptoChat;
        public event EventHandler SwaptoSetting;

        #region Eigenschaften
        private int selectedChatroomIndex;

        public int SelectedChatroomIndex
        {
            get { return selectedChatroomIndex; }
            set
            {
                selectedChatroomIndex = value;
                RaisePropertyChanged();
            }
        }

        public string SearchForGroupName { get; set; }

        public static SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();

        public string Filter
        {
            get { return filter; }
            set
            {
                if (value != filter)
                {
                    filter = value;
                    chatroomCollectionView.Refresh();
                    RaisePropertyChanged();
                }
            }
        }

        public ChatRoomViewModel NewChatRoom
        {
            get => newChatroom;
            set
            {
                if (value != newChatroom)
                {
                    newChatroom = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ChatRoomViewModel SelectedChatRoom
        {
            get => this.selectedChatRoom;
            set
            {
                if (value != this.selectedChatRoom)
                {
                    if (SelectedChatRoom != null)
                        SelectedChatRoom.LeavGroupChatEventHandler -= (sender, _) => LeaveGroupchat(sender);
                    this.selectedChatRoom = value;
                    this.RaisePropertyChanged();
                    if (value != null)
                        SelectedChatRoom.LeavGroupChatEventHandler += (sender, _) => LeaveGroupchat(sender);
                }
            }
        }

        public ObservableCollection<ChatRoomViewModel> Chatrooms
        {
            get => chatrooms;
            set
            {
                if (chatrooms != value)
                {
                    this.chatrooms = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        //Action ICloseWindow.Close { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        //private IHost server = GrpcServer.Program.CreateHostBuilder(new string[0]).Build();
        private GreeterService services = new GrpcServer.GreeterService();
        private Server server;
        private ISender grpcSender = new GrpcSender();

        public MainWindowViewModel()
        {
            //server.Run();

            RegisterOnServerEvents(services);
            server = new Grpc.Core.Server()
            {
                Services = { Greeter.BindService(services) },
                Ports = { new ServerPort("0.0.0.0", 5000, ServerCredentials.Insecure) }
            };
            server.Start();

            //MeAsUser = new User(IPAddress.Parse("69.69.69.69"), Properties.Settings.Default.Name, Properties.Settings.Default.Pfp);
            Properties.Settings.Default.PropertyChanged += UpdateUser;
            // Me as User
            Contacts.Add(new User(IPAddress.Parse(GetIpAddressFromHost()), Properties.Settings.Default.Name, Properties.Settings.Default.Pfp, DateTime.Now.Ticks));

            // zum testen
            {
                Chatrooms = new ObservableCollection<ChatRoomViewModel>();
                Contacts.Add(new User(IPAddress.Parse("1.1.1.1"), "PrivateCHat", Viewmodels.BaseViewModel.Pfps[0], -2));
                Chatrooms.Add(
                    new ChatRoomViewModel(
                        new PrivateChat(Contacts[Contacts.Count - 1], Viewmodels.BaseViewModel.Pfps[0], Contacts[0], grpcSender)
                        {
                            ChatHistory = new ObservableCollection<Message>()
                            {
                                new Message(Contacts[Contacts.Count -1], "Hallo"),
                                new Message(Contacts[0], "zeas" )
                            }
                        }));
                Chatrooms[0].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
                CreateGroupChatroomForTesting();
            }
            this.AddChatroomCommand = new DelegateCommand(
            _ =>
            {
                var addVm = new AddContacsViewModel();
                Windows.AddContacts addCon = new() { AddContacsViewModel = addVm };
                addCon.Resources = MainWindow.Instance.Resources;
                //addCon.Owner = this;
                //addCon.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                var result = addCon.ShowDialog();
                if (result == true)
                {
                    if (addVm.IpEnabled)
                    {
                        CreatePrivateChat(IPAddress.Parse(addVm.IpField));
                    }
                    else if (addVm.GroupEnabled)
                    {
                        CreateGroupChatroom(addVm.GroupName);
                    }
                }
            });

            this.SettingsCommand = new DelegateCommand(
            _ =>
            {
                SwaptoSetting?.Invoke(this, EventArgs.Empty);
            });

            chatroomCollectionView = CollectionViewSource.GetDefaultView(Chatrooms);
            chatroomCollectionView.Filter = o => String.IsNullOrEmpty(Filter) ? true : ((ChatRoomViewModel)o).ChatRoom.Name.ToLower().Contains(Filter.ToLower());
        }

        private static string GetIpAddressFromHost()
        {
            string hostname = Dns.GetHostName();
            //Get the Ip
            try
            {
                return Dns.GetHostByName(hostname).AddressList[1].ToString();
            }
            catch
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("8.8.8.8", 65530);
                        IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                        return endPoint?.Address.ToString();
                    }
                }
                catch
                {
                    return null;
                }
            }
        }


        private void RegisterOnServerEvents(GreeterService services)
        {

            services.RequestedUserHandler += async (sender, e) => await SendUserOnRequest(e);
            services.PrivateMessageRecivedHandler += AddRecivedPrivateMessage;
            services.GroupMessageRecivedHandler += AddRecifedGroupMessage;
            services.AddedToGroupchatHandler += AddGroupChatroom;
            services.TransmitChatroomParticipantHandler += AddParticipantToCharoom;
            services.OpenPrivateChatHandler += AddPrivateChat;
            //services.RequestedUserHandler += (sender, args) => ;
        }

        private void AddPrivateChat(object sender, (string, long, string, string) e)
        {
            IPAddress ip = IPAddress.Parse(e.Item1);
            long id = e.Item2;
            string name = e.Item3;
            string pfp = e.Item4;
            User newUser = null;
            foreach (User user in Contacts)
                if (user.UserId == id)
                    newUser = user;
            foreach (ChatRoomViewModel chatRoomViewModel in Chatrooms)
                if (chatRoomViewModel.ChatRoom is PrivateChat privateChat && privateChat.OtherUser.UserId == id)
                    return;
            if (newUser == null)
            {
                Contacts.Add(new User(ip, name, pfp, id));
                newUser = Contacts[Contacts.Count - 1];
            }
            MainWindow.Instance.Dispatcher.Invoke(delegate ()
            {
                Chatrooms.Insert(0, new ChatRoomViewModel(new PrivateChat(newUser, newUser.Picture, Contacts[0], grpcSender)));
            });
        }

        private void AddParticipantToCharoom(object sender, (long, long, string, string, string) e)
        {
            long roomId = e.Item1;
            long userId = e.Item2;
            string userName = e.Item3;
            string pfp = e.Item4;
            IPAddress ip = IPAddress.Parse(e.Item5);
            User userToAdd = null;
            foreach (User user in Contacts)
                if (user.UserId == userId)
                    userToAdd = user;
            if (userToAdd == null)
            {
                Contacts.Add(new User(ip, userName, pfp, userId));
                userToAdd = Contacts[Contacts.Count - 1];
            }
            foreach (ChatRoomViewModel chatRoomViewModel in Chatrooms)
                if (chatRoomViewModel.ChatRoom is Groupchat groupchat && groupchat.RoomId == roomId)
                {
                    foreach (User user in groupchat.Participants)
                        if (user.UserId == userId)
                            return;
                    groupchat.Participants.Add(userToAdd);
                }
        }

        private async Task SendUserOnRequest(string ip)
        {
            await grpcSender.SendUser(IPAddress.Parse(ip), Contacts[0].Ip, Contacts[0].UserId, Contacts[0].UserName, Contacts[0].Picture); ;
        }

        /// <summary>
        /// Brings the chatroom the user jusst sent a message in to the top of the chatroom list
        /// </summary>
        private void BringChatroomToTop(int index = -1)
        {
            if (SelectedChatroomIndex > 0)
            {
                Chatrooms.Move(0, SelectedChatroomIndex);
            }
            if (index > 0)
            {
                Chatrooms.Move(0, index);
            }
        }

        /// <summary>
        /// Deletes the Groupchat from the list
        /// </summary>
        /// <param name="sender">Chatroom which should be deleted/left</param>
        private void LeaveGroupchat(object sender)
        {
            for (int i = 0; i < Chatrooms.Count; i++)
            {
                if (Chatrooms[i] == sender)
                {
                    Chatrooms.RemoveAt(i);
                    this.LeftGroup?.Invoke(this, EventArgs.Empty);
                    // Tell others u left group
                }
            }
        }

        private async void CreatePrivateChat(IPAddress ip)
        {
            try
            {
                await grpcSender.RequestedUserPrivate(ip, Contacts[0].Ip);
                services.RecivedUserHandler += CreatePrivateChatroom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("could not establish connection therefore cant open chat", "Could not establish connection", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Creates/adds a new private chatroom to the list
        /// </summary>
        /// <param name="ip">IpAddress from the user u want to create a chatroom with</param>
        private async void CreatePrivateChatroom(object sender, (string, long, string, string) e)
        {
            services.RecivedUserHandler -= CreatePrivateChatroom;
            IPAddress ip = IPAddress.Parse(e.Item1);
            long id = e.Item2;
            string name = e.Item3;
            string pfp = e.Item4;

            for (int i = 0; i < Contacts.Count; i++)
                if (Contacts[i].UserId == id)
                {
                    for (int c = 0; c < Chatrooms.Count; c++)
                        if (Chatrooms[c].ChatRoom is PrivateChat privateChat && privateChat.OtherUser.UserId == id)
                        {
                            SelectedChatRoom = Chatrooms[c];
                            return;
                        }
                    MainWindow.Instance.Dispatcher.Invoke(delegate ()
                    {
                        Chatrooms.Insert(0, new ChatRoomViewModel(new PrivateChat(Contacts[i], pfp, Contacts[0], grpcSender)));

                    });
                    Chatrooms[0].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
                    return;
                }
            // send your uerdata (ip, name, pfp, id)
            //await server.OpenConnection(id, ip);
            Contacts.Add(new User(ip, name, pfp, id));
            MainWindow.Instance.Dispatcher.Invoke(delegate ()
            {
                Chatrooms.Insert(0, new ChatRoomViewModel(new PrivateChat(Contacts[Contacts.Count - 1], Contacts[Contacts.Count - 1].Picture, Contacts[0], grpcSender)));
            });
            Chatrooms[0].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
            await grpcSender.OpenPrivateChat(ip, Contacts[0].Ip, Contacts[0].UserId, Contacts[0].UserName, Contacts[0].Picture);
        }

        /// <summary>
        /// Creates/adds a group chat to list
        /// </summary>
        /// <param name="groupName">The name the group chat should have</param>
        private void AddGroupChatroom(object sender, (long, string, string, string, long, string, string) e)
        {
            long roomId = e.Item1;
            string groupName = e.Item2;
            string pfp = e.Item3;
            IPAddress senderIp = IPAddress.Parse(e.Item4);
            long senderId = e.Item5;
            string senderUserName = e.Item6;
            string senderPicture = e.Item7;
            User senderUser = null;
            foreach (User user in Contacts)
                if (user.UserId == senderId)
                    senderUser = user;
            if (senderUser == null)
            {
                Contacts.Add(new User(senderIp, senderUserName, senderPicture, senderId));
                senderUser = Contacts[Contacts.Count - 1];
            }
            MainWindow.Instance.Dispatcher.Invoke(delegate ()
            {
                Chatrooms.Insert(0, new ChatRoomViewModel(new Groupchat(roomId, groupName, pfp, Contacts[0], grpcSender)));
            });
            MainWindow.Instance.Dispatcher.Invoke(delegate ()
            {
                ((Groupchat)Chatrooms[0].ChatRoom).Participants.Add(senderUser);
            });
            Chatrooms[0].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
        }

        private void CreateGroupChatroom(string name)
        {
            Chatrooms.Insert(0, new ChatRoomViewModel(new Groupchat(DateTime.Now.Ticks, name, Viewmodels.BaseViewModel.Pfps[5], Contacts[0], grpcSender)));
            Chatrooms[0].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
        }

        /// <summary>
        /// Updates the user if the name or pfp changed so it changes in the chatrooms
        /// also updates the theme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateUser(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Properties.Settings.Default.Name))
                Contacts[0].UserName = Properties.Settings.Default.Name;
            if (e.PropertyName == nameof(Properties.Settings.Default.Pfp))
                Contacts[0].Picture = Properties.Settings.Default.Pfp;
            if (e.PropertyName == nameof(Properties.Settings.Default.Theme))
                if (ThemeChanged != null)
                    ThemeChanged.Invoke(sender, EventArgs.Empty);

            //if(e.PropertyName == nameof(Properties.Settings.Default.Name))
            //    MeAsUser.UserName = Properties.Settings.Default.Name;
            //if(e.PropertyName == nameof(Properties.Settings.Default.Pfp))
            //    MeAsUser.Picture = Properties.Settings.Default.Pfp;
        }

        private void AddRecivedPrivateMessage(object sender, (long, string) e)
        {
            long senderId = e.Item1;
            string content = e.Item2;
            ChatRoom chatRoom = null;
            for (int i = 0; i < Chatrooms.Count; i++)
            {
                if (Chatrooms[i].ChatRoom is PrivateChat privateChat && privateChat.OtherUser.UserId == senderId)
                {
                    chatRoom = Chatrooms[i].ChatRoom;
                    BringChatroomToTop(i);
                    break;
                }

            }
            if (chatRoom != null)
            {
                User senderUser = GetUser(senderId);
                MainWindow.Instance.Dispatcher.Invoke(delegate ()
                {
                    chatRoom.ChatHistory.Add(new Message(senderUser, content));
                });
            }
        }

        private void AddRecifedGroupMessage(object sender, (long, long, string) e)
        {
            long roomId = e.Item1;
            long senderId = e.Item2;
            string content = e.Item3;
            ChatRoom chatRoom = null;
            for (int i = 0; i < Chatrooms.Count; i++)
            {
                if (Chatrooms[i].ChatRoom is Groupchat groupchat && groupchat.RoomId == roomId)
                {
                    chatRoom = Chatrooms[i].ChatRoom;
                    BringChatroomToTop(i);
                    break;
                }
            }
            if (chatRoom != null)
            {
                User senderUser = GetUser(senderId);
                MainWindow.Instance.Dispatcher.Invoke(delegate ()
                {
                    chatRoom.ChatHistory.Add(new Message(senderUser, content));
                });
            }

        }

        /// <summary>
        /// Returns the corresponding user from the Contacts list
        /// </summary>
        /// <param name="senderId">The id of the sender</param>
        /// <returns></returns>
        private User GetUser(long senderId)
        {
            User sender = null;
            for (int i = 0; i < Contacts.Count; i++)
                if (Contacts[i].UserId == senderId)
                    sender = Contacts[i];
#warning Wenn sender null und GroupChat user zu GroupChat hinzufügen
            //if (chatroom is PrivateChat)
            //    sender = ((PrivateChat)chatroom).OtherUser;
            //else
            //    foreach (User user in ((Groupchat)chatroom).Participants)
            //        if (user.UserId == senderId)
            //            sender = user;
            return sender;
        }

        /// <summary>
        /// Creates a groupchaat for testing
        /// </summary>
        public void CreateGroupChatroomForTesting()
        {
            Chatrooms.Add(new ChatRoomViewModel(new Groupchat(DateTime.Now.Ticks, "GruppenChat", Viewmodels.BaseViewModel.Pfps[5], Contacts[0], grpcSender)));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "peda", Pfps[1], -4));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[0], "leil des schoff ma"));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "hackl", Pfps[3], -5));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[1], "ja"));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "gruawa", Pfps[5], -6));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[2], "fix"));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "höß", Pfps[4], -7));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[3], "des moch ma"));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "schwabbi", Pfps[0], 8));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[4], "guda einstellung"));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants.Add(new User(IPAddress.Parse("1.1.1.1"), "stephan PO", Pfps[0], -10));
            ((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).ChatHistory.Add(new Message(((Groupchat)Chatrooms[Chatrooms.Count - 1].ChatRoom).Participants[5], "es is scheiß egal oba wir fertig werdn da ned \nEs geht nur drum dass ma drecks scrum moi gmocht hom \nUnd umd Präsentation und so \nObs wirklich funktioniert is ziemlich egal es wird kana wirklich hernema"));
            Chatrooms[1].MessageSentBringChatToTopHandler += (_, _) => BringChatroomToTop();
        }
    }
}