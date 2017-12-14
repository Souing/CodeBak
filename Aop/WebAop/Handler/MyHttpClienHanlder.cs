using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAop.Handle
{
    public class MyHttpClienHanlder// : HttpClientHandler
    {
//        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            //request.Headers.Add("Authorization", "Basic d2VsbG1lZDpXZWxsbWVkMDE=");
//            request.Headers.Add("Authorization", "Digest username=\"q\", realm=\"RealmOfBadri\", nonce=\"8a8b2651296a5be29819a265b32f8ef5\", uri=\"/api/employees/12345\", response=\"625ba685ec8ba41a1c9597b02151a50a\", qop=auth, nc=00000003, cnonce=\"74ceb92a93259e1d\"");
//            return base.SendAsync(request, cancellationToken);
//        }
    }
}