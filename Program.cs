//Written for Divine Souls. https://store.steampowered.com/app/300040
using System.IO;

namespace Divine_Souls_Extractor
{
    class Program
    {
        public static BinaryReader br;
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]);
            BinaryReader dfh = new(File.OpenRead(path + ".dfh"));
            BinaryReader dfp = new(File.OpenRead(path + ".dfp"));

            dfh.ReadInt32();
            int fileCount = dfh.ReadInt32();
            dfh.ReadInt32();

            Directory.CreateDirectory(path);
            for (int i = 0; i < fileCount; i++)
            {
                int size = dfh.ReadInt32();
                int unknown = dfh.ReadInt32();
                dfp.BaseStream.Position = dfh.ReadInt32();
                BinaryWriter bw = new(File.Create(path + "//" + i));
                bw.Write(dfp.ReadBytes(size));
                bw.Close();

                BinaryReader br = new(File.OpenRead(path + "//" + i));
                string magic = new string(br.ReadChars(4));
                if (magic == "Game")
                    magic += new string(br.ReadChars(9));
                br.Close();

                switch (magic)
                {
                    case "DDS ":
                        File.Move(path + "//" + i, path + "//" + i + ".dds");
                        break;
                    case "Gamebryo File":
                            File.Move(path + "//" + i, path + "//" + i + ".nif");
                        break;
                    case "Gamebryo KFM ":
                        File.Move(path + "//" + i, path + "//" + i + ".kfm");
                        break;
                    case "Lua":
                        File.Move(path + "//" + i, path + "//" + i + ".luac");
                        break;
                    case "RIFF":
                        File.Move(path + "//" + i, path + "//" + i + ".wav");
                        break;
                    case "ID3":
                    case "ÿûâ`":
                        File.Move(path + "//" + i, path + "//" + i + ".mp3");
                        break;
                    //Todo: list of file names = .ifl
                    //Todo: 0x200000 = ???
                    default: break;
                }
            }
        }
    }
}
