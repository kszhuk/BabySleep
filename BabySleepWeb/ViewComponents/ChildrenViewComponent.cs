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
        private readonly IChildService _service;
        private readonly IMemoryCache _memoryCache;

        public ChildrenViewComponent(IChildService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var childrenListItem = new List<SelectListItem>();

            var children = new List<ChildDto>();

            if (!_memoryCache.TryGetValue(CacheKeys.Children, out children))
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userId, out _))
                {
                    children = _service.GetChildren(new Guid(userId)).ToList();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    _memoryCache.Set(CacheKeys.Children, children, cacheEntryOptions);
                }
            }

            if (children != null)
            {
                foreach (var child in children)
                {
                    childrenListItem.Add(new SelectListItem(child.Name, child.ChildGuid.ToString()));
                }
            }

            return View("Index", childrenListItem);
        }
    }
}
