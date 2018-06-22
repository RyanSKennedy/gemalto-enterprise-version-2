﻿using System;
using SentinelConnector;

namespace TestConsoleUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            SentinelEMSClass emsClass = new SentinelEMSClass("http://10.211.55.3:8080/ems/v78/ws");
            string pKey = "c03e2c49-225e-4984-b75f-d8ffe5c84a2b";

            string action = "";
            Console.WriteLine("Input: " + Environment.NewLine +
                              "1 - for send login request;" + Environment.NewLine +
                              "2 - for send ativation request;" + Environment.NewLine +
                              "0 - for Exit." + Environment.NewLine);

            action = Console.ReadLine();
            Console.WriteLine("/-------------------------/" + Environment.NewLine);

            switch (action) {
                case "1":
                    Console.WriteLine("Let's try doing login..." + Environment.NewLine);
                    Console.WriteLine(emsClass.LoginByPK(pKey));
                    break;

                case "2":
                    //Console.WriteLine("Let's try doing activation..." + Environment.NewLine);
                    break;

                case "0":
                    //Console.WriteLine("Let's try close application..." + Environment.NewLine);
                    break;

                default:
                    //Console.WriteLine("Input something wrong!" + Environment.NewLine);
                    break;
            }
        }
    }
}
