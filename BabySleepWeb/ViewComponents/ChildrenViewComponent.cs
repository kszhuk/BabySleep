using BabySleep.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
            
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Guid userGuid;
            if(!Guid.TryParse(userId, out userGuid))
            {
                return View("Index", new List<SelectListItem>());
            }

            var children = _service.GetChildren(new Guid(userId));

            foreach (var child in children)
            {
                childrenListItem.Add(new SelectListItem(child.Name, child.ChildGuid.ToString()));
            }

            return View("Index", childrenListItem);
        }
    }
}
