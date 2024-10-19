using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetworkSpeedTest.Services
{
    public class NetworkService
    {
        public long MeasurePing(string address)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(address);

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Ping to {address}: {reply.RoundtripTime} ms");
                    return reply.RoundtripTime;
                }
                else
                {
                    Console.WriteLine($"Ping failed: {reply.Status}");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ping error: {ex.Message}");
                return -1;
            }
        }

        public async Task<double> MeasureDownloadSpeed()
        {
            HttpClient client = new HttpClient();
            Stopwatch stopwatch = new Stopwatch();
            string downloadUrl = "http://speedtest.tele2.net/50MB.zip";

            try
            {
                stopwatch.Start();
                var data = await client.GetByteArrayAsync(downloadUrl);
                stopwatch.Stop();

                double speed = data.Length / (stopwatch.Elapsed.TotalSeconds * 1024 * 1024);
                Console.WriteLine($"Download speed: {speed:F2} MB/s");
                return speed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Download error: {ex.Message}");
                return 0;
            }
        }

        public async Task<double> MeasureUploadSpeed()
        {
            HttpClient client = new HttpClient();
            Stopwatch stopwatch = new Stopwatch();
            string uploadUrl = "https://httpbin.org/post";
            try
            {
                byte[] data = new byte[5 * 1024 * 1024];
                var content = new ByteArrayContent(data);

                stopwatch.Start();
                await client.PostAsync(uploadUrl, content);
                stopwatch.Stop();

                double speed = data.Length / (stopwatch.Elapsed.TotalSeconds * 1024 * 1024);
                Console.WriteLine($"Upload speed: {speed:F2} MB/s");
                return speed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                return 0;
            }
        }
    }
}
