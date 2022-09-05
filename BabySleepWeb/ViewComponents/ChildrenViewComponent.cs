using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabySleepWeb.ViewComponents
{
    [ViewComponent(Name = "Children")]
    public class ChildrenViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var children = new List<SelectListItem>();

            return View("Index", children);
        }
    }
}
