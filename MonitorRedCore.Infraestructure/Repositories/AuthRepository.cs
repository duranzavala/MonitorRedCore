using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using MonitorRedCore.API.Responses;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.DTOs;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;
        private readonly IAwsService _awsService;

        public AuthRepository(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool pool,
            IAwsService awsService)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _pool = pool;
            _awsService = awsService;
        }

        public async Task<bool> SignUp(Users user)
        {
            var userPool = _pool.GetUser(user.Email);
            userPool.Attributes.Add(CognitoAttribute.Email.AttributeName, user.Email);

            var result = await _userManager.CreateAsync(userPool, user.Password);

            return result.Succeeded;
        }

        public async Task<LoginResponse> SignIn(AuthDto authDto)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast2);
            AwsOptions awsOptions = await _awsService.GetAwsOptions();
            var secretHash = _awsService.GetSecretHash(
                authDto.Email,
                awsOptions.UserPoolClientId,
                awsOptions.UserPoolClientIdSecret
            );

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = awsOptions.UserPoolId,
                ClientId = awsOptions.UserPoolClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", authDto.Email },
                    { "PASSWORD", authDto.Password },
                    { "SECRET_HASH", secretHash }
                }
            };

            var loginResult = new LoginResponse { Success = false };

            try
            {
                var result = await cognito.AdminInitiateAuthAsync(request);
                loginResult.Data = result.AuthenticationResult.IdToken;
                loginResult.Success = true;
            }
            catch (Exception ex)
            {
                loginResult.Data = ex.Message;
            }

            return loginResult;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}