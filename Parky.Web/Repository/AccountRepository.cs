using Newtonsoft.Json;
using Parky.Web.Models;
using Parky.Web.Repository.IRepository;
using System.Text;

namespace Parky.Web.Repository;

public class AccountRepository : Repository<User>, IAccountRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<User> LoginAsync(string url, User objToCreate)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (objToCreate is not null)
        {
            request.Content = new StringContent(
                JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
        }
        else
        {
            return new User();
        }

        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(jsonString)!;
        }
        else
        {
            return new User();
        }
    }

    public async Task<bool> RegisterAsync(string url, User objToCreate)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (objToCreate is not null)
        {
            request.Content = new StringContent(
                JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
        }
        else
        {
            return false;
        }

        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
