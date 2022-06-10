// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Interfaces;
using Grpc.Net.Client;
using GrpcShared;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basics.Models
{
    public class GrpcSender : ISender
    {
        public async Task<bool> SendGroupMessage(IPAddress reciverIp, long senderId, string messsage, long roomId)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.GroupMessageRecivedAsync(new GroupMessage() { Id = senderId, Content = messsage, RoomId = roomId });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> SendPrivateMessage(IPAddress reciverIp, long senderId, string messsage)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.PrivateMessageRecivedAsync(new PrivateMessage() { Id = senderId, Content = messsage });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> TellOthersANewUserWasAddedToChatroom(IPAddress reciverIp, long roomId, IPAddress addedUserIp, long addedUserId, string addedUserName, string addedUserPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.NewUserAddedToGroupchatRecivedAsync(new NewUserAddedToGroupchat() { RoomId = roomId, UserIp = addedUserIp.ToString(), UserId = addedUserId, UserName = addedUserName, UserPfp = addedUserPfp});
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> TransmitChatroomParticipantsToAddedUser(IPAddress reciverIp, long roomId, IPAddress participantIp, long participantId, string participantName, string participantPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.TransmitChatroomParticipantRecivedAsync(new TransmitChatroomParticipant() { RoomId = roomId, UserIp = participantIp.ToString(), UserId = participantId, UserName = participantName, UserPfp = participantPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> RequestedUserPrivate(IPAddress reciverIp, IPAddress senderIp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.RequestedUserPrivateAsync(new RequestUserMsg() { Ip = senderIp.ToString() });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> SendUser(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.SendUserAsync(new SendUserMsg() { Ip = myIp.ToString(), Id = myId, Name = myName, Pfp = myPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> OpenPrivateChat(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.OpenPrivateChatAsync(new OpenPrivateChatMsg() { Ip = myIp.ToString(), Id = myId, Name = myName, Pfp = myPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> AddToGroupchat(IPAddress reciverIp, long roomId, string name, string pfp, IPAddress senderIp, long senderId, string sendername, string senderPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.AddedToGroupchatAsync(new AddedToGroupchatMsg() { RoomId = roomId, RoomName = name, RoomPfp = pfp, SenderIp = senderIp.ToString(), SenderId = senderId, SenderName = sendername, SenderPfp = senderPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> JoinGroupchat(IPAddress reciverIp, long roomId, IPAddress senderIp, long senderId, string sendername, string senderPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.JoinGroupchatAsync(new JoinGroupchatMsg() { RoomId = roomId, SenderIp = senderIp.ToString(), SenderId = senderId, SenderName = sendername, SenderPfp = senderPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> PfpChanged(IPAddress reciverIp, long senderId, string senderNewPfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.PfpChangedRecivedAsync(new PfpChanged() { Id = senderId, NewPfp = senderNewPfp });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> NameChanged(IPAddress reciverIp, long senderId, string senderNewName)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.NameChangedRecivedAsync(new NameChanged() { Id = senderId, NewName = senderNewName });
            await channel.ShutdownAsync();
            return res.Done;
        }

        public async Task<bool> LeaveGroup(IPAddress reciverIp, long roomId, long senderId)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.LeftGroupAsync(new LeftGroupMsg() { RoomId = roomId, SenderId = senderId});
            await channel.ShutdownAsync();
            return res.Done;
        }

        private async Task<bool> Send(IPAddress reciverIp, Func<Task> action)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            await action();
            await channel.ShutdownAsync();
            return true;
        }
    }
}