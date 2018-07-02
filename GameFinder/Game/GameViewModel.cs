using Jellyfish;

namespace GameFinder.Game
{
    public class GameViewModel : ViewModel
    {
        private string _iconUrl;

        private string _name;

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
    }
}