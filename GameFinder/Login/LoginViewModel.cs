using Jellyfish;

namespace GameFinder.Login
{
    public class LoginViewModel : ViewModel
    {
        private string _apiKey;

        public string ApiKey
        {
            get => _apiKey;
            set => Set(ref _apiKey, value);
        }


        private string _userId;

        public string UserId
        {
            get => _userId;
            set => Set(ref _userId, value);
        }
    }
}
