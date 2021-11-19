using System;
using static System.Console;
using System.IO;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using System.Threading.Tasks;

namespace download
{
    class Program
    {
        async Task Download()
        {
            string path = Environment.CurrentDirectory;
            //WriteLine(path);

            Write("다운로드할 유튜브 주소: ");
            string url = ReadLine();

            WriteLine("MP4: 4,  MP3: 3");
            int checkMP3;
            try
            {
                checkMP3 = int.Parse(ReadLine());
            }
            catch
            {
                checkMP3 = 3;
            }

            if (checkMP3 != 3 && checkMP3 != 4)
            {
                checkMP3 = 3;
            }

            YouTube youtube = YouTube.Default;
            Video video = await youtube.GetVideoAsync(url);

            //string path = Path.GetDirectoryName("");
            string fileName = path + "/" + video.FullName;
            File.WriteAllBytes(fileName, await video.GetBytesAsync());

            var inputFile = new MediaFile { Filename = $"{path}/{video.FullName}" };
            var outputFile = new MediaFile { Filename = $"{path}/{video.FullName}.mp3" };

            if (checkMP3 == 3) //이 안에 들어오면서 실행되는 코드들은 작동이 안됨..
            {
                using (var enging = new Engine())
                {
                    enging.GetMetadata(inputFile);
                    enging.Convert(inputFile, outputFile);
                }

                File.Delete($"{fileName}.mp4");
            }
        }

        static async Task Main(string[] args)
        {
            Program p = new Program();
            await p.Download();
        }
    }
}
