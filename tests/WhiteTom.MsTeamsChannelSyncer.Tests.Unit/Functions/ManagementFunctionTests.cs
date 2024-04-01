using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WhiteTom.MsTeamsChannelSyncer.Functions;
using WhiteTom.MsTeamsChannelSyncer.Services;

namespace WhiteTom.MsTeamsChannelSyncer.Tests.Unit.Functions
{
    public class ManagementFunctionTests
    {
        private readonly ManagementFunction _managementFunction;
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly SynchronizationService _synchronizationService = Substitute.For<SynchronizationService>();


        public ManagementFunctionTests()
        {
            _managementFunction = new ManagementFunction(_loggerFactory, _synchronizationService);
        }

        [Fact]
        public void Test1()
        {
            _managementFunction.Run(Substitute.For<TimerInfo>());
            _synchronizationService.Received().FullSynchronization();
        }
    }
}