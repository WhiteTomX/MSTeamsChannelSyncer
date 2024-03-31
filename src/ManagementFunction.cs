using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace WhiteTom.MsTeamsChannelSyncer
{
    public class ManagementFunction
    {
        private readonly ILogger _logger;
        private readonly GraphServiceClient _graphClient;

        public ManagementFunction(ILoggerFactory loggerFactory, GraphServiceClient graphClient)
        {
            _logger = loggerFactory.CreateLogger<ManagementFunction>();
            _graphClient = graphClient;
        }

        [Function("Management")]
        public void Run([TimerTrigger("0 0 0 */2 * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
