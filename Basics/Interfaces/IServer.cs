// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;
using System.Net;
using System.Threading.Tasks;

namespace Basics.Interfaces
{
    /// <summary>
    /// All methods should be async
    /// </summary>
    public interface IServer
    {
        public event EventHandler<EventArgs> GroupMessageReceivedEventHandler;
        public event EventHandler<EventArgs> PrivateMessageReceivedEventHandler;
        public event EventHandler<EventArgs> AddedToGroupChatEventHandler;
        public event EventHandler<EventArgs> AddedToPrivateChatEventHandler;
        public event EventHandler<EventArgs> NewUserWasAddedToGroupchatEventHandler;
        public event EventHandler<EventArgs> ProfilePictureWasChangedEventHandler;
        public event EventHandler<EventArgs> NameWasChangedEventHandler;

        public void SendPrivate(IPAddress reciverIp, long senderId, string content);

        public void SendGroup(IPAddress reciverIp, long senderId, string content, long groupId);

        public Task AddUserToPrivateChat(IPAddress reciverIp, long senderId);

        /// <summary>
        /// Tells a user he got added to a chatroom
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="roomId"></param>
        /// <returns>string name from added user, string pfp from added user, long userId from added user</returns>
        public (string, string, long) AddUserToGroupChat(IPAddress reciverIp, long roomId, string groupChatPicture, string groupChatName);

        /// <summary>
        /// Tells others in the groupchat that a new person was added
        /// and gives them the info to the user
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="name"></param>
        /// <param name="pfp"></param>
        public void TellOthersANewUserWasAddedToChatroomAsync(IPAddress reciverIp, IPAddress addedUserIp);

        /// <summary>
        /// Transmits all participants in a groupchat to a newly added person
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="roomId"></param>
        /// <param name="name"></param>
        /// <param name="pfp"></param>
        public void TransmitChatroomParticipantsToAddedUser(IPAddress reciverIp, long roomId, IPAddress participantIp);

        public void TellOthersNameWasChanged(IPAddress reciverIp, long senderId, string newName);

        public void TellOthersProfilePictureWasChanged(IPAddress reciverIp, long senderId, string newPfpLink);
    }
}



//using System;
//using System.Net;
//using Basics.Models;

//namespace Basics.Interfaces
//{
//    /// <summary>
//    /// All methods should be async
//    /// </summary>
//    public interface IServer
//    {
//        public event EventHandler<EventArgs> PrivateMessageReceived;
//        public event EventHandler<EventArgs> GroupMessageReceived;
//        public void SendPrivate(string content, IPAddress ip, long userId);
//        public void SendGroup(string content, long groupId, IPAddress[] ips, IPAddress serndeIp);

//        /// <summary>
//        /// Gets the groupchat if reciving a groupchat message to a room
//        /// that does exist
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="roomId"></param>
//        /// <returns>string Name, string picture, long roomId, User[] participants</returns>
//        public  (string, string, long, User[]) GetGroupChatFromGrpc(IPAddress ip, long roomId);

//        /// <summary>
//        /// Gets sender information if recifing a message from an unknown person
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <returns>string userName, string picture, IpAddress ip</returns>
//        public  (string, string, IPAddress) GetPrivateChatFromGrpc(IPAddress ip);

//        /// <summary>
//        /// Tells a user he got added to a chatroom
//        /// 
//        /// Gets the users information
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="roomId"></param>
//        /// <returns>string name from added user, string pfp from added user</returns>
//        public (string, string) AddUserToChat(IPAddress ip, long roomId, string picture);

//        /// <summary>
//        /// Transmits all participants in a groupchat to a newly added person
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="roomId"></param>
//        /// <param name="name"></param>
//        /// <param name="pfp"></param>
//        public void TransmitChatroomParticipantsToAddedUser(IPAddress ip, long roomId, string name, string pfp);

//        /// <summary>
//        /// TElls others in the groupchat that a new person was added
//        /// and gives them the info to the user
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="name"></param>
//        /// <param name="pfp"></param>
//        public void TellOthersANewUserWasAddedToChatroom(IPAddress ip, IPAddress addedUserIp, string name, string pfp);
//    }
//}