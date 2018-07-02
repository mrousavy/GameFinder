using System;
using Jellyfish;

namespace GameFinder.ErrorDialog
{
    public class ErrorDialogViewModel : ViewModel
    {
        private string _errorText;

        public ErrorDialogViewModel(string errorText)
        {
            ErrorText = errorText;
        }

        public ErrorDialogViewModel(Exception exception)
        {
            ErrorText = exception.Message;
        }

        public string ErrorText
        {
            get => _errorText;
            set => Set(ref _errorText, value);
        }
    }
}