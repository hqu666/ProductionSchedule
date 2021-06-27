﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ProductionSchedule.Properties;                //Settings

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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace ProductionSchedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // :ViewModel, INotifyPropertyChanged

        public Views.MainWindow MyView { get; set; }
        public double MainWindowWidth { get; set; }
        public double MainWindowHight { get; set; }
        public double CalenderTop { get; set; }
        public Grid CalenderGR { get; set; }
        public DataGrid CalenderDG { get; set; }
        public List<Models.MyMenu> _MyMenu { get; set; }
        /// <summary>
        /// イベントボタンリスト
        /// </summary>
   //     public List<Button> BtList;
        public Dictionary<string,Label> LbList;
        public Dictionary<string, StackPanel> SPList;
        public int GridRowCount = 25;
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
        public bool ConnectVisibility { set; get; }
        /// <summary>
        /// 解除ボタン表示
        /// </summary>
        public bool CancelVisibility { set; get; }


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
                    if (!String.IsNullOrEmpty(_GoogleAcountStr) &&
                        !_GoogleAcountStr.Equals(Constant.GoogleAcountMSG)) {

                        Connect();
                    }
                    //変更イベントに使用
                    MyLog(TAG, dbMsg);
                }
                catch (Exception er)
                {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        public List<string> AcounntList { set; get; }

        /// <summary>
        /// 使用するカレンダ
        /// </summary>
        public string CalenderNameStr { set; get; }
        public List<string> CalenderNameList { set; get; }
        public DateTime SelectedDateTime;
        /// <summary>
        /// 表示対象年月
        /// </summary>
        public string CurrentDateStr { get; set; }


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
                EventButtons = new ObservableCollection<EventButton>();
            //    BtList = new List<Button>();
                //本日日付
                SelectedDateTime = DateTime.Today;
                dbMsg += "今日は" + SelectedDateTime;
                CurrentDateStr = String.Format("{0:yyyy年MM月}", SelectedDateTime);
                dbMsg += ">>" + CurrentDateStr;
                this.TargetURLStr = Constant.WebStratUrl;
                dbMsg += ",遷移先URL=  " + TargetURLStr;
                NotifyPropertyChanged("TargetURI");
                //起動時は接続側のみ
                Cancel();
                //    GoogleAcountStr = "hkuwauama@gmail.com";
                var settings = Settings.Default;
                GoogleAcountStr = Constant.GoogleAcountMSG;
                dbMsg += " ,前回使用したアカウント=  " + settings.MyGoogleAcount;
                if (!String.IsNullOrEmpty(settings.MyGoogleAcount)) {
                    GoogleAcountStr = settings.MyGoogleAcount;
                    dbMsg += "、これまでに使用したアカウント=  " + settings.MyAcounts;
                    if (!String.IsNullOrEmpty(settings.MyAcounts)) {
                        string rStr = settings.MyAcounts.Replace("System.String[],","");//誤記入対策
                        string[] acounntList = rStr.Split(',');
                        AcounntList = new List<string>(acounntList);
                        dbMsg += "[  " + AcounntList.Count +"件]" ;
                        NotifyPropertyChanged("AcounntList");
                        //foreach (string lItem in AcounntList) {
                        //    // 項目を追加する
                        //    MyView.GoogleAcountCB.Items.Add(lItem);
                        //}
                    }
                }


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
                dbMsg += ",初期表示=" + this.TargetURLStr;
                ww.VM.TargetURLStr = this.TargetURLStr;
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
                dbMsg += ",アカウント=" + GoogleAcountStr;
                if (String.IsNullOrEmpty(GoogleAcountStr) ||
                    GoogleAcountStr.Equals(Constant.GoogleAcountMSG) ||
                    !GoogleAcountStr.EndsWith("@gmail.com")) {
                    //メッセージボックスを表示する
                    String titolStr = Constant.ApplicationName;
                    String msgStr = "カレンダーを使うGoogleアカウントを入力してください。";
                    MessageBoxResult result = MessageShowWPF(titolStr, msgStr, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    //webでアカウントを拾えるなら　　 MessageBoxButton.YesNoCancel    \r\nwebでアカウントを選択しますか？
                    dbMsg += ",result=" + result;
                    if (result== MessageBoxResult.Yes) {
                        this.TargetURLStr=Constant.GoogleLogInPage;
                        WebStart();
                    }
                } else {
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
                    ConnectVisibility = false;          //colapsed
                    NotifyPropertyChanged("ConnectVisibility");
                    CancelVisibility = true;
                    NotifyPropertyChanged("CancelVisibility");
                    NotifyPropertyChanged();
                    CalenderWrite();
                }

                MyLog(TAG, dbMsg);
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
                if (Constant.MyDriveCredential == null) {
                    //メッセージボックスを表示する
                    String titolStr = Constant.ApplicationName;
                    String msgStr = "認証されませんでした。\r\n更新ボタンをクリックして下さい";
                    MessageBoxResult result = MessageShowWPF(titolStr, msgStr, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    dbMsg += ",result=" + result;
                } else{
                    dbMsg += "\r\nAccessToken=" + Constant.MyDriveCredential.Token.AccessToken;
                    dbMsg += "\r\nRefreshToken=" + Constant.MyDriveCredential.Token.RefreshToken;
                    Constant.MyDriveService = new DriveService(new BaseClientService.Initializer(){
                        HttpClientInitializer = Constant.MyDriveCredential,
                        ApplicationName = Constant.ApplicationName,
                    });
                    dbMsg += ",MyDriveService:ApiKey=" + Constant.MyDriveService.ApiKey;
                    Constant.MyCalendarCredential = Constant.MyDriveCredential;
                    Constant.MyCalendarService = new CalendarService(new BaseClientService.Initializer(){
                        HttpClientInitializer = Constant.MyCalendarCredential,
                        ApplicationName = Constant.ApplicationName,
                    });
                    dbMsg += ",MyCalendarService:ApiKey=" + Constant.MyCalendarService.ApiKey;
                    dbMsg += ">>UserId=" + userCredential.Result.UserId;
                    GoogleAcountStr = userCredential.Result.UserId;
                    var settings = Settings.Default;
                    settings.MyGoogleAcount = GoogleAcountStr;
                    dbMsg += "接続できたアカウント=" + GoogleAcountStr;
                    bool isCont = true;
                    foreach (string chStr in AcounntList) {
                        if (chStr.Equals(GoogleAcountStr)) {
                            isCont = false;
                            dbMsg += "::リストアップ済" ;
                        }
                    }
                    if (isCont) {
                        AcounntList.Add(GoogleAcountStr);
                        dbMsg += "[  " + AcounntList.Count + "件]";
                        string rStrs = "";
                        foreach (string rStr in AcounntList) {
                            if (String.IsNullOrEmpty(rStrs)) {
                                rStrs = rStr;
                            } else {
                                rStrs += ',' + rStr;
                            }
                        }
                        dbMsg += "," + rStrs + "を書込み";
                        settings.MyAcounts = rStrs;
                        settings.Save();
                    }
                }
                CalenderNameStr = "praimary";
                CalenderNameList.Add(CalenderNameStr);


                //Constant.RootFolderID = GDriveUtil.MakeAriadneGoogleFolder();
                //if (Constant.RootFolderID.Equals("")){
                //            dbMsg += ">フォルダ作成>失敗";
                //        }
                //        else
                //        {
                //            dbMsg += "[" + Constant.RootFolderID + "]" + Constant.RootFolderName;
                //        }
                NotifyPropertyChanged();
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
                ConnectVisibility = true;
                CancelVisibility = false;
                NotifyPropertyChanged();
            } catch (Exception er)
            {
                MyErrorLog(TAG, dbMsg, er);
            }

        }
        //カレンダ作成///////////////////////////////////////////////////////////////////Google接続///
        //X_1_4ViewModel
        //表示対象年月
        /// <summary>
        /// Viewの高さ
        /// </summary>
        public double vHeight { get; set; }
        public double rHeight;
        public int startHour;
        public int endHour;


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

        public ObservableCollection<MyListItem> MyListItems { get; set; }
        public ObservableCollection<EventButton> EventButtons { get; set; }

        public ObservableCollection<CalenderRecord> CalenderItems { get; set; }

        public int selectedDateIndex { set; get; }

        public int selectedIndex { set; get; }

        public string[] GoogleColors={"#FFFFFF","#7986CB", "#33B679	", "#8E24AA", "#E67C73	", "#F6BF26",
                                        "#F4511E", "#039BE5", "#616161","#3F51B5", "#0B8043", "#D50000" };
        public List< Button> btList;
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

        //     public ICommand MakeCalender => new DelegateCommand(MakeCalenderBase);
        /// <summary>
        /// 起動時のカレンダ作成
        /// </summary>
        public void MakeCalenderBase() {
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            try {

                //CalenderGR.HorizontalAlignment = HorizontalAlignment.Left;
                //CalenderGR.VerticalAlignment = VerticalAlignment.Top;
                CalenderGR.ShowGridLines = true;
                //CalenderGR.Height = MainWindowHight - CalenderTop;

                // Rowを設定する
                rHeight = 30;
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(0, GridUnitType.Star);
                CalenderGR.RowDefinitions.Add(rowDef);
                //タイトル列
                LbList = new Dictionary<string, Label>();
                for (int wDay = 0; wDay <= 31; wDay++) {
                    // Columnsを設定する
                    ColumnDefinition colDef = new ColumnDefinition();
                    CalenderGR.ColumnDefinitions.Add(colDef);
                    Label nLabel = new Label();
                    nLabel.Name = "l" + wDay;
                    if (0 == wDay) {
                        colDef.MinWidth = 20;
                    } else {
                        colDef.MinWidth = 40;
                        nLabel.MinWidth = 40;
                        nLabel.Content = wDay;
                        nLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
                        //課題：Gridにすると着色するエレメントのマージン分隙間ができる。
                        //         nLabel.Margin ={ -8,-8,-8,-8}:
                        nLabel.SetValue(Grid.RowProperty, 0);
                        nLabel.SetValue(Grid.ColumnProperty, wDay);
                        CalenderGR.Children.Add(nLabel);
                    }
                    nLabel.Height = 40;
                    nLabel.Margin = new Thickness(0, 0, 0, 0);
                    LbList.Add(nLabel.Name,nLabel);
                }
                dbMsg += ",LbList=" + LbList.Count + "件";
                //データレコード
                SPList = new Dictionary<string, StackPanel>();
                //課題：stackPanelに枠線を入れる
                GridRowCount++;
                for (int rowCount = 1; rowCount < GridRowCount; rowCount++) {
                    // Rowを設定する
                    rowDef = new RowDefinition();
                    //    rowDef.Height = new GridLength(1.0, GridUnitType.Star);          // <RowDefinition Height="*"/> と同じ
                    rowDef.Height = GridLength.Auto;            // <RowDefinition Height="Auto"/> と同じ
                    CalenderGR.RowDefinitions.Add(rowDef);
                    //行ラベル
                    Label nLabel = new Label();
                    if (1== rowCount) {
                        nLabel.Content =  "終日";
                    } else {
                        nLabel.Content = (rowCount - 2) + ":00";
                    }
          //          nLabel.Height = 0;
                    nLabel.Margin = new Thickness(0, 0, 0, 0);
                    nLabel.SetValue(Grid.RowProperty, rowCount);
                    nLabel.SetValue(Grid.ColumnProperty, 0);
                    CalenderGR.Children.Add(nLabel);
                    for (int colCount = 1; colCount <= 31; colCount++) {
                        string cellName = "R" + (rowCount-1) + "C" + colCount;
                        StackPanel stackPanel = new StackPanel();
                        stackPanel.Name = cellName;
                        stackPanel.SetValue(Grid.RowProperty, rowCount);
                        stackPanel.SetValue(Grid.ColumnProperty, colCount);
                        CalenderGR.Children.Add(stackPanel);
                        SPList.Add(cellName,stackPanel);
                    }
                }
                dbMsg += ",SPList=" + SPList.Count + "件";
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// 毎月のカレンダ更新
        /// </summary>
        public void CalenderWrite()
        {
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            try
            {
                DateTime cStart = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1);
                DateTime cEnd = cStart.AddMonths(1).AddSeconds(-1);
                dbMsg += cStart + "～" + cEnd + "の予定を読み出しています"; ;
         //       double BtWidth = 0;
                int lEnd = cEnd.Day;
                waitingDLog = new Controls.WaitingDLog();
                waitingDLog.Show(); //.ShowDialog(); だとこのオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
                                    //waitingDLog=wDLog.Result;  だと　呼び出しスレッドは、多数の UI コンポーネントが必要としているため、STA である必要があります。
                                    //	waitingDLog.ShowDialog();  // Show() にするとWaitingCircleが回らない
                                    //						///////////WindowにユーザーコントロールをWindowに読み込む方法//
                                    // this.EDays = CalenderWriteBody(waitingDLog);
                Task<ObservableCollection<MyListItem>> retEvents = Task.Run(() => {
                    //waitingDLog.Dispatcher.Invoke((Action)(() => {
                    //	waitingDLog.ShowDialog();  // Show() にするとWaitingCircleが回らない
                    //}));　だと表示もされない
                    //waitingDLog.ShowDialog(); だとこのオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。
                    return GrtMyEvent(cStart, cEnd);
                });
                retEvents.Wait();
                waitingDLog.Close();
                waitingDLog.QuitMe();
                waitingDLog = null;
                //Taskの戻り値が使えないのでグローバル変数から取得
                if (0==MyListItems.Count) {
                    String titolStr = Constant.ApplicationName;
                    String msgStr = GoogleAcountStr + "のカレンダに登録がありません。\r\nアカウントか年月を変えてみて下さい";
                    MessageBoxResult result = MessageShowWPF(titolStr, msgStr, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }else{
                    dbMsg += ",ウインドウ＝;" + MainWindowWidth;
                    //double gridLeft = CalenderGR.Margin.Left;
                    //dbMsg += ",グリッド左＝;" + gridLeft;
                    //double myCellWidth = (MainWindowWidth - (gridLeft * 2) - 200) / 31;
                    //dbMsg += ",セルj幅＝;" + myCellWidth;
                    dbMsg += "\r\n削除";
                    //foreach (Button dBt in BtList) {
                    //    dbMsg += "," + dBt.Name;
                    //    CalenderGR.Children.Remove(dBt);
                    //}
                    foreach (EventButton eb in EventButtons) {
                        Button dBt = eb.eButton;
                        dbMsg += "," + dBt.Name;
                        if (eb.stackPanel !=null) {
                            eb.stackPanel.Children.Remove(dBt);
                            //Buttinが配置時に上げた階層を下げる
                            Panel.SetZIndex(eb.stackPanel, Panel.GetZIndex(eb.stackPanel) - 2);
                        } else {
                            CalenderGR.Children.Remove(dBt);
                        }
                    }

                    //タイトル列の曜日着色
                    for (int wDay = 1; wDay <= 31; wDay++) {
                        DateTime cDay = cStart.AddDays(wDay - 1);
                        DayOfWeek dow = cDay.DayOfWeek;
                        string lavelName = "l" + wDay;
                        Label nLabel = LbList[lavelName];
                        if (nLabel != null) {
                  //          dbMsg += ",日付ラベル" + nLabel.Name;
                            nLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
                            nLabel.Foreground = Brushes.White;
                            Brush stackPanelBackground = Brushes.White;
                            if (lEnd < wDay) {
                                nLabel.Background = Brushes.DarkGray;
                                stackPanelBackground = Brushes.DarkGray;
                            } else if (dow == DayOfWeek.Sunday) {
                                nLabel.Background = Brushes.Red;
                                stackPanelBackground = Brushes.LightPink;
                            } else if (dow == DayOfWeek.Saturday) {
                                nLabel.Background = Brushes.Blue;
                                stackPanelBackground = Brushes.LightBlue;
                            } else {
                                nLabel.Foreground = Brushes.Black;
                                nLabel.Background = Brushes.White;
                            }
                            for (int rowCount = 0; rowCount < (GridRowCount-1); rowCount++) {
                                string cellName = "R" + rowCount + "C" + wDay;
                                StackPanel stackPanel = SPList[cellName]; ;           // new StackPanel();
                                if (stackPanel != null) {
                             //       dbMsg += ",stackPanel" + stackPanel.Name;
                                    stackPanel.Background = stackPanelBackground;
                                }
                            }
                        }
                    }

                    dbMsg += "、全高=" + CalenderGR.ActualHeight;
             //       CalenderGR.Height=CalenderGR.ActualHeight;
                    int endRowNo = CalenderGR.RowDefinitions.Count;
                    dbMsg += "、最終行=" + endRowNo;
                    rHeight = CalenderGR.ActualHeight/ endRowNo;
                    dbMsg += "、行高=" + rHeight;
                    //for (int rowCount = 2; rowCount <= endRowNo; rowCount++) {
                    // //   GridLength rHi = CalenderGR.RowDefinitions[rowCount].Height;
                    //    if ((startHour-1) <=rowCount && rowCount <=(endHour+1)) {
                    //        CalenderGR.RowDefinitions[rowCount].Height= new GridLength(rHeight, GridUnitType.Star);
                    //    } else {
                    //        CalenderGR.RowDefinitions[rowCount].Height = new GridLength(0, GridUnitType.Star);
                    //    }
                    //}


                    //イベント配置
                    int btCount = 0;
                    EventButtons = new ObservableCollection<EventButton>();
       //             BtList = new List<Button>();
                    CS_Util Util = new CS_Util();

                    foreach (MyListItem MLI in MyListItems) {
                        btCount++;
                        dbMsg += "\r\n[" + btCount+"]"+ MLI.startDTStr+ "～" + MLI.endDTStr;
                        EventButton eventButton = new EventButton();
                        eventButton.eListItem = MLI;
                        string ButtonFace = "";
                        if (!String.IsNullOrEmpty(MLI.startDT.ToString("yyyyMMdd"))) {
                            ButtonFace += MLI.description;
                        }
                        if (!String.IsNullOrEmpty(MLI.summary)) {
                            ButtonFace += MLI.summary;
                        }
                        dbMsg += ";" + ButtonFace + "[" + MLI.googleEvent.ColorId + "]";
                        int ColorId = 7;
                        if (! String.IsNullOrEmpty(MLI.googleEvent.ColorId)) {
                            ColorId = int.Parse(MLI.googleEvent.ColorId);
                        }
                        Thickness btMargin =  new Thickness(2, 2, 2, 2);

                        Button wBt = new Button();
                        wBt.Name = "Bt_" + MLI.startDT.ToString("yyyyMMdd") + "_" + btCount;
                        string ColorIdStr = MLI.googleEvent.ColorId;
                        object obj = System.Windows.Media.ColorConverter.ConvertFromString(GoogleColors[ColorId]);
                        SolidColorBrush BGColor = new SolidColorBrush((System.Windows.Media.Color)obj);
                        wBt.Background = BGColor;
                        dbMsg += "、背景色=" + BGColor;
                        if (Util.IsForegroundWhite(BGColor.ToString())) {
                            dbMsg += "に白文字";
                            wBt.Foreground = Brushes.White;
                        } else {
                            dbMsg += "に黒文字";
                            wBt.Foreground = Brushes.Black;
                        }
                        wBt.Click += (sender, e) => EventButtonClick(sender);
                        ButtonFace = MLI.startDT.ToString("HH:mm") + "～" + MLI.endDT.ToString("HH:mm") + "\r\n" + ButtonFace;
                        wBt.Content = ButtonFace;
                        wBt.BorderBrush = Brushes.White;
                        wBt.BorderThickness = new Thickness(2, 2, 2, 2);
                        //       wBt.Padding = new Thickness(2, 2, 2, 2);
                        wBt.Margin = btMargin;


                        //課題：stackPanel内に配置させる
                        //        Google.Apis.Calendar.v3.Data.EventDateTime startDT = MLI.googleEvent.Start;
                        int startRow = MLI.startDT.Hour+2;
                        int endtRow = MLI.endDT.Hour + 2;
                        dbMsg += "～" + endtRow;
                        int rowSpan = endtRow - startRow;
                        dbMsg += "[" + rowSpan + "時間]";
                        dbMsg += "、R=" + startRow;
                        int startCol = MLI.startDT.Day;
                        dbMsg += "、C=" + startCol;
                        if (MLI.startDTStr.Contains('-')) {
                            dbMsg += "、終日";
                            startRow = MLI.startDT.Hour + 1;
                            ButtonFace = MLI.startDT.ToString("MM/dd") + "\r\n" + ButtonFace;
                        }else if (rowSpan <=1) {
                            string cellName = "R" + (startRow-1) + "C" + startCol;
                            eventButton.stackPanel = SPList[cellName];
                            //既にButtinが配置されている場合もあるので2階層づつ上げる
                            Panel.SetZIndex(eventButton.stackPanel, Panel.GetZIndex(eventButton.stackPanel) +2);
                            eventButton.stackPanel.Children.Add(wBt);

                        } else {
                            //課題；左肩に文字表示
                            wBt.HorizontalContentAlignment = HorizontalAlignment.Left;
                            wBt.VerticalContentAlignment = VerticalAlignment.Top;
                            //課題；文字回転
                            //課題；開始分数はみ出させる。
                            if (0 < MLI.startDT.Minute) {
                                int startMinute = MLI.startDT.Minute;
                                dbMsg += "開始分=" + startMinute;
                                double startMargin = rHeight * (startMinute / 60);
                                dbMsg += ",はみ出し=" + startMargin;
                                btMargin.Top = startMargin;
                            }
                            wBt.SetValue(Grid.RowProperty, startRow);
                            if (!MLI.startDTStr.Contains('-')) {
                                if (1 < rowSpan) {
                                    wBt.SetValue(Grid.RowSpanProperty, rowSpan);
                                }
                            }
                            //終了分数はみ出させる。
                            if (0 < MLI.endDT.Minute) {
                                int endMinute = MLI.endDT.Minute;
                                dbMsg += "終了分=" + endMinute;
                                double endMargin = -rHeight * (endMinute / 60);
                                dbMsg += ",はみ出し=" + endMargin;
                                btMargin.Bottom = endMargin;
                            }
                            wBt.SetValue(Grid.ColumnProperty, startCol);
                            int endCol = MLI.endDT.Day;
                            dbMsg += "～" + endCol + "日";
                            int colSpan = endCol - startCol;
                            dbMsg += "[" + colSpan + "日間]";
                            if (1 < colSpan) {
                                wBt.SetValue(Grid.ColumnSpanProperty, colSpan + 1);
                            }
                            CalenderGR.Children.Add(wBt);
                        }
                        eventButton.eButton = wBt;
                        EventButtons.Add(eventButton);
         //               BtList.Add(wBt);
                    }
                                     
                }
/*
                //最低限のレコード作成でDataGridのItemsSource作成
                CalenderItems = new ObservableCollection<CalenderRecord>();
                CalenderRecord CalenderRowItem = new CalenderRecord();
                CalenderItems.Add(CalenderRowItem);
                CalenderRowItem = new CalenderRecord();
                CalenderItems.Add(CalenderRowItem);
                NotifyPropertyChanged("CalenderItems");
                DataGridCell cell = GetDataGridCell(CalenderDG, 0, 0);
                cell.MinWidth = 150;
                dbMsg += ",ウインドウ＝;" + MainWindowWidth;
                double gridLeft = CalenderDG.Margin.Left;
                dbMsg += ",グリッド左＝;" + gridLeft;
                double myCellWidth = (MainWindowWidth - (gridLeft*2) - 200) / 31;
                dbMsg += ",セルj幅＝;" + myCellWidth;
                for (int rowCount=0; rowCount < 2; rowCount++){
                    if (0== rowCount) {
                        for (int colCount = 1; colCount <= 31; colCount++) {
                            DateTime cDay = cStart.AddDays(colCount - 1);
                            DayOfWeek dow = cDay.DayOfWeek;
                            cell = GetDataGridCell(CalenderDG, rowCount, colCount);
                            cell.MinWidth = myCellWidth;   //  反映されない
                            object content = cell.Content;
                            cell.Content = colCount.ToString();
                            //文字の右詰め//
                            cell.HorizontalContentAlignment = HorizontalAlignment.Right;
                            //効かず　HorizontalContentAlignment    HorizontalAlignment
                            //textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                            cell.Foreground = Brushes.White;
                            if (lEnd < colCount) {
                                cell.Background = Brushes.DarkGray;
                            } else if (dow == DayOfWeek.Sunday) {
                                cell.Background = Brushes.Red;
                            } else if (dow == DayOfWeek.Saturday) {
                                cell.Background = Brushes.Blue;
                            } else {
                                cell.Background = Brushes.White;
                                cell.Foreground = Brushes.Black;
                            }
                        }
                    } else {
                        for (int colCount = 1; colCount <= 31; colCount++) {
                            DateTime cDay = cStart.AddDays(colCount - 1);
                            DayOfWeek dow = cDay.DayOfWeek;
                            cell = GetDataGridCell(CalenderDG, rowCount, colCount);
                            cell.Name = "R" + rowCount + "C" + colCount;
                            cell.Content = cell.Name;
                            //     TextBlock textBlock = content as TextBlock;
                            //    textBlock.Text = colCount.ToString();
                            if (lEnd < colCount) {
                                cell.Background = Brushes.DarkGray;
                            } else if (dow == DayOfWeek.Sunday) {
                                cell.Background = Brushes.LightPink;
                            } else if (dow == DayOfWeek.Saturday) {
                                cell.Background = Brushes.LightCyan;
                            } else {
                                cell.Background = Brushes.White;
                            }
                       //     object content = cell.Content;
                       //     StackPanel stackPanel = content as StackPanel;
                       ////     stackPanel.Name = "R" + rowCount + "C" + colCount;
                       //     stackPanel.Orientation = Orientation.Vertical;
                       //     stackPanel.Background.Opacity = 0.5;

                        }
                        foreach (MyListItem MLI in MyListItems) {
                            dbMsg += "\r\n" + MLI.startDTStr + "～" + MLI.endDTStr;
                            int startCol = MLI.startDT.Day;

                            //Google.Apis.Calendar.v3.Data.EventDateTime startDT = MLI.googleEvent.Start;
                            //int startCol = startDT.DateTime.Value.Day;
                            cell = GetDataGridCell(CalenderDG, rowCount, startCol);
                            object content = cell.Content;
                            Button eButton = content as Button;

                            //                  TextBlock textBlock = content as TextBlock;
                            string ButtonFace = "";
                            if (!String.IsNullOrEmpty(MLI.description)) {
                                ButtonFace += MLI.description;
                            }
                            if (!String.IsNullOrEmpty(MLI.summary)) {
                                ButtonFace += MLI.summary;
                            }
                            dbMsg += ";" + ButtonFace + "[" + MLI.googleEvent.ColorId + "]";
                            eButton.Content = ButtonFace;
                            //            textBlock.Text = ButtonFace;
                            int ColorId = 7;
                            if (!String.IsNullOrEmpty(MLI.googleEvent.ColorId)) {
                                ColorId = int.Parse(MLI.googleEvent.ColorId);
                            }

                            //Button wBt = new Button();
                            //wBt.Content = ButtonFace;
                            ////        wBt.Padding.Top = (dDouble)-5;
                            //wBt.Width = BtWidth;
                            string ColorIdStr = MLI.googleEvent.ColorId;
                            object obj = System.Windows.Media.ColorConverter.ConvertFromString(GoogleColors[ColorId]);
                            SolidColorBrush ret = new SolidColorBrush((System.Windows.Media.Color)obj);
                            eButton.Background = ret;
                            //           textBlock.Background = ret;
                            //wBt.Background = ret;
                            //wBt.SetValue(Grid.RowProperty, 1);
                            //wBt.SetValue(Grid.ColumnProperty, startCol);
                            //CalenderGR.Children.Add(wBt);
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
        }


        private void EventButtonClick(object sender) {
            string TAG = "EventButtonClick";
            string dbMsg = "";
            MyListItems = new ObservableCollection<MyListItem>();
            try {
                Button bt = (Button)sender;
                dbMsg += bt.Name + "がクリックされました。";
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }



        //https://www.paveway.info/entry/2019/05/27/wpf_datagridbackground から
        public DataGridCell GetDataGridCell(DataGrid dataGrid, int rowIndex, int columnIndex) {
            if (dataGrid.Items == null || dataGrid.Items.IsEmpty) {
                return null;
            }

            var row = GetDataGridRow(dataGrid, rowIndex);
            if (row == null) {
                return null;
            }

            var presenter = GetVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) {
                return null;
            }

            var generator = presenter.ItemContainerGenerator;
            var cell = generator.ContainerFromIndex(columnIndex) as DataGridCell;
            if (cell == null) {
                dataGrid.UpdateLayout();
                var column = dataGrid.Columns[columnIndex];
                dataGrid.ScrollIntoView(row, column);
                cell = generator.ContainerFromIndex(columnIndex) as DataGridCell;
            }
            return cell;
        }

        public DataGridRow GetDataGridRow(DataGrid dataGrid, int index) {
            if (dataGrid.Items == null || dataGrid.Items.IsEmpty) {
                return null;
            }

            var generator = dataGrid.ItemContainerGenerator;
            var row = generator.ContainerFromIndex(index) as DataGridRow;
            if (row == null) {
                dataGrid.UpdateLayout();
                var item = dataGrid.Items[index];
                dataGrid.ScrollIntoView(item);
                row = generator.ContainerFromIndex(index) as DataGridRow;
            }
            return row;
        }

        private T GetVisualChild<T>(Visual parent) where T : Visual {
            T result = default(T);
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; ++i) {
                var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                result = child as T;
                if (result != null) {
                    break;
                }

                result = GetVisualChild<T>(child);
            }
            return result;
        }

        /// <summary>
        /// カレンダ作成本体
        /// </summary>
        private ObservableCollection<ADay> CalenderWriteBody(Controls.WaitingDLog waitingDLog)
        {
            //
            string TAG = MethodBase.GetCurrentMethod().Name;
            string dbMsg = "";
            try
            {
                DateTime cStart = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1);
                DateTime cEnd = cStart.AddMonths(1).AddSeconds(-1);
                string msgStr = cStart + "～" + cEnd + "の予定を読み出しています"; ;
                dbMsg += msgStr;
                waitingDLog.SetMes(msgStr);
                ObservableCollection<t_events> Events = WriteEvent();

                //Application.Current.Dispatcher.Invoke(new System.Action(() => this.waitingDLog.SetMes(msgStr)));
                /*
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
                  */
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
        /// 対象期間内にGoogleカレンダに登録してあるイベントを取得する
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MyListItem> GrtMyEvent(DateTime timeMin, DateTime timeMax) {
            string TAG = "GrtMyEvent";
            string dbMsg = "";
            MyListItems = new ObservableCollection<MyListItem>();
            try {

                startHour=23;
                endHour=0;
                dbMsg += "" + SelectedDateTime;
                CS_Util Util = new CS_Util();
                //予定取得///////////////////////////////////////////
                GoogleCalendarUtil GCU = new GoogleCalendarUtil();
                ////月初め
                //DateTime timeMin = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1);
                ////月終わり
                //DateTime timeMax = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).AddMonths(1).AddDays(-1);
                dbMsg += "対象期間：" + timeMin + "～" + SelectedDateTime + "～" + timeMax;
                IList<Google.Apis.Calendar.v3.Data.Event> ReadEvents = GCU.GEventsListUp(timeMin, timeMax);
                dbMsg += "、イベント：" + ReadEvents.Count + "件";
                if (0 < ReadEvents.Count) {
                    string format = "yyyy/MM/dd HH:mm:ss";
                    ///    EDays = new ObservableCollection<ADay>();
                    foreach (var rEvent in ReadEvents) {
                        MyListItem MLI = new MyListItem();
                        MLI.isChanged = false;
                        MLI.startDTStr = rEvent.Start.Date;
                   //     dbMsg += "\r\n" + MLI.startDTStr;
                        if (rEvent.Start.DateTime!=null) {
                            MLI.startDTStr = rEvent.Start.DateTime.ToString();
                            dbMsg += ">>" + MLI.startDTStr;
                            if (rEvent.Start.DateTime.Value.Hour< startHour) {
                                startHour =rEvent.Start.DateTime.Value.Hour;
                            }
                        }
                        MLI.endDTStr = rEvent.End.Date;
                        //     dbMsg += "\r\n" + MLI.startDTStr;
                        if (rEvent.End.DateTime != null) {
                            MLI.endDTStr = rEvent.End.DateTime.ToString();
                            dbMsg += ">>" + MLI.endDTStr;
                            if (rEvent.End.DateTime.Value.Hour > endHour) {
                                endHour = rEvent.End.DateTime.Value.Hour;
                            }
                        }
                        MLI.description = rEvent.Description;
                        MLI.summary = rEvent.Summary;
                        if (String.IsNullOrEmpty(MLI.summary)) {
                            MLI.summary = " - ";
                        }
                        MLI.googleEvent = rEvent;
                        MyListItems.Add(MLI);
                    }
                    //ItemsSourceを強制的に更新；無いと更新されない
                }
                NotifyPropertyChanged("MyListItems");
                dbMsg += "\r\n" + MyListItems.Count + "レコード";
                dbMsg += ",イベント：" + startHour + "～" + endHour + "時";
                MyLog(TAG, dbMsg);
            }catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
            return MyListItems;
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
                //月初め
                DateTime timeMin = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1);
                //月終わり
                DateTime timeMax = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, 1).AddMonths(1).AddDays(-1);
                dbMsg +="対象期間：" + timeMin+ "～" + SelectedDateTime + "～" + timeMax;
                IList<Google.Apis.Calendar.v3.Data.Event> ReadEvents = GCU.GEventsListUp(timeMin, timeMax);
                dbMsg += "、イベント：" + ReadEvents.Count + "件";
                MyListItems = new ObservableCollection<MyListItem>();
                if (0<ReadEvents.Count){
                    string format = "yyyy/MM/dd HH:mm:ss";
                    ///    EDays = new ObservableCollection<ADay>();
                    foreach (var rEvent in ReadEvents) {
                        MyListItem MLI = new MyListItem();
                        MLI.startDTStr = rEvent.Start.DateTime.ToString();
                        dbMsg += "\r\n" + MLI.startDTStr;
                        if (String.IsNullOrEmpty(MLI.startDTStr)) {
                            MLI.startDTStr = rEvent.Start.Date;
                            dbMsg += ">>" + MLI.startDTStr;
                        }
                        MLI.endDTStr = rEvent.End.DateTime.ToString();
                        dbMsg += "～" + MLI.endDTStr;
                        if (String.IsNullOrEmpty(MLI.endDTStr)) {
                            MLI.endDTStr = rEvent.End.Date;
                            dbMsg += ">>" + MLI.endDTStr;
                        }
                        dbMsg += ";" + rEvent.Description + ";" + rEvent.Summary;
                        MLI.description = rEvent.Description;
                        MLI.summary = rEvent.Summary;
                        if (String.IsNullOrEmpty(MLI.summary)) {
                            MLI.summary = " - ";
                        }
                        MLI.googleEvent = rEvent;
                        MyListItems.Add(MLI);
                    }
                    //ItemsSourceを強制的に更新；無いと更新されない
                }
                NotifyPropertyChanged("MyListItems");
                dbMsg += "\r\n" + MyListItems.Count + "レコード";

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
                               DateTime startDTStr = DateTime.Today.AddHours(SelectedDateTime.Hour).ToUniversalTime();
                               DateTime endDTStr = startDTStr.AddHours(1).AddMinutes(30);
                               // Infragistics.Controls.Schedules のメタデータ
                               for (EventCount = Events.Count + 1; EventCount < endCount; EventCount++)
                               {
                                   dbMsg += "\r\n[" + EventCount + "]" + startDTStr + "～" + endDTStr;
                                   Models.t_events OneEvent = new Models.t_events();
                                   OneEvent.id = -1;
                                   OneEvent.event_title = "Test" + EventCount;         //タイトル
                                   OneEvent.event_date_start = startDTStr.Date;            //開始日
                                   OneEvent.event_time_start = startDTStr.Hour;           //開始時刻
                                   OneEvent.event_date_end = endDTStr.Date;               //終了日
                                   OneEvent.event_time_end = endDTStr.Hour;               //終了時刻
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
                                       startDTStr = SelectedDateTime.AddMonths(-1);
                                       endDTStr = startDTStr.AddMonths(2);
                                   }
                                   else if (6 == EventCount)
                                   {
                                       startDTStr = SelectedDateTime.AddDays(-4);
                                       endDTStr = startDTStr.AddDays(8);
                                   }
                                   else if (4 == EventCount)
                                   {
                                       startDTStr = SelectedDateTime.AddDays(-1);
                                       endDTStr = startDTStr.AddDays(2);
                                   }
                                   else
                                   {
                                       startDTStr = startDTStr.AddHours(1);
                                       endDTStr = startDTStr.AddHours(1).AddMinutes(30);
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

        //戻し/////////////////////////////////////////////////////////////////////////
        public ICommand BackDate => new DelegateCommand(DateBack);
        /// <summary>
        /// 戻る
        /// </summary>
        public void DateBack() {
            string TAG = "DateBack";
            string dbMsg = "";
            try {
                dbMsg += "対象年月=" + SelectedDateTime;
                SelectedDateTime=SelectedDateTime.AddMonths(-1);
                dbMsg += ">>" + SelectedDateTime;
                CurrentDateStr = String.Format("{0:yyyy年MM月}", SelectedDateTime);
                dbMsg += ">>" + CurrentDateStr;
                NotifyPropertyChanged("CurrentDateStr");
                CalenderWrite();
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
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
                CurrentDateStr = String.Format("{0:yyyy年MM月}", SelectedDateTime);
                dbMsg += ">>" + CurrentDateStr;
                NotifyPropertyChanged("CurrentDateStr");
                CalenderWrite();
                MyLog(TAG, dbMsg);
            } catch (Exception er){
                MyErrorLog(TAG, dbMsg, er);
            }
        }
        //進める/////////////////////////////////////////////////////////////////////////
        public ICommand SendDate => new DelegateCommand(DateSend);
        /// <summary>
        /// 進める
        /// </summary>
        public void DateSend() {
            string TAG = "DateSend";
            string dbMsg = "";
            try {
                dbMsg += "対象年月=" + SelectedDateTime;
                SelectedDateTime=SelectedDateTime.AddMonths(1);
                dbMsg += ">>" + SelectedDateTime;
                CurrentDateStr = String.Format("{0:yyyy年MM月}", SelectedDateTime);
                dbMsg += ">>" + CurrentDateStr;
                NotifyPropertyChanged("CurrentDateStr");
                CalenderWrite();
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
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
    /// リスト表示のためのモデル
    /// </summary>
    public class MyListItem {
        ///<summary>
        ///変更された
        ///</summary>
        public bool isChanged { get; set; }
        ///<summary>
        ///終日
        ///</summary>
        public bool isDaylong { get; set; }

        public DateTime startDT { get; set; }
        private string _startDTStr { set; get; }
        /// <summary>
        /// 開始日時
        /// </summary>
        public string startDTStr {
            get { return _startDTStr; }
            set {
                if (_startDTStr == value)
                    return ;
                _startDTStr = value;
                startDT = DateTime.Parse(value);
            }
        }


        public DateTime endDT { get; set; }
        private string _endDTStr { set; get; }
        /// <summary>
        /// 終了日時
        /// </summary>
        public string endDTStr {
            get { return _endDTStr; }
            set {
                if (_endDTStr == value)
                    return;
                _endDTStr = value;
                endDT = DateTime.Parse(value);
            }
        }


        /// <summary>
        /// タイトル
        /// </summary>
        public string description { get; set; }
        
        /// <summary>
        /// 表示する要約
        /// </summary>
        public string summary { get; set; }

        /// <summary>
        /// GoogleCalendarの登録済みEvent
        /// </summary>
        public Google.Apis.Calendar.v3.Data.Event googleEvent { get; set; }
    }

    public class CalenderRecord {
        public string titol { get; set; }
        public string d01 { get; set; }
        public string d02 { get; set; }
        public string d03 { get; set; }
        public string d04 { get; set; }
        public string d05 { get; set; }
        public string d06 { get; set; }
        public string d07 { get; set; }
        public string d08 { get; set; }
        public string d09 { get; set; }
        public string d10 { get; set; }
        public string d11 { get; set; }
        public string d12 { get; set; }
        public string d13 { get; set; }
        public string d14 { get; set; }
        public string d15 { get; set; }
        public string d16 { get; set; }
        public string d17 { get; set; }
        public string d18 { get; set; }
        public string d19 { get; set; }
        public string d20 { get; set; }
        public string d21 { get; set; }
        public string d22 { get; set; }
        public string d23 { get; set; }
        public string d24 { get; set; }
        public string d25 { get; set; }
        public string d26 { get; set; }
        public string d27 { get; set; }
        public string d28 { get; set; }
        public string d29 { get; set; }
        public string d30 { get; set; }
        public string d31 { get; set; }
    }

    /// <summary>
    /// イベントボタン
    /// </summary>
    public class EventButton {

        public bool isInStackPanel { get; set; }
        public Button eButton { get; set; }
        /// <summary>
        /// 格納するスタックパネル
        /// </summary>
        public StackPanel stackPanel { get; set; }

        public int startRow { get; set; }
        public double startMargint { get; set; }
        public int endRow { get; set; }
        public double enrMargint { get; set; }
        public int rowSpan { get; set; }

        public int startCol { get; set; }
        public double startColMargint { get; set; }
        public int endCol { get; set; }
        public double enrColMargint { get; set; }
        public int colSpan { get; set; }


        /// <summary>
        /// リスト表示のためのモデル
        /// </summary>
        public MyListItem eListItem { get; set; }

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
