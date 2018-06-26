using System;
using System.Collections.Generic;
using SentinelConnector;

namespace TestConsoleUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.3:8080/ems/v78/ws"); // right server address for test
            //SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.254:8080/ems/v78/ws"); // wrong server address for test
            //SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.3:8080/emsA/v78/ws"); // wrong link for test

            string pKey = "c03e2c49-225e-4984-b75f-d8ffe5c84a2b"; // right PK for test
            //string pKey = "c03e2c49-225e-4984-b75f-d8ffe5c84a2b_e"; // wrong PK for test

            // 0071eb86 - 911c - 40aa - a07b - 552d23ce37a1
            // c03e2c49 - 225e - 4984 - b75f - d8ffe5c84a2b
            //     8    -   4  -  4   -  4   -      12 
            //    {8}   -  {4} - {4}  - {4}  -     {12}
            //       \w{8}-\w{4}-\w{4}-\w{4}-\w{12}

            string authXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                             "<authenticationDetail>" +
                                "<userName>admin</userName>" +
                                "<password>admin</password>" +
                             "</authenticationDetail>";

            string actXml = "<activation>" +
                               "<activationInput>" +
                                  "<activationAttribute>" + 
                                     "<attributeValue>" +
                                        "<![CDATA[<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                           "<hasp_info>" +
                                              "<host_fingerprint>MXhJSYmNpjnybwMQmXCmqPiNEj1XJk6XGATACNudoCILIEUrZc/z14yWKbGXxi6iKDbiEQtnVdXiVHoOE1XumZZjF+Qy</host_fingerprint>" +
                                           "</hasp_info>]]>" +
                                        "</attributeValue>" + 
                                     "<attributeName>C2V</attributeName>" + 
                                  "</activationAttribute>" + 
                                  "<comments>New Comments Added By Web Services</comments>" + 
                               "</activationInput>" +
                            "</activation>";

            string targetXml = "";

            string action = "";
            Console.WriteLine("Input: " + Environment.NewLine +
                              "1 - for login by PK request;" + Environment.NewLine +
                              "2 - for send ativation request;" + Environment.NewLine +
                              "3 - for get info about ProductKey after login;" + Environment.NewLine +
                              "4 - for ISV login request;" + Environment.NewLine +
                              "5 - for update request via C2V;" + Environment.NewLine +
                              "0 - for Exit." + Environment.NewLine);

            action = Console.ReadLine();
            Console.WriteLine("/-------------------------/" + Environment.NewLine);

            switch (action) {
                case "1":
                    Console.WriteLine("Let's try doing login by PK..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("loginByProductKey.ws", new KeyValuePair<string, string>("productKey", pKey)) + Environment.NewLine);
                    break;

                case "2":
                    Console.WriteLine("Let's try doing activation..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("productKey/" + pKey + "/activation.ws", new KeyValuePair<string, string>("activationXml", actXml)) + Environment.NewLine);
                    break;

                case "3":
                    Console.WriteLine("Let's try get info about PK..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("productKey/" + pKey + ".ws", new KeyValuePair<string, string>("productKey", pKey)) + Environment.NewLine);
                    break;

                case "4":
                    Console.WriteLine("Let's try doing ISV login..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("login.ws", new KeyValuePair<string, string>("authenticationDetail", authXml)) + Environment.NewLine);
                    break;

                case "5":
                    Console.WriteLine("Let's try get update via C2V..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("activation/target.ws", new KeyValuePair<string, string>("targetXml", targetXml)) + Environment.NewLine);
                    break;

                case "0":
                    Console.WriteLine("Let's try close application..." + Environment.NewLine);
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Input something wrong!" + Environment.NewLine);
                    break;
            }
        }
    }
}
