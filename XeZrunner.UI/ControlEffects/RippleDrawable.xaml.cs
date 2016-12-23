// XeZrunner Proprietary Projects

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XeZrunner.UI.ControlEffects
{
    // RippleDrawable

    public partial class RippleDrawable : UserControl
    {
        public RippleDrawable()
        {
            InitializeComponent();

            s_rippleEnter = FindResource("Ripple_Enter") as Storyboard;
            s_rippleExit = FindResource("Ripple_Exit") as Storyboard;
        }

        private void usercontrol_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Private variables

        private double m_radius = 0; // the radius the ripple should expand to before fading out
        private float m_speed = 1.0f; // the speed the ripple should expand at

        private Point m_startPoint = new Point(0, 0); // the starting point of the ripple

        private bool m_useCustomStartPoint = false;

        // inner-working vars

        bool DownAnimHappening;

        double translateX;
        double translateY;

        #endregion

        #region Public variables

        /// <summary>
        /// The radius the ripple will expand to before fading out.
        /// </summary>
        [Description("Ripple radius"), Category("Common")]
        public double Radius
        {
            get { return m_radius; }
            set
            {
                m_radius = value;

                var s_WidthHeight = (EasingDoubleKeyFrame)this.Resources["s_WidthHeightKeyFrame"];

                if (m_radius != 0)
                    s_WidthHeight.Value = m_radius;
            }
        }

        /// <summary>
        /// The ripple effect's expansion speed rate. 1.0 = normal
        /// </summary>
        [Description("The speed of the ripple"), Category("Common")]
        public float Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        /// <summary>
        /// The starting point for the ripple.
        /// This is only used if StartPointEnabled = true.
        /// </summary>
        [Description("Ripple start location"), Category("Common")]
        public Point StartPoint
        {
            get { return m_startPoint; }
            set { m_startPoint = value; }
        }

        /// <summary>
        /// Determines whether to use a custom starting point. (StartX and StartY)
        /// false = use mouse location, true = use custom starting point
        /// </summary>
        [Description("Use custom start point"), Category("Common")]
        public bool StartPointEnabled
        {
            get { return m_useCustomStartPoint; }
            set { m_useCustomStartPoint = value; }
        }

        #region Color

        static FrameworkPropertyMetadata ColorPropertyMetaData = new FrameworkPropertyMetadata(new SolidColorBrush(), new PropertyChangedCallback(ColorProperty_Changed));

        static void ColorProperty_Changed(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            RippleDrawable main = dobj as RippleDrawable;
            main.rippleEllipse.Fill = e.NewValue as SolidColorBrush;
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(RippleDrawable), ColorPropertyMetaData);

        /// <summary>
        /// The color of the ripple.
        /// </summary>
        /// 
        [Description("The color of the ripple"), Category("Brush")]
        public SolidColorBrush Color
        {
            get { return GetValue(ColorProperty) as SolidColorBrush; }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        #endregion

        #region Fill color

        static FrameworkPropertyMetadata FillColorPropertyMetaData = new FrameworkPropertyMetadata(new SolidColorBrush(), new PropertyChangedCallback(FillColorProperty_Changed));

        static void FillColorProperty_Changed(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            RippleDrawable main = dobj as RippleDrawable;
            main.rippleBackgroundFill.Fill = e.NewValue as SolidColorBrush;
        }

        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(SolidColorBrush), typeof(RippleDrawable), FillColorPropertyMetaData);

        /// <summary>
        /// The color of the mouse down fill.
        /// </summary>
        [Description("The mouse down fill color"), Category("Brush")]
        public SolidColorBrush FillColor
        {
            get { return GetValue(FillColorProperty) as SolidColorBrush; }
            set
            {
                SetValue(FillColorProperty, value);
            }
        }

        #endregion

        #endregion

        Storyboard s_rippleEnter;
        Storyboard s_rippleExit;

        DispatcherTimer LongDowntimer = new DispatcherTimer();

        // Size changed
        private void usercontrol_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var s_WidthHeight = (EasingDoubleKeyFrame)this.Resources["s_WidthHeightKeyFrame"];
            var s_Margin = (EasingThicknessKeyFrame)this.Resources["s_MarginKeyFrame"];

            s_Margin.Value = new Thickness(-this.ActualWidth);

            if (m_radius == 0)
                s_WidthHeight.Value = (this.ActualWidth + this.ActualHeight);

            rippleGrid.Width = this.ActualWidth * 2;
            rippleGrid.Height = this.ActualHeight * 2;

            rippleGrid.Margin = new Thickness(-this.ActualWidth / 2, -this.ActualHeight / 2, -this.ActualWidth / 2, -this.ActualHeight / 2);
        }

        // Click!
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DownAnimHappening)
            {
                // start pos

                if (m_useCustomStartPoint)
                {
                    translateX = m_startPoint.X;
                    translateY = m_startPoint.Y;
                }
                else
                {
                    translateX = Mouse.GetPosition(this).X - this.ActualWidth / 2;
                    translateY = Mouse.GetPosition(this).Y - this.ActualHeight / 2;
                }

                var s_GoToMiddleX = (EasingDoubleKeyFrame)this.Resources["s_GoToMiddleKeyFrameX"];
                var s_GoToMiddleY = (EasingDoubleKeyFrame)this.Resources["s_GoToMiddleKeyFrameY"];

                s_GoToMiddleX.Value = translateX;
                s_GoToMiddleY.Value = translateY;

                //TranslateTransform myTranslate = new TranslateTransform();
                //myTranslate.X = translateX;
                //myTranslate.Y = translateY;

                //rippleEllipse.RenderTransform = myTranslate;

                // move towards center

                //TranslateTransform myTranslate2 = new TranslateTransform();

                //myTranslate2.X = translateX;
                //myTranslate2.Y = translateY;

                //DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(m_speed * 2) );
                //myTranslate2.BeginAnimation(TranslateTransform.XProperty, anim);
                //myTranslate2.BeginAnimation(TranslateTransform.YProperty, anim);

                //rippleEllipse.RenderTransform = myTranslate2;

                // start anim

                s_rippleEnter.Begin();
                s_rippleEnter.SetSpeedRatio(m_speed * 6);

                DispatcherTimer endTimer = new DispatcherTimer();
                endTimer.Interval = TimeSpan.FromMilliseconds(300);
                endTimer.Tick += (s, ev) => { endTimer.Stop(); usercontrol_PreviewMouseUp(this, new RoutedEventArgs()); };
                endTimer.Start();
            }
        }

        // Hold down
        private void usercontrol_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LongDowntimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            LongDowntimer.Tick += (s1, ev) =>
            {
                LongDowntimer.Stop();

                // start pos

                if (m_useCustomStartPoint)
                {
                    translateX = m_startPoint.X;
                    translateY = m_startPoint.Y;
                }
                else
                {
                    translateX = e.MouseDevice.GetPosition(this).X - this.ActualWidth / 2;
                    translateY = e.MouseDevice.GetPosition(this).Y - this.ActualHeight / 2;
                }

                var s_GoToMiddleX = (EasingDoubleKeyFrame)this.Resources["s_GoToMiddleKeyFrameX"];
                var s_GoToMiddleY = (EasingDoubleKeyFrame)this.Resources["s_GoToMiddleKeyFrameY"];

                s_GoToMiddleX.Value = translateX;
                s_GoToMiddleY.Value = translateY;

                //TranslateTransform myTranslate = new TranslateTransform();
                //myTranslate.X = translateX;
                //myTranslate.Y = translateY;

                //rippleEllipse.RenderTransform = myTranslate;

                // move towards center

                //TranslateTransform myTranslate2 = new TranslateTransform();

                //myTranslate2.X = translateX;
                //myTranslate2.Y = translateY;

                //DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(m_speed * 2) );
                //myTranslate2.BeginAnimation(TranslateTransform.XProperty, anim);
                //myTranslate2.BeginAnimation(TranslateTransform.YProperty, anim);

                //rippleEllipse.RenderTransform = myTranslate2;

                // start anim

                s_rippleEnter.Begin();
                s_rippleEnter.SetSpeedRatio(m_speed);

                DownAnimHappening = true;
            };
            LongDowntimer.Start();
        }

        // Release
        private void usercontrol_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            LongDowntimer.Stop();

            DispatcherTimer timer1 = new DispatcherTimer();
            if (DownAnimHappening)
                timer1.Interval = TimeSpan.FromSeconds(0);
            else
                timer1.Interval = TimeSpan.FromSeconds( ( m_speed / 6 ) + 0.3 ); // this is a bit buggy
            timer1.Tick += (s2, ev2) =>
            {
                timer1.Stop();
                s_rippleExit.Begin();
            };
            timer1.Start();

            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromSeconds(.1);
            timer2.Tick += (s2, ev2) =>
            {
                timer2.Stop();

                DownAnimHappening = false;
                //i_allowMouseDisposition = true;
            };
            timer2.Start();
        }

        // Move
        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (e.LeftButton == MouseButtonState.Pressed & i_allowMouseDisposition)
            {
                translateX = e.MouseDevice.GetPosition(this).X - this.ActualWidth / 2;
                translateY = e.MouseDevice.GetPosition(this).Y - this.ActualHeight / 2;

                if ((translateX >= this.ActualWidth / 2 || translateX <= -this.ActualWidth / 2) || (translateY >= this.ActualHeight / 2 || translateY <= -this.ActualHeight / 2))
                {
                    s_rippleEnter.SetSpeedRatio(1); return;
                }

                TranslateTransform myTranslate = new TranslateTransform();
                myTranslate.X = translateX;
                myTranslate.Y = translateY;

                rippleEllipse.RenderTransform = myTranslate;
            }
            */
        }
    }
}