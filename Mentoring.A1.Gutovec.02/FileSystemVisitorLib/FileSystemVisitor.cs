using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorLib
{
    public class FileSystemVisitor : IEnumerable<string>
    {
        #region Private fields
        private readonly DirectoryInfo _rootPath;
        private readonly Func<string, bool> _filter;
        #endregion 

        #region Constructors
        public FileSystemVisitor(DirectoryInfo rootPath)
        {
            _rootPath = rootPath ?? throw new ArgumentNullException($"Argument {nameof(rootPath)} has null value.");
            _filter = (s) => true;
        }

        public FileSystemVisitor(DirectoryInfo rootPath, Func<string, bool> filter) : this(rootPath)
        {
            _filter = filter ?? throw new ArgumentNullException($"Argument {nameof(filter)} has null value.");
        }
        #endregion

        #region Events
        public event EventHandler Start = delegate { };

        public event EventHandler Finish = delegate { };

        public event EventHandler<ItemFoundInfoEventArgs> FileFound = delegate { };

        public event EventHandler<ItemFoundInfoEventArgs> DirectoryFound = delegate { };

        public event EventHandler<ItemFoundInfoEventArgs> FilteredFileFound = delegate { };

        public event EventHandler<ItemFoundInfoEventArgs> FilteredDirectoryFound = delegate { };
        #endregion

        #region Public methods
        public IEnumerable<string> GetDirectoriesAndFilesFromRoot()
        {
            OnStart(new EventArgs());

            foreach (var element in GetDirectories(_rootPath.FullName))
            {
                yield return element;
            }

            OnFinish(new EventArgs());
        }
        #endregion

        #region Protected methods
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
        #endregion

        #region Private methods
        private IEnumerable<string> GetDirectories(string path)
        {
            foreach (var subFolder in Directory.EnumerateDirectories(path))
            {
                var args = new ItemFoundInfoEventArgs() { ItemName = path };
                OnDirectoryFound(args);

                if (_filter(subFolder) && !args.Exclude)
                {
                    if (args.Stop)
                    {
                        yield break;
                    }

                    var argsFilteredDirectory = new ItemFoundInfoEventArgs() { ItemName = path };
                    OnFilteredDirectoryFound(argsFilteredDirectory);

                    if (argsFilteredDirectory.Stop)
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
                var args = new ItemFoundInfoEventArgs() { ItemName = file };
                OnFileFound(args);

                if (_filter(file) && !args.Exclude)
                {
                    if (args.Stop)
                    {
                        yield break;
                    }

                    var argsFilteredFile = new ItemFoundInfoEventArgs() { ItemName = file };
                    OnFilteredFileFound(argsFilteredFile);

                    if (argsFilteredFile.Stop)
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
        #endregion

        #region Implementations
        public IEnumerator<string> GetEnumerator()
        {
            return GetDirectoriesAndFilesFromRoot().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}