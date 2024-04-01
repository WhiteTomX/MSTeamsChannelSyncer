using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using WhiteTom.MsTeamsChannelSyncer.Services;

namespace WhiteTom.MsTeamsChannelSyncer.Functions
{
    public class ManagementFunction
    {
        private readonly ILogger _logger;
        private readonly ISynchronizationService _synchronizationService;

        public ManagementFunction(ILoggerFactory loggerFactory, ISynchronizationService synchronizationService)
        {
            _logger = loggerFactory.CreateLogger<ManagementFunction>();
            _synchronizationService = synchronizationService;
        }

        [Function("Management")]
        public void Run([TimerTrigger("0 0 0 */2 * *")] TimerInfo timer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (timer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {timer.ScheduleStatus.Next}");
            }
            _synchronizationService.FullSynchronization();
        }
    }
}
