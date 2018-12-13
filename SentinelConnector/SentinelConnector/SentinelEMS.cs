using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace SentinelConnector
{
    public class SentinelEMSClass
    {
        private struct currentRequest
        {
            public string key;
            public string value;
        }

        private string emsUrl;
        private string connectionType;
        private string hostName;
        private string port;
        private string startDir;
        private string emsVersion;
        private string requestType;
        private currentRequest cRequest;

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

        private string UrlBuilder(string reqSubStr) 
        {
            string fullUrl = "";

            fullUrl = emsUrl + reqSubStr;

            return fullUrl;
        }

        public string GetRequest(string rString, KeyValuePair<string, string> rData = new KeyValuePair<string, string>())
        {
            string fullRequestUrl = UrlBuilder(rString);
            var patterns = new[] { 
                @"login.ws",
                @"loginByProductKey.ws",
                @"activation/target.ws",
                @"productKey/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}.ws",
                @"productKey/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}/activation.ws"
            };
            Regex regex;
            cRequest = new currentRequest();

            foreach (string p in patterns) {
                regex = new Regex(p);

                if (regex.IsMatch(rString)) {
                    if (p.Contains("activation.ws")) {
                        cRequest.key = "activation.ws";
                        cRequest.value = rString.Split('/')[1];
                    } else if (p.Contains("target.ws")) {
                        cRequest.key = "target.ws";
                        cRequest.value = (rString.Contains("/")) ? rString.Split('/')[1] : rString;
                    } else {
                        cRequest.key = (p.Contains("/")) ? p.Split('/')[0] : p;
                        cRequest.value = (rString.Contains("/")) ? rString.Split('/')[1] : rString;
                    }
                    break;
                }
            }

            HttpClient request = new HttpClient();
            HttpResponseMessage response;
            string responseStr = "Error: Incorrect request... | ";

            switch (cRequest.key)
            {
                case "login.ws":
                    try
                    {
                        var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (System.AggregateException e)
                    {
                        responseStr += e.InnerException.InnerException.Message;
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStr += hE.Message;
                    }
                    break;

                case "loginByProductKey.ws":
                    try
                    {
                        var content = new FormUrlEncodedContent(new[] { rData });
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {

                        responseStr += e.InnerException.InnerException.Message;

                    }
                    catch (HttpRequestException hE)
                    {
                        responseStr += hE.Message;
                    }
                    break;

                case "productKey":
                    try
                    {
                        var content = new FormUrlEncodedContent(new[] { rData });
                        response = request.PostAsync(UrlBuilder("loginByProductKey.ws"), content).Result;
                        responseStr = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {
                        responseStr += e.InnerException.InnerException.Message;
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStr += hE.Message;
                    }

                    if (responseStr == "OK")
                    {
                        try
                        {
                            response = request.GetAsync(fullRequestUrl).Result;
                            responseStr = response.Content.ReadAsStringAsync().Result;

                            if (response.StatusCode.ToString() == "OK")
                            {
                                XDocument tmpPKInfo = XDocument.Parse(responseStr);
                                string tmpResponseStr = "";

                                if (!string.IsNullOrEmpty(tmpPKInfo.Root.Element("customerId").Value))
                                {
                                    try
                                    {
                                        response = request.GetAsync(UrlBuilder("customer/" + tmpPKInfo.Root.Element("customerId").Value + ".ws")).Result;
                                        tmpResponseStr = response.Content.ReadAsStringAsync().Result;
                                    }
                                    catch (System.AggregateException e)
                                    {
                                        tmpResponseStr += e.InnerException.InnerException.Message + " | in get info request about customer ID after login by PK.";
                                    }
                                    catch (HttpRequestException hE)
                                    {
                                        tmpResponseStr += hE.Message + " | in get info request about customer ID after login by PK.";
                                    }

                                    if (response.StatusCode.ToString() == "OK") {
                                        XDocument tmpCustomerInfo = XDocument.Parse(tmpResponseStr);

                                        responseStr = responseStr.Replace("<customerId>" + tmpPKInfo.Root.Element("customerId").Value + "</customerId>", "<customerId>" + tmpCustomerInfo.Root.Element("defaultContact").Element("emailId").Value + "</customerId>");
                                    }
                                }
                            }
                        }
                        catch (System.AggregateException e)
                        {
                            responseStr += e.InnerException.InnerException.Message + " | in get info request after login by PK.";
                        }
                        catch (HttpRequestException hE)
                        {
                            responseStr += hE.Message + " | in get info request after login by PK.";
                        }
                    }
                    else
                    {
                        responseStr += " | in request login by PK.";
                    }
                    break;

                case "activation.ws":
                    try
                    {
                        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("productKey", cRequest.value) });
                        response = request.PostAsync(UrlBuilder("loginByProductKey.ws"), content).Result;
                        responseStr = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {
                        responseStr += e.InnerException.InnerException.Message;
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStr += hE.Message;
                    }

                    if (responseStr == "OK")
                    {
                        try
                        {
                            var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                            response = request.PostAsync(fullRequestUrl, content).Result;
                            responseStr = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (System.AggregateException e)
                        {
                            responseStr += e.InnerException.InnerException.Message + " | in activate request after login by PK.";
                        }
                        catch (HttpRequestException hE)
                        {
                            responseStr += hE.Message + " | in activate request after login by PK.";
                        }
                    }
                    else
                    {
                        responseStr += " | in request login by PK.";
                    }
                    break;

                case "target.ws":
                    try {
                        var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (System.AggregateException e)
                    {
                        responseStr += e.InnerException.InnerException.Message + " | in get update by C2V request.";
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStr += hE.Message + " | in get update by C2V request.";
                    }
                    break;

                default:
                    // Передали в качестве запроса что-то невразумительное
                    responseStr += " | Something whrong...";
                    break;
            }

            return responseStr;
        }
    }
}
