using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Crestron.SimplSharp; // For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;
using Abscraft.Licence;

namespace Abscraft_TheList
{
    public class TheList
    {
        private static string _fileName = string.Empty;
        private ushort _nextAvailable;
        private List<ListItems> _theList;

        private static string _filePath = string.Empty;
        private FileInfo _fileInfo;

        public event EventHandler<ItemsNameEventArgs> ItemsNameUpdated = delegate { };
        private ItemsNameEventArgs _namesArgs;

        public event EventHandler<ItemRecallEventArgs> ItemRecallEvent = delegate { };
        private ItemRecallEventArgs _recallEventArgs;

        public ushort[] IntegerValues { get; set; }
        public string[] StringValues { get; set; }

        private AbscraftLicence _licence;
        public ushort IsLicenced = 0;

        /// <summary>
        /// SIMPL+ can only execute the default constructor. If you have variables that require initialization, please
        /// use an Initialize method
        /// </summary>
        public TheList()
        {
            _licence = new AbscraftLicence();

            //if (!_licence.LicenceCheck())
            //    return;

            IsLicenced = 1;

            _theList = new List<ListItems>();
            IntegerValues = new ushort[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            StringValues = new[]
            {
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };
        }

        public string FileName
        {
            set
            {
                _fileName = value;
                _filePath = string.Format("/nvram/{0}.json", _fileName);
                _fileInfo = new FileInfo(_filePath);
            }
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
                var item = new ListItems
                {
                    ItemName = name,
                    ItemId = id,
                    ItemStringValues = StringValues,
                    ItemIntegerValues = IntegerValues
                };
                
                _theList.Add(item);

                WriteFile();
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error adding item to the list: {0}", ex.Message);
            }
        }

        public void DeleteItem(int index)
        {
            try
            {
                _theList.RemoveAt(index);
                WriteFile();         
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error deleting item: {0}", ex.Message);                
            }   
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
            try
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
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error printing item: {0}", ex.Message);
            }
        }

        public void Recall(int index)
        {
            try
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
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error recalling item: {0}", ex.Message);                
            }            
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

                using (var streamWriter = new StreamWriter(_filePath))
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
                    using (var fileStream = new FileStream(_filePath, FileMode.Open))
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
                try
                {
                    _namesArgs = new ItemsNameEventArgs();

                    for (var i = 0; i < _theList.Count; i++)
                    {
                        _namesArgs.Names[i] = _theList[i].ItemName;

                        for (int j = 0; j < _theList[i].ItemStringValues.Length; j++)
                        {
                            _namesArgs.ItemsStringValues[i].Strings[j] = _theList[i].ItemStringValues[j];
                        }
                    }

                    for (ushort i = 0; i <= _theList.Count; i++)
                    {
                        if (_theList.Count == 0)
                        {
                            _nextAvailable = 1;
                        }
                        else
                        {
                            if (ListContainsValue(i + 1)) continue;
                            _nextAvailable = (ushort)(i + 1);   
                        }
                        _namesArgs.NextAvailable = _nextAvailable;
                        break;
                    }

                    ItemsNameUpdated(this, _namesArgs);
                }
                catch (Exception ex)
                {
                    CrestronConsole.PrintLine("Error in finally: {0}", ex.Message);
                }
            }
        }

        public void DeleteAll()
        {
            _theList.Clear();
            WriteFile();
        }
    }
}
