using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HubManPractices.Models;
using WebApp.App_Start;
using HubManPractices.Service.AuthStartup;
using HubManPractices.Service.ViewModels;
using HubManPractices.Service;
using System.Data.Entity.Infrastructure;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
       

            private ApplicationSignInManager _signInManager;
            private ApplicationUserManager _userManager;
            private readonly IRoleService roleService;
            private readonly IClientService ClientService;
            private readonly IResellerService ResellerService;
            private readonly ISubscriptionsService SubscriptionsService;
            private readonly IActionService ActionService;

        public AccountController(IActionService As,IRoleService Rs, IClientService Cs,IResellerService ResellerServ,ISubscriptionsService SS)
            {
                ActionService = As;
                roleService = Rs;
                ClientService = Cs;
                ResellerService = ResellerServ;
                SubscriptionsService = SS;
            }

            //Added By Hesham El-Elamy ... Send An Email 
            [HttpPost]
            public async Task<ActionResult> SendEmail(FormCollection Fc)
            {
                if (EmailIsCorrectFormat(Fc["ContactMail"]) && Fc["ContactNumber"].ToString().Count()==11)
              {
                if (ClientService.ClientNameAndMailExistsAndDeleted(Fc["ClientName"], Fc["ContactMail"]))
                {
                    TempData["NameAndMailExistsAndDeleted"] = "The Client you are trying to add was deleted , Choose What To Do :";
                    Client client = new Client() { Expiry = null, IsExpiryNull = true, ClientName = Fc["ClientName"], ContactMail = Fc["ContactMail"], ContactName = Fc["ContactName"], ContactNumber = Int32.Parse(Fc["ContactNumber"]), ContactTitle = Fc["ContactTitle"], ResellerID = Guid.Parse(Fc["ResellerID"]),Seats=Int32.Parse(Fc["Seats"]),Location=Fc["Location"],CreationDate=DateTime.Now};
                        IEnumerable<OfficeSubscription> Subscriptions = SubscriptionsService.GetAllSubscriptions();
                        foreach (var Sub in Subscriptions)
                        {
                            string idx = Sub.MonthlyFee.ToString();
                            if (Fc[Sub.SubscriptionName] != "" && Fc[Sub.SubscriptionName] != "0")
                            {
                                client.ClientSubscriptions.Add(new ClientSubscriptions() { SubscriptionID = Guid.Parse(Fc[idx]), UsersPerSubscription = Int32.Parse(Fc[Sub.SubscriptionName]), OfficeSubscription = SubscriptionsService.GetById(Guid.Parse(Fc[idx])) });
                            }
                        }
                    
              
                    return View("~/Views/Client/Exist.cshtml", ClientService.MapToViewModel(client));
                }

                try
                {
                    Client client = new Client() { ClientID = Guid.NewGuid(), Expiry = null, IsExpiryNull = true, ClientName = Fc["ClientName"], ContactMail = Fc["ContactMail"], ContactName = Fc["ContactName"], ContactNumber = Int32.Parse(Fc["ContactNumber"]), ContactTitle = Fc["ContactTitle"], ResellerID = Guid.Parse(Fc["ResellerID"]) };
                    client.reseller = ResellerService.GetById(Guid.Parse(Fc["ResellerID"]));

                    IEnumerable<OfficeSubscription> Subscriptions = SubscriptionsService.GetAllSubscriptions();
        
          
                   
                    if (!ClientService.Exists(client))
                    {
                        client.Seats = Int32.Parse(Fc["Seats"]);
                        client.Status = "On Hold";
                        client.CreationDate = DateTime.Now;
                        client.Location = Fc["Location"];
                        //UNCOMMENT when its available in PopUp ..
                        /*foreach (var Sub in Subscriptions)
                        {
                            if (Fc[Sub.SubscriptionName] != "" && Fc[Sub.SubscriptionName] != "0")
                            {
                                string idx = Sub.MonthlyFee.ToString();
                                client.ClientSubscriptions.Add(new ClientSubscriptions() { ClientID = client.ClientID, SubscriptionID = Guid.Parse(Fc[idx]), UsersPerSubscription = Int32.Parse(Fc[Sub.SubscriptionName]) });
                            }
                        }*/
                        ClientService.CreateClient(client);
                        HubManPractices.Models.Action OnHold = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "On Hold", Client = client, Date = DateTime.Now };
                        ActionService.CreateAction(OnHold);
                    }
                    else
                    {
                        TempData["Exists"] = "This Client is already added in the system";
                        return RedirectToAction("Create", "Client", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                    }

                    ApplicationUser LoggedInUser = UserManager.FindById(User.Identity.GetUserId());

                    IEnumerable<ApplicationUser> GlobalAdmins = roleService.GetUserInRole("Global Admin");


                    foreach (var global in GlobalAdmins)
                    {
                        string Body = String.Format("Dear {0}, <br />"
                      + "The Client {1} has been added to your system by Reseller {2} <br /> "
                      + "Please Click {3}", global.UserName, client.ClientName, User.Identity.GetUserName(), "<a href='http://localhost:41301/Client?ResellerID=" + LoggedInUser.ResellerID + "'>here</a>");

                        await UserManager.SendEmailAsync(global.Id, "Client is Added ", Body);
                    }
                    TempData["Create Success"] = "Client Created Successfully";
                    return RedirectToAction("ResellerIndex", "Reseller", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                }
                catch (DbUpdateException ex)
                {
                    TempData["Exists"] = "Contact Mail Can not be repeated";
                    return RedirectToAction("Create", "Client", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                }
                catch (Exception ex)
                {
                    TempData["ErrorCreate"] = "Error in adding Client";
                    return RedirectToAction("ResellerIndex", "Reseller", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                }

            }
               else
            {
                if (!EmailIsCorrectFormat(Fc["ContactMail"]))
                {
                    TempData["EmailWrongFormhat"] = "The E-mail You entered is not in the correct format";
                    return RedirectToAction("Create", "Client", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                }
                else
                {
                    TempData["NumberWrongFormat"] = "The Contact Number You entered is not a Mobile Number";
                    return RedirectToAction("Create", "Client", new { ResellerID = Guid.Parse(Fc["ResellerID"]) });
                }
            }

        }

        private bool EmailIsCorrectFormat(string email)
        {
            if (email.Contains(".com") && email.Contains("@"))
                return true;
            else return false;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            {
                UserManager = userManager;
                SignInManager = signInManager;
               
            }

            public ApplicationSignInManager SignInManager
            {
                get
                {
                    return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                private set
                {
                    _signInManager = value;
                }
            }

            public ApplicationUserManager UserManager
            {
                get
                {
                    return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                private set
                {
                    _userManager = value;
                }
            }

            //
            // GET: /Account/Login
            [AllowAnonymous]
            public ActionResult Login(string returnUrl)
            {
                if(User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Reseller");
                }
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            //
            // POST: /Account/Login
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }

            //
            // GET: /Account/VerifyCode
            [AllowAnonymous]
            public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
            {
                // Require that the user has already logged in via username/password or external login
                if (!await SignInManager.HasBeenVerifiedAsync())
                {
                    return View("Error");
                }
                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }

            //
            // POST: /Account/VerifyCode
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // The following code protects for brute force attacks against the two factor codes. 
                // If a user enters incorrect codes for a specified amount of time then the user account 
                // will be locked out for a specified amount of time. 
                // You can configure the account lockout settings in IdentityConfig
                var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }

            //
            // GET: /Account/Register
            [AllowAnonymous]
            public ActionResult Register()
            {
                return View();
            }

            //
            // POST: /Account/Register
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }

            //
            // GET: /Account/ConfirmEmail
            [AllowAnonymous]
            public async Task<ActionResult> ConfirmEmail(string userId, string code)
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }

            //
            // GET: /Account/ForgotPassword
            [AllowAnonymous]
            public ActionResult ForgotPassword()
            {
                return View();
            }

            //
            // POST: /Account/ForgotPassword
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                    // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    // return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }

            //
            // GET: /Account/ForgotPasswordConfirmation
            [AllowAnonymous]
            public ActionResult ForgotPasswordConfirmation()
            {
                return View();
            }

            //
            // GET: /Account/ResetPassword
            [AllowAnonymous]
            public ActionResult ResetPassword(string code)
            {
                return code == null ? View("Error") : View();
            }

            //
            // POST: /Account/ResetPassword
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
                return View();
            }

            //
            // GET: /Account/ResetPasswordConfirmation
            [AllowAnonymous]
            public ActionResult ResetPasswordConfirmation()
            {
                return View();
            }

            //
            // POST: /Account/ExternalLogin
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult ExternalLogin(string provider, string returnUrl)
            {
                // Request a redirect to the external login provider
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }

            //
            // GET: /Account/SendCode
            [AllowAnonymous]
            public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
            {
                var userId = await SignInManager.GetVerifiedUserIdAsync();
                if (userId == null)
                {
                    return View("Error");
                }
                var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }

            //
            // POST: /Account/SendCode
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> SendCode(SendCodeViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Generate the token and send it
                if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                {
                    return View("Error");
                }
                return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
            }

            //
            // GET: /Account/ExternalLoginCallback
            [AllowAnonymous]
            public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    return RedirectToAction("Login");
                }

                // Sign in the user with this external login provider if the user already has a login
                var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                    case SignInStatus.Failure:
                    default:
                        // If the user does not have an account, then prompt the user to create an account
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                }
            }

            //
            // POST: /Account/ExternalLoginConfirmation
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Manage");
                }

                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        return View("ExternalLoginFailure");
                    }
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            //
            // POST: /Account/LogOff
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult LogOff()
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }

            //
            // GET: /Account/ExternalLoginFailure
            [AllowAnonymous]
            public ActionResult ExternalLoginFailure()
            {
                return View();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_userManager != null)
                    {
                        _userManager.Dispose();
                        _userManager = null;
                    }

                    if (_signInManager != null)
                    {
                        _signInManager.Dispose();
                        _signInManager = null;
                    }
                }

                base.Dispose(disposing);
            }

            #region Helpers
            // Used for XSRF protection when adding external logins
            private const string XsrfKey = "XsrfId";

            private IAuthenticationManager AuthenticationManager
            {
                get
                {
                    return HttpContext.GetOwinContext().Authentication;
                }
            }

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            private ActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            internal class ChallengeResult : HttpUnauthorizedResult
            {
                public ChallengeResult(string provider, string redirectUri)
                    : this(provider, redirectUri, null)
                {
                }

                public ChallengeResult(string provider, string redirectUri, string userId)
                {
                    LoginProvider = provider;
                    RedirectUri = redirectUri;
                    UserId = userId;
                }

                public string LoginProvider { get; set; }
                public string RedirectUri { get; set; }
                public string UserId { get; set; }

                public override void ExecuteResult(ControllerContext context)
                {
                    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                    if (UserId != null)
                    {
                        properties.Dictionary[XsrfKey] = UserId;
                    }
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                }
            }
            #endregion
        }
    }

