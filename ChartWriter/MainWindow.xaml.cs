﻿using System;
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
using System.Windows.Threading;

namespace ChartWriter
{
   
    public partial class MainWindow : Window
    {
        public DispatcherTimer SamplingTimer { get; set; }
        public Random MVGenerator { get; set; }
        public const int cMaxNumOfMV = 300;
        public const double MaxMV = 100.0;
        public const double MinMV = 0.0;
        public const double cBorderTop = 0.05;
        public const double cBorderBottom = 0.15;
        public const double cBorderLeft = 0.05;
        public const double cBorderRight = 0.05;

        public List <double> MVList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblMesswert.Content = slider_value.Value;
            progbar.Value = slider_value.Value;
        }

        private void sliderSampling_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblSampling.Content = sliderSampling.Value + "ms";

        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            SamplingTimer.IsEnabled = !SamplingTimer.IsEnabled;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MVGenerator = new Random();
            MVList = new List<double>();
            SamplingTimer = new DispatcherTimer { IsEnabled= false,Interval = new TimeSpan(0,0,0,0,50)};
            SamplingTimer.Tick += SamplingTimer_Tick;
            
        }

        private void SamplingTimer_Tick(object? sender, EventArgs e)
        {
            canChart.Children.Clear();
            double newMV = MVGenerator.NextDouble()*(MaxMV - MinMV) + MinMV;
            MVList.Add(newMV);
            while (MVList.Count > cMaxNumOfMV)
            {
                MVList.RemoveAt(0);
            }

            PointCollection pc = new PointCollection();
            for (int index = 0; index < MVList.Count; index++)
            {
                pc.Add(new Point(Index2PixX(index) , Index2Pix( MVList[index])));

            }
            Polyline plChart = new Polyline
            {
                Stroke = new SolidColorBrush(Colors.Yellow),
                Points = pc         
            
            
            };
            canChart.Children.Add(plChart);
        }

        private double Index2PixX(double index)
        {
            double percent = index / cMaxNumOfMV ;
            return percent * canChart.ActualWidth * (1.0 - cBorderLeft - cBorderRight) + canChart.ActualWidth*cBorderLeft;
        }

        private double Index2Pix(double mv)
        {
            double percent = (mv  - MinMV) / (MaxMV - MinMV) ;
            return canChart.ActualHeight-(percent * canChart.ActualHeight * (1.0 - cBorderTop - cBorderBottom) + canChart.ActualHeight* cBorderBottom);
        }
    }
}
