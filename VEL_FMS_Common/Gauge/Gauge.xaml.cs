using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VEL_FMS_Common.Gauge
{
    /// <summary>
    /// Gauge.xaml 的交互逻辑
    /// </summary>
    public partial class Gauge : UserControl
    {
        #region 依赖属性
        public static readonly DependencyProperty RangeProperty = DependencyProperty.Register("Range", typeof(double), typeof(Gauge), new PropertyMetadata(100.0, OnRangeChanged));
        public double Range
        {
            get => (double)GetValue(RangeProperty);
            set => SetValue(RangeProperty, value);
        }



        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Gauge), new PropertyMetadata(20.0, OnValueChanged));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Gauge()
        {
            InitializeComponent();
            DrawGauge();
        }

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Gauge)d).DrawGauge();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Gauge)d).DrawGauge();
        }

        #endregion

        private const double Radius = 100;
        private const double CenterX = 150;
        private const double CenterY = 150;

        private void DrawGauge()
        {
            CanvasArea.Children.Clear();
            ValueTextBlock.Text = Value.ToString("F1");

            double middata = Value / Range;
            double angle = middata * 180;
            var backgroundArc = new Path
            {
                Stroke = Brushes.LightGray,
                StrokeThickness = 20,
                Fill = Brushes.Transparent,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(CenterX - Radius, CenterY),
                            Segments = new PathSegmentCollection
                            {
                                new ArcSegment
                                {
                                    Point = new Point(CenterX + Radius, CenterY),
                                    Size = new Size(Radius, Radius),
                                    SweepDirection = SweepDirection.Clockwise,
                                    IsLargeArc = false,
                                    RotationAngle = 0
                                }
                            }
                        }
                    }
                }
            };
            CanvasArea.Children.Add(backgroundArc);

            var fillArc = new Path
            {
                Stroke = Brushes.Cyan,
                StrokeThickness = 20,
                Fill = Brushes.Transparent,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(CenterX - Radius, CenterY),
                            Segments = new PathSegmentCollection
                            {
                                new ArcSegment
                                {
                                    Point = new Point(CenterX + Radius * Math.Cos((180 - angle) * Math.PI / 180),
                                                      CenterY - Radius * Math.Sin((180 - angle) * Math.PI / 180)),
                                    Size = new Size(Radius, Radius),
                                    SweepDirection = SweepDirection.Clockwise,
                                    IsLargeArc = angle > 180,
                                    RotationAngle = 0
                                }
                            }
                        }
                    }
                }
            };
            CanvasArea.Children.Add(fillArc);
        }

    }
}
