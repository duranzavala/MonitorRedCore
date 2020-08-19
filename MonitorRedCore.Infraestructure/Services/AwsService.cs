using System;
using System.Security.Cryptography;
using System.Text;
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
            var ssmClient = new AmazonSimpleSystemsManagementClient(RegionEndpoint.USEast2);

            var response = await ssmClient.GetParameterAsync(new GetParameterRequest
            {
                Name = parameter,
                WithDecryption = true
            });

            return response.Parameter.Value;
        }

        public string GetSecretHash(string username, string appClientId, string appSecretKey)
        {
            var dataString = username + appClientId;

            var data = Encoding.UTF8.GetBytes(dataString);
            var key = Encoding.UTF8.GetBytes(appSecretKey);

            return Convert.ToBase64String(HmacSHA256(data, key));
        }

        private byte[] HmacSHA256(byte[] data, byte[] key)
        {
            using (var shaAlgorithm = new HMACSHA256(key))
            {
                var result = shaAlgorithm.ComputeHash(data);
                return result;
            }
        }
    }
}
