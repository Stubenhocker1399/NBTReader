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
            var worldfolder = "C:\\Users\\Chris\\AppData\\LocalLow\\Facepunch Studios\\Chunks\\Worlds\\testworldblocks";
            var file = "debugDecompressedChunks.0.0";
            //var file = "level.dat";
            if (!File.Exists(worldfolder + "\\" + file))
                Console.WriteLine("Couldn't locate Minecraft World to import.");
            else
            {
                //byte[] leveldatcompressed = File.ReadAllBytes(worldfolder + "\\" + file);
                //byte[] leveldat = Decompress(leveldatcompressed);
                byte[] leveldat = File.ReadAllBytes(worldfolder + "\\" + file);
                Console.WriteLine(BitConverter.ToString(leveldat));
                Console.WriteLine(Encoding.Default.GetString(leveldat));
                Console.WriteLine("");

                var nbt = NBTReader.readNBT(leveldat);
                //var inner = (double) nbt.tree["Data"]["Player"]["Pos"][0];
                ;
            }
            Console.ReadKey();
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
