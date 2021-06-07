using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using ProductionSchedule.Views;
using ProductionSchedule.Models;
//using ProductionSchedule.Enums;
using System.Reflection;
using System.Windows.Input;
using Google.Apis.Drive.v3;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

using ProductionSchedule.Controls;
using Google.Apis.Auth.OAuth2;

namespace ProductionSchedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // :ViewModel, INotifyPropertyChanged

        public Views.MainWindow MyView { get; set; }
		public List<Models.MyMenu> _MyMenu { get; set; }

		public System.Collections.IEnumerable TabItems { get; set; }

		public string InfoLavel { get; set; }
		public string ReTitle="";
        private string _TargetURLStr;
        /// <summary>
        /// 遷移先URL文字列
        /// </summary>
        public string TargetURLStr {
            get {
                return _TargetURLStr;
            }
            set {
                string TAG = "TargetURLStr.set";
                string dbMsg = "";
                try
                {
                    dbMsg += ">>遷移先URL=  " + value;
                    if (value == _TargetURLStr)
                        return;
                    _TargetURLStr = value;
                    //if (value != null)
                    //{
                    //    TargetURI = new Uri(value);
                    //    NotifyPropertyChanged("TargetURI");
                    //}
                    MyLog(TAG, dbMsg);
                }
                catch (Exception er)
                {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _ConnectVisibility;
        /// <summary>
        /// 接続ボタン表示
        /// </summary>
        public string ConnectVisibility {
            get {
                return _ConnectVisibility;
            }
            set {
                string TAG = "ConnectVisibility.set";
                string dbMsg = "";
                try
                {
                    dbMsg += ">>接続ボタン表示=  " + value;
                    if (value == _ConnectVisibility)
                        return;
                    _ConnectVisibility = value;
                    MyLog(TAG, dbMsg);
                }
                catch (Exception er)
                {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _CancelVisibility;
        /// <summary>
        /// 解除ボタン表示
        /// </summary>
        public string CancelVisibility {
            get {
                return _CancelVisibility;
            }
            set {
                string TAG = "CancelVisibility.set";
                string dbMsg = "";
                try
                {
                    dbMsg += ">>解除ボタン表示=  " + value;
                    if (value == _CancelVisibility)
                        return;
                    _CancelVisibility = value;
                    MyLog(TAG, dbMsg);
                }
                catch (Exception er)
                {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }


        private string _GoogleAcountStr;
        /// <summary>
        /// 接続先アカウント
        /// </summary>
        public string GoogleAcountStr {
            get {
                return _GoogleAcountStr;
            }
            set {
                string TAG = "_GoogleAcountStr.set";
                string dbMsg = "";
                try
                {
                    dbMsg += ">>接続先アカウント=  " + value;
                    if (value == _GoogleAcountStr)
                        return;
                    _GoogleAcountStr = value;
                    MyLog(TAG, dbMsg);
                }
                catch (Exception er)
                {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        /// <summary>
        /// 使用許諾を受けるAPIのリスト
        /// </summary>
        public static string[] AllScopes = { DriveService.Scope.DriveFile,
                                                                    DriveService.Scope.DriveAppdata,			//追加
																	DriveService.Scope.Drive,
                                                                    CalendarService.Scope.Calendar,
                                                                    CalendarService.Scope.CalendarEvents
                                                            };


        public MainViewModel()
		{
			Initialize();
		}

        public void Initialize()
        {
            string TAG = "Initialize";
            string dbMsg = "";
            try
            {
                this.TargetURLStr = Constant.WebStratUrl;
                GoogleAcountStr = "YourGoogleAcount@gmail.com";
                NotifyPropertyChanged("TargetURI");
                dbMsg += ",遷移先URL=  " + TargetURLStr;
                //起動時は接続側のみ
                Cancel();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        ///  Windowクラスをダイアログ表示する
        ///  private void OpenDialog_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDialog()
		{
			string dbMsg = "";
			//Views.ChildView rContent = new Views.ChildView();
			//dbMsg += "WindowクラスのViewを\r\n";
			//AddCount++;
			//dbMsg += AddCount + "番目に生成したダイアログで表示";
			//MyView.Info_lv.Content = dbMsg;
			//ViewModels.ChildViewModel VM = new ViewModels.ChildViewModel();
			//VM.MW = (Views.MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault((w) => w.IsActive); 
			////	VM.TC = this;
			//rContent.DataContext = VM;
			//rContent.CInfo_lb.Content = dbMsg + "\r\nダイアログは複数表示されません";
			//rContent.ShowDialog();
		}

        public ICommand GotoCommand2 => new DelegateCommand(Goto2);
        /// <summary>
        /// Windowクラスを別ウインドウで開く
        /// </summary>
        public void Goto2()
		{
			string dbMsg = "";
			//Views.ChildView rContent = new Views.ChildView();
			//dbMsg += "WindowクラスのViewを\r\n";
			//AddCount++;
			//dbMsg += AddCount + "番目に生成したダイアログで表示";
			//MyView.Info_lv.Content = dbMsg;
			//ViewModels.ChildViewModel VM = new ViewModels.ChildViewModel();
			//VM.MW  = (Views.MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault((w) => w.IsActive);
			//VM.CInfo_lb = dbMsg ;
			//rContent.DataContext = VM;
			//rContent.Show();
		}

        //Livet.ViewModelCommand と置き換え
        public ICommand WebStartCommand => new DelegateCommand(WebStart);

        public void WebStart()
        {
            string TAG = "WebStart";
            string dbMsg = "";
            try
            {
                WebWindow ww = new WebWindow();
                ww.VM.TargetURLStr = Constant.WebStratUrl;
                ww.Show();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        public ICommand ConnectCommand => new DelegateCommand(Connect);
        /// <summary>
        /// 接続前の状況確認
        /// Z_1_4ViewModelから
        /// </summary>
        public void Connect()
        {
            string TAG = "Connect";
            string dbMsg = "";
            try
            {
                dbMsg += ",CliantId=" + Constant.CliantId;
                //if (OneAccount.CliantId == null || OneAccount.CliantId.Equals(""))
                //{
                //    dbMsg += ",JsonReadへ";
                //    JsonRead();
                //}
                //else
                //{
                dbMsg += ",接続へ";
                ConnectBody();
                //}
                //Constant.RootFolderID = GDriveUtil.MakeAriadneGoogleFolder();
                //if (Constant.RootFolderID.Equals(""))
                //{
                //    dbMsg += ">フォルダ作成>失敗";
                //}
                //else
                //{
                //    dbMsg += "[" + Constant.RootFolderID + "]" + Constant.RootFolderName;
                //}
                MyLog(TAG, dbMsg);
    //            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }
        /// <summary>
        /// 接続開始
        /// </summary>
        public void ConnectBody()
        {
            string TAG = "ConnectBody";
            string dbMsg = "";
            try
            {
                        dbMsg += ",CliantId=" + Constant.CliantId;
                  Controls.WaitingDLog progressWindow = new Controls.WaitingDLog();
                        progressWindow.Show();
                        progressWindow.SetMes("Googleサービス接続中...");
                        Task<UserCredential> userCredential = Task.Run(() => {
                            return MakeAllCredentialAsync();
                        });
                        userCredential.Wait();

                        progressWindow.Close();

                        Constant.MyDriveCredential = userCredential.Result;                           //作成結果が格納され戻される
                        if (Constant.MyDriveCredential == null)
                        {
                            //メッセージボックスを表示する
                            String titolStr = Constant.ApplicationName;
                            String msgStr = "認証されませんでした。\r\n更新ボタンをクリックして下さい";
                            MessageBoxResult result = MessageShowWPF(titolStr, msgStr, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            dbMsg += ",result=" + result;
                        }
                        else
                        {
                            dbMsg += "\r\nAccessToken=" + Constant.MyDriveCredential.Token.AccessToken;
                            dbMsg += "\r\nRefreshToken=" + Constant.MyDriveCredential.Token.RefreshToken;
                            Constant.MyDriveService = new DriveService(new BaseClientService.Initializer()
                            {
                                HttpClientInitializer = Constant.MyDriveCredential,
                                ApplicationName = Constant.ApplicationName,
                            });
                            dbMsg += ",MyDriveService:ApiKey=" + Constant.MyDriveService.ApiKey;
                            Constant.MyCalendarCredential = Constant.MyDriveCredential;
                            Constant.MyCalendarService = new CalendarService(new BaseClientService.Initializer()
                            {
                                HttpClientInitializer = Constant.MyCalendarCredential,
                                ApplicationName = Constant.ApplicationName,
                            });
                            dbMsg += ",MyCalendarService:ApiKey=" + Constant.MyCalendarService.ApiKey;
                    dbMsg += ">>UserId=" + userCredential.Result.UserId;
                    GoogleAcountStr = userCredential.Result.UserId;

                }

                ConnectVisibility = "Hidden";
                //RaisePropertyChanged("ConnectVisibility");
                CancelVisibility = "Visible";
                //RaisePropertyChanged("CancelVisibility");
                //Constant.RootFolderID = GDriveUtil.MakeAriadneGoogleFolder();
                if (Constant.RootFolderID.Equals(""))
                        {
                            dbMsg += ">フォルダ作成>失敗";
                        }
                        else
                        {
                            dbMsg += "[" + Constant.RootFolderID + "]" + Constant.RootFolderName;
                        }
                        MyLog(TAG, dbMsg);
        //                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));*/
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// OAuthのJSONファイルを読む
        /// Modelに格納して
        /// </summary>
        private void JsonRead()
        {
            string TAG = "JsonRead";
            string dbMsg = "[GoogleAuth]";
            try
            {
                /*
                // ダイアログのインスタンスを生成
                OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.Title = "Googleコンソールで取得したJSONファイルを選択して下さい";
                // ファイルの種類を設定
                dialog.Filter = "テキストファイル (*.json)|*.json|全てのファイル (*.*)|*.*";
                // ダイアログを表示する
                DialogResult res = dialog.ShowDialog();
                int rCount = dialog.FileNames.Count();
                dbMsg += "," + rCount + "件";
                if (0 < rCount)
                {
                    // 選択されたファイル名 (ファイルパス) をメッセージボックスに表示
                    foreach (String fileOne in dialog.FileNames)
                    {
                        string jsonPath = fileOne.ToString();
                        dbMsg += "\r\n" + jsonPath;
                        dbMsg += ",jsonPath=" + jsonPath;
                        using (System.IO.FileStream stream = new System.IO.FileStream(jsonPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            //読込んだファイルを文字列に変換
                            StreamReader sr = new StreamReader(stream);
                            string rStr = sr.ReadToEnd();
                            sr.Close();
                            dbMsg += ",rStr=" + rStr;
                            GOAuthModel gOAuthModel = JsonConvert.DeserializeObject<GOAuthModel>(rStr);
                            //JSONからMODELへ転記////////////////////////////////////////////
                            dbMsg += "\r\nclient_id= " + gOAuthModel.client_id;
                            Constant.CliantId = gOAuthModel.client_id;
                            dbMsg += " \r\nclient_secret= " + gOAuthModel.client_secret;
                            OneAccount.client_secret = gOAuthModel.client_secret;
                            dbMsg += "\r\nproject_id= " + gOAuthModel.project_id;
                            OneAccount.project_id = gOAuthModel.project_id;
                            dbMsg += "\r\nauth_uri= " + gOAuthModel.auth_uri;
                            OneAccount.auth_uri = gOAuthModel.auth_uri;
                            dbMsg += "\r\ntoken_uri= " + gOAuthModel.token_uri;
                            OneAccount.token_uri = gOAuthModel.token_uri;
                            dbMsg += "\r\nauth_provider_x509_cert_url= " + gOAuthModel.auth_provider_x509_cert_url + "\r\n";
                            OneAccount.auth_provider_x509_cert_url = gOAuthModel.auth_provider_x509_cert_url;
                            ///テーブルに書込む////////////////////JSONからMODELへ転記//
                            using (MySqlConnection mySqlConnection = new MySqlConnection(Constant.ConnectionString))
                            {
                                string CommandStr = "update " + TargetTableName + " set " +
                                                                            "client_id = @client_id" +
                                                                            " , client_secret = @client_secret" +
                                                                            " , project_id = @project_id" +
                                                                            " , updated_user = @updated_user" +
                                                                            " , updated_at = @updated_at" +
                                                                            " where id = @id";
                                MySqlCommand cmd = new MySqlCommand(CommandStr, mySqlConnection);
                                // パラメータ設定
                                cmd.Parameters.Add(
                                    new MySqlParameter("id", OneAccount.id));
                                //cmd.Parameters.Add(
                                //	new MySqlParameter("m_contract_id", OneAccount.m_contract_id));
                                //cmd.Parameters.Add(
                                //	new MySqlParameter("google_account", OneAccount.google_account));
                                cmd.Parameters.Add(
                                    new MySqlParameter("client_id", gOAuthModel.client_id));
                                cmd.Parameters.Add(
                                    new MySqlParameter("client_secret", gOAuthModel.client_secret));
                                cmd.Parameters.Add(
                                    new MySqlParameter("project_id", gOAuthModel.project_id));
                                //ログインユーザーに要書き換え//
                                cmd.Parameters.Add(
                                    new MySqlParameter("updated_user", OneAccount.m_contract_id));
                                //ログインユーザーに要書き換え//
                                cmd.Parameters.Add(
                                    new MySqlParameter("updated_at", DateTime.Now));
                                // オープン
                                cmd.Connection.Open();
                                // 実行  ※レコード作成直後、エラー発生?
                                cmd.ExecuteNonQuery();
                                // クローズ
                                cmd.Connection.Close();
                                dbMsg += " >>書き込み終了";
                                ConnectBody();
                            }
                        }
                        //複数選ばれても一件目で強制的に処理開始
                        break;
                    }
                }
                */
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }


        /// <summary>
        /// UserCredentialを作成する
        /// 初回アクセス時に使用するAPIをScopesで申請する
        /// </summary>
        /// <returns>UserCredential</returns>
        private async Task<UserCredential> MakeAllCredentialAsync()
        {
            string TAG = "MakeAllCredentialAsync";
            string dbMsg = "";
            UserCredential userCedential = null;
            try
            {
                ClientSecrets clientSecrets = new ClientSecrets();
                dbMsg += ",clientId=" + Constant.CliantId;
                clientSecrets.ClientId = Constant.CliantId;
                dbMsg += ",clientSecret=" + Constant.CliantSeacret;
                clientSecrets.ClientSecret = Constant.CliantSeacret;
                                //there are different scopes, which you can find here https://cloud.google.com/storage/docs/authentication
                                //	var scopes = new[] { @"https://www.googleapis.com/auth/devstorage.full_control" };
                CancellationTokenSource cts = new CancellationTokenSource();
                dbMsg += ",メールアドレスt=" + GoogleAcountStr;
                ///PCで標準にしているブラウザーでGoogleログインを開く
                userCedential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, AllScopes, GoogleAcountStr, cts.Token);
                dbMsg += ">>Token=" + userCedential.Token;
       //         dbMsg += ">>AccessToken=" + userCedential.AccessToken;
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
            return userCedential;
        }

        public ICommand CancelCommand => new DelegateCommand(Cancel);
        public void Cancel()
        {
            string TAG = "Cancel";
            string dbMsg = "";
            try
            {
                dbMsg += ";Token" + Constant.MyDriveCredential.Token.ToString();
                Constant.MyDriveService = null;
                Constant.MyCalendarService = null;
                dbMsg += "\r\n>>" + Constant.MyDriveCredential.Token.ToString();
                MyLog(TAG, dbMsg);
                ConnectVisibility = "Visible";
                //RaisePropertyChanged("ConnectVisibility");
                CancelVisibility = "Hidden";
                //RaisePropertyChanged("CancelVisibility");
                //RaisePropertyChanged();
                //		Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }

        }


        /// <summary>
        /// このウィンドウを閉じる
        /// </summary>
        public void Close()
		{
//			MyView.Info_lv.Content = "お疲れさまでした";
			var window = Application.Current.Windows.OfType<Window>().SingleOrDefault((w) =>w.IsActive);
			window.Close();
		}


        /// <summary>
        /// プロパティ変更通知を行うBindableBase
        /// ；ヘルパなしでRaisePropertyChangedの代りにNotifyPropertyChangedを使う
        /// </summary>

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //System.Windows.Input; Livet Messengerでも使う///////////////////////
        void RequeryCommands()
        {
            // Seems like there should be a way to bind CanExecute directly to a bool property
            // so that the binding can take care keeping CanExecute up-to-date when the property's
            // value changes, but apparently there isn't.  Instead we listen for the WebView events
            // which signal that one of the underlying bool properties might have changed and
            // bluntly tell all commands to re-check their CanExecute status.
            //
            // Another way to trigger this re-check would be to create our own bool dependency
            // properties on this class, bind them to the underlying properties, and implement a
            // PropertyChangedCallback on them.  That arguably more directly binds the status of
            // the commands to the WebView's state, but at the cost of having an extraneous
            // dependency property sitting around for each underlying property, which doesn't seem
            // worth it, especially given that the WebView API explicitly documents which events
            // signal the property value changes.
            CommandManager.InvalidateRequerySuggested();
        }


        ///////////////////////
        public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
																		MessageBoxButton buttns,
																		MessageBoxImage icon
																		)
		{
			CS_Util Util = new CS_Util();
			return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
		}

        public static void MyLog(string TAG, string dbMsg)
        {
            dbMsg = "[" + MethodBase.GetCurrentMethod().Module.Name + "]" + dbMsg;
            //dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyLog(TAG, dbMsg);
        }

        public static void MyErrorLog(string TAG, string dbMsg, Exception err)
        {
            dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyErrorLog(TAG, dbMsg, err);
        }
        //////////////////////////////////////

    }
}
