using Microsoft.EntityFrameworkCore;
using Parky.API.Models;

namespace Parky.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<NationalPark> NationalParks { get; set; } = null!;
    public DbSet<Trail> Trails { get; set; } = null!;

}
