using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Jellyfish;

namespace GameFinder.User
{
    public class UserViewModel : ViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        private string _realName;
        public string RealName
        {
            get => _realName;
            set => Set(ref _realName, value);
        }

        private string _avatar;
        public string Avatar
        {
            get => _avatar;
            set => Set(ref _avatar, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                Set(ref _url, value);
                UpdateImageSource(value);
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        private void UpdateImageSource(string url)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();
            ImageSource = bitmap;
        }
    }
}
