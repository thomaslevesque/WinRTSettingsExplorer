using System;
using System.Collections.Generic;
using Windows.Storage;

namespace WinRTSettingsExplorer.ViewModel
{
    public class SettingsValueViewModel : ObservableBase
    {
        private readonly Action<string, object> _valueSetter;

        public SettingsValueViewModel(KeyValuePair<string, object> entry, Action<string, object> valueSetter)
        {
            _valueSetter = valueSetter;
            _name = entry.Key;
            _value = entry.Value;
            var adcv = entry.Value as ApplicationDataCompositeValue;
            if (adcv != null)
            {
                _displayValue = new CompositeValueViewModel(entry.Key, adcv);
                _typeString = "ApplicationDataCompositeValue";
                _type = typeof(ApplicationDataCompositeValue);
            }
            else
            {
                _displayValue = entry.Value;
                if (entry.Value == null)
                {
                    _type = null;
                    _typeString = "(Unknown)";
                }
                else
                {
                    _type = entry.Value.GetType();
                    _typeString = _type.Name;
                }
            }
        }

        private readonly string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private object _value;
        public object Value
        {
            get
            {
                return _value ?? "(null)";
            }
            set
            {
                if (Set(ref _value, value))
                {
                    if (_valueSetter != null)
                        _valueSetter(Name, value);

                    var adcv = value as ApplicationDataCompositeValue;
                    if (adcv != null)
                        DisplayValue = new CompositeValueViewModel(_name, adcv);
                    else
                        DisplayValue = value;
                }
            }
        }

        private object _displayValue;
        public object DisplayValue
        {
            get { return _displayValue; }
            set { Set(ref _displayValue, value); }
        }


        private readonly string _typeString;
        public string TypeString
        {
            get
            {
                return _typeString;
            }
        }

        private readonly Type _type;
        public Type Type
        {
            get { return _type; }
        }

        
    }
}