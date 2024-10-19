using NetworkSpeedTest.Services;
using Microsoft.Extensions.Configuration;

namespace NetworkSpeedTest
{
    class NetworkSpeedApp
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            IConfiguration config = builder.Build();

            var userInputService = new UserInputService();
            var networkService = new NetworkService();
            var fileService = new FileService();
            var emailService = new EmailService(config["SendGrid:apiKey"]);

            string outputChoice = userInputService.GetValidInput("Would you like to: \n1) Save the report to a file \n2) Send it to your email");

            string filePath = config["Settings:filePath"] ?? "network_test.txt";
            string email = "";
            string senderEmail = config["SendGrid:senderEmail"];
            string senderName = config["SendGrid:senderName"];

            if (outputChoice == "2")
            {
                email = userInputService.GetValidInput("Please enter your email address:");
            }

            string runChoice = userInputService.GetValidInput("Would you like to run this: \n1) Once \n2) Run it in the background");

            if (runChoice == "1")
            {
                await MeasureAndSaveResults(networkService, fileService, emailService,
                                            filePath, email, outputChoice,
                                            senderEmail, senderName);
            }
            else if (runChoice == "2")
            {
                Console.WriteLine("Please enter the duration (in minutes) you want this to run:");
                int durationMinutes = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Please enter the interval (in minutes) between each test:");
                int intervalMinutes = int.Parse(Console.ReadLine() ?? "0");

                DateTime endTime = DateTime.Now.AddMinutes(durationMinutes);
                List<string> allResults = new List<string>();

                while (DateTime.Now < endTime)
                {
                    string result = await MeasureAndSaveResults(networkService, fileService, emailService, 
                                                                filePath, email, outputChoice, senderEmail, 
                                                                senderName, false);
                    allResults.Add(result);
                    Console.WriteLine($"Waiting for {intervalMinutes} minutes...");
                    await Task.Delay(TimeSpan.FromMinutes(intervalMinutes));
                }

                if (outputChoice == "2")
                {
                    await emailService.SendEmailAsync(email, string.Join("\n", allResults), senderEmail, senderName);
                }
            }

            Console.WriteLine("Operation completed.");
        }

        static async Task<string> MeasureAndSaveResults(NetworkService networkService, FileService fileService, EmailService emailService,
                                                        string filePath, string email, string outputChoice,
                                                        string senderEmail, string senderName, bool sendImmediate = true)
        {
            string pingAddress = "google.com";
            long pingTime = networkService.MeasurePing(pingAddress);

            double downloadSpeed = await networkService.MeasureDownloadSpeed();
            double uploadSpeed = await networkService.MeasureUploadSpeed();

            DateTime currentTime = DateTime.Now;
            string result = $"Measurement at: {currentTime}\n" +
                            $"--------------------\n" +
                            $"Ping: {pingTime} ms\n" +
                            $"Download Speed: {downloadSpeed:F2} MB/s\n" +
                            $"Upload Speed: {uploadSpeed:F2} MB/s\n" +
                            "=====================\n";

            if (outputChoice == "1")
            {
                fileService.SaveResultsToFile(filePath, result);
            }
            else if (outputChoice == "2" && sendImmediate)
            {
                await emailService.SendEmailAsync(email, result, senderEmail, senderName);
            }

            return result;
        }
    }
}
