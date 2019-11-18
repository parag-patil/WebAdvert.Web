using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Accounts;
using Microsoft.AspNetCore.Identity;

namespace WebAdvert.Web.BO
{
    public class SignupBO
    {
        private readonly string USER_EXISTS = "User with this email already exists.";
        private readonly CognitoUserPool _pool;
        private readonly UserManager<CognitoUser> _userManager;
        public SignupBO(CognitoUserPool pool, UserManager<CognitoUser> userManager)
        {
            _pool = pool;
            _userManager = userManager;
        }
        public async Task<SignupViewModel> CreateUser(SignupViewModel model)
        {
            var user = _pool.GetUser(model.Email);          //We are using Email as userid.

            if(user.Status != null)
            {
                model.IsValid = false;
                model.Message = USER_EXISTS;

                return model;
            }

            user.Attributes.Add(CognitoAttribute.Name.ToString(),model.Email);         //We are using Email as name just to get started.
            var createdUser = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            if (createdUser.Succeeded)
            {
                model.IsValid = true;
                model.Message = string.Format("{0} is created successfully.", model.Email);
            }

            return model;
        }
    }
}
