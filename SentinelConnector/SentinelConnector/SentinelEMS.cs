using System;
using System.Net.Http;

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

        public string GetRequest(string requestType, string requestString, object requestData) 
        {
            string fullRequestUrl = urlBuilder(requestString);


            switch (requestType) {
                case "GET":
                    break;

                case "PUT":
                    break;

                case "POST":
                    break;

                case "DELETE":
                    break;

                default:
                    // Передали в качестве типа запроса что-то невразумительное
                    break;
            }

            return "Hey you tryed do HTTP request! Good boy! =)";
        }

        public string LoginByPK(string productKeyString) 
        {
            string requestResponse = "";
            requestResponse = GetRequest("POST", "loginByProductKey.ws", productKeyString);

            return requestResponse;
        }

        private string urlBuilder(string reqSubStr) 
        {
            string fullUrl = "";

            fullUrl = emsUrl + reqSubStr;

            return fullUrl;
        }
    }
}
