using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EARTH.Jaguar.Security
{
    public class CorsHandler : DelegatingHandler
    {
        const string Origin = "Origin";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    return Task.Factory.StartNew<HttpResponseMessage>(() =>
                    {


                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        response.Headers.Add("Access-Control-Allow-Methods", "POST");
                        response.Headers.Add("Access-Control-Allow-Methods", "GET");
                        response.Headers.Add("Access-Control-Allow-Credentials", "true");
                        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");



                        return response;
                    }, cancellationToken);
                }
                else
                {
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = t.Result;

                        resp.Headers.Add("Access-Control-Allow-Origin", "*");
                        resp.Headers.Add("Access-Control-Allow-Methods", "POST");
                        resp.Headers.Add("Access-Control-Allow-Methods", "GET");
                        resp.Headers.Add("Access-Control-Allow-Credentials", "true");
                        resp.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
                        return resp;
                    });
                }
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}