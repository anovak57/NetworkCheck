namespace NetworkSpeedTest.Services
{
    public class FileService
    {
        public void SaveResultsToFile(string filePath, string result)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(result);
            }
            Console.WriteLine($"Network parameters saved to {filePath}");
        }
    }
}
