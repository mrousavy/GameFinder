using Jellyfish;

namespace GameFinder.LoadingDialog
{
    public class LoadingDialogViewModel : ViewModel
    {
        private int _progress;

        private string _title;

        public LoadingDialogViewModel(string title)
        {
            Title = title;
        }

        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
    }
}