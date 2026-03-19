using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PongOne
{
	public partial class MainWindow : Window
	{
		DispatcherTimer timer=new DispatcherTimer();
		Random rnd=new Random();
		double dx, dy;
		int Score
		{
			get => int.Parse(lbScore.Content.ToString());	
			set => lbScore.Content = value.ToString();
		}
		public MainWindow()
		{
			InitializeComponent();
			timer.Interval=TimeSpan.FromMilliseconds(100);
			timer.Tick += Timer_Tick;
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{	double Ballx = (double)elBall.GetValue(Canvas.LeftProperty);
			double Bally = (double)elBall.GetValue(Canvas.TopProperty);
			double Paddlex = (double)rcPaddle.GetValue(Canvas.LeftProperty);
			double Paddley = (double)rcPaddle.GetValue(Canvas.TopProperty); ;	
			double newx = Ballx + dx;
			if (newx <= 0)
			{	newx = 0;	dx = -dx;
			}
			double newy = Bally + dy;
			if (newy <= 0)
			{	newy = 0;	dy = -dy;
			}
			else
				if (newy >= cvPlayground.ActualHeight - elBall.ActualHeight)
				{	newy = cvPlayground.ActualHeight - elBall.ActualHeight;
					dy = -dy;
				}
			if (newx > Paddlex - elBall.ActualWidth &&
				newy >= Paddley - elBall.ActualHeight / 2 &&
				newy <= Paddley + rcPaddle.ActualHeight - elBall.ActualHeight / 2)
			{	newx = Paddlex - elBall.ActualWidth; dx = -dx;
			}
			MoveBallTo(newx, newy);
			if (newx > cvPlayground.ActualWidth)
			{	Score--;
				MoveBallToRandomPosition();
			}
			if (Score == 0)
			{
				MessageBox.Show("Game over!");
      }
		}

		private void btStart_Click(object sender, RoutedEventArgs e)
		{
			rcPaddle.SetValue(Canvas.LeftProperty,
				cvPlayground.ActualWidth - rcPaddle.ActualWidth);
			MoveBallToRandomPosition();
			timer.Start();
		}

		private void btStop_Click(object sender, RoutedEventArgs e)
		{
			timer.Stop();	
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			double step = 10;
			double newy = 0;
			double y = (double)rcPaddle.GetValue(Canvas.TopProperty);
			if (e.Key == Key.Up)
			{
				newy = Math.Max(0,y-step);
			}
			else
				if(e.Key == Key.Down) 
				{ newy=Math.Min(y+step,cvPlayground.ActualHeight-rcPaddle.ActualHeight);	
				}
			rcPaddle.SetValue(Canvas.TopProperty, newy);	
		}
		private void MoveBallTo(double x, double y) 
		{
			elBall.SetValue(Canvas.LeftProperty,x);
			elBall.SetValue(Canvas.TopProperty,y);
		}
		private void MoveBallToRandomPosition()
		{
			MoveBallTo(rnd.NextDouble()*(
				cvPlayground.ActualWidth-rcPaddle.ActualWidth-elBall.ActualWidth),
				rnd.NextDouble()*(
				cvPlayground.ActualHeight-elBall.ActualHeight));
			dx = rnd.Next(-20, 20);
			dy = rnd.Next(-20, 20);
		}
	}
}
