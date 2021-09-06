using Interface.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using SartainStudios.Token;
using Services;
using SharedModels;

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
        Log.LogInformation("GetAuthenticationToken", GetType().Name, nameof(GetAuthenticationToken), null);

        var token = await _authentication.GetAuthenticationTokenAsync(userModel);

        await _userService.LogUserAuthentication(await _userService.GetUserId(userModel.Username));

        return token;
    }
}