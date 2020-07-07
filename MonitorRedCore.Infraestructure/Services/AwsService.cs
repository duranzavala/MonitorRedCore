using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Options;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.Interfaces;

namespace MonitorRedCore.Infraestructure.Services
{
    public class AwsService : IAwsService
    {
        private readonly RegionEndpoint _region = RegionEndpoint.USEast2;
        private readonly AwsOptions _awsOptions;

        public AwsService(IOptions<AwsOptions> options)
        {
            _awsOptions = options.Value;
        }

        public async Task<AwsOptions> GetAwsOptions()
        {
            var awsOptions = new AwsOptions
            {
                UserPoolId = await GetAwsParameters(_awsOptions.UserPoolId),
                UserPoolClientId = await GetAwsParameters(_awsOptions.UserPoolClientId),
                UserPoolClientIdSecret = await GetAwsParameters(_awsOptions.UserPoolClientIdSecret),
                MetaDataUrl = await GetAwsParameters(_awsOptions.MetaDataUrl)
            };

            return awsOptions;
        }

        private async Task<string> GetAwsParameters(string parameter)
        {
            var ssmClient = new AmazonSimpleSystemsManagementClient(_region);

            var response = await ssmClient.GetParameterAsync(new GetParameterRequest
            {
                Name = parameter,
                WithDecryption = true
            });

            return response.Parameter.Value;
        }
    }
}
