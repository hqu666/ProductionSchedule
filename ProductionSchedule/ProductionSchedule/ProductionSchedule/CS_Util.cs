using System;
using System.Windows;

namespace ProductionSchedule
{
	class CS_Util {
		/// <summary>
		/// デバッグログ
		/// 出荷時は　debugNow　をfalseに
		/// </summary>
		/// <param name="TAG"></param>
		/// <param name="dbMsg"></param>
		public  void MyLog(String TAG, String dbMsg)
		{
			if (Constant.debugNow) {
				Console.WriteLine(TAG + " : " + dbMsg);
				//System.Diagnostics.Trace.WriteLine(TAG + " : " + dbMsg);
				//System.Diagnostics.Debug.WriteLine(TAG + " : " + dbMsg);
			}
		}

		public  void MyErrorLog(String TAG, String dbMsg , Exception err)
		{
			if (Constant.errorCheckNow) {
				//					Xamarin.Forms.Internals.Log.Warning(TAG, dbMsg);
				Console.WriteLine(TAG + " : " + dbMsg + "でエラー発生;" + err);
			}
		}

		public MessageBoxResult MessageShowWPF( String msgStr,
																				String titolStr = null,
																				MessageBoxButton buttns = MessageBoxButton.OK,
																				MessageBoxImage icon = MessageBoxImage.None
																				)
		{
			String TAG = "MessageShowWPF";
			String dbMsg = "開始";
			MessageBoxResult result = 0;
			try {
				dbMsg = "titolStr=" + titolStr;
				dbMsg += "mggStr=" + msgStr;
				//メッセージボックスを表示する		https://docs.microsoft.com/ja-jp/dotnet/api/system.windows.messagebox?view=netcore-3.1
				if (titolStr == null) {
					result = MessageBox.Show(msgStr);
				} else if(icon == MessageBoxImage.None) {
					result = MessageBox.Show(msgStr, titolStr, buttns);
				}else{
					result = MessageBox.Show(msgStr, titolStr, buttns, icon);
				}
				dbMsg += ",result=" + result;
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyLog(TAG, dbMsg + "で" + er.ToString());
			}
			return result;
		}

		/// <summary>
		/// ダイアログ表示
		/// </summary>
		/// <param name="titolStr">タイトル</param>
		/// <param name="msgStr">メッセージ</param>
		/// 		/// http://furuya02.hatenablog.com/entry/20140426/1398477869
		/// <param name="titolStr"></param>
		/// <param name="msgStr"></param>
		/// <param name="buttns">MessageBoxButtons.OKCancel</param>
		/// <param name="icon">MessageBoxIcon.Exclamation</param>
		/// <param name="defaultButton">MessageBoxDefaultButton.Button2</param>
		/// <returns></returns>
		//public DialogResult MessageShow(String titolStr, String msgStr, MessageBoxButtons buttns, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		//{
		//	String TAG = "MessageShow";
		//	String dbMsg = "開始";
		//	DialogResult result = 0;
		//	try {
		//		dbMsg = "titolStr=" + titolStr;
		//		dbMsg += "mggStr=" + msgStr;

		//		//メッセージボックスを表示する
		//		result = MessageBox.Show(msgStr, titolStr, buttns, icon, defaultButton);
		//		dbMsg += ",result=" + result;
		//		MyLog(TAG, dbMsg);
		//	} catch (Exception er) {
		//		MyLog(TAG, dbMsg + "で" + er.ToString());
		//	}
		//	return result;
		//}
	}
}
