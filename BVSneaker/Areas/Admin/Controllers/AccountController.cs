using BVSneaker.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();


        public AccountController()
        {
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
        // GET: Admin/Account
        public ActionResult Index()
        {
            var items = _dbConnect.Users.ToList();
            return View(items);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
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
        //Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: /Account/Register
        public ActionResult Create()
        {
            ViewBag.Role = new SelectList(_dbConnect.Roles.ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, model.Role);
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(_dbConnect.Roles.ToList(), "Name", "Name");

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Edit(string Id)
        {
            var user = UserManager.FindById(Id);
            EditAccountViewModel userToEdit = new EditAccountViewModel()
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone
            };
            ViewBag.Role = new SelectList(_dbConnect.Roles.ToList(), "Name", "Name");
            return View(userToEdit);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser UserToEdit = UserManager.FindById(model.UserId);

                if (UserToEdit.FullName != model.FullName)
                    UserToEdit.FullName = model.FullName;

                if (UserToEdit.Email != model.Email)
                    UserToEdit.Email = model.Email;

                if (UserToEdit.Phone != model.Phone)
                    UserToEdit.Phone = model.Phone;

                IdentityResult result = UserManager.Update(UserToEdit);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(UserToEdit.Id, model.Role);
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(_dbConnect.Roles.ToList(), "Name", "Name");
            return View(model);
        }
        public ActionResult Delete(string Id)
        {
            ApplicationUser UserToDelete = UserManager.FindById(Id);
            if (UserToDelete != null)
            {
                IdentityResult result = UserManager.Delete(UserToDelete);
                if (result.Succeeded)
                    return Json(new { success = true });
            }
            return Json(new { success = false });
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
    }
}