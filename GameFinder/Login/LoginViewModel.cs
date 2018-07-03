using System.Diagnostics;
using System.Windows.Input;
using GameFinder.ErrorDialog;
using GameFinder.LoadingDialog;
using Jellyfish;

namespace GameFinder.Login
{
    public class LoginViewModel : ViewModel
    {
        private string _apiKey;

        private ICommand _apiKeyLaunchCommand;

        private object _dialogViewModel;

        private bool _isDialogOpen;

        private ICommand _loginCommand;

        private string _userId;

        private ICommand _userIdLaunchCommand;

        public LoginViewModel()
        {
            Model = new LoginModel();
            LoginCommand = new RelayCommand(LoginAction);
            ApiKeyLaunchCommand = new RelayCommand(ApiKeyAction);
            UserIdLaunchCommand = new RelayCommand(UserIdAction);

            if (Session.Config != null)
            {
                ApiKey = Session.ApiKey;
                UserId = Session.UserId.ToString();
            }
        }

        public string ApiKey
        {
            get => _apiKey;
            set => Set(ref _apiKey, value);
        }

        public string UserId
        {
            get => _userId;
            set => Set(ref _userId, value);
        }

        public ICommand ApiKeyLaunchCommand
        {
            get => _apiKeyLaunchCommand;
            set => Set(ref _apiKeyLaunchCommand, value);
        }

        public ICommand UserIdLaunchCommand
        {
            get => _userIdLaunchCommand;
            set => Set(ref _userIdLaunchCommand, value);
        }

        public ICommand LoginCommand
        {
            get => _loginCommand;
            set => Set(ref _loginCommand, value);
        }

        public object DialogViewModel
        {
            get => _dialogViewModel;
            set
            {
                Set(ref _dialogViewModel, value);
                IsDialogOpen = value != null;
            }
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
        }

        public LoginModel Model { get; }

        private static void UserIdAction(object o)
        {
            Process.Start("https://steamidfinder.com/");
        }

        private static void ApiKeyAction(object o)
        {
            Process.Start("https://steamcommunity.com/dev/apikey");
        }

        private void LoginAction(object o)
        {
            DialogViewModel = new LoadingDialogViewModel("Loading...");
            try
            {
                Model.Login(ApiKey, UserId);
                IsDialogOpen = false;
            } catch
            {
                DialogViewModel = new ErrorDialogViewModel("The given API Key or User ID is invalid!\n\r" +
                                                           "Please check your inputs and try again!");
            }
        }
    }
}