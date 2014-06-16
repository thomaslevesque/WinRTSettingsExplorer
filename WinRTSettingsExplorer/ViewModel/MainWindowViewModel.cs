using System.Collections.ObjectModel;
using System.Linq;
using Windows.Management.Deployment;

namespace WinRTSettingsExplorer.ViewModel
{
    public class MainWindowViewModel : ObservableBase
    {
        private ObservableCollection<PackageViewModel> _packages;
        private PackageViewModel _selectedPackage;

        public MainWindowViewModel()
        {
            var packageManager = new PackageManager();
            Packages =
                new ObservableCollection<PackageViewModel>(
                    packageManager.FindPackages().Select(p => new PackageViewModel(p)).OrderBy(p => p.Name));
        }

        public ObservableCollection<PackageViewModel> Packages
        {
            get { return _packages; }
            private set { Set(ref _packages, value); }
        }

        public PackageViewModel SelectedPackage
        {
            get { return _selectedPackage; }
            set { Set(ref _selectedPackage, value); }
        }
    }
}