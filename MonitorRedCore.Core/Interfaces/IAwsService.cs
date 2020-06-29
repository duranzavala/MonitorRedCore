using System.Threading.Tasks;
using MonitorRedCore.Core.CustomEntities;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IAwsService
    {
        public Task<AwsOptions> GetAwsOptions();
    }
}
