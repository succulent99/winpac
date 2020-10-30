using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace winpac
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("packagelist.txt"))
            {
                Console.WriteLine("No package list was located.");
                Environment.Exit(1);
            }
            string[] packages = File.ReadAllLines("packagelist.txt");
            if (args.Length != 0)
            {
                if (args[0] == "i" || args[0] == "install")
                {
                    
                    if (args.Length == 1)
                    {
                        Console.WriteLine("Please specify the package you want to install.");
                        Environment.Exit(0);
                    }
                    else
                    {
                        string packageName = args[1];
                        Console.WriteLine("Searching For Package: " + args[1] + "...");
                        for (int i = 0; i < packages.Length; i++)
                        {
                            string[] currentPackage = packages[i].Split(';');
                            if (currentPackage[0] == packageName)
                            {
                                Console.WriteLine("Package Located...");
                                Console.WriteLine("Download Started...");

                                var client = new WebClient();
                                client.DownloadFile(currentPackage[1], currentPackage[0] + ".exe");

                                Console.WriteLine("Download Finished...");
                                Console.WriteLine("Starting installer...");
                                try
                                {
                                    System.Diagnostics.Process.Start(currentPackage[0] + ".exe").WaitForExit();
                                    File.Delete(currentPackage[0] + ".exe");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Install failed. Ex: " + ex);
                                    File.Delete(currentPackage[0] + ".exe");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Installation Finished. No Error Reported.");
                                Environment.Exit(0);
                            }
                        }
                    }
                }
                if (args[0] == "u" || args[0] == "update")
                {

                }
                if (args[0] == "l" || args[0] == "list")
                {
                    for (int i = 0; i < packages.Length; i++)
                    {
                        string[] currentPackage = packages[i].Split(';');
                        Console.WriteLine(currentPackage[0]);
                    }
                }
                if (args[0] == "v" || args[0] == "version")
                {
                    Console.WriteLine("Version: v0.1\nBranch: Debug");
                }
                if (args[0] == "h" || args[0] == "help")
                {
                    Console.WriteLine("Thanks for trying out WinPac, Here are some commands:\nwinpac install: installs a specified package.\nwinpac help: brings you here.\nwinpac list: lists all packages.\nwinpac version: tells your what version you are using.");
                }
            }
            else Console.WriteLine("Type `winpac help`, or `winpac h`");
        }
    }
}
