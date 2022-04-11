using Parky.Web.Models;
using Parky.Web.Repository.IRepository;

namespace Parky.Web.Repository;

public class TrailRepository : Repository<Trail>, ITrailRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
    {
        _clientFactory = clientFactory;
    }
}
