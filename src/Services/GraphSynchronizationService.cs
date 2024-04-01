using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using WhiteTom.MsTeamsChannelSyncer.Repositories;

namespace WhiteTom.MsTeamsChannelSyncer.Services
{
    public class GraphSynchronizationService : ISynchronizationService
    {
        private readonly ILogger<GraphSynchronizationService> _logger;
        private readonly IConfigurationRepository _config;
        private readonly IChannelRepository _channelRepository;

        public GraphSynchronizationService(ILogger<GraphSynchronizationService> logger, IConfigurationRepository configuration, IChannelRepository channelRepository)
        {
            _logger = logger;
            _config = configuration;    
            _channelRepository = channelRepository;
        }

        /// <summary>
        /// Synchronize the members from source channel to target channel
        /// </summary>
        public void FullSynchronization()
        {
            var sourceMembers = _channelRepository.GetAllMembers(teamId: _config.SourceTeamId, channelId: _config.SourceChannelId);
            var targetMembers = _channelRepository.GetAllMembers(teamId: _config.TargetTeamId, channelId: _config.TargetChannelId);

            var missingMembers = sourceMembers.Except(targetMembers);

            foreach (var missingmember in missingMembers)
            {
                _channelRepository.AddMember(teamId: _config.TargetTeamId, channelId: _config.TargetChannelId, id: missingmember);
            }

            var membersToRemove = targetMembers.Except(sourceMembers);

            foreach (var memberToRemove in membersToRemove)
            {
                _channelRepository.RemoveMember(teamId: _config.TargetTeamId, channelId: _config.TargetChannelId, id: memberToRemove);
            }
        }
    }
}
