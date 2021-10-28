using Interface.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using SartainStudios.SharedModels.Users;
using SartainStudios.Token;
using Services;

namespace Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authentication;
    private readonly IUserService _userService;

    public AuthenticationController(IAuthenticationService authentication, IUserService userService)
    {
        _authentication = authentication;
        _userService = userService;
    }

    [HttpPost("Token")]
    public async Task<ActionResult<TokenModel>> GetAuthenticationToken(UserModel userModel)
    {
        var token = await _authentication.GetAuthenticationTokenAsync(userModel);

        await _userService.LogUserAuthentication(await _userService.GetUserId(userModel.Username));

        return token;
    }
}