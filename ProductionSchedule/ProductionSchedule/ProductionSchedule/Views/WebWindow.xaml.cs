using System;
using System.Windows;
using System.Windows.Controls;


namespace ProductionSchedule.Views
{
    /// <summary>
    /// WebWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class WebWindow : Window
    {
        //public GoogleAuth authWindow;
        //public GCalender mainView;

        /// <summary>
        /// このページで表示するwebページのURL
        /// </summary>
        public Uri TaregetURL { set; get; }

        public WebWindow()
        {
            string TAG = "WebWindow";
            string dbMsg = "[WebWindow]";
            try
            {
                InitializeComponent();
                this.FontSize = Constant.MyFontSize;
                this.Loaded += this_loaded;
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        //ViewModelのViewプロパティに自分のインスタンス（つまりViewのインスタンス）を渡しています。
        private void this_loaded(object sender, RoutedEventArgs e)
        {
            //    VM.MyView = this;
            //	((ViewModels.MainViewModel)this.DataContext).MyView =this;
          //  TaregetURL = new Uri("https://www.yahoo.co.jp/?fr=top_ga1_ext1_bookmark");

            SetMyURL(TaregetURL);
        }
        /// <summary>
        /// この画面が表示された後で、他クラスからURLを与える
        /// </summary>
        /// <param name="taregetURL"></param>
        public void SetMyURL(Uri taregetURL)
        {
            string TAG = "SetMyURL";
            string dbMsg = "[GEventEdit]";
            try
            {
                dbMsg += "taregetURL=" + taregetURL;
                TaregetURL = taregetURL;
                url_tb.Text = taregetURL.ToString();    //TextChangedが発生する
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }


        /// <summary>
        /// URL書き換え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Url_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string TAG = "Url_tb_TextChanged";
            string dbMsg = "[GEventEdit]";
            try
            {
                string urlTbText = url_tb.Text;
                dbMsg += "urlTbText=" + urlTbText;
                if (!urlTbText.Equals(""))
                {
                    //		TaregetURL = new Uri(urlTbText, UriKind.Absolute);
                    //   web_wb.Navigate(TaregetURL);
                    web_wb.Source = TaregetURL;
                }
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// WPFでクローズボックスなど、ウインドウを閉じる時に発生するイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string TAG = "Window_Closing";
            string dbMsg = "[GEventEdit]";
            try
            {
                QuitMe();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// このフォームを閉じる
        /// ※this.Close();だと再表示でクラッシュするのでthis.Visible = false;でこのオブジェクトを破棄させない
        /// </summary>
        private void QuitMe()
        {
            string TAG = "QuitMe";
            string dbMsg = "[GEventEdit]";
            try
            {
                //if (mainView != null)
                //{
                //    mainView.webWindow = null;
                //}
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
            this.Close();
        }

        ////////////////////////////////////////////////////
        public void MyLog(string TAG, string dbMsg)
        {
            CS_Util Util = new CS_Util();
            Util.MyLog(TAG, dbMsg);
        }

        public void MyErrorLog(string TAG, string dbMsg, Exception err)
        {
            CS_Util Util = new CS_Util();
            Util.MyErrorLog(TAG, dbMsg, err);
        }

        public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
                                                                        MessageBoxButton buttns,
                                                                        MessageBoxImage icon
                                                                        )
        {
            CS_Util Util = new CS_Util();
            return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
        }

        //public static implicit operator WebWindow(GEventEdit v)
        //{
        //    throw new NotImplementedException();
        //}
    }

    /*
	WebbrowserクラスはIE7相当で動作する
	WPFの場合
	https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/wpf-winforms/webview
	①パッケージマネージャで　
	Install-Package Microsoft.Toolkit.Wpf.UI.Controls.WebView -Version 6.0.1　でEdge相当のWebViewに変更する
	②Visualstudio再起動
	③using Microsoft.Toolkit.Wpf.UI.Controls;
		C:\Users\hkuwayama\.nuget\packages\microsoft.toolkit.wpf.ui.controls.webview\6.0.1\lib\netcoreapp3.0に
		Microsoft.Toolkit.Wpf.UI.Controls.WebView.dll　がインストールされる
	④Nugetの設定を変更してPackageReferenceを使う
			Must use PackageReference対策	

	 */

}








