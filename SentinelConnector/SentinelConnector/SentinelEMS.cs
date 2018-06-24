using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

namespace SentinelConnector
{
    public class SentinelEMSClass
    {
        private string emsUrl;
        private string connectionType;
        private string hostName;
        private string port;
        private string startDir;
        private string emsVersion;
        private string requestType;

        public SentinelEMSClass(string url)
        {
            string tmpUrl = url.Replace(":", "/");
            tmpUrl = tmpUrl.Replace("///", "/");
            string[] tmpMass = tmpUrl.Split('/');

            connectionType = tmpMass[0];
            hostName = tmpMass[1];
            port = tmpMass[2];
            startDir = tmpMass[3];
            emsVersion = tmpMass[4];
            requestType = tmpMass[5];

            emsUrl = connectionType + "://" + hostName + ":" + port + "/" + startDir + "/" + emsVersion + "/" + requestType + "/";
        }

        public string SayHello()
        {
            return "Hi Man!";
        }

        private string GetRequest(string rType, string rString, KeyValuePair<string, string> rData, string rContent = "") 
        {
            string fullRequestUrl = urlBuilder(rString);
            HttpClient request = new HttpClient();
            HttpResponseMessage response;
            string responseStr = "";

            switch (rType)
            {
                case "GET":
                    break;

                case "PUT":
                    break;

                case "POST":
                    try
                    {
                        var content = new FormUrlEncodedContent(new[] { rData });
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.StatusCode.ToString();
                    } catch (System.AggregateException e) {
                        responseStr = e.InnerException.InnerException.Message;
                    } catch (HttpRequestException hE) {
                        responseStr = hE.Message;
                    }
                    break;

                case "DELETE":
                    break;

                default:
                    // Передали в качестве типа запроса что-то невразумительное
                    break;
            }

            return responseStr;
        }

        public string LoginByPK(string productKeyString) 
        {
            var loginParams = new KeyValuePair<string, string>("productKey", productKeyString);

            return GetRequest("POST", "loginByProductKey.ws", loginParams);
        }

        private string urlBuilder(string reqSubStr) 
        {
            string fullUrl = "";

            fullUrl = emsUrl + reqSubStr;

            return fullUrl;
        }
    }
}
