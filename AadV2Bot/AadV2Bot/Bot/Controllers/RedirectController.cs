using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Microsoft.Bot.Sample.AadV2Bot.Controllers
{
    public class RedirectController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Redirection()
        {
            //get the dictionary key and entry
            var guid = Request.RequestUri.PathAndQuery;
            var url = CustomCode.RedirectDictionary[guid];

            //make our redirect reponse as a 307 redirect 
            //NOTE: 302 redirects will NOT work for twilio
            var response = Request.CreateResponse(HttpStatusCode.TemporaryRedirect);
            response.Headers.Location = new Uri(url);
            return response;
        }
    }
}