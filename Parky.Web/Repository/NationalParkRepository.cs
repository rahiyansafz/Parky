using Parky.Web.Models;
using Parky.Web.Repository.IRepository;

namespace Parky.Web.Repository;

public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
    {
        _clientFactory = clientFactory;
    }
}
