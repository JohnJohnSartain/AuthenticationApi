using Http;
using SartainStudios.Token;
using SharedModels;

namespace Services;

public interface IUserService
{
    Task<bool> AreCredentialsValid(UserModel userModel, string token = null);
    Task<string> GetUserId(string username, string token = null);
    Task<UserModel> GetUserInformation(string userId, string token = null);
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
}