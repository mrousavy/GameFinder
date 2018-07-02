using System;
using Jellyfish;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace GameFinder.Game
{
    public class GameViewModel : ViewModel
    {
        private string _iconUrl;

        private string _name;

        private ICommand _launchCommand;

        private ulong _appId;

        public ulong AppId
        {
            get => _appId;
            set => Set(ref _appId, value);
        }
        public string IconUrl
        {
            get => _iconUrl;
            set => Set(ref _iconUrl, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand LaunchCommand
        {
            get => _launchCommand;
            set => Set(ref _launchCommand, value);
        }

        public GameViewModel(ulong appId)
        {
            AppId = appId;
            LaunchCommand = new RelayCommand(LaunchAction);
        }

        private void LaunchAction(object o)
        {
            try
            {
                Process.Start($"steam://run/{AppId}");
            } catch (Exception ex)
            {
                Debug.WriteLine($"Could not launch app! {ex.Message}");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GameViewModel model &&
                   IconUrl == model.IconUrl &&
                   Name == model.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 1139736271;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IconUrl);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}