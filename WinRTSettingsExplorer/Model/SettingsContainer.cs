using System.Linq;
using Microsoft.Win32;

namespace WinRTSettingsExplorer.Model
{
    public class SettingsContainer
    {
        private readonly string _name;
        private readonly SettingsItem[] _values;
        private readonly SettingsContainer[] _containers;

        private SettingsContainer(string name, SettingsItem[] values, SettingsContainer[] containers)
        {
            _name = name;
            _values = values;
            _containers = containers;
        }

        public string Name
        {
            get { return _name; }
        }

        public SettingsItem[] Values
        {
            get { return _values; }
        }

        public SettingsContainer[] Containers
        {
            get { return _containers; }
        }

        public static SettingsContainer Read(RegistryKey key, string containerName)
        {
            var valueNames = key.GetValueNames();
            var values = valueNames.Select(name => SettingsItem.Read(key, name));

            var subKeyNames = key.GetSubKeyNames();
            var containers =
                subKeyNames.Select(name =>
                {
                    using (var subKey = key.OpenSubKey(name))
                        return Read(subKey, name);
                });

            return new SettingsContainer(containerName, values.ToArray(), containers.ToArray());
        }
    }
}