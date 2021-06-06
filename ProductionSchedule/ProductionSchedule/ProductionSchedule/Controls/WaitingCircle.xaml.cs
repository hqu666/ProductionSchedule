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
using System.Windows.Media.Animation;   //DoubleAnimationUsingKeyFramesなど
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace ProductionSchedule.Controls {
	/// <summary>
	/// WaitingCircle.xaml の相互作用ロジック
	/// </summary>
	public partial class WaitingCircle : UserControl {
		public static readonly DependencyProperty CircleColorProperty =
			DependencyProperty.Register(
				"CircleColor", // プロパティ名を指定
				typeof(Color), // プロパティの型を指定
				typeof(WaitingCircle), // プロパティを所有する型を指定
									   //一番濃い時の色
				new UIPropertyMetadata(Color.FromRgb(90, 90, 90),
					//		new UIPropertyMetadata(Color.FromRgb(90, 117, 153),
					(d, e) => { (d as WaitingCircle).OnCircleColorPropertyChanged(e); }));
		public Color CircleColor {
			get { return (Color)GetValue(CircleColorProperty); }
			set { SetValue(CircleColorProperty, value); }
		}


		public WaitingCircle()
		{
			string TAG = "WaitingCircle";
			string dbMsg = "";
			try {
				InitializeComponent();
				dbMsg += "MainCanvas[" + MainCanvas.Width + " × " + MainCanvas.Height + "]";
				// 円の中心座標 default : 50.0
				double cx = MainCanvas.Width / 2;
				double cy = MainCanvas.Height / 2;
				dbMsg += ",円の中心座標(" + cx + " , " + cy + ")";
				//円の半径 default : 45.0
				double r = cx * 0.8;
				//円の分割数 default : 14
				int cnt = 12;

				double deg = 360.0 / (double)cnt;
				double degS = deg * 0.2;
				for (int i = 0; i < cnt; ++i) {
					dbMsg += "\r\n" + i + ")";
					var si1 = Math.Sin((270.0 - (double)i * deg) / 180.0 * Math.PI);
					var co1 = Math.Cos((270.0 - (double)i * deg) / 180.0 * Math.PI);
					var si2 = Math.Sin((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
					var co2 = Math.Cos((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
					var x1 = r * co1 + cx;
					var y1 = r * si1 + cy;
					var x2 = r * co2 + cx;
					var y2 = r * si2 + cy;

					var path = new Path();
					//一点ごとの形状 default : 円弧
					//		path.Data = Geometry.Parse(string.Format("M {0},{1} A {2},{2} 0 0 0 {3},{4}", x1, y1, r, x2, y2));
					//円
					EllipseGeometry myEllipseGeometry = new EllipseGeometry();
					myEllipseGeometry.Center = new Point(x1, y1);
					dbMsg += "," + myEllipseGeometry.Center;
					myEllipseGeometry.RadiusX = cx / 20;
					myEllipseGeometry.RadiusY = cx / 20;
					path.Data = myEllipseGeometry;

					path.Stroke = new SolidColorBrush(Color.FromArgb((byte)(255 - (i * 256 / cnt)), CircleColor.R, CircleColor.G, CircleColor.B));
					dbMsg += "," + path.Stroke.ToString();
					path.StrokeThickness = 10.0;
					MainCanvas.Children.Add(path);
				}

				var kf = new DoubleAnimationUsingKeyFrames();
				kf.RepeatBehavior = RepeatBehavior.Forever;

				//一点当たりの表示時間 default : 80
				int OneDrow = 80;
				dbMsg += ",deg=" + deg;

				for (int i = 0; i < cnt; ++i) {
					kf.KeyFrames.Add(new DiscreteDoubleKeyFrame() {
						KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * OneDrow)),
						Value = i * deg
					});
				}
				//			dbMsg += ",kf=" + kf.KeyFrames.ToString();
				MainTrans.BeginAnimation(RotateTransform.AngleProperty, kf);
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}

		}

		public void OnCircleColorPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			string TAG = "OnCircleColorPropertyChanged";
			string dbMsg = "";
			try {
				if (null == MainCanvas) return;
				if (null == MainCanvas.Children) return;

				foreach (var child in MainCanvas.Children) {
					var shp = child as Shape;
					var sb = shp.Stroke as SolidColorBrush;
					var a = sb.Color.A;
					shp.Stroke = new SolidColorBrush(Color.FromArgb(a, CircleColor.R, CircleColor.G, CircleColor.B));
				}
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}
		////////////////////////////////////////////////////
		public void MyLog(string TAG, string dbMsg)
		{
			CS_Util Util = new CS_Util();
			dbMsg = "[WaitingCircle]" + dbMsg;
			Util.MyLog(TAG, dbMsg);
		}

		public void MyErrorLog(string TAG, string dbMsg, Exception err)
		{
			CS_Util Util = new CS_Util();
			dbMsg = "[WaitingCircle]" + dbMsg;
			Util.MyErrorLog(TAG, dbMsg, err);
		}

	}
}
/// https://araramistudio.jimdo.com/2016/11/24/wpf%E3%81%A7waitingcircle%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AD%E3%83%BC%E3%83%AB%E3%82%92%E4%BD%9C%E3%82%8B/
