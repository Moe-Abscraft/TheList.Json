using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Crestron.SimplSharp; // For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;

namespace Abscraft_TheList
{
    public class TheList
    {
        private ushort _nextAvailable;
        private List<ListItems> _theList;

        // private static string _filePath;
        // public string FilePath { get { return _filePath; } set { _filePath = value; } }

        private static string _filePath = string.Empty;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                _fileInfo = new FileInfo(_filePath);
            }
        }

        // private const string FilePath = "nvram/listName.json";
        // private static string _filePath;
        //public string FilePath {
        //    get { return _filePath; }
        //    set { _filePath = value; }
        //}
        private FileInfo _fileInfo;
        // private readonly FileInfo _fileInfo = new FileInfo(_filePath);

        public event EventHandler<ItemsNameEventArgs> ItemsNameUpdated = delegate { };
        private ItemsNameEventArgs _namesArgs;

        public event EventHandler<ItemRecallEventArgs> ItemRecallEvent = delegate { };
        private ItemRecallEventArgs _recallEventArgs;

        public ushort[] IntegerValues { get; set; }
        public string[] StringValues { get; set; }

        /// <summary>
        /// SIMPL+ can only execute the default constructor. If you have variables that require initialization, please
        /// use an Initialize method
        /// </summary>
        public TheList()
        {
            _theList = new List<ListItems>();
            IntegerValues = new ushort[]
                {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            StringValues = new[]
            {
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };

            // _fileInfo = new FileInfo(_filePath);
        }

        public ushort NextAvailable
        {
            set { _nextAvailable = value; }
            get { return _nextAvailable; }
        }

        public string ListName { get; set; }

        public void AddItem(string name, ushort id)
        {
            try
            {
                //var item = new ListItems
                //{
                //    ItemName = name,
                //    ItemId = id,
                //    ItemStringValues = StringValues,
                //    ItemIntegerValues = IntegerValues
                //};
                //_theList.Add(item);
                // See if an item with this ID already exists
                var existing = _theList.FirstOrDefault(x => x.ItemId == id);

                if (existing != null)
                {
                    // Update the existing item
                    existing.ItemName = name;
                    existing.ItemStringValues = StringValues;
                    existing.ItemIntegerValues = IntegerValues;
                }
                else
                {
                    // Create and add a new item
                    var item = new ListItems
                    {
                        ItemName = name,
                        ItemId = id,
                        ItemStringValues = StringValues,
                        ItemIntegerValues = IntegerValues
                    };

                    _theList.Add(item);
                }

                WriteFile();
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error adding item to the list: {0}", ex.Message);
            }
        }

        public void DeleteItem(int index)
        {
            _theList.RemoveAt(index);

            WriteFile();            
        }

        public void ModifyItem(int index, ushort id, string name)
        {
            try
            {
                _theList[index].ItemName = name;
                _theList[index].ItemId = id;
                _theList[index].ItemStringValues = StringValues;
                _theList[index].ItemIntegerValues = IntegerValues;

                WriteFile();
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error modifying item: {0}", ex.Message);
            }
        }

        public void PrintItem(int index)
        {
            CrestronConsole.PrintLine(_theList[index].ItemId.ToString(CultureInfo.InvariantCulture));
            foreach (var shortValue in _theList[index].ItemIntegerValues)
            {
                CrestronConsole.PrintLine(_theList[index].ItemName + ":" +
                                          shortValue.ToString(CultureInfo.InvariantCulture));
            }
            foreach (var stringValue in _theList[index].ItemStringValues)
            {
                CrestronConsole.PrintLine(_theList[index].ItemName + ":" + stringValue);
            }
        }

        public void Recall(int index)
        {
            _recallEventArgs = new ItemRecallEventArgs
            {
                ItemId = _theList[index].ItemId,
                ItemName = _theList[index].ItemName,
                ItemIntegerValues = _theList[index].ItemIntegerValues,
                ItemStringValues = _theList[index].ItemStringValues
            };
            ItemRecallEvent(this, _recallEventArgs);
        }

        private bool ListContainsValue(int value)
        {
            var contains = false;
            foreach (var listItem in _theList)
            {
                if (listItem.ItemId == value)
                    contains = true;
            }
            return contains;
        }

        private void WriteFile()
        {
            try
            {
                var fileContent = JsonConvert.SerializeObject(_theList);                

                using (var streamWriter = new StreamWriter(FilePath))
                {
                    if (fileContent == null) return;
                    // CrestronConsole.PrintLine(fileContent);
                    streamWriter.Write(fileContent);
                }
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error writing the file: {0}", ex.Message);
            }
            finally
            {
                ReadFile();
            }
        }

        public void ReadFile()
        {
            try
            {
                _fileInfo.Refresh();

                if (_fileInfo.Exists)
                {
                    using (var fileStream = new FileStream(FilePath, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            var fileContent = streamReader.ReadToEnd();
                            _theList.Clear();
                            _theList = JsonConvert.DeserializeObject<List<ListItems>>(fileContent);
                            foreach (var item in _theList)
                            {
                                // CrestronConsole.PrintLine(item.ItemName + ":" + item.ItemName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error writing the file: {0}", ex.Message);
            }
            finally
            {
                _namesArgs = new ItemsNameEventArgs();

                for (var i = 0; i < _theList.Count; i++)
                {
                    _namesArgs.Names[i] = _theList[i].ItemName;
                }

                for (ushort i = 1; i <= 40; i++)
                {
                    if (ListContainsValue(i)) continue;
                    _nextAvailable = i;
                    _namesArgs.NextAvailable = _nextAvailable;
                    break;
                }

                ItemsNameUpdated(this, _namesArgs);
            }
        }

        public void DeleteAll()
        {
            _theList.Clear();
            WriteFile();
        }

        public string FindMyIp()
        {
            return 
            CrestronEthernetHelper.GetEthernetParameter(
                CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_ADDRESS, 0);
        }
    }
}
