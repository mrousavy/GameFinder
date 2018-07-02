using System;
using Jellyfish;

namespace GameFinder.Game
{
    public class GameViewModel : ViewModel
    {

        private string _iconUrl;
        public string IconUrl
        {
            get => _iconUrl;
            set => Set(ref _iconUrl, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private TimeSpan _playtime;
        public TimeSpan Playtime
        {
            get => _playtime;
            set => Set(ref _playtime, value);
        }
    }
}
