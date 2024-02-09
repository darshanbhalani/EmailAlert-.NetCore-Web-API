using EmailAlert.Models;

namespace EmailAlert.Services
{
    public interface EmailAlertService
    {
        void SendEmail(EmailModel request);
    }
}
