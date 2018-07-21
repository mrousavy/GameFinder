using System.Windows;
using System.Windows.Controls;

namespace GameFinder.Finder
{
    /// <summary>
    ///     Interaction logic for FinderView.xaml
    /// </summary>
    public partial class FinderView : UserControl
    {
        public FinderView()
        {
            InitializeComponent();
        }

        private void ScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = ScrollViewer.ActualWidth;
            if (DataContext is FinderViewModel model)
                model.TileColumns = (int) width / 300; // 300 is a Tile's width
        }
    }
}