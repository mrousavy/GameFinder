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
    }
}
