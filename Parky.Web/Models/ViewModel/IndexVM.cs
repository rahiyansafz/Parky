namespace Parky.Web.Models.ViewModel;

public class IndexVM
{
    public IEnumerable<NationalPark> NationalParkList { get; set; } = null!;
    public IEnumerable<Trail> TrailList { get; set; } = null!;
}
