using System.Windows.Input;
using GameFinder.ErrorDialog;
using Jellyfish;
using MaterialDesignThemes.Wpf.Transitions;
using SteamWebAPI2.Interfaces;

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

        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get => _loginCommand;
            set => Set(ref _loginCommand, value);
        }

        private object _dialogViewModel;
        public object DialogViewModel
        {
            get => _dialogViewModel;
            set => Set(ref _dialogViewModel, value);
        }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginAction);

            ApiKey = Session.ApiKey;
            UserId = Session.UserId.ToString();

            if (!string.IsNullOrWhiteSpace(Session.ApiKey) && Session.UserId > 0)
            {
                LoginAction(null);
            }
        }

        private void LoginAction(object o)
        {
            try
            {
                Session.ApiKey = ApiKey;
                Session.UserId = long.Parse(UserId);
                Session.SteamUser = new SteamUser(ApiKey);
                Transitioner.MoveNextCommand.Execute(null, null);
            } catch
            {
                DialogViewModel = new ErrorDialogViewModel("The given API Key or User ID is invalid!\n\r" +
                                                           "Please check your inputs and try again!");
                IsDialogOpen = true;
            }
        }
    }
}
