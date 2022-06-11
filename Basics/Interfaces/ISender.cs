// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Basics.Interfaces
{
    public interface ISender
    {
        public Task<bool> SendPrivateMessage(IPAddress reciverIp, long senderId, string messsage);
        public Task<bool> SendGroupMessage(IPAddress reciverIp, long senderId, string messsage, long roomId);

        public Task<bool> TransmitChatroomParticipantsToAddedUser(IPAddress reciverIp, long roomId, IPAddress participantIp, long participantId, string participantName, string participantPfp);
        public Task<bool> TellOthersANewUserWasAddedToChatroom(IPAddress reciverIp, long roomId, IPAddress addedUserIp, long addedUserId, string addedUserName, string addedUserPfp);
        public Task<bool> RequestedUserPrivate(IPAddress reciverIp, IPAddress senderIp);
        public Task<bool> SendUser(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        public Task<bool> OpenPrivateChat(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        //public Task<bool> OpenPrivateChatroom(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        public Task<bool> AddToGroupchat(IPAddress reciverIp, long roomId, string name, string pfp, IPAddress senderIp, long senderId, string sendername, string senderPfp);
        public Task<bool> JoinGroupchat(IPAddress reciverIp, long roomId, IPAddress senderIp, long senderId, string sendername, string senderPfp);
        public Task<bool> PfpChanged(IPAddress reciverIp, long senderId, string senderNewPfp);
        public Task<bool> NameChanged(IPAddress reciverIp, long senderId, string senderNewName);
        public Task<bool> LeaveGroup(IPAddress reciverIp, long roomId, long senderId);
        public Task<bool> SendFilePrivateSteam(IPAddress reciverIp, long senderId, string filePath);
    }
}
