using Jellyfish;
using System.Collections.Generic;

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