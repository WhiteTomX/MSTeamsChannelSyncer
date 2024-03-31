using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using NSubstitute;

namespace WhiteTom.MsTeamsChannelSyncer.Tests.Unit
{
    public class ManagementFunctionTests
    {
        private readonly ManagementFunction _managementFunction;
        private readonly ILoggerFactory _loggerFactory = Substitute.For< ILoggerFactory>();
        private readonly GraphServiceClient _graphClient = Substitute.For<GraphServiceClient>();


        public ManagementFunctionTests()
        {
            _managementFunction = new ManagementFunction(_loggerFactory, _graphClient);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}