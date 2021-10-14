using Http;
using SartainStudios.SharedModels.Users;
using SartainStudios.Token;

namespace Services;

public interface IUserService
{
    Task<bool> AreCredentialsValid(UserModel userModel, string token = null);
    Task<string> GetUserId(string username, string token = null);
    Task<UserModel> GetUserInformation(string userId, string token = null);
    Task<string> LogUserAuthentication(string userId, string token = null);
}

public class UserService : ExternalService<UserModel>, IUserService
{
    public UserService(IAutoWrapperHttp<UserModel> httpClientWrapper, IToken token) : base(httpClientWrapper,
        token)
    { }

    public async Task<bool> AreCredentialsValid(UserModel userModel, string token = null) =>
        bool.Parse((await _httpRequest.PostWithResultAsync("User/Credentials/Valid", userModel,
            token ?? ServiceAccountToken)).Result);

    public async Task<string> GetUserId(string username, string token = null) =>
        (await _httpRequest.GetAsync<string>("User/Username", username, token ?? ServiceAccountToken)).Result;

    public async Task<UserModel> GetUserInformation(string userId, string token = null) =>
        (await _httpRequest.GetAsync("User", userId, token ?? ServiceAccountToken)).Result;

    public async Task<string> LogUserAuthentication(string userId, string token = null)
    {
        var userModel = await GetUserInformation(userId, token);

        TrackNewAuthenticationHistory(userModel);

        var result = await UpdateUser(userModel, token);

        return result;
    }

    private async Task<string> UpdateUser(UserModel userModel, string token = null) =>
        (await _httpRequest.PatchWithResultAsync("User", userModel, token ?? ServiceAccountToken)).Result;

    private static void TrackNewAuthenticationHistory(UserModel userModel)
    {
        if (userModel.AuthenticationHistory != null)
        {
            var authenticationHistory = userModel.AuthenticationHistory;
            Array.Resize(ref authenticationHistory, authenticationHistory.Length + 1);
            authenticationHistory[^1] = DateTime.Now;
            userModel.AuthenticationHistory = authenticationHistory;
        }
    }
}