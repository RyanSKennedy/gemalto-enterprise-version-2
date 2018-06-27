﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise
{
    class SentinelData
    {
        public static string vendorCode = "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"; //DEMOMA SentinelData
        public static string keyScope = "<haspscope>" +
                                        "<feature id=\"1\"/>" +
                                        "<feature id=\"2\"/>" +
                                        "<feature id=\"3\"/>" + 
                                        "</haspscope>";
        public static string keyFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                            "<haspformat root=\"hasp_info\">" +
                                                "<hasp>" +
                                                    "<element name=\"id\"/>" +
                                                    "<element name=\"type\"/>" +
                                                    "<element name=\"version\"/>" + 
                                                    "<element name=\"hw_version\"/>" +
                                                    "<element name=\"key_model\"/>" +
                                                    "<element name=\"key_type\"/>" +
                                                    "<element name=\"form_factor\"/>" +
                                                    "<element name=\"hw_platform\"/>" +
                                                    "<element name=\"driverless\"/>" +
                                                    "<element name=\"fingerprint_change\"/>" +
                                                    "<element name=\"vclock_enabled\"/>" +
		                                            "<product>" +
			                                            "<element name=\"id\"/>" +
                                                        "<element name=\"name\"/>" +
			                                            "<feature>" +
				                                            "<element name=\"id\"/>" +
                                                            "<element name=\"name\"/>" +
				                                            "<element name=\"license\"/>" +
                                                            "<element name=\"locked\"/>" +
			                                            "</feature>" +
		                                            "</product>" +
                                                "</hasp>" +
                                            "</haspformat>";
        public static string emsUrl = "http://localhost:8080/ems/v78/ws";

        public static bool logIsEnabled = true;

        public SentinelData() {}
    }
}
