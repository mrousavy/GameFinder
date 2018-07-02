using System;
using Jellyfish;

namespace GameFinder.Game
{
    public class GameViewModel : ViewModel
    {
        private string _iconUrl;

        private string _name;

        private TimeSpan _playtime;

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

        public TimeSpan Playtime
        {
            get => _playtime;
            set => Set(ref _playtime, value);
        }
    }
}