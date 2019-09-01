using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorLib
{
    public sealed class ItemFoundInfoEventArgs : EventArgs
    {
        public ItemFoundInfoEventArgs()
        {
        }

        public ItemFoundInfoEventArgs(string itemName, bool stop, bool exclude)
        {
            ItemName = itemName ?? throw new ArgumentNullException($"Name {nameof(itemName)} can't be null.");
            Stop = stop;
            Exclude = exclude;
        }

        public string ItemName { get; set; }

        public bool Stop { get; set; }

        public bool Exclude { get; set; }
    }
}