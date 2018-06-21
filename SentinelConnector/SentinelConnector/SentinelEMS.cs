using System;

namespace SentinelConnector
{
    public class SentinelEMSClass
    {
        private string emsUrl;
        private string hostName;
        private string port;
        private string startDir;
        private string emsVersion;
        private string requestType;

        public SentinelEMSClass(string url)
        {
            int n = url.IndexOf("//");
            string tmpUrl = url.Remove(0, n + 2);
            string[] tmpMass = tmpUrl.Split(new char[] { ':', '/' });
            emsUrl = url;
            hostName = tmpMass[0];
            port = tmpMass[1];
            startDir = tmpMass[2];
            emsVersion = tmpMass[3];
            requestType = tmpMass[4];
        }

        public string SayHello() 
        {
            return "Hi Man!";
        }

        public string GetRequest(string requestType, string requestString) 
        {

            return "";
        }
    }
}
