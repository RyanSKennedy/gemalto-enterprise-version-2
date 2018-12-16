using System;
using System.Net.Http;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace SentinelConnector
{
    public class RequestData 
    {
        public HttpClient httpClient;
        public HttpResponseMessage httpClientResponse;
        public string httpClientResponseStr;
        public string httpClientResponseStatus;

        public RequestData(HttpClient newClient = null, HttpResponseMessage newResponse = null, string newResponseStr = null, string newResponseStatus = null) 
        {
            httpClient = newClient;
            httpClientResponse = newResponse;
            httpClientResponseStr = newResponseStr;
            httpClientResponseStatus = newResponseStatus;
        }
    }

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

        public RequestData GetRequest(string rString, KeyValuePair<string, string> rData = new KeyValuePair<string, string>(), RequestData client = null)
        {
            string fullRequestUrl = UrlBuilder(rString);
            var patterns = new[] { 
                @"login.ws",
                @"loginByProductKey.ws",
                @"customer.ws",
                @"activation/target.ws",
                @"customer/[0-9]+.ws",
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

            RequestData tmpRes = new RequestData();

            HttpClient request;
            if (client != null) {
                request = client.httpClient;
            } else {
                request = new HttpClient();
            }

            HttpResponseMessage response = null;
            string responseStr = "";
            string responseStatus = "Error: Incorrect request... | ";

            switch (cRequest.key)
            {
                case "login.ws":
                    try
                    {
                        var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.Content.ReadAsStringAsync().Result;
                        responseStatus = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {
                        responseStatus += e.InnerException.InnerException.Message;
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStatus += hE.Message;
                    }
                    break;

                case "loginByProductKey.ws":
                    try
                    {
                        var content = new FormUrlEncodedContent(new[] { rData });
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.Content.ReadAsStringAsync().Result;
                        responseStatus = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {

                        responseStatus += e.InnerException.InnerException.Message;

                    }
                    catch (HttpRequestException hE)
                    {
                        responseStatus += hE.Message;
                    }
                    break;

                case "customer.ws":
                    if (client != null)
                    {
                        tmpRes = new RequestData(client.httpClient, client.httpClientResponse, client.httpClientResponseStr, client.httpClientResponseStatus);
                        request = tmpRes.httpClient;

                        if (tmpRes.httpClientResponseStatus == "OK")
                        {
                            try
                            {
                                var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                                response = request.PutAsync(fullRequestUrl, content).Result;
                                responseStr = response.Content.ReadAsStringAsync().Result;
                                responseStatus = response.StatusCode.ToString();
                            }
                            catch (System.AggregateException e)
                            {
                                responseStatus += e.InnerException.InnerException.Message + " | in customer create request by PK after login by PK.";
                            }
                            catch (HttpRequestException hE)
                            {
                                responseStatus += hE.Message + " | in customer create request by PK after login by PK.";
                            }
                        }
                        else
                        {
                            response = tmpRes.httpClientResponse;
                            responseStr = tmpRes.httpClientResponseStr;
                            responseStatus = tmpRes.httpClientResponseStatus;
                        }
                    }
                    else
                    {
                        responseStatus = "Not set HttpClient instance.";
                    }
                    break;

                case "productKey":
                    if (client != null)
                    {
                        tmpRes = new RequestData(client.httpClient, client.httpClientResponse, client.httpClientResponseStr, client.httpClientResponseStatus);
                        request = tmpRes.httpClient;

                        if (tmpRes.httpClientResponseStatus == "OK")
                        {
                            try
                            {
                                response = request.GetAsync(fullRequestUrl).Result;
                                responseStr = response.Content.ReadAsStringAsync().Result;
                                responseStatus = response.StatusCode.ToString();

                                if (responseStatus == "OK")
                                {
                                    XDocument tmpPKInfo = XDocument.Parse(responseStr);
                                    string tmpResponseStr = "";

                                    if (!string.IsNullOrEmpty(tmpPKInfo.Root.Element("customerId").Value))
                                    {
                                        tmpResponseStr = GetRequest("customer/" + tmpPKInfo.Root.Element("customerId").Value + ".ws", new KeyValuePair<string, string>(null, null), new RequestData(request, response, responseStr, responseStatus)).httpClientResponseStr;

                                        if (!string.IsNullOrEmpty(tmpResponseStr))
                                        {
                                            responseStr = responseStr.Replace("<customerId>" + tmpPKInfo.Root.Element("customerId").Value + "</customerId>", "<customerId>" + tmpPKInfo.Root.Element("customerId").Value + "</customerId>" + "<customerEmail>" + tmpResponseStr + "</customerEmail>");
                                        }
                                    }
                                }
                            }
                            catch (System.AggregateException e)
                            {
                                responseStatus += e.InnerException.InnerException.Message + " | in get info request after login by PK.";
                            }
                            catch (HttpRequestException hE)
                            {
                                responseStatus += hE.Message + " | in get info request after login by PK.";
                            }
                        }
                        else
                        {
                            response = tmpRes.httpClientResponse;
                            responseStr = tmpRes.httpClientResponseStr;
                            responseStatus = tmpRes.httpClientResponseStatus;
                        }
                    }
                    else
                    {
                        responseStatus = "Not set HttpClient instance.";
                    }
                    break;

                case "activation.ws":
                    if (client != null)
                    {
                        tmpRes = new RequestData(client.httpClient, client.httpClientResponse, client.httpClientResponseStr, client.httpClientResponseStatus);
                        request = tmpRes.httpClient;

                        if (tmpRes.httpClientResponseStatus == "OK")
                        {
                            try
                            {
                                var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                                response = request.PostAsync(fullRequestUrl, content).Result;
                                responseStr = response.Content.ReadAsStringAsync().Result;
                                responseStatus = response.StatusCode.ToString();
                            }
                            catch (System.AggregateException e)
                            {
                                responseStatus += e.InnerException.InnerException.Message + " | in activate request after login by PK.";
                            }
                            catch (HttpRequestException hE)
                            {
                                responseStatus += hE.Message + " | in activate request after login by PK.";
                            }
                        }
                        else
                        {
                            response = tmpRes.httpClientResponse;
                            responseStr = tmpRes.httpClientResponseStr;
                            responseStatus = tmpRes.httpClientResponseStatus;
                        }
                    }
                    else
                    {
                        responseStatus = "Not set HttpClient instance.";
                    }
                    break;

                case "target.ws":
                    try {
                        var content = new StringContent(rData.Value, Encoding.UTF8, "application/xml");
                        response = request.PostAsync(fullRequestUrl, content).Result;
                        responseStr = response.Content.ReadAsStringAsync().Result;
                        responseStatus = response.StatusCode.ToString();
                    }
                    catch (System.AggregateException e)
                    {
                        responseStatus += e.InnerException.InnerException.Message + " | in get update by C2V request.";
                    }
                    catch (HttpRequestException hE)
                    {
                        responseStatus += hE.Message + " | in get update by C2V request.";
                    }
                    break;

                case "customer":
                    if (client != null) {
                        tmpRes = new RequestData(client.httpClient, client.httpClientResponse, client.httpClientResponseStr, client.httpClientResponseStatus);
                        request = tmpRes.httpClient;

                        if (tmpRes.httpClientResponseStatus == "OK")
                        {
                            try
                            {
                                response = request.GetAsync(fullRequestUrl).Result;
                                responseStr = response.Content.ReadAsStringAsync().Result;
                                responseStatus = response.StatusCode.ToString();

                                if (responseStatus == "OK")
                                {
                                    XDocument tmpCustomerInfo = XDocument.Parse(responseStr);

                                    responseStr = tmpCustomerInfo.Root.Element("defaultContact").Element("emailId").Value;
                                }
                            }
                            catch (System.AggregateException e)
                            {
                                responseStatus += e.InnerException.InnerException.Message + " | in get info request about customer ID after login by PK.";
                            }
                            catch (HttpRequestException hE)
                            {
                                responseStatus += hE.Message + " | in get info request about customer ID after login by PK.";
                            }
                        }
                        else
                        {
                            response = tmpRes.httpClientResponse;
                            responseStr = tmpRes.httpClientResponseStr;
                            responseStatus = tmpRes.httpClientResponseStatus;
                        }
                    } else {
                        responseStatus = "Not set HttpClient instance.";
                    }
                    break;

                default:
                    // Передали в качестве запроса что-то невразумительное
                    responseStatus = "Something whrong...";
                    break;
            }

            return new RequestData(request, response, responseStr, responseStatus);
        }
    }
}
