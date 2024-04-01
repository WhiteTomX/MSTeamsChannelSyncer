using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WhiteTom.MsTeamsChannelSyncer.Functions;
using WhiteTom.MsTeamsChannelSyncer.Repositories;
using WhiteTom.MsTeamsChannelSyncer.Services;

namespace WhiteTom.MsTeamsChannelSyncer.Tests.Unit.Services
{
    public class SynchronizationServiceTests
    {
        private readonly ILogger<GraphSynchronizationService> _logger = Substitute.For<ILogger<GraphSynchronizationService>>();
        private readonly IConfigurationRepository _config = Substitute.For<IConfigurationRepository>();
        private readonly IChannelRepository _channelRepository = Substitute.For<IChannelRepository>();
        private readonly ISynchronizationService _synchronizationService;
        public SynchronizationServiceTests() 
        {
            _synchronizationService = new GraphSynchronizationService(_logger, _config, _channelRepository);
        }

        [Fact]
        public void FullSynchronizationEmptyChannels()
        {
            // Arrange


            // Act
            _synchronizationService.FullSynchronization();

            //Assert
            _channelRepository.DidNotReceiveWithAnyArgs().AddMember(default, default, default);
            _channelRepository.DidNotReceiveWithAnyArgs().RemoveMember(default, default, default);
        }

    }
}
