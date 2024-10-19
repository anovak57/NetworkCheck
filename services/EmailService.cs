using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetworkSpeedTest.Services
{
    public class EmailService
    {
        private readonly string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task SendEmailAsync(string email, string result)
        {
            var client = new SendGridClient(_apiKey);

            var from = new EmailAddress("networktestapp57@gmail.com", "Network Test");
            var subject = "Network Parameters Report";
            var to = new EmailAddress(email);

            var plainTextContent = $"Here is the network report:\n{result}";
            var htmlContent = $"<strong>Here is the network report:</strong><br>{result.Replace("\n", "<br>")}";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            Console.WriteLine($"Email sent: {response.StatusCode}");
        }
    }
}
