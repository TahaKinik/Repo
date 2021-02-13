using System;
using System.IO;
using System.IO.Compression;

namespace Repo
{
    class Program
    {
        static string LOGO =      "______                  \n"+
                                  "| ___ \\                 \n"+
                                  "| |_/ /___ _ __   ___     \n"+
                                  "|    // _ \\ '_ \\ / _ \\  \n"+
                                  "| |\\ \\  __/ |_) | (_) | \n"+
                                  "\\_| \\_\\___| .__/ \\___/  \n"+
                                  "          | |           \n"+
                                  "          |_|           \n"+
                                  "                        \n";
        static string[] help = {"-init : Inits the Repo", "-store : Adds Current Version to Storage", "-listver : lists Versions", "-load : Load a specific version" };               
                        
        //@TODO:
        // ---Add envoriment var automaticly
        // Make a override control mechanism
        // Make Metadata of the each version and some how attach to version
        // Handle errors at unknown or miss valued argss 
        // Delete all files before loading the version
        // Check if already init
        // Functions to add:
        // Diff, Tree
        // -upload (version number or uploads last version) 

        static void Main(string[] args)
        {
            string rootpath = Environment.CurrentDirectory; //@"C:\Users\MMD\Desktop\Test";
            string RepoFolderName = ".repo";
            string repopath = rootpath + "\\" + RepoFolderName; //@"C:\Users\MMD\Desktop\Test\.repo";
            string storeVersionPath = repopath + "\\versions";

            int version = 0;

            if (args.Length == 0)
            {
                Console.WriteLine(LOGO);
                ConsoleHelp();
            }
            else
            {
                switch (args[0])
                {
                    case "-init":
                        Init(repopath);
                        break;
                    case "-store":
                        Store(rootpath, storeVersionPath, version);
                        break;
                    case "-listver":
                        ListVerions(storeVersionPath);
                        break;
                    case "-load":
                        if (args.Length>1) {Load(rootpath,storeVersionPath,int.Parse(args [1]));}
                        break;
                    default:
                        Console.WriteLine("This command does not exist!!");
                        ConsoleHelp();
                        break;
                }
            }

        }

        static void Diff ()
        {

        }

        static void Upload ()
        {

        }

        static void GenMetaData ()
        {

        }
        static void ConsoleHelp()
        {
            Console.WriteLine("Help :");
            foreach (var item in help)
            {
                Console.WriteLine(item);
            }
        }

        static void Init (string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path initialized already.");
                    return;
                }

                // Try to create the directory.
                Directory.CreateDirectory(path);
                File.SetAttributes(path, FileAttributes.Hidden);

                Directory.CreateDirectory(path + "\\versions");

                Console.WriteLine("The Repo was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        static void ListVerions(string StorePath)
        {
            Console.WriteLine(StorePath);
    
            if (System.IO.Directory.Exists(StorePath))
            {
                DirectoryInfo di = new DirectoryInfo(StorePath);
                DirectoryInfo[] diArr = di.GetDirectories();
                Console.WriteLine("Versions :");
                foreach (DirectoryInfo dri in diArr)
                    Console.WriteLine(dri.Name);
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }
        static void Load(string rootpath, string storePath, int version)
        {
            Console.WriteLine("version:" + version + " Loading ...");
            // say time

            string versionPath = storePath + "\\" + "version_" + version;

            if (System.IO.Directory.Exists(versionPath))
            {
                string[] files = System.IO.Directory.GetFiles(versionPath);

                
                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {

                    Console.WriteLine(s);
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(rootpath, fileName);

                    System.IO.File.Copy(s, destFile, true);
                }
            }

            else
            {
                Console.WriteLine("This Version Does not exist!");
            }


        }



        static void Store(string rootpath, string StorePath, int version)
        {
            string fileName = "";
            string destFile = "";

            Console.WriteLine("Version:" + version);
            if (Directory.Exists(StorePath + "\\version_" + version))
            {
                Console.WriteLine("That path exists already.");
                Store(rootpath,StorePath,++version);
                return;
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(StorePath + "\\version_" + version);

                if (System.IO.Directory.Exists(rootpath))
                {
                    string[] files = System.IO.Directory.GetFiles(rootpath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(StorePath, "version_" + version);
                        destFile = System.IO.Path.Combine(destFile, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }

                Console.WriteLine("Storage was created successfully at {0}.", Directory.GetCreationTime(StorePath + "\\version_" + version));
            }

        }


    }
}
