using System;

namespace Abscraft_TheList
{
    public class ItemRecallEventArgs : EventArgs
    {
        public string ItemName { get; set; }
        public ushort ItemId { get; set; }
        public ushort[] ItemIntegerValues { get; set; }
        public string[] ItemStringValues { get; set; }

        public ItemRecallEventArgs()
        {
            ItemName = string.Empty;
            ItemIntegerValues = new ushort[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 
            };
            ItemStringValues = new[]
            {
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };
            ItemId = 0;
        }
    }
}