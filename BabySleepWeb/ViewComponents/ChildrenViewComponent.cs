using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleepWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace BabySleepWeb.ViewComponents
{
    [ViewComponent(Name = "Children")]
    public class ChildrenViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IChildrenHelper _childrenHelper;

        public ChildrenViewComponent(IMemoryCache memoryCache, IChildrenHelper childrenHelper)
        {
            _memoryCache = memoryCache;
            _childrenHelper = childrenHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var childrenListItem = new List<SelectListItem>();

            List<ChildDto>? children;
            if (!_memoryCache.TryGetValue(CacheKeys.Children, out children))
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _childrenHelper.LoadChildren(userId);
            }

            Guid currentChildGuid;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out currentChildGuid);

            if (children != null)
            {
                foreach (var child in children)
                {
                    childrenListItem.Add(new SelectListItem(child.Name, child.ChildGuid.ToString(), child.ChildGuid == currentChildGuid));
                }
            }

            return View("Index", childrenListItem);
        }
    }
}
