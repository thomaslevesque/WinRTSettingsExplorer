using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace WinRTSettingsExplorer.ViewModel
{
    public class SettingsContainerViewModel : ObservableBase
    {
        private readonly ApplicationDataContainer _container;
        private readonly string _name;

        public SettingsContainerViewModel(ApplicationDataContainer container, string name = null)
        {
            _container = container;
            _name = name ?? _container.Name;
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<SettingsValueViewModel> Values
        {
            get { return _container.Values.Select(v => new SettingsValueViewModel(v, ValueSetter)); }
        }

        public IEnumerable<SettingsContainerViewModel> Containers
        {
            get { return _container.Containers.Select(c => new SettingsContainerViewModel(c.Value)); }
        }

        private void ValueSetter(string name, object value)
        {
            _container.Values[name] = value;
        }
    }
}
