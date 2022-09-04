using BabySleep.Resources.Resx;
using BabySleepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BabySleepWeb.ViewComponents
{
    [ViewComponent(Name = "Language")]
    public class LanguageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var isUkSelected = CultureInfo.DefaultThreadCurrentCulture != null && 
                CultureInfo.DefaultThreadCurrentCulture.Name == LanguageType.uk.ToString();

            var languages = new List<SelectListItem>()
            {
                new SelectListItem(){ Value = LanguageType.en.ToString(), Text = EditSettingsResources.English, Selected = !isUkSelected},
                new SelectListItem(){ Value = LanguageType.uk.ToString(), Text = EditSettingsResources.Ukrainian, Selected = isUkSelected}
            };

            return View("Index", languages);
        }
    }
}
