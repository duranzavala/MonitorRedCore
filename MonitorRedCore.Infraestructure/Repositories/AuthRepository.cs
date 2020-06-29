using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public AuthRepository(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool pool)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _pool = pool;
        }

        public async Task<bool> SignUp(Users user)
        {
            var userPool = _pool.GetUser(user.Email);
            userPool.Attributes.Add(CognitoAttribute.Email.AttributeName, user.Email);

            var result = await _userManager.CreateAsync(userPool, user.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userPool, isPersistent: false);
            }

            return true;
        }

        public async Task<bool> SignIn(Users user)
        {

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
