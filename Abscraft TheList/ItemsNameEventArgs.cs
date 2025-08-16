using System;
using System.Collections.Generic;

namespace Abscraft_TheList
{
    public class ItemsNameEventArgs : EventArgs
    {
        public string[] Names = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public ushort NextAvailable { get; set; }
        public ItemsStringValuesClass[] ItemsStringValues = new ItemsStringValuesClass[]
        {
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}}, 
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}},
            new ItemsStringValuesClass{Strings = new []{"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}}
        };
    }

    public class ItemsStringValuesClass
    {
        public string[] Strings { get; set; }
    }
}