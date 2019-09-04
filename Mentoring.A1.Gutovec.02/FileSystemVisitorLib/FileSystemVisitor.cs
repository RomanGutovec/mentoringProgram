using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemVisitorLib
{
    public class FileSystemVisitor : IEnumerable<string>
    {
        private readonly DirectoryInfo _rootPath;
        private readonly Func<string, bool> _filter;
        private readonly List<ItemFoundInfoEventArgs> _args = new List<ItemFoundInfoEventArgs>();

        public FileSystemVisitor(DirectoryInfo rootPath)
        {
            _rootPath = rootPath ?? throw new ArgumentNullException($"Argument {nameof(rootPath)} has null value.");
            _filter = (s) => true;
        }

        public FileSystemVisitor(DirectoryInfo rootPath, Func<string, bool> filter) : this(rootPath)
        {
            _filter = filter ?? throw new ArgumentNullException($"Argument {nameof(filter)} has null value.");
        }

        public event EventHandler Start;

        public event EventHandler Finish;

        public event EventHandler<ItemFoundInfoEventArgs> FileFound;

        public event EventHandler<ItemFoundInfoEventArgs> DirectoryFound;

        public event EventHandler<ItemFoundInfoEventArgs> FilteredFileFound;

        public event EventHandler<ItemFoundInfoEventArgs> FilteredDirectoryFound;

        public IEnumerator<string> GetEnumerator()
        {
            return GetDirectoriesAndFilesFromRoot().GetEnumerator();
        }

        protected virtual void OnStart(EventArgs e)
        {
            Start?.Invoke(this, e);
        }

        protected virtual void OnFinish(EventArgs e)
        {
            Finish?.Invoke(this, e);
        }

        protected virtual void OnFileFound(ItemFoundInfoEventArgs e)
        {
            FileFound?.Invoke(this, e);
        }

        protected virtual void OnDirectoryFound(ItemFoundInfoEventArgs e)
        {
            DirectoryFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredFileFound(ItemFoundInfoEventArgs e)
        {
            FilteredFileFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredDirectoryFound(ItemFoundInfoEventArgs e)
        {
            FilteredDirectoryFound?.Invoke(this, e);
        }

        private IEnumerable<string> GetDirectoriesAndFilesFromRoot()
        {
            OnStart(EventArgs.Empty);

            foreach (var element in GetDirectories(_rootPath.FullName))
            {
                yield return element;
            }

            OnFinish(EventArgs.Empty);
        }

        private IEnumerable<string> GetDirectories(string path)
        {
            foreach (var subFolder in Directory.EnumerateDirectories(path))
            {
                var args = new ItemFoundInfoEventArgs { ItemName = path };
                _args.Add(args);
                OnDirectoryFound(args);

                if (_filter(subFolder) && !args.Exclude)
                {
                    if (_args.Any(a=>a.Stop))
                    {
                        yield break;
                    }

                    var argsFilteredDirectory = new ItemFoundInfoEventArgs { ItemName = path };
                    _args.Add(argsFilteredDirectory);
                    OnFilteredDirectoryFound(argsFilteredDirectory);

                    if (_args.Any(a => a.Stop))
                    {
                        yield break;
                    }

                    if (!argsFilteredDirectory.Exclude)
                    {
                        yield return subFolder;
                    }

                }
                foreach (var element in GetDirectories(subFolder))
                {
                    yield return element;
                }
            }

            foreach (var file in GetFiles(path))
            {
                yield return file;
            }
        }

        private IEnumerable<string> GetFiles(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var args = new ItemFoundInfoEventArgs { ItemName = file };
                _args.Add(args);
                OnFileFound(args);

                if (_filter(file) && !args.Exclude)
                {
                    if (_args.Any(a => a.Stop))
                    {
                        yield break;
                    }

                    var argsFilteredFile = new ItemFoundInfoEventArgs { ItemName = file };
                    _args.Add(argsFilteredFile);
                    OnFilteredFileFound(argsFilteredFile);

                    if (_args.Any(a => a.Stop))
                    {
                        yield break;
                    }

                    if (!argsFilteredFile.Exclude)
                    {
                        yield return file;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}