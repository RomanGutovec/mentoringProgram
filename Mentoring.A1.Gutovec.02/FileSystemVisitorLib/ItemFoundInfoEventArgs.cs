using System;

namespace FileSystemVisitorLib
{
    public sealed class ItemFoundInfoEventArgs : EventArgs
    {
        public ItemFoundInfoEventArgs()
        {
        }

        public ItemFoundInfoEventArgs(string itemName, bool stop, bool exclude)
        {
            ItemName = itemName;
            Stop = stop;
            Exclude = exclude;
        }

        public string ItemName { get; set; }

        public bool Stop { get; set; }

        public bool Exclude { get; set; }
    }
}