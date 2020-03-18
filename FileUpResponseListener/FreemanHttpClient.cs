using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpResponseListener
{
    class FreemanHttpClient : HttpClient
    {
        public FreemanHttpClient() : base(new WebRequestHandler
        {
            ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
        })
        { }
    }
}
