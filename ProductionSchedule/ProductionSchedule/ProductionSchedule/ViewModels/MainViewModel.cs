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
using System.Collections.ObjectModel;

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

        /// <summary>
        /// 接続ボタン表示
        /// </summary>
        public string ConnectVisibility { set; get; }
        /// <summary>
        /// 解除ボタン表示
        /// </summary>
        public string CancelVisibility { set; get; }


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

        /// <summary>
        /// ローディングダイアログクラス
        /// </summary>
        public Controls.WaitingDLog waitingDLog;


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
                GoogleAcountStr = "hkuwauama@gmail.com";
            //    GoogleAcountStr = "YourGoogleAcount@gmail.com";
                NotifyPropertyChanged("TargetURI");
                dbMsg += ",遷移先URL=  " + TargetURLStr;
                //起動時は接続側のみ
                Cancel();
      //          ToDaySet();

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

        //Google接続/////////////////////////////////////////////////////////////////////
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
                ToDaySet();

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
      //          RaisePropertyChanged("ConnectVisibility");
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
        //カレンダ作成///////////////////////////////////////////////////////////////////Google接続///
        //X_1_4ViewModel
        //表示対象年月
        public DateTime SelectedDateTime;
        /// <summary>
        /// 表示対象年月
        /// </summary>
        public string CurrentDate { get; set; }
        /// <summary>
        /// Viewの高さ
        /// </summary>
        public double vHeight { get; set; }
        /// <summary>
        /// Viewを格納するGridの高さ
        /// </summary>
        public double DateColWidth { get; set; }

        /// <summary>
        /// イベントが有る日の配列
        /// </summary>
        public ObservableCollection<ADay> EDays { get; set; }
        /// <summary>
        /// 選択された日
        /// </summary>
        public ADay TatagetDay { get; set; }
        /// <summary>
        /// 開始日順の対象イベント配列
        /// </summary>
        public ObservableCollection<t_events> OrderedByStart { get; set; }
        public int selectedDateIndex { set; get; }

        public int selectedIndex { set; get; }

        #region TargetEvent変更通知プロパティ
        private t_events _TargetEvent;
        /// <summary>
        /// 操作対象の予定
        /// </summary>
        public t_events TargetEvent {
            get { return _TargetEvent; }
            set {
                if (_TargetEvent == value)
                    return;
                _TargetEvent = value;
            //RaisePropertyChanged("TargetEvent");
            //    //if(_TargetEvent != null) {
            //    //	Edit();
            //    //}
            }
        }
        #endregion

        public void CalenderWrite()
        {
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            try
            {
                waitingDLog = new Controls.WaitingDLog();
                waitingDLog.Show(); //.ShowDialog(); だとこのオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
                                    //waitingDLog=wDLog.Result;  だと　呼び出しスレッドは、多数の UI コンポーネントが必要としているため、STA である必要があります。
                                    //	waitingDLog.ShowDialog();  // Show() にするとWaitingCircleが回らない
                                    //						///////////WindowにユーザーコントロールをWindowに読み込む方法//
                                    // this.EDays = CalenderWriteBody(waitingDLog);
                Task<ObservableCollection<ADay>> EDays = Task.Run(() => {
                    //waitingDLog.Dispatcher.Invoke((Action)(() => {
                    //	waitingDLog.ShowDialog();  // Show() にするとWaitingCircleが回らない
                    //}));　だと表示もされない
                    //waitingDLog.ShowDialog(); だとこのオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
                    return CalenderWriteBody(waitingDLog);
                });
                EDays.Wait();

                //waitingDLog.Close();
                waitingDLog.QuitMe();
                waitingDLog = null;
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// カレンダ作成本体
        /// </summary>
        private ObservableCollection<ADay> CalenderWriteBody(Controls.WaitingDLog waitingDLog)
        {
            //
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            EDays = new ObservableCollection<ADay>();
            try
            {
                DateTime cStart = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1);
                DateTime cEnd = cStart.AddMonths(1).AddSeconds(-1);
                string msgStr = cStart + "～" + cEnd + "の予定を読み出しています"; ;
                dbMsg += msgStr;
                waitingDLog.SetMes(msgStr);
                //Application.Current.Dispatcher.Invoke(new System.Action(() => this.waitingDLog.SetMes(msgStr)));
                ObservableCollection<t_events> Events = WriteEvent();
                msgStr = Events.Count + "件の予定が有りました";

                //	Application.Current.Dispatcher.Invoke(new System.Action(() => waitingDLog.SetMes(msgStr)));
                //対象期間中の予定を開始日時が早いものから配列化
                OrderedByStart =
                    new ObservableCollection<t_events>(
                             Events.OrderBy(rec => rec.event_date_start)
                                        .ThenBy(rec => rec.event_time_start)
                                        .ThenBy(rec => rec.event_date_end)
                            );
                //日毎にまとめる
                DateTime tDate = OrderedByStart.First().event_date_start;
                List<string> summarys = new List<string>();
                ObservableCollection<t_events> dEvents = new ObservableCollection<t_events>();
                ADay aDay = new ADay(tDate, summarys, dEvents, this);
                int cIndex = 0;

                foreach (t_events ev in OrderedByStart)
                {
                    if (tDate < ev.event_date_start)
                    {          // && 0< dEvents.Count
                        msgStr = ":開始" + tDate + ">>" + ev.event_date_start + ":" + EDays.Count + "件";
                        //				Application.Current.Dispatcher.Invoke(new System.Action(() => waitingDLog.SetMes(msgStr)));
                        dbMsg += "\r\n[" + ev.id + "]" + ev.event_title + "::" + msgStr;
                        aDay = new ADay(tDate, summarys, dEvents, this);
                        EDays.Add(aDay);
                        summarys = new List<string>();
                        dEvents = new ObservableCollection<t_events>();
                        cIndex = 0;
                        if (ev.event_type == 1)
                        {
                            dbMsg += "[案件；" + ev.t_project_base_id + "]";
                        }
                    }
                    tDate = ev.event_date_start;
                    ev.childIndex = cIndex;
                    cIndex++;
                    dEvents.Add(ev);
                    summarys.Add(ev.summary);
                }
                aDay = new ADay(tDate, summarys, dEvents, this);
                EDays.Add(aDay);
                dbMsg += ",DateColWidth=" + DateColWidth;
          //      RaisePropertyChanged(); //	"dataManager"
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
            return EDays;
        }

        /// <summary>
        /// 予定作成
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Models.t_events> WriteEvent()
        {
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            ObservableCollection<t_events> Events = new ObservableCollection<t_events>();
            try
            {
                dbMsg += "" + SelectedDateTime;
                CS_Util Util = new CS_Util();
                //予定取得///////////////////////////////////////////
                GoogleCalendarUtil GCU = new GoogleCalendarUtil();
                //本日日付
                DateTime today = DateTime.Today;

                //月初め
                DateTime timeMin = new DateTime(today.Year, today.Month, 1);

                //月終わり
                DateTime timeMax = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                IList<Google.Apis.Calendar.v3.Data.Event> SelectEvents = GCU.GEventsListUp(timeMin, timeMax);

                //			ActivityCategoryCollection activityCategoryCollection = new ActivityCategoryCollection();
                /*           MySQLUtil = new MySQL_Util();
                           if (MySQLUtil.MySqlConnection())
                           {
                               //ObservableCollection<object> rTable = MySQLUtil.ReadTable("t_events");
                               //if(rTable != null) {
                               //	dbMsg += "rTable" + rTable.Count + "件";
                               //}

                               using (MySqlConnection mySqlConnection = new MySqlConnection(Constant.ConnectionString))
                               {
                                   mySqlConnection.Open();
                                   using (MySqlCommand command = mySqlConnection.CreateCommand())
                                   {
                                       command.CommandText = $"SELECT * FROM {"t_events"}";
                                       using (MySqlDataReader reader = command.ExecuteReader())
                                       {
                                           //int RecordCount = reader.;
                                           //dbMsg += "," + RecordCount + "レコード";
                                           int FieldCount = reader.FieldCount;
                                           dbMsg += "," + FieldCount + "項目";

                                           //一行づつデータを読み取りモデルに書込む
                                           while (reader.Read())
                                           {
                                               t_events OneEvent = new t_events();
                                               for (int i = 0; i < FieldCount; i++)
                                               {
                                                   string rName = reader.GetName(i);
                                                   string rType = reader.GetFieldType(i).Name;
                                                   dbMsg += "\r\n(" + i + ")" + rName + ",rType=" + rType;
                                                   var rVar = reader.GetValue(i);
                                                   dbMsg += ",rVar=" + rVar;
                                                   if (rVar == null || rVar.Equals("") || reader.IsDBNull(i))
                                                   {
                                                       dbMsg += ">>スキップ";
                                                   }
                                                   else if (rName.Equals("id"))
                                                   {
                                                       OneEvent.id = (int)rVar;
                                                   }
                                                   else if (rName.Equals("m_contract_id"))
                                                   {
                                                       OneEvent.m_contract_id = (int)rVar;         //契約ID
                                                   }
                                                   else if (rName.Equals("t_project_base_id"))
                                                   {
                                                       OneEvent.t_project_base_id = (int)rVar;         //案件ID
                                                   }
                                                   else if (rName.Equals("event_title"))
                                                   {
                                                       OneEvent.event_title = (string)rVar;         //タイトル
                                                   }
                                                   else if (rName.Equals("event_date_start"))
                                                   {
                                                       OneEvent.event_date_start = (DateTime)rVar;              //開始日
                                                   }
                                                   else if (rName.Equals("event_time_start"))
                                                   {
                                                       OneEvent.event_time_start = (int)rVar;           //開始時刻
                                                   }
                                                   else if (rName.Equals("event_date_end"))
                                                   {
                                                       OneEvent.event_date_end = (DateTime)rVar;                  //終了日
                                                   }
                                                   else if (rName.Equals("event_time_end"))
                                                   {
                                                       OneEvent.event_time_end = (int)rVar;             //終了時刻
                                                   }
                                                   else if (rName.Equals("event_is_daylong"))
                                                   {
                                                       OneEvent.event_is_daylong = (bool)rVar;                       //終日
                                                   }
                                                   else if (rName.Equals("event_place"))
                                                   {
                                                       OneEvent.event_place = (string)rVar;                           //場所
                                                   }
                                                   else if (rName.Equals("event_memo"))
                                                   {
                                                       OneEvent.event_memo = (string)rVar;                              //メモ
                                                   }
                                                   else if (rName.Equals("google_id"))
                                                   {
                                                       OneEvent.google_id = (string)rVar;                        //GoogleイベントID:未登録は空白文字
                                                   }
                                                   else if (rName.Equals("event_status"))
                                                   {
                                                       OneEvent.event_status = (int)rVar;                       //ステータス
                                                   }
                                                   else if (rName.Equals("event_type"))
                                                   {
                                                       OneEvent.event_type = (SByte)rVar;                           //イベント種別
                                                   }
                                                   else if (rName.Equals("event_bg_color"))
                                                   {
                                                       OneEvent.event_bg_color = (string)rVar;                       //背景色
                                                   }
                                                   dbMsg += ">>読取";
                                                   //	}
                                               }
                                               //string rCol = OneEvent.event_bg_color;
                                               //if (rCol == null || rCol.Equals("")) {
                                               if (Util.IsForegroundWhite(OneEvent.event_bg_color))
                                               {
                                                   dbMsg += "に白文字";
                                                   OneEvent.event_font_color = Brushes.White.ToString();
                                               }
                                               else
                                               {
                                                   dbMsg += "に黒文字";
                                                   OneEvent.event_font_color = Brushes.Black.ToString();
                                               }
                                               //} else {
                                               //	dbMsg += "背景未指定";
                                               //}
                                               if (OneEvent.event_date_start < OneEvent.event_date_end)
                                               {
                                                   OneEvent.event_is_daylong = true;
                                                   OneEvent.event_time_start = 0;           //開始時刻
                                                   OneEvent.event_time_end = 23;               //終了時刻
                                               }
                                               string summary = "";
                                               if (!OneEvent.event_is_daylong)
                                               {
                                                   summary = OneEvent.event_time_start + "～" + OneEvent.event_time_end;
                                               }
                                               else
                                               {
                                                   summary += "～" + String.Format("{0:yyyy/MM/dd}", OneEvent.event_date_end);
                                               }
                                               summary += ": " + OneEvent.event_title + " : " + OneEvent.event_place + " : " + OneEvent.event_memo;
                                               OneEvent.summary = summary;
                                               //				OneEvent.isSetect = false;
                                               Events.Add(OneEvent);
                                           }
                                       }
                                   }
                               }
                               int rCount = Events.Count;
                               dbMsg += ",Events=" + rCount + "件";
                               MySQLUtil.DisConnect();
                           }
                           //実データが少なければテストデータ作成
                           if (Events.Count < 10)
                           {
                               int EventCount = Events.Count + 1;
                               int endCount = EventCount + 10;
                               DateTime dt = DateTime.Now;
                               // タイムゾーンはこのスニペットで設定しないため、日付をグリニッジ標準時へ変換します
                               DateTime StartDT = DateTime.Today.AddHours(SelectedDateTime.Hour).ToUniversalTime();
                               DateTime EndDT = StartDT.AddHours(1).AddMinutes(30);
                               // Infragistics.Controls.Schedules のメタデータ
                               for (EventCount = Events.Count + 1; EventCount < endCount; EventCount++)
                               {
                                   dbMsg += "\r\n[" + EventCount + "]" + StartDT + "～" + EndDT;
                                   Models.t_events OneEvent = new Models.t_events();
                                   OneEvent.id = -1;
                                   OneEvent.event_title = "Test" + EventCount;         //タイトル
                                   OneEvent.event_date_start = StartDT.Date;            //開始日
                                   OneEvent.event_time_start = StartDT.Hour;           //開始時刻
                                   OneEvent.event_date_end = EndDT.Date;               //終了日
                                   OneEvent.event_time_end = EndDT.Hour;               //終了時刻
                                   OneEvent.event_is_daylong = false;                           //終日
                                   if (OneEvent.event_date_start < OneEvent.event_date_end)
                                   {
                                       OneEvent.event_is_daylong = true;
                                       OneEvent.event_time_start = 0;           //開始時刻
                                       OneEvent.event_time_end = 23;               //終了時刻
                                   }
                                   OneEvent.event_place = "第" + EventCount + "会議室";                           //場所
                                   OneEvent.event_memo = EventCount + "つ目のメモ";                           //メモ
                                   OneEvent.google_id = "";                           //GoogleイベントID:未登録は空白文字
                                   OneEvent.event_status = 1;                           //ステータス
                                   OneEvent.event_type = 3;                           //イベント種別:通常イベント

                                   string summary = "";
                                   if (!OneEvent.event_is_daylong)
                                   {
                                       summary = OneEvent.event_time_start + "～" + OneEvent.event_time_end;
                                   }
                                   else
                                   {
                                       summary += "～" + String.Format("{0:yyyy/MM/dd}", OneEvent.event_date_end);
                                   }
                                   summary += ": " + OneEvent.event_title + " : " + OneEvent.event_place + " : " + OneEvent.event_memo;
                                   OneEvent.summary = summary;
                                   //			OneEvent.isSetect = false;
                                   //背景色
                                   ColorConverter cc = new ColorConverter();
                                   int rCode = EventCount * 20;
                                   if (255 < rCode)
                                   {
                                       rCode = rCode % 255;
                                   }
                                   string rStr = rCode.ToString("X");
                                   if (rStr.Length < 2)
                                   {
                                       rStr = 0 + rStr;
                                   }
                                   int gCode = rCode * EventCount;
                                   if (255 < gCode)
                                   {
                                       gCode = gCode % 255;
                                   }
                                   string gStr = gCode.ToString("X");
                                   if (gStr.Length < 2)
                                   {
                                       gStr = 0 + gStr;
                                   }
                                   int bCode = gCode * EventCount;
                                   if (255 < bCode)
                                   {
                                       bCode = bCode % 255;
                                   }
                                   string bStr = bCode.ToString("X");
                                   if (bStr.Length < 2)
                                   {
                                       bStr = 0 + bStr;
                                   }
                                   Color color = (Color)cc.ConvertFrom("#FF" + rStr + gStr + bStr);
                                   dbMsg += ",color=" + color;
                                   OneEvent.event_bg_color = color.ToString();                           //背景色
                                   if (Util.IsForegroundWhite(OneEvent.event_bg_color))
                                   {
                                       dbMsg += "に白文字";
                                       OneEvent.event_font_color = Brushes.White.ToString();
                                   }
                                   else
                                   {
                                       dbMsg += "に黒文字";
                                       OneEvent.event_font_color = Brushes.Black.ToString();
                                   }
                                   //1レコード追加
                                   Events.Add(OneEvent);
                                   //次の日時設定
                                   if (8 == EventCount)
                                   {
                                       StartDT = SelectedDateTime.AddMonths(-1);
                                       EndDT = StartDT.AddMonths(2);
                                   }
                                   else if (6 == EventCount)
                                   {
                                       StartDT = SelectedDateTime.AddDays(-4);
                                       EndDT = StartDT.AddDays(8);
                                   }
                                   else if (4 == EventCount)
                                   {
                                       StartDT = SelectedDateTime.AddDays(-1);
                                       EndDT = StartDT.AddDays(2);
                                   }
                                   else
                                   {
                                       StartDT = StartDT.AddHours(1);
                                       EndDT = StartDT.AddHours(1).AddMinutes(30);
                                   }
                               }
                           }
           */

                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
            return Events;
        }


        //本日/////////////////////////////////////////////////////////////////////////
        public ICommand SetToDay => new DelegateCommand(ToDaySet);
        /// <summary>
        /// 本日に指定
        /// </summary>
        public void ToDaySet()
        {
            string TAG = "ToDaySet";
            string dbMsg = "";
            try
            {
                SelectedDateTime = DateTime.Now;
                dbMsg += "今日は" + SelectedDateTime;
                CurrentDate = String.Format("{0:yyyy年MM月dd日}", SelectedDateTime);
                dbMsg += ">>" + CurrentDate;
           //     RaisePropertyChanged("CurrentDate");
                CalenderWrite();
                MyLog(TAG, dbMsg);
            }
            catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////カレンダ作成//
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

    /// <summary>
    /// 一日分の箱
    /// </summary>
    public class ADay
    {
        MainViewModel rootClass;
        /// <summary>
        /// 対象日
        /// </summary>
        public DateTime date { get; }
        /// <summary>
        /// 表示する要約
        /// </summary>
        public List<string> summarys { get; }

        public ObservableCollection<t_events> events { get; set; }

        private int _selectedIndex { set; get; }
        /// <summary>
        /// DataGrid上の選択インデックス
        /// </summary>
        public int selectedIndex {
            get { return _selectedIndex; }
            set {
                if (_selectedIndex == value)
                    return;
                _selectedIndex = value;
                rootClass.selectedIndex = value;
                //			RaisePropertyChanged("selectedIndex");
            }
        }

        public bool _IsChecked;
        /// <summary>
        /// リスト先頭のチェック
        /// </summary>
        public bool IsChecked {
            get { return _IsChecked; }
            set {
                if (_IsChecked == value)
                    return;

                _IsChecked = value;
            }
        }

        /// <summary>
        /// 一日分の箱
        /// </summary>
        public ADay(DateTime _date, List<string> _summarys, ObservableCollection<t_events> _events, MainViewModel _rootClass)
        {
            this.date = _date;
            this.summarys = _summarys;
            this.events = _events;
            this.rootClass = _rootClass;
        }

        //public t_events _TargetEvent;
        ///// <summary>
        ///// 操作対象の予定
        ///// </summary>
        //public t_events TargetEvent {
        //	get { return _TargetEvent; }
        //	set {
        //		if (_TargetEvent == value)
        //			return;
        //		_TargetEvent = new t_events();
        //		_TargetEvent = value;
        //		rootClass.TargetEvent = value;
        //		//			rootClass.RaisePropertyChanged("TargetEvent");
        //	}
        //}
    }

}
