using Microsoft.Extensions.Options;
using Parky.API.Data;
using Parky.API.Models;
using Parky.API.Repository.IRepository;

namespace Parky.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    //private readonly AppSettings _appSettings;

    public UserRepository(ApplicationDbContext applicationDbContext) // , IOptions<AppSettings> appsettings
    {
        _applicationDbContext = applicationDbContext;
        //_appSettings = appsettings.Value;
    }

    public User Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public bool IsUniqueUser(string username)
    {
        throw new NotImplementedException();
    }

    public User Register(string username, string password)
    {
        throw new NotImplementedException();
    }
}
