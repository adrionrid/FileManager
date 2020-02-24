using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace FileManager
{
    class FileSearcher
    {
        public static async Task TraverseTree(string path, string searchPattern, Action<string[]> OnGotFiles, CancellationToken? token)
        {
            var _token = token.HasValue ? token.Value : CancellationToken.None;

            Stack<string> dirs = new Stack<string>(20);

            if (!Directory.Exists(path))
            {
                throw new ArgumentException();
            }
            dirs.Push(path);

            while (!_token.IsCancellationRequested && dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                List<string> subDirs = new List<string>(128);
                try
                {
                    await Task.Run(() => subDirs = Directory.GetDirectories(currentDir).ToList(), _token);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    await Task.Run(() => files = Directory.GetFiles(currentDir, $"*{searchPattern}*"), _token);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                try
                {
                    OnGotFiles(files);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }
    }
}
