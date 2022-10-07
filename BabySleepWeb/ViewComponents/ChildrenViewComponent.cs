using BabySleep.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabySleepWeb.ViewComponents
{
    [ViewComponent(Name = "Children")]
    public class ChildrenViewComponent : ViewComponent
    {
        private readonly IChildService _service;

        public ChildrenViewComponent(IChildService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var childrenListItem = new List<SelectListItem>();

            var children = _service.GetChildren(new Guid("CDAFE18A-09E4-4AFF-9896-21A9DD17FC9F"));

            foreach (var child in children)
            {
                childrenListItem.Add(new SelectListItem(child.Name, child.ChildGuid.ToString()));
            }

            return View("Index", childrenListItem);
        }
    }
}
