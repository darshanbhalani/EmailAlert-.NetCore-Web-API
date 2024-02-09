using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;
using EmailAlert.Services;

namespace EmailAlert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailAlertController : ControllerBase
    {
        private EmailAlertService _emailService;

        public EmailAlertController(EmailAlertService service) { 
            _emailService = service;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailModel request)
        {

            _emailService.SendEmail(request);

            return Ok();
        }
    }
}
