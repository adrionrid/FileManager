using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FileManager
{
    public class FolderController
    {
        public string CurrentPath { get; private set; } = AppDomain.CurrentDomain.BaseDirectory;

        public ObservableCollection<string> Menu { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Content { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Info { get; private set; } = new ObservableCollection<string>();

        CancellationTokenSource searchCanceler = new CancellationTokenSource();

        public FolderController()
        {
            CurrentPath = AppDomain.CurrentDomain.BaseDirectory;

            OpenDirectory(CurrentPath);
            CreateMenu(CurrentPath);
        }

        public void Open(int index)
        {
            if (index < 0) return;

            var path = Path.GetFullPath(Content[index], CurrentPath);

            if (IsDirectory(path))
            {
                OpenDirectory(path);
                Menu.Add(Path.GetFileName(CurrentPath));
            }
            else
                OpenFile(path);
        }

        static void OpenFile(string path)
        {
            var info = new ProcessStartInfo(path);
            info.UseShellExecute = true;
            Process.Start(info);
        }

        void OpenDirectory(string path)
        {
            CurrentPath = path;
            UpdateContent(Content, GetDirectoryContent(path));
        }


        public void Select(int index)
        {
            if (index < 0) return;

            CurrentPath = GetDirectoryParent(CurrentPath, index);

            for (int i = Menu.Count - 1; i > index; i--)
                Menu.RemoveAt(i);

            OpenDirectory(CurrentPath);
        }

        void CreateMenu(string path) => UpdateContent(Menu, GetDirectories(path));

        static void UpdateContent(ObservableCollection<string> list, string[] array)
        {
            if (list == null)
                list = new ObservableCollection<string>();
            else
                list.Clear();

            foreach (var item in array)
                list.Add(item);
        }

        static string[] GetDirectoryContent(string path) => Directory.GetFileSystemEntries(path).Select(entrie => Path.GetFileName(entrie)).ToArray();

        static string[] GetDirectories(string path) => path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

        static string GetDirectoryParent(string path, int index)
        {
            if (index == 0)
               return Path.GetPathRoot(path);

            var dirs = path.Split(Path.DirectorySeparatorChar, index + 2, StringSplitOptions.RemoveEmptyEntries);

            if (dirs.Length - 1 != index)
                dirs[dirs.Length - 1] = string.Empty;

            var s = Path.Combine(dirs);
            return s;
        }


        public async Task Search(string text)
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(ClearSearch);

            searchCanceler.Cancel();

        retry:
            await FileSearcher.TraverseTree(CurrentPath, text, UpdateSearch, searchCanceler.Token);
            if (searchCanceler.IsCancellationRequested)
            {
                searchCanceler = new CancellationTokenSource();
                if (!(text == null || text == string.Empty))
                    goto retry;
            }
            if (text == null || text == string.Empty)
            {
                await Dispatcher.CurrentDispatcher.InvokeAsync(ClearSearch);
                OpenDirectory(CurrentPath);
            }
            searchCanceler = new CancellationTokenSource();
        }

        void UpdateSearch(string[] array)
        {
            foreach (var item in array)
                Content.Add(item);
        }
        void ClearSearch()
        {
            for (int i = Content.Count - 1; i >= 0; i--)
                Content.Remove(Content[i]);
        }


        public void DisplayInfo(int index)
        {
            if (index < 0)
            {
                Info.Clear();
                return;
            }

            var path = Path.GetFullPath(Content[index], CurrentPath);
            UpdateInfo(Info, path);
        }

        static async void UpdateInfo(ObservableCollection<string> list, string path)
        {
            if (list == null)
                list = new ObservableCollection<string>();
            else
                list.Clear();

            if (IsDirectory(path))
                foreach (var item in await GetDirectoryInfo(path))
                    list.Add(item);
            else
                foreach (var item in GetFileInfo(path))
                    list.Add(item);
        }

        static async Task<string[]> GetDirectoryInfo(string path)
        {
            var files = await Task.Run(() => GetFiles(path));
            //var files = await Dispatcher.CurrentDispatcher.InvokeAsync(() => GetFiles(path), DispatcherPriority.Send);

            var result = new string[2];
            result[0] = $"Files: {files.Length}";
            result[1] = $"Size: {files.Sum(x => x.Length)} bytes";
            return result;
        }

        static string[] GetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path, string.Empty, SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                DysplayError(ex);
            }
            return new string[0];
        }

        static void DysplayError(Exception ex) => MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);

        static string[] GetFileInfo(string path)
        {
            var result = new string[4];
            var info = new FileInfo(path);
            result[0] = $"Name: {Path.GetFileNameWithoutExtension(path)}";
            result[1] = $"Extension: {info.Extension}";
            result[2] = $"Length: {info.Length} bytes";
            result[3] = $"Creation time: {info.CreationTime}";
            return result;
        }

        static bool IsDirectory(string path) => File.GetAttributes(path).HasFlag(FileAttributes.Directory);
    }
}
