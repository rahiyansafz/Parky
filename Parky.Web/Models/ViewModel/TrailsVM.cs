using Microsoft.AspNetCore.Mvc.Rendering;

namespace Parky.Web.Models.ViewModel;

public class TrailsVM
{
    public IEnumerable<SelectListItem> NationalParkList { get; set; } = null!;
    public Trail Trail { get; set; } = null!;
}
