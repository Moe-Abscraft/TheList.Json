using System;

namespace Abscraft_TheList
{
    public class ItemsNameEventArgs : EventArgs
    {
        public string[] Names = new string[]
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" 
        };
        public ushort NextAvailable { get; set; }
    }
}