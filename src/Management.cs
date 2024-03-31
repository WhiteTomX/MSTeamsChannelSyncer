using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WhiteTom.MsTeamsChannelSyncer
{
    public class Management
    {
        private readonly ILogger _logger;

        public Management(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Management>();
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
