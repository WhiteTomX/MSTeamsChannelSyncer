using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace WhiteTom.MsTeamsChannelSyncer.Repositories
{
    public class GraphChannelRepository : IChannelRepository
    {
        private readonly ILogger<GraphChannelRepository> _logger;
        private readonly GraphServiceClient _graphclient;

        public GraphChannelRepository(ILogger<GraphChannelRepository> logger, GraphServiceClient graphclient) 
        {
            _logger = logger;
            _graphclient = graphclient;
        }

        public void AddMember(string teamId, string channelId, string id)
        {
            var body = new AadUserConversationMember
            {
                OdataType = "#microsoft.graph.aadUserConversationMember",
                Roles = new List<string>(),
                AdditionalData = new Dictionary<string,object>
                {
                    {
                    "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{id}')"
                    }
                },
            };
            var task = _graphclient.Teams[teamId].Channels[channelId].Members.PostAsync(body);
            task.Wait();
            if (task.IsCompletedSuccessfully)
            {
                return;
            }
            else
            {
                _logger.LogError(Events.UserAddedFailed, null, $"failed to add {id} to {channelId} in {teamId}", task.Result);
            }
        }

        public IEnumerable<string> GetAllMembers(string teamId, string channelId)
        {
            var task = _graphclient.Teams[teamId].Channels[channelId].Members.GetAsync();
            task.Wait();
            if(null !=  task.Result && null != task.Result.Value)
            {
                var members = task.Result.Value.Select(m => m.AdditionalData["userId"].ToString());
                if(members.Any(m=> m==null))
                {
                    _logger.LogWarning(Events.MemberNull, "Id of a member was null.");
                    return members.Where(m => m != null)!;
                }
                else
                {
                    return members!;
                }
            }
            else
            {
                return new System.Collections.Generic.List<string>();
            }
        }

        public void RemoveMember(string teamId, string channelId, string id)
        {
            var task = _graphclient.Teams[teamId].Channels[channelId].Members.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = $"(microsoft.graph.aadUserConversationMember/userId eq '{id}')";
            });
            task.Wait();
            var conversationMemberId = task.Result.Value[0].Id;

            var deleteTask = _graphclient.Teams[teamId].Channels[channelId].Members[conversationMemberId].DeleteAsync();
            deleteTask.Wait();
        }
    }
}
