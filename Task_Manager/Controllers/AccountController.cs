using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Task_Manager.Core.Abstract.Token;
using Task_Manager.DTOs;
using Task_Manager.Infrastructure.Models;



[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    readonly ITokenHandler _tokenHandler;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { FirstName = model.FirstName,UserName=model.FirstName,LastName=model.LastName, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(); 
            }

       
        }

        return BadRequest(model);
    }

 

    [HttpPost("Login")]
    public async Task<Token> Login(string UserNameOrEmail, string Password)
    {
        User user = await _userManager.FindByNameAsync(UserNameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(UserNameOrEmail);
        }
        if (user == null)
        {
            throw new Exception("Kullanıcı adı veya şifre hatalı");
        }
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, Password, false);

        if (result.Succeeded)
        {
            Token token = _tokenHandler.CreateAccessToken(60, user);

            return token;
        }
        else
        {
            throw new Exception("hata oluştu");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {

        try
        {
            await _signInManager.SignOutAsync();
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);  
            
        }

      
    }
}
