using Jellyfish;

namespace GameFinder.LoadingDialog
{
    public class LoadingDialogViewModel : ViewModel
    {
        private int _progress;

        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public LoadingDialogViewModel(string title)
        {
            Title = title;
        }
    }
}