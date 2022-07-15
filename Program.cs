using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.IO.Compression;

namespace DepotDLGUI_cs
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.Write("Automatic installer for DepotDownloader\n");
            await DLLatestURL();
        }

        private static async Task DLLatestURL()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.github.com/repos/SteamRE/DepotDownloader/releases/latest");

            var msg = await stringTask;

            Console.WriteLine("Downloading and extracting... May take a while...");

            //convert msg into json 
            var json = JObject.Parse(msg);
            var url = json["assets"][0]["browser_download_url"];

            if (File.Exists("./DepotDownloader/DepotDownloader.dll")) 
            {
                //for loop for every file in ./DepotDownloader/
                foreach (string file in Directory.GetFiles("./DepotDownloader/"))
                {
                    //delete file
                    File.Delete(file);
                }
            }

            //download and extract url to ./DepotDownloader/
            var client2 = new WebClient();
            client2.DownloadFile(url.ToString(), "./DepotDownloader/DepotDownloader.zip");
            ZipFile.ExtractToDirectory("./DepotDownloader/DepotDownloader.zip", "./DepotDownloader/");
            File.Delete("./DepotDownloader/DepotDownloader.zip");
            Console.WriteLine("Downloaded and extracted to ./DepotDownloader/");

            //get user input
            Console.WriteLine("If the following does not work, you may need to disable steam guard.");
            Console.WriteLine("Your Steam username:");
            string username = Console.ReadLine();
            Console.WriteLine("Your Steam password:");
            string password = Console.ReadLine();

            //run DepotDownloader.exe with the username and password as arguements
            //string depotdlcommand = $"dotnet DepotDownloader/DepotDownloader.dll -app 413150 -depot 413151 -manifest 7802000804251603756 -username {username} -password {password}";

            Process.Start("dotnet", $" DepotDownloader/DepotDownloader.dll -app 413150 -depot 413151 -manifest 7802000804251603756 -username {username} -password {password}").WaitForExit();

        }

    }
}