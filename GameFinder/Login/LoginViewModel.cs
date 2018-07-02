﻿using System.Windows.Input;
using GameFinder.ErrorDialog;
using GameFinder.LoadingDialog;
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
            set
            {
                Set(ref _dialogViewModel, value);
                IsDialogOpen = value != null;
            }
        }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
        }

        public LoginModel Model { get; }

        public LoginViewModel()
        {
            Model = new LoginModel();
            LoginCommand = new RelayCommand(LoginAction);

            if (Session.Config != null)
            {
                ApiKey = Session.ApiKey;
                UserId = Session.UserId.ToString();

                if (!string.IsNullOrWhiteSpace(Session.ApiKey) && Session.UserId > 0)
                    LoginAction(null);
            }
        }

        private void LoginAction(object o)
        {
            try
            {
                DialogViewModel = new LoadingDialogViewModel();
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
