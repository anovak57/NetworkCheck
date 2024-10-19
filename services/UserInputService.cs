namespace NetworkSpeedTest.Services
{
    public class UserInputService
    {
        public string GetValidInput(string prompt)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(input));
            return input;
        }
    }
}
