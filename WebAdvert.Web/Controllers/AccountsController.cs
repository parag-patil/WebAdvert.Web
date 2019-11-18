using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.BO;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signinManager;
        private readonly CognitoUserPool _pool;
        public AccountsController(UserManager<CognitoUser> userManager, SignInManager<CognitoUser> signinManager, CognitoUserPool pool)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _pool = pool;
        }

        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel signupViewModel)
        {
            if (ModelState.IsValid)
            {
                SignupViewModel vModel = new SignupViewModel();
                SignupBO bo = new SignupBO(_pool, _userManager);
                vModel = await bo.CreateUser(signupViewModel);
                if (!vModel.IsValid)
                {
                    ModelState.AddModelError("Signup", vModel.Message);
                    return View(vModel);
                }
                else
                {
                    RedirectToAction("Confirm");
                }

            }
            return View();
        }

        public async Task<IActionResult> Confirm()
        {
            return View(new ConfirmViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmViewModel confirmViewModel)
        {
            return View(confirmViewModel);
        }
    }
}