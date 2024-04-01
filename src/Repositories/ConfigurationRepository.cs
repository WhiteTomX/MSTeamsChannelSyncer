using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WhiteTom.MsTeamsChannelSyncer.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IConfiguration _config;
        public string SourceTeamId { get; }
        public string SourceChannelId { get; }
        public string TargetTeamId { get; }
        public string TargetChannelId { get; }

        public ConfigurationRepository(IConfiguration configuration) 
        {
            _config = configuration;
            SourceTeamId = GetFromConfiguration(nameof(SourceTeamId));
            SourceChannelId = GetFromConfiguration(nameof(SourceChannelId));
            TargetTeamId = GetFromConfiguration(nameof(TargetTeamId));
            TargetChannelId = GetFromConfiguration(nameof(TargetChannelId));

        }

        private string GetFromConfiguration(string name)
        {
            var value = _config.GetValue<string>(name);
            if (String.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration {name} is required but was not found.");
            }
            else
            {
                return value;
            }
        }
    }
}
