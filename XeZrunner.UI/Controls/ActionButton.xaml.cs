﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XeZrunner.UI.Controls
{
    public partial class ActionButton : UserControl
    {
        public ActionButton()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Click;

        [Description("The color the Text's Foreground changes to on mouseover"), Category("Brush")]
        public SolidColorBrush MouseOverColor
        {
            get { return this.Resources["MouseOverColor"] as SolidColorBrush; }
            set
            {
                this.Resources["MouseOverColor"] = value;
            }
        }

        [Description("The icon of the button"), Category("Common")]
        public string Icon
        {
            get { return iconLabel.Content as string; }
            set { iconLabel.Content = value; }
        }

        [Description("The text of the button"), Category("Common")]
        public string Text
        {
            get { return textLabel.Content as string; }
            set { textLabel.Content = value.ToUpper(); }
        }

        [Description("The horizontal alignment of the Text"), Category("Common")]
        public HorizontalAlignment TextAlignment
        {
            get { return textLabel.HorizontalAlignment; }
            set { textLabel.HorizontalAlignment = value; }
        }

        [Description("RippleDrawable Color"), Category("Brush")]
        public SolidColorBrush RippleDrawable_Color
        {
            get { return rippledrawable.Color; }
            set { rippledrawable.Color = value; }
        }

        [Description("RippleDrawable FillColor"), Category("Brush")]
        public SolidColorBrush RippleDrawable_FillColor
        {
            get { return rippledrawable.FillColor; }
            set { rippledrawable.FillColor = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
