using Microsoft.EntityFrameworkCore;
using Parky.API.Data;
using Parky.API.Models;
using Parky.API.Repository.IRepository;

namespace Parky.API.Repository;

public class TrailRepository : ITrailRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TrailRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public bool CreateTrail(Trail trail)
    {
        _applicationDbContext.Trails.Add(trail);
        return Save();
    }

    public bool DeleteTrail(Trail trail)
    {
        _applicationDbContext.Trails.Remove(trail);
        return Save();
    }

    public Trail GetTrail(int trailId)
    {
        return _applicationDbContext.Trails.Include(c => c.NationalPark).FirstOrDefault(a => a.Id == trailId)!;
    }

    public ICollection<Trail> GetTrails()
    {
        return _applicationDbContext.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
    }

    public bool TrailExists(string name)
    {
        bool value = _applicationDbContext.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        return value;
    }

    public bool TrailExists(int id)
    {
        return _applicationDbContext.Trails.Any(a => a.Id == id);
    }

    public bool Save()
    {
        return _applicationDbContext.SaveChanges() >= 0 ? true : false;
    }

    public bool UpdateTrail(Trail trail)
    {
        _applicationDbContext.Trails.Update(trail);
        return Save();
    }

    public ICollection<Trail> GetTrailsInNationalPark(int npId)
    {
        return _applicationDbContext.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
    }
}
