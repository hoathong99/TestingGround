using Abp;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Localization;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using MHPQ.Chat;
using MHPQ.RoomChats;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Web.Host.Chat
{
    public class ChatHub : OnlineClientHubBase, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        //public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the session.
        /// </summary>
       // public IAbpSession AbpSession { get; set; }

        private readonly IChatMessageManager _chatMessageManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IRoomChatManager _roomChatManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        [Obsolete]
        public ChatHub(
            IChatMessageManager chatMessageManager,
            ILocalizationManager localizationManager,
            IOnlineClientManager onlineClientManager,
            IRoomChatManager roomChatManager,
            IOnlineClientInfoProvider clientInfoProvider) : base(onlineClientManager, clientInfoProvider)
        {
            _chatMessageManager = chatMessageManager;
            _localizationManager = localizationManager;
            _roomChatManager = roomChatManager;


            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        [Obsolete]
        public string SendMessage(SendChatMessageInput input)
        {
            var sender = AbpSession.ToUserIdentifier();
            //var sender = new UserIdentifier(input.TenantId, input.SenderId);
            var receiver = new UserIdentifier(input.TenantId, input.UserId);
           
            try
            {
                
                if (input.Message.StartsWith("http"))
                {
                    input.Message = string.Format("<div class=\"flex-shrink-1 bg-primary rounded py-2 px-3 ml-3\"><a href=\"{0}\">{0}</a></div>", input.Message);
                   
                }
                else
                {
                    input.Message = string.Format("<div class=\"flex-shrink-1 bg-primary rounded py-2 px-3 ml-3\">{0}</div>", input.Message);
                }
                _chatMessageManager.SendMessage(sender, receiver, input.Message, input.TenancyName, input.UserName, input.ProfilePictureId);
                return string.Empty;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return _localizationManager.GetSource("AbpWeb").GetString("InternalServerError");
            }
        }


        [Obsolete]
        public async Task Join(string roomName)
        {
            try
            {
                var sender = AbpSession.ToUserIdentifier();
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                // Tell others to update their list of users
                await Clients.OthersInGroup(roomName).SendAsync("addUser", sender);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }

        }

        [Obsolete]
        public void SendMessageGroup(SendGroupChatMessageInput input)
        {
            var user = AbpSession.ToUserIdentifier();
            //var sender = new UserIdentifier(input.TenantId, input.SenderId);
            var receiver = new UserIdentifier(input.TenantId, input.UserId);

            try
            {

                if (input.Message.StartsWith("http"))
                {
                    input.Message = string.Format("<div class=\"flex-shrink-1 bg-primary rounded py-2 px-3 ml-3\"><a href=\"{0}\">{0}</a></div>", input.Message);

                }
                else
                {
                    input.Message = string.Format("<div class=\"flex-shrink-1 bg-primary rounded py-2 px-3 ml-3\">{0}</div>", input.Message);
                }
                //_chatMessageManager.SendMessage(sender, receiver, input.Message, input.TenancyName, input.UserName, input.ProfilePictureId);
               // return string.Empty;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);

            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

    }
}
