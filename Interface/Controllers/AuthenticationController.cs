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

    public AuthenticationController(IAuthenticationService authentication) => _authentication = authentication;

    [HttpPost("Token")]
    public async Task<ActionResult<TokenModel>> GetAuthenticationToken(UserModel userModel)
    {
       Log.LogInformation("GetAuthenticationToken", GetType().Name, nameof(GetAuthenticationToken), null);

        return await _authentication.GetAuthenticationTokenAsync(userModel);
    }
}