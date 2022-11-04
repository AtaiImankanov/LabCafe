using homework_64_Atai.Models;
using homework_64_Atai.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LabInsta.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)

        {
            
            _userManager = userManager;

            _signInManager = signInManager;
        }


        [HttpGet]

        public IActionResult Register()

        {
           

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)

        {

            if (ModelState.IsValid)

            {

                User user = new User
                {
                    Role="user",
                    Email = model.Email,
                    UserName = model.UserName,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                

                //Создание пользователя средствами Identity на основе объекта пользователя и его пароля

                //Возвращает результат выполнения в случае успеха впускаем пользователя в систему

                if (result.Succeeded)

                {
                    await _userManager.AddToRoleAsync(user,"user");
                    await _signInManager.SignInAsync(user, false);
                    

                    return RedirectToAction("Index", "Home");

                }

                foreach (var error in result.Errors)

                    ModelState.AddModelError(string.Empty, error.Description);

            }

            return View(model);
 
        }
        [HttpGet]

        public IActionResult Login(string returnUrl = null)

        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)

        {

            if (ModelState.IsValid)

            {
                Microsoft.AspNetCore.Identity.SignInResult result = null;
                User user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    result = await _signInManager.PasswordSignInAsync(

                        user,

                        model.Password,

                        model.RememberMe,

                        false

                        );
                    if (result.Succeeded)

                    {

                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))

                        {

                            return Redirect(model.ReturnUrl);

                        }



                        return RedirectToAction("Index", "Home");

                    }
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
                else { 
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            }

            return View(model);

        }



        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LogOff()

        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }
        public bool EmailValid(string Email)
        {
            foreach (var e in _userManager.Users)
            {
                if (e.Email == Email)
                {
                    return false;
                }
            }
            return true;
        }
        public bool NameValid(string UserName)
        {
            foreach (var e in _userManager.Users)
            {
                if (e.UserName == UserName)
                {
                    return false;
                }
            }
            return true;
        }
        public bool PhoneNumberValid(string PhoneNumber)
        {
            foreach (var e in _userManager.Users)
            {
                if (e.PhoneNumber == PhoneNumber)
                {
                    return false;
                }
            }
            return true;
        }
       
    }
}
