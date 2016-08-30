using System.Numerics;
using Composition.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Composition.UI.Behaviors
{
    public class StretchyHeaderBehavior : Behavior<FrameworkElement>
    {
        private ScrollViewer _scroller;

        public double StretchyFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            nameof(StretchyFactor),
            typeof(double),
            typeof(StretchyHeaderBehavior),
            new PropertyMetadata(0.25));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += OnSizeChanged;
            _scroller = AssociatedObject.GetParentOfType<ScrollViewer>();
            if (_scroller == null)
            {
                AssociatedObject.Loaded += OnLoaded;
                return;
            }
            AssignEffect();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _scroller = AssociatedObject.GetParentOfType<ScrollViewer>();
            AssignEffect();
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AssignEffect();
        }

        private void AssignEffect()
        {
            if (_scroller == null) return;

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scroller);

            var compositor = scrollerViewerManipulation.Compositor;

            // See documentation for Lerp and Clamp: 
            // https://msdn.microsoft.com/en-us/windows/uwp/graphics/composition-animation
            var scaleAnimation = compositor.CreateExpressionAnimation(
                 "Lerp(1, 1+Amount, Clamp(ScrollManipulation.Translation.Y/50, 0, 1))");
            scaleAnimation.SetScalarParameter("Amount", (float)StretchyFactor);
            scaleAnimation.SetReferenceParameter("ScrollManipulation", scrollerViewerManipulation);

            var visual = ElementCompositionPreview.GetElementVisual(AssociatedObject);
            var backgroundImageSize = new Vector2((float)AssociatedObject.ActualWidth, (float)AssociatedObject.ActualHeight);
            visual.Size = backgroundImageSize;

            // CenterPoint defaults to the top left (0,0). We want the strecth to occur from the center
            visual.CenterPoint = new Vector3(backgroundImageSize / 2, 1);
            visual.StartAnimation("Scale.X", scaleAnimation);
            visual.StartAnimation("Scale.Y", scaleAnimation);
        }
    }
}
