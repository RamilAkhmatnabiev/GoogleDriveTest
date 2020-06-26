using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleDriveTest
{
    class Program
    {
        private static string[] Scopes = { DriveService.Scope.Drive };
        private static string ApplicationName = "myTestApp";
        static void Main(string[] args)
        {
            UserCredential credential = GetUserCredential();

            DriveService service = GetDriveService(credential); //получаем Driveservice

            IList<Google.Apis.Drive.v3.Data.File> files = service.Files.List().Execute().Item; // получаем масси файлов File - это класс google Api Drive
            foreach (var file in files)
            {
                Console.WriteLine(file.Id);
            }
        }
        private static UserCredential GetUserCredential() // получаем credentials
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                creadPath = Path.Combine(creadPath, "driveApiCredentials", "drive-credentials.json"); // записываем авторизацию в моих доках в json

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "User",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
            }
        }
        private static DriveService GetDriveService(UserCredential credential)
        {
            return new DriveService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                }
                );
        }
    }
}
