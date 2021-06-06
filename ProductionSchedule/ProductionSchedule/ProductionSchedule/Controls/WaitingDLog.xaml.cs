using System;
using System.Windows.Controls;
using System.Reflection;
using System.Windows;
using System.Threading.Tasks;

namespace ProductionSchedule.Controls {
	/// <summary>
	/// WaitingDLog.xaml の相互作用ロジック
	/// </summary>
	public partial class WaitingDLog : Window {
		public Window MyWindow;
	/// <summary>
	/// 呼べばローディングダイアログを開く
	/// </summary>
		public WaitingDLog()
		{
			string TAG = "WaitingDLog";
			string dbMsg = "";
			try {
				InitializeComponent();
				MyLog(TAG, dbMsg);
				//Loaded += this_loaded;
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}

		//ViewModelのViewプロパティに自分のインスタンス（つまりViewのインスタンス）を渡しています。
		private void this_loaded(object sender, RoutedEventArgs e)
		{
			string TAG = "this_loaded";
			string dbMsg = "";
			try {
				//UserContlorの場合、Windowを生成して自分を入れる/////////////
				//MyWindow = Window.GetWindow(this);
				//MyWindow.Width = 500;
				//MyWindow.Height = 140;
				//MyWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				//MyWindow.Topmost = true;
				//MyWindow.Title = "しばらくお待ちください";
				/////////////UserContlorの場合、Windowを生成して自分を入れる//
				//	Application.Current.Dispatcher.Invoke(new System.Action(() => ShowDialog()));
				//ShowDialog(); だけだと　このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
				//	MyWindow.Show();
				//ShowDialog();にするとこのダイアログの操作まで処理がとまる
				//	Show();			//にするとWaitingCircleが回らない
				MyWindow = this;
				MyWindow.Closing += window_Closing;
				dbMsg += "＞＞表示";
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}
		void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
		{
			string TAG = "window_Closing";
			string dbMsg = "";
			try {
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}

		public void SetMes(string Msg)
		{
			string TAG = MethodBase.GetCurrentMethod().Name;
			string dbMsg = "";
			try {
				dbMsg += "Msg=" + Msg;
				//System.InvalidOperationException: このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
				Progress_msg_tb.Content = Msg;

				//this.Dispatcher.Invoke((Action)(() =>
				//{
				//	Progress_msg_tb.Content = Msg;
				//}));
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}

		/// <summary>
		/// ローディングダイアログを閉じる
		/// </summary>
		public void QuitMe()
		{
			string TAG = MethodBase.GetCurrentMethod().Name;
			string dbMsg = "";
			try {
				Close();
				//if (MyWindow != null) {
				//	MyWindow.Close();
				//}
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}


		////////////////////////////////////////////////////
		public void MyLog(string TAG, string dbMsg)
		{
			dbMsg = "[WaitingDLog]" + dbMsg;
			CS_Util Util = new CS_Util();
			Util.MyLog(TAG, dbMsg);
		}

		public void MyErrorLog(string TAG, string dbMsg, Exception err)
		{
			dbMsg = "[WaitingDLog]" + dbMsg;
			CS_Util Util = new CS_Util();
			Util.MyErrorLog(TAG, dbMsg, err);
		}
	}
}
/// https://araramistudio.jimdo.com/2016/11/24/wpf%E3%81%A7waitingcircle%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AD%E3%83%BC%E3%83%AB%E3%82%92%E4%BD%9C%E3%82%8B/
