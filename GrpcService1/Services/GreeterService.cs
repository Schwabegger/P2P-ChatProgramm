using Grpc.Core;
using GrpcShared;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GrpcServer
{
    public class GreeterService : Greeter.GreeterBase
    {
        public event EventHandler<string> RequestedUserHandler;
        public event EventHandler<(string, long, string, string)> RecivedUserHandler;
        public event EventHandler<(string, long, string, string)> OpenPrivateChatHandler;
        public event EventHandler<(long, string, string, string, long, string, string)> AddedToGroupchatHandler;
        public event EventHandler<(long, string, long, string, string)> JoinedGroupchatHandler;
        public event EventHandler<(long, string)> PrivateMessageRecivedHandler;
        public event EventHandler<(long, long, string)> GroupMessageRecivedHandler;
        public event EventHandler<(long, string, long, string, string)> NewUserAddedToGroupchatHandler;
        public event EventHandler<(long, long, string, string, string)> TransmitChatroomParticipantHandler;
        public event EventHandler<(long, string)> NameChangedHandler;
        public event EventHandler<(long, string)> PfpChangedHandler;
        
        public override Task<Recived> RequestedUserPrivate(RequestUserMsg request, ServerCallContext context)
        {
            RequestedUserHandler?.Invoke(this, request.Ip);
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> AddedToGroupchat(AddedToGroupchatMsg request, ServerCallContext context)
        {
            AddedToGroupchatHandler?.Invoke(this, (request.RoomId, request.RoomName, request.RoomPfp, request.SenderIp, request.SenderId, request.SenderName, request.SenderPfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> OpenPrivateChat(OpenPrivateChatMsg request, ServerCallContext context)
        {
            OpenPrivateChatHandler?.Invoke(this, (request.Ip, request.Id, request.Name, request.Pfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> PrivateMessageRecived(PrivateMessage request, ServerCallContext _)
        {
            PrivateMessageRecivedHandler?.Invoke(this, (request.Id, request.Content));
            return Task.FromResult(new Recived 
            {
                Done = true
            });
        }
        public override Task<Recived> GroupMessageRecived(GroupMessage request, ServerCallContext _)
        {
            GroupMessageRecivedHandler?.Invoke(this, (request.RoomId, request.Id, request.Content));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> NewUserAddedToGroupchatRecived(NewUserAddedToGroupchat request, ServerCallContext _)
        {
            NewUserAddedToGroupchatHandler?.Invoke(this, (request.RoomId, request.UserIp, request.UserId, request.UserName, request.UserPfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> TransmitChatroomParticipantRecived(TransmitChatroomParticipant request, ServerCallContext _)
        {
            TransmitChatroomParticipantHandler?.Invoke(this, (request.RoomId, request.UserId, request.UserName, request.UserPfp, request.UserIp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> NameChangedRecived(NameChanged request, ServerCallContext _)
        {
            NameChangedHandler?.Invoke(this, (request.Id, request.NewName));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> PfpChangedRecived(PfpChanged request, ServerCallContext _)
        {
            PfpChangedHandler?.Invoke(this, (request.Id, request.NewPfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> SendUser(SendUserMsg request, ServerCallContext _)
        {
            RecivedUserHandler?.Invoke(this, (request.Ip, request.Id, request.Name, request.Pfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        public override Task<Recived> JoinGroupchat(JoinGroupchatMsg request, ServerCallContext _)
        {
            JoinedGroupchatHandler?.Invoke(this, (request.RoomId, request.SenderIp, request.SenderId, request.SenderName, request.SenderPfp));
            return Task.FromResult(new Recived
            {
                Done = true
            });
        }
        

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    _logger.LogInformation($"Saying hello to {request.Name}\n");
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}
    }
}