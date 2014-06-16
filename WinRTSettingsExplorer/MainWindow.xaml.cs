using System;
using System.Windows;
using System.Windows.Documents;
using WinRTSettingsExplorer.ViewModel;

namespace WinRTSettingsExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void CompositeDetailClick(object sender, RoutedEventArgs e)
        {
            var element = (Hyperlink)sender;
            var sv = (SettingsValueViewModel)element.DataContext;
            var composite = sv.DisplayValue as CompositeValueViewModel;
            if (composite == null)
                return;
            var window = new CompositeValueWindow { DataContext = composite, Owner = this };
            window.ShowDialog();
        }

        private void EditLinkClick(object sender, RoutedEventArgs e)
        {
            var element = (Hyperlink)sender;
            var sv = (SettingsValueViewModel)element.DataContext;
            var editorType = typeof(ValueEditorWindow<>).MakeGenericType(sv.Type);
            var editor = (ValueEditorWindow) Activator.CreateInstance(editorType);
            editor.Value = sv.Value;
            editor.ValueType = sv.TypeString;
            editor.ValueName = sv.Name;
            if (editor.ShowDialog() == true)
            {
                sv.Value = editor.Value;
            }
        }
    }
}

