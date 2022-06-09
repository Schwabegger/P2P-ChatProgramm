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

        public async Task<bool> TellOthersANewUserWasAddedToChatroomAsync(IPAddress reciverIp, long roomId, IPAddress addedUserIp, long addedUserId, string addedUserName, string addedUserPfp)
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

        public async Task<bool> OpenPrivateChatroom(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddToGroupchat(IPAddress reciverIp, long roomId, string name, string pfp)
        {
            var channel = GrpcChannel.ForAddress($"http://{reciverIp}:5000");
            var client = new Greeter.GreeterClient(channel);
            var res = await client.AddedToGroupchatAsync(new AddedToGroupchatMsg() { RoomId = roomId, RoomName = name, RoomPfp = pfp });
            await channel.ShutdownAsync();
            return res.Done;
        }
    }
}