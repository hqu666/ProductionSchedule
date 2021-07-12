using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
//using ProductionSchedule.Properties;                //Settings
//using ProductionSchedule.Views;
//using ProductionSchedule.Models;
//using ProductionSchedule.Enums;
using System.Reflection;
using System.Windows.Input;
using Google.Apis.Drive.v3;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.IO;
//using ProductionSchedule.Controls;

namespace ProductionSchedule.ViewModels {
    class DBViewModel : INotifyPropertyChanged {
        public Views.DBWindow MyView { get; set; }

        private int _SelParentID;
        /// <summary>
        /// 選択しているアイテムの親のID
        /// </summary>
        public int SelParentID {
            get {
                return _SelParentID;
            }
            set {
                string TAG = "SelParentIDStr.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>親のID=  " + value;
                    if (value == _SelParentID)
                        return;
                    _SelParentID = value;
                    string varStr = _SelParentID.ToString();
                    if (varStr == SelParentIDStr)
                        return;
                    SelParentIDStr = varStr;

                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _SelParentIDStr;
        /// <summary>
        /// 選択しているアイテムの親のID：表示
        /// </summary>
        public string SelParentIDStr {
            get {
                return _SelParentIDStr;
            }
            set {
                string TAG = "SelParentIDStr.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>親のID=  " + value;
                    if (value == _SelParentIDStr)
                        return;
                    _SelParentIDStr = value;
                    int varInt = int.Parse(_SelParentIDStr);
                    if (varInt == SelParentID)
                        return;
                    SelParentID = varInt;

                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _SelParentName;
        /// <summary>
        /// 選択しているアイテムの親の名称
        /// </summary>
        public string SelParentName {
            get {
                return _SelParentName;
            }
            set {
                string TAG = "SelParentIDStr.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>親のID=  " + value;
                    if (value == _SelParentName)
                        return;
                    _SelParentName = value;
                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private int _SelItemID;
        /// <summary>
        /// 選択しているアイテムのID
        /// </summary>
        public int SelItemID {
            get {
                return _SelItemID;
            }
            set {
                string TAG = "SelItemID.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>選択しているアイテムのID=  " + value;
                    if (value == _SelParentID)
                        return;
                    _SelItemID = value;

                    string varStr = _SelItemID.ToString();
                    if (varStr == SelItemIDStr)
                        return;
                    SelItemIDStr = varStr;

                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _SelItemIDStr;
        /// <summary>
        /// 選択しているアイテムのID：表示
        /// </summary>
        public string SelItemIDStr {
            get {
                return _SelItemIDStr;
            }
            set {
                string TAG = "SelItemIDStr.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>選択しているアイテムのID=  " + value;
                    if (value == _SelItemIDStr)
                        return;
                    _SelItemIDStr = value;

                    int varInt = int.Parse(_SelItemIDStr);
                    if (varInt == SelItemID)
                        return;
                    SelItemID = varInt;

                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _SelItemName;
        /// <summary>
        /// 選択しているアイテムの名称
        /// </summary>
        public string SelItemName {
            get {
                return _SelItemName;
            }
            set {
                string TAG = "SelItemName.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>選択しているアイテムの名称=  " + value;
                    if (value == _SelItemName)
                        return;
                    _SelItemName = value;
                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private int _SelItemhierarchy;
        /// <summary>
        /// 選択しているアイテムのID
        /// </summary>
        public int SelItemHierarchy {
            get {
                return _SelItemhierarchy;
            }
            set {
                string TAG = "SelItemhierarchy.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>選択しているアイテムのID=  " + value;
                    if (value == _SelItemhierarchy)
                        return;
                    _SelItemhierarchy = value;

                    string varStr = _SelItemhierarchy.ToString();
                    if (varStr == SelItemHierarchyStr)
                        return;
                    SelItemHierarchyStr = varStr;
                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }

        private string _SelItemHierarchyStr;
        /// <summary>
        /// 選択しているアイテムのID：表示
        /// </summary>
        public string SelItemHierarchyStr {
            get {
                return _SelItemHierarchyStr;
            }
            set {
                string TAG = "SelItemHierarchyStr.set";
                string dbMsg = "";
                try {
                    dbMsg += ">>選択しているアイテムのID=  " + value;
                    if (value == _SelItemHierarchyStr)
                        return;
                    _SelItemHierarchyStr = value;

                    int varInt = int.Parse(_SelItemHierarchyStr);
                    if (varInt == SelItemHierarchy)
                        return;
                    SelItemHierarchy = varInt;

                    MyLog(TAG, dbMsg);
                } catch (Exception er) {
                    MyErrorLog(TAG, dbMsg, er);
                }
            }
        }



        public DBViewModel() {
            Initialize();
        }

        public void Initialize() {
            string TAG = "Initialize";
            string dbMsg = "";
            try {
                //SelItemID = 999999;
                //SelItemName = "選択しているアイテム";
                //SelItemHierarchy = 9;
                //SelParentID = 888888;
                //SelParentName = "親の名称";
                NotifyPropertyChanged();
                //EventButtons = new ObservableCollection<EventButton>();
                ////本日日付
                //SelectedDateTime = DateTime.Today;
                //dbMsg += "今日は" + SelectedDateTime;
                //CurrentDateStr = String.Format("{0:yyyy年MM月}", SelectedDateTime);
                //dbMsg += ">>" + CurrentDateStr;
                //this.TargetURLStr = Constant.WebStratUrl;
                //dbMsg += ",遷移先URL=  " + TargetURLStr;
                //NotifyPropertyChanged("TargetURI");
                ////起動時は接続側のみ
                //Cancel();
                ////    GoogleAcountStr = "hkuwauama@gmail.com";
                //var settings = Settings.Default;
                //GoogleAcountStr = Constant.GoogleAcountMSG;
                //dbMsg += " ,前回使用したアカウント=  " + settings.MyGoogleAcount;
                //if (!String.IsNullOrEmpty(settings.MyGoogleAcount)) {
                //    GoogleAcountStr = settings.MyGoogleAcount;
                //    dbMsg += "、これまでに使用したアカウント=  " + settings.MyAcounts;
                //    if (!String.IsNullOrEmpty(settings.MyAcounts)) {
                //        string rStr = settings.MyAcounts.Replace("System.String[],", "");//誤記入対策
                //        string[] acounntList = rStr.Split(',');
                //        AcounntList = new List<string>(acounntList);
                //        dbMsg += "[  " + AcounntList.Count + "件]";
                //        NotifyPropertyChanged("AcounntList");
                //        //foreach (string lItem in AcounntList) {
                //        //    // 項目を追加する
                //        //    MyView.GoogleAcountCB.Items.Add(lItem);
                //        //}
                //    }
                //}
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        // ファイルの操作/////////////////////

        // スプレットシート/////////////////////ファイルの操作
        // https://www.ka-net.org/blog/?p=7007
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string AppClientId = "(クライアント ID)";
        static string AppClientSecret = "(クライアント シークレット)";
        static string SpreadSheetId = "(操作対象となるシートのID)";

        public static void ReadSheet(string[] args) {
            UserCredential credential;
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
              new ClientSecrets {
                  ClientId = AppClientId,
                  ClientSecret = AppClientSecret
              },
              Scopes,
              "user",
              CancellationToken.None,
              new FileDataStore(credPath, true)
            ).Result;
            //Console.WriteLine("Credential file saved to: " + credPath);

            var service = new SheetsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //Sheet1のセルA1の値取得
            SpreadsheetsResource.ValuesResource.GetRequest req1 = service.Spreadsheets.Values.Get(SpreadSheetId, "Sheet1!A1");
            IList<IList<Object>> values = req1.Execute().Values;
            Console.WriteLine(values[0][0]);
            Console.ReadKey(true);
        }
        ///////////////////////
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //System.Windows.Input; Livet Messengerでも使う///////////////////////
        void RequeryCommands() {
            CommandManager.InvalidateRequerySuggested();
        }


        ///////////////////////
        public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
                                                                        MessageBoxButton buttns,
                                                                        MessageBoxImage icon
                                                                        ) {
            CS_Util Util = new CS_Util();
            return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
        }

        public static void MyLog(string TAG, string dbMsg) {
            dbMsg = "[" + MethodBase.GetCurrentMethod().Module.Name + "]" + dbMsg;
            //dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyLog(TAG, dbMsg);
        }

        public static void MyErrorLog(string TAG, string dbMsg, Exception err) {
            dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyErrorLog(TAG, dbMsg, err);
        }
        //////////////////////////////////////

    }
}
