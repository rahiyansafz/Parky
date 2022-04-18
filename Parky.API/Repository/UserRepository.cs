using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parky.API.Data;
using Parky.API.Models;
using Parky.API.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Parky.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly AppSettings _appSettings;

    public UserRepository(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appsettings)
    {
        _applicationDbContext = applicationDbContext;
        _appSettings = appsettings.Value;
    }

    public User Authenticate(string username, string password)
    {
        var user = _applicationDbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

        //user not found
        if (user is null)
        {
            return null!;
        }

        //if user was found generate JWT Token
        JwtSecurityTokenHandler tokenHandler = new();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials
                            (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);
        user.Password = "";
        return user;
    }

    public bool IsUniqueUser(string username)
    {
        var user = _applicationDbContext.Users.SingleOrDefault(x => x.Username == username);

        // return null if user not found
        if (user is null)
            return true;

        return false;
    }

    public User Register(string username, string password)
    {
        User userObj = new()
        {
            Username = username,
            Password = password,
            Role = "Admin"
        };

        _applicationDbContext.Users.Add(userObj);
        _applicationDbContext.SaveChanges();
        userObj.Password = "";
        return userObj;
    }
}
