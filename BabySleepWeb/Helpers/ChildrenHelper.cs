using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BabySleepWeb.Helpers
{
    public class ChildrenHelper: IChildrenHelper
    {
        private readonly IChildService _service;
        private readonly IMemoryCache _memoryCache;

        public ChildrenHelper(IChildService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }

        public void LoadChildren(string userGuid)
        {
            if (!_memoryCache.TryGetValue(CacheKeys.Children, out _))
            {
                var children = _service.GetChildren(new Guid(userGuid)).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(CacheKeys.Children, children, cacheEntryOptions);

                if (children.Any())
                {
                    _memoryCache.Set(CacheKeys.CurrentChildGuid, children.First().ChildGuid);
                }
            }
        }
    }
}
