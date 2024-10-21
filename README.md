# NetworkCheck

NetworkCheck is a simple command-line application that allows users to measure network parameters such as ping, download speed, and upload speed. The application provides options to either save the network test results to a file or send them via email using the SendGrid API. Additionally, users can choose to run the test once or continuously over a specified period with an interval between each test.

## Features

- **Ping Measurement**: Measures the round-trip time to a specified address (default: google.com).
- **Download Speed Test**: Measures the download speed by downloading a file from the internet.
- **Upload Speed Test**: Measures the upload speed by posting data to a test server.
- **Save to File**: Allows the user to save the test results to a file.
- **Send via Email**: Sends the test results to an email address using the SendGrid service.
- **Continuous Testing**: Option to run the test at specified intervals for a given duration.

## Prerequisites

To run this application, you need the following:

1. [.NET Core SDK](https://dotnet.microsoft.com/download)
2. [SendGrid API Key](https://sendgrid.com/) -- Here you will also need to create a sender and use that sender as senderEmail and senderName values in the next step. *This is only needed for email sending functionality, if you want to use the app to save reuslts to a file you can do that without this step*.
3. Basic configuration settings in a `appsettings.Development.json` file

### appsettings.Development.json

```json
{
  "SendGrid": {
    "apiKey": "your-sendgrid-api-key",
    "senderEmail": "your-email@example.com",
    "senderName": "Your Name"
  },
  "Settings": {
    "filePath": "network_test.txt"
  }
}
