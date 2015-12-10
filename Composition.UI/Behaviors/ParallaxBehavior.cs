using Composition.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Composition.UI.Behaviors
{
    public class ParallaxBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Gets or sets the element that will parallax while scrolling.
        /// </summary>
        public UIElement ParallaxContent
        {
            get { return (UIElement)GetValue(ParallaxContentProperty); }
            set { SetValue(ParallaxContentProperty, value); }
        }

        public static readonly DependencyProperty ParallaxContentProperty = DependencyProperty.Register(
            "ParallaxContent", 
            typeof(UIElement), 
            typeof(ParallaxBehavior), 
            new PropertyMetadata(null, OnParallaxContentChanged));
        
        /// <summary>
        /// Gets or sets the rate at which the ParallaxContent parallaxes.
        /// </summary>
        public double ParallaxMultiplier
        {
            get { return (double)GetValue(ParallaxMultiplierProperty); }
            set { SetValue(ParallaxMultiplierProperty, value); }
        }

        public static readonly DependencyProperty ParallaxMultiplierProperty = DependencyProperty.Register(
            "ParallaxMultiplier", 
            typeof(double), 
            typeof(ParallaxBehavior), 
            new PropertyMetadata(0.3d));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssignParallax();
        }

        private void AssignParallax()
        {
            if (ParallaxContent == null) return;
            if (AssociatedObject == null) return;

            var scroller = AssociatedObject as ScrollViewer;
            if (scroller == null)
            {
                scroller = AssociatedObject.GetChildOfType<ScrollViewer>();
            }
            if (scroller == null) return;

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            ExpressionAnimation expression = compositor.CreateExpressionAnimation("ScrollManipululation.Translation.Y * ParallaxMultiplier");

            expression.SetScalarParameter("ParallaxMultiplier", (float)ParallaxMultiplier);
            expression.SetReferenceParameter("ScrollManipululation", scrollerViewerManipulation);

            Visual textVisual = ElementCompositionPreview.GetElementVisual(ParallaxContent);
            textVisual.StartAnimation("Offset.Y", expression);
        }

        private static void OnParallaxContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as ParallaxBehavior;
            b.AssignParallax();
        }
    }
}
