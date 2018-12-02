using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;

namespace DediServerCron
{
    public class Utilities
    {
       
        public static string GetServerName(int id, string filePath)
        {
            if (!File.Exists(filePath)) return "Uknown";
            

            string serverName = "";
            // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        if (reader.Name == "property")
                        {

                            // Detect this element.
                            var servName = reader.GetAttribute("name");
                            if (servName == "ServerName")
                            {
                                serverName = reader.GetAttribute("value");
                                var servs = new ServerSettings();
                                servs.ServerName = serverName;
                            }

                        }
                    }
                }

            }

            return serverName;
        }

        public static void ExecuteBatch(string batchloc)
        {
            string trimLoc = "";
            if (!File.Exists($"{batchloc}"))
            {
                 return;
            }

            if (batchloc.Contains("startdedicated.bat"))
            {
                trimLoc = batchloc.Replace("\\startdedicated.bat", "");
            }

            if (batchloc.Contains("ConanSandboxServer"))
            {
                trimLoc = batchloc.Replace("\\ConanSandboxServer.exe", "");
            }
            var processInfo =
                new ProcessStartInfo(@"cmd.exe", "/c " + $@"{batchloc}")
                {
                    WorkingDirectory = trimLoc, //@"H:\7d2d\steamcmd\steamapps\common\7d2d",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

            var process = Process.Start(processInfo);

            if (process != null)
            {
                //    process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                //   Console.WriteLine("output>>" + e.Data);
                //   process.BeginOutputReadLine();
                //
                // process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                //  Console.WriteLine("error>>" + e.Data);
                //  process.BeginErrorReadLine();

                //process.WaitForExit();

                //Console.WriteLine("ExitCode: {0}", process.ExitCode);
                // process.Close();
            }
        }

        public static object GetCpuCounter(string appName)
        {
            try
            {
                PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

                PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", appName);
                var process_cpu_usage = (total_cpu.NextValue() / 100) * process_cpu.NextValue();
                //var processUsage = process_cpu_usagenextValue() / Environment.ProcessorCount;

                float t = total_cpu.NextValue();
                Thread.Sleep(1000);
                var p = process_cpu.NextValue() / Environment.ProcessorCount;
                Decimal theVal = Convert.ToDecimal(p);
                return Math.Round(theVal, 2);
            }
            catch (Exception e)
            {
                
                return "error";
            }
           
           



        }


    }
}

