using BabySleep.Application.Interfaces;
using BabySleep.Resources.Resx;
using BabySleepWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BabySleepWeb.Pages
{
    [AllowAnonymous]
    public class ContactModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;
        private readonly ISmtpMailService _mailService;

        public ContactModel(ILogger<ContactModel> logger, ISmtpMailService mailService)
        {
            _mailService = mailService;
            _logger = logger;
        }

        [BindProperty]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostSendEmail([FromBody] Message message)
        {
            try
            {
                _mailService.Send(new BabySleep.Application.DTO.EmailMessageDto()
                {
                    Body = message.Body,
                    Subject = message.Subject,
                    Email = message.From
                });
                return new JsonResult(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error sending email from {0}. Error: {1}", Message.From, ex.Message));
            }

            return new JsonResult(false);
        }
    }
}
