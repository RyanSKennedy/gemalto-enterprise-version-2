using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections.Generic;
using SentinelConnector;

namespace TestConsoleUtility
{
    class Program
    {
        public static XDocument loadedXml;

        static void Main(string[] args)
        {
            // Base EMS URL for my EM which hosted in Parallels on my Macbook Pro 
            SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.3:8080/ems/v78/ws"); // right server address for test
            //SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.254:8080/ems/v78/ws"); // wrong server address for test
            //SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.3:8080/emsA/v78/ws"); // wrong link for test

            // Defaulft PK from DEMOMA Batch code in EMS default data
            //string pKey = "c03e2c49-225e-4984-b75f-d8ffe5c84a2b"; // right PK for test REG IS NOT REQUIRED
            string pKey = "325c2607-e722-42c8-9d9f-5ab55c04213a"; // right PK for test REG IS MANDATORY
            //string pKey = "ea6b8253-78d4-44e8-9047-d6ac7ae41f7c"; // right PK for test REG IS DESIRED
            //string pKey = "c03e2c49-225e-4984-b75f-d8ffe5c84a2b_e"; // wrong PK for test

            // Pattern PK for Regex
            // 0071eb86 - 911c - 40aa - a07b - 552d23ce37a1
            // c03e2c49 - 225e - 4984 - b75f - d8ffe5c84a2b
            //     8    -   4  -  4   -  4   -      12 
            //    {8}   -  {4} - {4}  - {4}  -     {12}
            //       \w{8}-\w{4}-\w{4}-\w{4}-\w{12}

            // Default login and pass for ISV login to the EMS
            string authXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                             "<authenticationDetail>" +
                                "<userName>admin</userName>" +
                                "<password>admin</password>" +
                             "</authenticationDetail>";

            // Default XML for activation (from EMS Web Service Guide) - is incorrect!
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

            // C2V from DEMOMA Sentinel HL Max Micro Driverless with Key ID = 1709529435
            string targetXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<hasp_info>" +
                                    "<haspscope>" +
                                        "<hasp id=\"1709529435\">" +
                                            "<vendor id=\"37515\"/>" +
                                            "<update_counter>38</update_counter>" +
                                        "</hasp>" +
                                    "</haspscope>" +
                                    "<c2v>" +
                                        "YYIhUIADY3R2oQaABFsyNT2iCYABAIEBAIIBAKMVgAEAgRAAAAAAAAAAAAAAAAAA" +
                                        "AAAApoIhHYAEZeVZW6SCIROggdWgXoADBwIAgQEFggEAgwEAhAJEa4UBAoYCFwKH" +
                                        "AgEAiAgAANH/DAwMDIkBDYoBF4sBAIwEWzI1PI0DAJKLjgRl5VlbjwEmkBDnQ0E0" +
                                        "TAjNUD0AAAAAAAAAkQIENpICAAChBoABAYEBAaIcgAEBgQIFeIICBUaDARmEAQWF" +
                                        "AgQshgIBGocBAKMkgAEAgQIFLYIENa+ce4MENa+ce4QDAIMAhQRAy/TUhgRAy/TU" +
                                        "pCeAEFsw/FRbMhwXWzIjM1syNTyBBDMlycGCBDMlycGDAQCEAQCFAQCBQAMI/wAD" +
                                        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                                        "AAAAAAAAAHoPqgyCQP////8ADwAAAgAA/////3gFAAAAAAAAAAAAAAAAAAAAAAAA" +
                                        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACjM4AEAv//AIEBAYIBAoMBAIQB" +
                                        "AIUBAIYBAIcBAIgBAIkQAAAAAAAAAAAAAAAAAAAAAIoBAKMwgAEAgQEBggECgwEA" +
                                        "hAEAhQEAhgEBhwEAiAEAiRAC//8QAAAAAAAAAAAAAAAAigEBozCAAQCBAQGCAQKD" +
                                        "AQCEAQCFAQCGAQGHAQCIAQCJEAL//xACAJKLAAAAAAAAAACKAQKjOYAEBP//EYEB" +
                                        "AIIBBIMBAIQBAIUBAIYBAIcBAIgBAIkQAv//EAIAkosAAAAAAAAAAIoBAqsEgAIA" +
                                        "p6NpgAQF//8SgQEAggEFgwEAhAEAhQEAhgEChwEQiAEBiRAC//8QAgCSiwAAAAAA" +
                                        "AAAAigECqzSAAQyhLzAtgAEAgQRbMjU8ghDMr/wCp6aJB2TrisptRh00gxDk4HWJ" +
                                        "7eK+h5AFTZYwqNLlo4IRQoAEEv//RIEBAIIBEoMBAIQBAIUBAIYBCIcBDIgBA4kQ" +
                                        "Av//EAIAkosAAAAAAAAAAIoBAquCEQuAAg/AoYIRAzCCAe+AAQCBBFsyNTyCEAPN" +
                                        "/KLghobN83d+AIhyQ8iDggHQ1A98rOJIPvFdMd9VinGJm+rbW0tPQxo/LY2Ai0V0" +
                                        "QOUZJKN4GHuBPn/BoyFU2xI+B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkwggHwgAIB0IEEWzI1PIIQ2gtM7oVuXa8ikW9joR159oOCAdAHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWTCCAfCAAgOggQRbMjU8ghDk2ORNkMgCLgeN" +
                                        "u6BUNjG1g4IB0AeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZMIIB8IAC" +
                                        "BXCBBFsyNTyCEGqiDmmZ2EvncLEITNlQG+CDggHQB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkwggHwgAIHQIEEWzI1PIIQwqkozCrQ//N2X7Q+JkAFr4OC" +
                                        "AdAHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWTCCAfCAAgkQgQRbMjU8" +
                                        "ghCUOxhAaxYadam2ND+m84ytg4IB0AeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZMIIB8IACCuCBBFsyNTyCEGI94+so9YSdv2K+A8goPWyDggHQB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeS" +
                                        "qTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkwggHwgAIMsIEEWzI1PIIQMrptxCOd" +
                                        "qEf00lr8t8yGDIOCAdAHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB" +
                                        "0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWTCC" +
                                        "AWCAAg6AgQRbMjU8ghDBNhfSpwsgCKqPcjJOA7zEg4IBQAeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6l" +
                                        "qFkHgdKel5KpNNaoDTmupahZo4II8IAEEv//RYEBAIIBEoMBAIQBAIUBAIYBCIcB" +
                                        "DIgBAYkQAv//EAIAkosAAAAAAAAAAIoBAquCCLmAAggAoYIIsTCCAe+AAQCBBFsy" +
                                        "NTyCEIZlMCduSJFsWEZ+YOjSrdmDggHQ2Lq34siqq9AE31QTLn1PkpZ4EstugBTJ" +
                                        "CfzyEWQGfM4HgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk0" +
                                        "1qgNOa6lqFkwggHwgAIB0IEEWzI1PIIQFYvxm+KpEJG8w45qJVcH0IOCAdAHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKe" +
                                        "l5KpNNaoDTmupahZB4HSnpeSqTTWqA05rqWoWTCCAfCAAgOggQRbMjU8ghBP4T57" +
                                        "Dk4ZsrTo8grcbVH1g4IB0AeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "B4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZ" +
                                        "MIIB8IACBXCBBFsyNTyCEGJxMztf9kmYhv545HGDYSeDggHQB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNaoDTmupahZB4HSnpeSqTTWqA05" +
                                        "rqWoWQeB0p6Xkqk01qgNOa6lqFkwgd+AAgdAgQRbMjU8ghDUGeQ78Fa0oFcJuA+Z" +
                                        "U/HIg4HAB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZB4HSnpeSqTTWqA05rqWoWQeB0p6Xkqk01qgNOa6lqFkHgdKel5KpNNao" +
                                        "DTmupahZozCAAQCBAQGCAQGDAQCEAQCFAQCGAQeHAQCIAQCJEAL//xACAJKLAf//" +
                                        "/wAAAACKAQOjMoABAIEBAYIBQIMBAIQBHIUDAP/LhgEHhwEIiAEDiRAC//8QAgCS" +
                                        "iwH///9AAAAAigEEo4GZgAQRAAABgQEAggERgwEAhAEAhQEAhgEIhwEMiAEBiRAC" +
                                        "//8QAgCSiwH///8AAAAAigEDq2SAATKhXzBdgAEAgQRbMjU8ghBHTEwX5/1VqoXz" +
                                        "7kQVo6Afg0AHgdKel5KpNNaoDTmupahZ0ivhb/TGGuNRQSlevvh1GnZlaSPOXoYa" +
                                        "FM4329aKWpnMHLiJk448JjbdJOyrs9Sbo4GZgAQRAAACgQEAggERgwEAhAEAhQEA" +
                                        "hgEIhwEMiAEDiRAC//8QAgCSiwH///8AAAAAigEDq2SAATKhXzBdgAEAgQRbMjU8" +
                                        "ghBfhwSZMZa0UUtnWJ3PmmE+g0AHgdKel5KpNNaoDTmupahZ0ivhb/TGGuNRQSle" +
                                        "vvh1Gqf11EIefyeVu/LD7r7O+5bMHLiJk448JjbdJOyrs9SbozCAAQCBAQGCAQGD" +
                                        "AQCEAQCFAQCGAQeHAQCIAQCJEAL//xACAJKLAQAABAAAAACKAQOjMIABAIEBAYIB" +
                                        "QIMBAIQBHIUBAYYBB4cBCIgBA4kQAv//EAIAkosBAAAEQAAAAYoBBKMwgAEAgQEB" +
                                        "ggFAgwEAhAEchQEDhgEHhwEIiAEDiRAC//8QAgCSiwEAAARAAAADigEEozCAAQCB" +
                                        "AQGCAUCDAQCEARyFASqGAQeHAQiIAQOJEAL//xACAJKLAQAABEAAACqKAQSjMIAB" +
                                        "AIEBAYIBQIMBAIQBHIUBZYYBB4cBCIgBA4kQAv//EAIAkosBAAAEQAAAZYoBBKMw" +
                                        "gAEAgQEBggEBgwEAhAEAhQEAhgEHhwEAiAEAiRAC//8QAgCSiwEAAAEAAAAAigED" +
                                        "ozCAAQCBAQGCAUCDAQCEARyFAQCGAQeHAQmIAQOJEAL//xACAJKLAQAAAUAAAAGK" +
                                        "AQSjMIABAIEBAYIBQIMBAIQBHIUBAIYBB4cBCYgBA4kQAv//EAIAkosBAAABQAAA" +
                                        "AooBBKMwgAEAgQEBggEBgwEAhAEAhQEAhgEHhwEAiAEAiRAC//8QAgCSiwEAAAIA" +
                                        "AAAAigEDozCAAQCBAQGCAUCDAQCEARyFAQCGAQeHAQmIAQOJEAL//xACAJKLAQAA" +
                                        "AkAAAAOKAQSjMIABAIEBAYIBQIMBAIQBHIUBAIYBB4cBCYgBA4kQAv//EAIAkosB" +
                                        "AAACQAAABIoBBKMwgAEAgQEBggFAgwEAhAEchQEAhgEHhwEJiAEDiRAC//8QAgCS" +
                                        "iwEAAAJAAAAFigEEhAwmAAAAAAAAAAAAAACFOABU/DBbPhccMls2MyMyWyY8NTJb" +
                                        "MoBRAQDBqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                                    "</c2v>" +
                                "</hasp_info>";

            string action = "";

            Console.WriteLine("Input: " + Environment.NewLine +
                              "k - for login by PK request;" + Environment.NewLine +
                              "a - for send activation request;" + Environment.NewLine +
                              "i - for get info about ProductKey after login;" + Environment.NewLine +
                              "l - for ISV login request;" + Environment.NewLine +
                              "u - for update request via C2V;" + Environment.NewLine +
                              "e - for Exit." + Environment.NewLine +
                              "h - for Help." + Environment.NewLine);

            action = Console.ReadLine();

            if (action.Length > 1 && (action.StartsWith('a') || action.StartsWith('l') || action.StartsWith('u')) && action.Contains("sc:") && !action.Contains("pk:"))
            {
                //int s = action.LastIndexOf("sc:");
                //int ss = action.Length - action.LastIndexOf("sc:") - 5;                         
                string pathToFile = action.Substring(action.LastIndexOf("sc:") + 5, action.Length - action.LastIndexOf("sc:") - 6);

                try
                {
                    if (System.IO.File.Exists(pathToFile))
                    {
                        loadedXml = XDocument.Load(pathToFile);
                    }
                    else
                    {
                        Console.WriteLine("File doesn't exist! Please check path and try again." + Environment.NewLine);
                        Environment.Exit(0);
                    }
                }
                catch (System.AggregateException e)
                {
                    Console.WriteLine("Can't open file! Please check path and try again. Error: " + e.Message + Environment.NewLine);
                    Environment.Exit(0);
                }

                if (action.StartsWith('a')) {
                    actXml = (!string.IsNullOrEmpty(loadedXml.Root.Value)) ? loadedXml.Root.Value : actXml;
                } else if (action.StartsWith('l')) {
                    authXml = (!string.IsNullOrEmpty(loadedXml.Root.Value)) ? loadedXml.Root.Value : authXml;
                } else if (action.StartsWith('u')) {
                    targetXml = (!string.IsNullOrEmpty(loadedXml.Root.Value)) ? loadedXml.Root.Value : targetXml;
                }
            }
            else if (action.Length > 1 && (action.StartsWith('k') || action.StartsWith('a') || action.StartsWith('i')) && action.Contains("pk:") && !action.Contains("sc:"))
            {
                string tmpPK = action.Substring(action.LastIndexOf("pk:") + 3, action.Length - action.LastIndexOf("pk:") - 3);
                Regex regEx = new Regex(@"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
                pKey = (regEx.IsMatch(tmpPK)) ? tmpPK : pKey;
            }
            else if (action.Length > 1 && action.StartsWith('a') && action.Contains("pk:") && action.Contains("sc:"))
            {
                Regex regEx = new Regex(@"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
                Match m = regEx.Match(action);
                pKey = (!string.IsNullOrEmpty(m.Value)) ? m.Value : pKey;

                string tmpStr = action.Substring(action.LastIndexOf("sc:") + 4);
                string[] tmpStrMass = tmpStr.Split('"');
                if (!string.IsNullOrEmpty(tmpStrMass[0])) {
                    try {
                        if (System.IO.File.Exists(tmpStrMass[0])) {
                            loadedXml = XDocument.Load(tmpStrMass[0]);
                            actXml = (!string.IsNullOrEmpty(loadedXml.Root.Value)) ? loadedXml.Root.Value : actXml;
                        } else {
                            Console.WriteLine("File doesn't exist! Please check path and try again." + Environment.NewLine);
                            Environment.Exit(0);
                        }
                    } catch (System.AggregateException e) {
                        Console.WriteLine("Can't open file! Please check path and try again. Error: " + e.Message + Environment.NewLine);
                        Environment.Exit(0);
                    }
                } 
            }

            Console.WriteLine("/-------------------------/" + Environment.NewLine);

            switch (action.Substring(0, 1))
            {
                case "k":
                    Console.WriteLine("Let's try doing login by PK..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("loginByProductKey.ws", new KeyValuePair<string, string>("productKey", pKey)) + Environment.NewLine);
                    break;

                case "a":
                    Console.WriteLine("Let's try doing activation..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("productKey/" + pKey + "/activation.ws", new KeyValuePair<string, string>("activationXml", actXml)) + Environment.NewLine);
                    break;

                case "i":
                    Console.WriteLine("Let's try get info about PK..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("productKey/" + pKey + ".ws", new KeyValuePair<string, string>("productKey", pKey)) + Environment.NewLine);
                    break;

                case "l":
                    Console.WriteLine("Let's try doing ISV login..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("login.ws", new KeyValuePair<string, string>("authenticationDetail", authXml)) + Environment.NewLine);
                    break;

                case "u":
                    Console.WriteLine("Let's try get update via C2V..." + Environment.NewLine);
                    Console.WriteLine(emsClass.GetRequest("activation/target.ws", new KeyValuePair<string, string>("targetXml", targetXml)) + Environment.NewLine);
                    break;

                case "e":
                    Console.WriteLine("Let's try close application..." + Environment.NewLine);
                    Environment.Exit(0);
                    break;

                case "h":
                    Console.WriteLine("For \"Activation\", \"Get update by C2V\" and \"ISV login\" you can load ActivationXml/C2V/LoginXml from file. Use the parameter \"sc:*path_to_file*\"");
                    Console.WriteLine("For example: \"a sc:\"C:\\ActivationXml.txt\"\"" + Environment.NewLine);
                    Console.WriteLine("In additional, for keys: a, k, i - you can set product key via parameter: \"pk:\"");
                    Console.WriteLine("For example: \"a sc:\"C:\\ActivationXml.txt\"\" pk:c03e2c49-225e-4984-b75f-d8ffe5c84a2s");
                    Console.WriteLine("NOTE!: Both parameters (\"sc\" and \"pk\") you can use together only in \"Activation\" request!" + Environment.NewLine);
                    break;

                default:
                    Console.WriteLine("Input something wrong!" + Environment.NewLine);
                    break;
            }
        }
    }
}
