using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBTReaderConsole
{
    class Program
    {     
        static void Main(string[] args)
        {
            if(args.Count()<=1 || args.Count()>= 5)
            {
                printUsageInfo();
            }else {                 
                var file = args[1];
                if (!File.Exists(file))
                    Console.WriteLine("Could not locate NBTFile.");
                else
                {
                    byte[] nbtfile = { };
                    if (args[0] == "/d")
                    {
                        nbtfile = File.ReadAllBytes(file);
                    }
                    if (args[0] == "/c")
                    {
                        try
                        {
                            nbtfile = Decompress(File.ReadAllBytes(file));
                        }
                        catch(System.IO.InvalidDataException e)
                        {
                            Console.WriteLine("Could not decompress the file, make sure the given file is a gzip compressed file or change /c to /d for an already decompressed file.");
                            Console.WriteLine(e.Message);
                            return;
                        }
                    }

                    if (args.Count()>=3)
                    { 
                        if (args[2] == "/h")
                        {
                            Console.WriteLine(BitConverter.ToString(nbtfile));
                            Console.WriteLine();
                        }
                        if (args[2] == "/s")
                        {
                            Console.WriteLine(Encoding.Default.GetString(nbtfile));
                            Console.WriteLine();
                        }
                    }
                    if (args.Count() == 4)
                    {
                        if (args[3] == "/h")
                        {
                            Console.WriteLine(BitConverter.ToString(nbtfile));
                            Console.WriteLine();
                        }
                        if (args[3] == "/s")
                        {
                            Console.WriteLine(Encoding.Default.GetString(nbtfile));
                            Console.WriteLine();
                        }
                    }
#if RELEASE
                    try
                    {
#endif
                        /*******************************************************************************************************/

                        //Here is where the magic happens
                        var nbt = NBTReader.readNBT(nbtfile);

                        //Example of accessing the player x position inside the level.dat file:

                        //var inner = (double) nbt.tree["Data"]["Player"]["Pos"][0];  
                        //Console.WriteLine(inner);

                        //Example of accessing bigtest.nbt 

                        var intTest = (int)nbt.tree["intTest"];
                        var floatTest = (float)nbt.tree["floatTest"];
                        var stringtest = (string)nbt.tree["stringTest"];

                        /*******************************************************************************************************/
                        Console.WriteLine("Sucessfully parsed the nbt file.");
#if RELEASE
                }
                    catch(System.Exception e)
                    {
                        Console.WriteLine("Error while trying to parse the NBT file.");
                        Console.WriteLine(e.Message);
                    }             
#endif   
                }
            }
#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void printUsageInfo()
        {
            Console.WriteLine("Reads NBT files.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("NBTReaderConsole.exe [/c|/d] [path]NBTFile [/h] [/s]");
            Console.WriteLine();
            Console.WriteLine("Required:");
            Console.WriteLine("  [path]NBTFile  The full path to the NBTFile to be read.");
            Console.WriteLine("  /c             The given file is gzip compressed.");
            Console.WriteLine("  /d             The given file is decompressed.");
            Console.WriteLine("Optional:");
            Console.WriteLine("  /h             Print the raw file in hex format.");
            Console.WriteLine("  /s             Print the raw file as a string.");
        }

        static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
