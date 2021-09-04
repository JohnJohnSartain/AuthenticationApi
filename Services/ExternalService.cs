using System.Collections.Generic;
using Http;
using Sartain_Studios_Common.SharedEntities;
using SartainStudios.Token;
using SharedModels;

namespace Services;

public interface IExternalService<TEntity> { }

public class ExternalService<TEntity> : IExternalService<TEntity>
{
    protected readonly UserModel _emptyUserModelWithServiceRole = new() { Roles = new string[] { Role.Service } };
    protected readonly IAutoWrapperHttp<TEntity> _httpRequest;
    protected readonly IToken _token;

    public ExternalService(IAutoWrapperHttp<TEntity> httpClientWrapper, IToken token)
    {
        _httpRequest = httpClientWrapper;
        _token = token;
    }

    protected string ServiceAccountToken => _token.GenerateToken(_emptyUserModelWithServiceRole);
}