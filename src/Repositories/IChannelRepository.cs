using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Models;

namespace WhiteTom.MsTeamsChannelSyncer.Repositories
{
    public interface IChannelRepository
    {
        /// <summary>
        /// Get all Members of a channel in team.
        /// </summary>
        /// <param name="teamId">The Id of the team to get the members from.</param>
        /// <param name="channelId">Id of the channel to get the members</param>
        /// <returns>Collection of tenantwide unique identitfiers.</returns>
        IEnumerable<string> GetAllMembers(string teamId, string channelId);

        /// <summary>
        /// Add a member with Id to a channel of a team.
        /// </summary>
        /// <param name="teamId">If of team</param>
        /// <param name="channelId">If of channel</param>
        /// <param name="id">Identifier of user to add. May be UPN or Id.</param>
        void AddMember(string teamId, string channelId, string id);

        /// <summary>
        /// Remove a member from a channel of a team.
        /// </summary>
        /// <param name="teamId">If of team</param>
        /// <param name="channelId">If of channel</param>
        /// <param name="Id">Identifier of user to remove. May be UPN or Id.</param>
        void RemoveMember(string teamId, string channelId, string id);
    }
}
