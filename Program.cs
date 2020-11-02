using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace winpac
{
    class Program
    {
        static double currentVersion = 0.1;
        static void Main(string[] args)
        {
            if (!File.Exists("packagelist.txt"))
            {
                Console.WriteLine("No package list was located."); Console.WriteLine("Downloading Latest Package List...");
                try
                {
                    File.Delete("packagelist.txt");
                    var client = new WebClient();
                    client.DownloadFile("https://winpac-mirror.succulent99.repl.co/packagelists/packagelist.txt", "packagelist.txt");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Package List Update Failed: " + ex);
                }
                Console.WriteLine("Package List Update Completed.");
                Environment.Exit(1);
            }
            string[] packages = File.ReadAllLines("packagelist.txt");
            if (args.Length != 0)
            {
                if (args[0] == "-i" || args[0] == "--install")
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
                                string[] currentPackageName = currentPackage[0].Split('/');

                                var client = new WebClient();
                                client.DownloadFile(currentPackage[1], currentPackageName[1] + ".exe");

                                Console.WriteLine("Download Finished...");
                                Console.WriteLine("Starting installer...");
                                try
                                {
                                    System.Diagnostics.Process.Start(currentPackageName[1] + ".exe").WaitForExit();
                                    File.Delete(currentPackageName[1] + ".exe");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Install failed. Ex: " + ex);
                                    File.Delete(currentPackageName[1] + ".exe");
                                    Environment.Exit(0);
                                }
                                Console.WriteLine("Installation Finished. No Error Reported.");
                                Environment.Exit(0);
                            }
                        }
                    }
                }
                if (args[0] == "-ul" || args[0] == "--updatelist")
                {
                    Console.WriteLine("Downloading Latest Package List...");
                    try
                    {
                        File.Delete("packagelist.txt");
                        var client = new WebClient();
                        client.DownloadFile("https://winpac-mirror.succulent99.repl.co/packagelists/packagelist.txt", "packagelist.txt");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Package List Update Failed: " + ex);
                    }
                    Console.WriteLine("Package List Update Completed.");
                }
                if (args[0] == "-u" || args[0] == "--update")
                {
                    Console.WriteLine("Finding latest version...");
                    var client = new WebClient();
                    if (File.Exists("winpacver"))
                    {
                        File.Delete("winpacver");
                    }
                    client.DownloadFile("https://winpac-mirror.succulent99.repl.co/winpacver", "winpacver");
                    Console.WriteLine("Latest version found...");
                    int updatableVersion = int.Parse(File.ReadAllText("winpacver"));
                    if (currentVersion < updatableVersion)
                    {
                        Console.WriteLine("Update is needed...");
                        Console.WriteLine("Downloading latest version...");
                        client.DownloadFile("https://github.com/succulent99/winpac/releases/download/v0.2/winpacInstaller.exe", @"C:\Users\" + Environment.UserName + @"\Downloads\winpacUpdate.exe");
                        System.Diagnostics.Process.Start(@"C:\Users\" + Environment.UserName + @"\Downloads\winpacUpdate.exe");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("No update required.");
                    }
                }
                if (args[0] == "-l" || args[0] == "--list")
                {
                    for (int i = 0; i < packages.Length; i++)
                    {
                        string[] currentPackage = packages[i].Split(';');
                        Console.WriteLine(currentPackage[0]);
                    }
                }
                if (args[0] == "-v" || args[0] == "--version")
                {
                    Console.WriteLine("Version: v" + currentVersion);
                }
                if (args[0] == "-h" || args[0] == "--help")
                {
                    Console.WriteLine("Thanks for trying out WinPac, Here are some commands:\nwinpac --install: installs a specified package.\nwinpac --help: brings you here.\nwinpac --list: lists all packages.\nwinpac --version: tells your what version you are using.\nwinpac --updatelist: updates the current official packagelist.");
                }
            }
            else Console.WriteLine("Type `winpac --help`, or `winpac -h`");
        }
    }
}
