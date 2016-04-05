using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ListBoxIssue.ViewModel;

namespace ListBoxIssue
{
    /// <summary>
    /// Interaction logic for BubbleUserControl.xaml
    /// </summary>
    public partial class BubbleUserControl : UserControl
    {
        public MessageViewModel MessageViewModel { get; set; }

        public BubbleUserControl()
        {
            DataContext = this;
            //MessageViewModel = messageViewModel;
            InitializeComponent();
            this.Loaded += OnLoaded;
            this.LayoutUpdated += OnLayoutUpdated;
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
            presentationSource.ContentRendered += TestUserControl_ContentRendered;


        }

        private void TestUserControl_ContentRendered(object sender, EventArgs e)
        {
            ((PresentationSource)sender).ContentRendered -= TestUserControl_ContentRendered;


        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
    }
}
