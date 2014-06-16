using Windows.ApplicationModel;
using Windows.Management.Core;

namespace WinRTSettingsExplorer.ViewModel
{
    public class PackageViewModel : ObservableBase
    {
        private readonly Package _package;

        public PackageViewModel(Package package)
        {
            _package = package;
        }

        public string Name
        {
            get { return _package.Id.Name; }
        }

        private SettingsContainerViewModel[] _settings;
        public SettingsContainerViewModel[] Settings
        {
            get
            {
                return _settings ?? (_settings = LoadSettings());
            }
        }

        private SettingsContainerViewModel[] LoadSettings()
        {
            var appData = ApplicationDataManager.CreateForPackageFamily(_package.Id.FamilyName);
            return new[]
                   {
                       new SettingsContainerViewModel(appData.LocalSettings, "LocalSettings"),
                       new SettingsContainerViewModel(appData.RoamingSettings, "RoamingSettings")
                   };

            //string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //string settingsPath = Path.Combine(localAppData, "Packages", _package.Id.FamilyName, "Settings", "settings.dat");
            //if (!File.Exists(settingsPath))
            //    return null;

            //SafeRegistryHandle handle;
            //int r = RegistryInterop.RegLoadAppKey(settingsPath, out handle, RegistryInterop.RegSam.Read , 0, 0);
            //if (r != 0)
            //    return null;

            //using (var key = RegistryKey.FromHandle(handle))
            //{
            //    var rootContainer = SettingsContainer.Read(key, "Root");
            //    var vm = new SettingsContainerViewModel(rootContainer);
            //    return vm.Containers.ToArray();
            //}
        }
    }
}