namespace Parky.Web.Models.ViewModel;

public class ViewModel
{
    public IEnumerable<NationalPark> NationalParkList { get; set; } = null!;
    public IEnumerable<Trail> TrailList { get; set; } = null!;
}
