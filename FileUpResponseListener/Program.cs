using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Relay;
using System.Net;
using System.Threading;
using System.Collections.Specialized;

namespace FileUpResponseListener
{
    class Program
    {
        // replace {RelayNamespace} with the name of your namespace
        private const string RelayNamespace = "FileUploadResponse.servicebus.windows.net";

        // replace {HybridConnectionName} with the name of your hybrid connection
        private const string ConnectionName = "hcuploadresponse";

        // replace {SAKKeyName} with the name of your Shared Access Policies key, which is RootManageSharedAccessKey by default
        private const string KeyName = "RootManageSharedAccessKey";

        // replace {SASKey} with the primary key of the namespace you saved earlier
        private const string Key = "X4Z66NOhrBQYvOoEPH5ZaO+opci03vaL0wPSW1NoU6M=";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var cts = new CancellationTokenSource();

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            var listener = new HybridConnectionListener(new Uri(string.Format("sb://{0}/{1}", RelayNamespace, ConnectionName)), tokenProvider);

            // Subscribe to the status events.
            listener.Connecting += (o, e) => { Console.WriteLine("Connecting"); };
            listener.Offline += (o, e) => { Console.WriteLine("Offline"); };
            listener.Online += (o, e) => { Console.WriteLine("Online"); };

            // Provide an HTTP request handler
            listener.RequestHandler = (context) =>
            {
                // Do something with context.Request.Url, HttpMethod, Headers, InputStream...
                context.Response.StatusCode = HttpStatusCode.OK;
                context.Response.StatusDescription = "OK, This is pretty neat";

                //send the data from the querystring to the function that will update the file information in the database
                string querystring = context.Request.Url.ToString();
                NameValueCollection qscoll = System.Web.HttpUtility.ParseQueryString(querystring);
                updatefile(qscoll);

                // The context MUST be closed here
                context.Response.Close();
            };

            // Opening the listener establishes the control channel to
            // the Azure Relay service. The control channel is continuously 
            // maintained, and is reestablished when connectivity is disrupted.
            await listener.OpenAsync();
            Console.WriteLine("Server listening");

            // Start a new thread that will continuously read the console.
            await Console.In.ReadLineAsync();

            // Close the listener after you exit the processing loop.
            await listener.CloseAsync();
        }

        private static void updatefile(NameValueCollection qsvals)
        {

            StringBuilder sb = new StringBuilder();

            foreach (String s in qsvals.AllKeys)
            {
               // s + " - " + qsvals[s]);
            }

        }

        
        //pulled this code from PMAPICommuncationService.cs in the FiletoPdfConversionService application

        public static void SendConversionStatusThroughApi(ConvertedFileCreateDto fileConversion)
        {
            var content = Helpers.SerializeToJson(fileConversion);
            
            var url = $"{ApiUri}v2/events/{EventName}/FileConversion/FileConversionComplete/{ApiKey}";

            try
            {
                var httpClient = new FreemanHttpClient();

                HttpContent myContent = new StringContent(content);
                myContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var request = httpClient.PostAsync(url, myContent);
                var response = request.Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _log.Error($"{nameof(PmApiComunicationService)} There was an error sending conversion information to the API. " +
                          $"The Conversion object is in JSON bellow. {Environment.NewLine}" +
                          $"{content}{Environment.NewLine}" +
                          $"Exception Information Bellow {Helpers.ExceptionInformationToString(ex)}");
            }

        }
    }
}
