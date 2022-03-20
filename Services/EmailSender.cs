using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace netProject.Services{

    //Email class that inherits from IEmailSender
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }


        public AuthMessageSenderOptions Options { get; } 

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            //Check if API key is set
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception();
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        //Method for sending email
        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var sendGridClient = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("YOUR EMAIL ADRESS", "YOUR NAME"),
                Subject = subject,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));


            msg.SetClickTracking(false, false);
            var response = await sendGridClient.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Mail till {toEmail} är köad."
                                   : $"Misslyckades att maila {toEmail}");
        }
    }
}

