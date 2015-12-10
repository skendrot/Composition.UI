using Composition.UI.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Composition.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            Pages = new List<PageItem>
            {
                new PageItem { Name="Stacked Parallax", Page = typeof(StackedParallaxPage) },
                new PageItem { Name="Background Parallax", Page=typeof(BackgroundParallaxPage) },
                new PageItem { Name="ListView Header Parallax", Page=typeof(ListViewHeaderParallaxPage) },
                //new PageItem { Name="Stacked with controls", Page=typeof(ParallaxPanelPage) },
                //new PageItem { Name="Circle Mask", Page=typeof(CircleMaskPage) },
            };
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                if (Frame.CanGoBack) Frame.GoBack();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public IEnumerable<PageItem> Pages { get; set; }

        private void PageList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as PageItem;
            Frame.Navigate(item.Page);

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
