using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WinRTSettingsExplorer.ViewModel;

namespace WinRTSettingsExplorer.XamlUtil
{
    [ContentProperty("Templates")]
    public class SettingsValueTemplateSelector : DataTemplateSelector
    {
        public SettingsValueTemplateSelector()
        {
            Templates = new Dictionary<Type, DataTemplate>();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var v = item as SettingsValueViewModel;
            if (v != null)
            {
                Debug.WriteLine("SelectTemplate: " + v.Name);
                object value = v.DisplayValue;
                if (value != null)
                {
                    Type type = value.GetType();
                    DataTemplate template;
                    if (Templates.TryGetValue(type, out template))
                        return template;
                }
            }
            return DefaultTemplate ?? base.SelectTemplate(item, container);
        }

        public DataTemplate DefaultTemplate { get; set; }

        public Dictionary<Type, DataTemplate> Templates
        {
            get;
            set;
        }
    }
}
