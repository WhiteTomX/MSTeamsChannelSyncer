using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteTom.MsTeamsChannelSyncer.Repositories
{
    public interface IConfigurationRepository
    {
        public string SourceTeamId { get; }
        public string SourceChannelId { get; }
        public string TargetTeamId { get; }
        public string TargetChannelId { get; }
    }
}
