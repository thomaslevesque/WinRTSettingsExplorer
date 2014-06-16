using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.Storage;
using Microsoft.Win32;
using WinRTSettingsExplorer.ViewModel;

namespace WinRTSettingsExplorer.Model
{
    public class SettingsItem
    {
        private readonly string _name;
        private readonly Type _type;
        private readonly int _size;
        private readonly DateTimeOffset _date;
        private readonly object _value;

        private SettingsItem(string name, Type type, int size, DateTimeOffset date, object value)
        {
            _name = name;
            _type = type;
            _size = size;
            _date = date;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public int Size
        {
            get { return _size; }
        }

        public DateTimeOffset Date
        {
            get { return _date; }
        }

        public static SettingsItem Read(RegistryKey key, string name)
        {
            object value;
            Type type = typeof(object);
            int size = 0;
            DateTimeOffset date = DateTimeOffset.MinValue;
            try
            {
                value = ReadValue(key, name, ref type, ref size, ref date);
            }
            catch (Exception ex)
            {
                value = string.Format("[Error: {0}]", ex.Message);
            }
            return new SettingsItem(name, type, size, date, value);
        }

        private static object ReadValue(RegistryKey key, string name, ref Type type, ref int size, ref DateTimeOffset date)
        {
            var eType = key.GetValueKind(name);
            if (eType != RegistryValueKind.Unknown)
                return key.GetValue(name);

            int lpType, lpcbData;
            int r = RegistryInterop.RegQueryValueEx(key.Handle, name, null, out lpType, null, out lpcbData);
            if (r != 0)
                return null;
            var lpData = new byte[lpcbData];
            r = RegistryInterop.RegQueryValueEx(key.Handle, name, null, out lpType, lpData, out lpcbData);
            if (r != 0 || lpcbData == 0)
                return null;

            size = lpcbData - 8;
            long timestamp = BitConverter.ToInt64(lpData, lpcbData - 8);
            date = DateTimeOffset.FromFileTime(timestamp);

            switch (lpType)
            {
                case 0x05f5e101: // Byte
                    type = typeof(byte);
                    return lpData[0];
                case 0x05f5e102: // Int16
                    type = typeof(short);
                    return BitConverter.ToInt16(lpData, 0);
                case 0x05f5e103: // UInt16
                    type = typeof(ushort);
                    return BitConverter.ToUInt16(lpData, 0);
                case 0x05f5e104: // Int32
                    type = typeof(int);
                    return BitConverter.ToInt32(lpData, 0);
                case 0x05f5e105: // UInt32
                    type = typeof(uint);
                    return BitConverter.ToUInt32(lpData, 0);
                case 0x05f5e106: // Int64
                    type = typeof(long);
                    return BitConverter.ToInt64(lpData, 0);
                case 0x05f5e107: // UInt64
                    type = typeof(ulong);
                    return BitConverter.ToUInt64(lpData, 0);
                case 0x05f5e108: // Single
                    type = typeof(float);
                    return BitConverter.ToSingle(lpData, 0);
                case 0x05f5e109: // Double
                    type = typeof(double);
                    return BitConverter.ToDouble(lpData, 0);
                case 0x05f5e10a: // Char
                    type = typeof(char);
                    return BitConverter.ToChar(lpData, 0);
                case 0x05f5e10b: // Boolean
                    type = typeof(bool);
                    return BitConverter.ToBoolean(lpData, 0);
                case 0x05f5e10c: // String
                    type = typeof(string);
                    return Encoding.Unicode.GetString(lpData, 0, lpcbData - 8).TrimEnd('\0');
                case 0x05f5e10d: // CompositeValue
                {
                    type = typeof(ApplicationDataCompositeValue);
                    string path = Path.Combine(@"D:\tmp\acvd", name + ".bin");
                    File.WriteAllBytes(path, lpData);
                    throw new NotImplementedException();
                }
                case 0x05f5e10e: // DateTimeOffset
                {
                    type = typeof(DateTimeOffset);
                    long d = BitConverter.ToInt64(lpData, 0);
                    return DateTimeOffset.FromFileTime(d);
                }
                case 0x05f5e10f: // TimeSpan
                {
                    type = typeof(TimeSpan);
                    long ticks = BitConverter.ToInt64(lpData, 0);
                    return TimeSpan.FromTicks(ticks);
                }
                case 0x05f5e110: // Guid
                {
                    type = typeof(Guid);
                    var guidBytes = lpData.Take(16).ToArray();
                    return new Guid(guidBytes);
                }
                case 0x05f5e111: // Point
                {
                    type = typeof(Point);
                    double x = BitConverter.ToSingle(lpData, 0);
                    double y = BitConverter.ToSingle(lpData, 4);
                    return new Point(x, y);
                }
                case 0x05f5e112: // Size
                {
                    type = typeof(Size);
                    double w = BitConverter.ToSingle(lpData, 0);
                    double h = BitConverter.ToSingle(lpData, 4);
                    return new Size(w, h);
                }
                case 0x05f5e113: // Rect
                {
                    type = typeof(Rect);
                    double x = BitConverter.ToSingle(lpData, 0);
                    double y = BitConverter.ToSingle(lpData, 4);
                    double w = BitConverter.ToSingle(lpData, 8);
                    double h = BitConverter.ToSingle(lpData, 12);
                    return new Rect(x, y, w, h);
                }
            }

            return null;
        }
    }
}
