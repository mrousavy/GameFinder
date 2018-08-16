using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Jellyfish;

namespace GameFinder.Game
{
    public class GameViewModel : ViewModel
    {
        private ulong _appId;
        private string _iconUrl;

        private ICommand _launchCommand;
        private ICommand _openStoreCommand;

        private string _name;

        public GameViewModel(ulong appId)
        {
            AppId = appId;
            LaunchCommand = new RelayCommand(LaunchAction);
            OpenStoreCommand = new RelayCommand(OpenStoreActin);
        }

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

        public ICommand OpenStoreCommand
        {
            get => _openStoreCommand;
            set => Set(ref _openStoreCommand, value);
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

        private void OpenStoreActin(object o)
        {
            try
            {
                Process.Start($"steam://store/{AppId}");
            } catch (Exception ex1)
            {
                try
                {
                    Process.Start($"https://store.steampowered.com/app/{AppId}");
                } catch (Exception ex2)
                {
                    Debug.WriteLine($"Could not open store page! {ex1.Message} /// {ex2.Message}");
                }
            }
        }

        public override bool Equals(object obj) => obj is GameViewModel model &&
                                                   IconUrl == model.IconUrl &&
                                                   Name == model.Name;

        public override int GetHashCode()
        {
            int hashCode = 1139736271;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IconUrl);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}