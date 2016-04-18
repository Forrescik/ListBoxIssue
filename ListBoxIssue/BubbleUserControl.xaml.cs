using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ListBoxIssue.ViewModel;

namespace ListBoxIssue
{
    /// <summary>
    /// Interaction logic for BubbleUserControl.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class BubbleUserControl : UserControl
    {
        public MessageViewModel MessageViewModel { get; set; }

        public BubbleUserControl()
        {
            DataContext = this;
            //MessageViewModel = messageViewModel;
            InitializeComponent();
            Loaded += OnLoaded;
            LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            if (sender != null)
            {
                
            }

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);

            // Subscribe to PresentationSource's ContentRendered event
            if (presentationSource != null)
            {
                presentationSource.ContentRendered += TestUserControl_ContentRendered;
            }
        }

        private void TestUserControl_ContentRendered(object sender, EventArgs e)
        {
            ((PresentationSource)sender).ContentRendered -= TestUserControl_ContentRendered;


        }
    }
}
