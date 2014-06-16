using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace WinRTSettingsExplorer.ViewModel
{
    public class CompositeValueViewModel : ObservableBase
    {
        private readonly string _name;
        private readonly ApplicationDataCompositeValue _compositeValue;

        public CompositeValueViewModel(string name, ApplicationDataCompositeValue compositeValue)
        {
            _name = name;
            _compositeValue = compositeValue;
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<SettingsValueViewModel> Items
        {
            get { return _compositeValue.Select(kvp => new SettingsValueViewModel(kvp, ValueSetter)); }
        }

        private void ValueSetter(string name, object value)
        {
            _compositeValue.Add(name, value);
        }
    }
}
