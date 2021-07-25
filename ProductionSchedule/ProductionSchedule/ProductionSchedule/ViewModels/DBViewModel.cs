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
using ProductionSchedule.Views;
using ProductionSchedule.Models;
using System.Windows.Documents;

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

        public List<MyHierarchy> _MyHierarchyList { get; set; }
        public ObservableCollection<MyHierarchy> MyHierarchyList { get; set; }
        public List<string> ColList { get; set; }
        public ObservableCollection<Object> RowList { get; set; }
        public ObservableCollection<MyHierarchy> HierarchyTreeList { get; set; }


        public DBViewModel() {
            Initialize();
        }

        public void Initialize() {
            string TAG = "Initialize";
            string dbMsg = "";
            try {
                //_MyHierarchyList = new List<MyHierarchy>();
                //MyHierarchyList = new ObservableCollection<MyHierarchy>();
                NotifyPropertyChanged();
                ReadSheet();
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        // ファイルの操作////////////////////////
        /// <summary>
        /// シートが無ければ新規作成
        /// </summary>

        // スプレットシート/////////////////////ファイルの操作
        // https://www.ka-net.org/blog/?p=7007
        //static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
     //   static string ApplicationName = Constant.ApplicationName;           //"Google Sheets API .NET Quickstart";
        static string AppClientId = Constant.CliantId;                      //"(クライアント ID)";
        static string AppClientSecret = Constant.CliantSeacret;           //"(クライアント シークレット)";
       // static string SpreadSheetId = "(操作対象となるシートのID)";

        public void ReadSheet() {
            string TAG = "ReadSheet";
            string dbMsg = "";
            try {
                var service = new SheetsService(new BaseClientService.Initializer() {
                    HttpClientInitializer = Constant.MyDriveCredential,
                    ApplicationName = Constant.ApplicationName,
                });
                dbMsg += "[" + Constant.HierarchyFileID + "]";
                var range = $"シート1!A1:L100";
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(Constant.HierarchyFileID, range);
                var response = request.Execute();
                var values = response.Values;
                int rCount = 0;
                int cCount = 0;
                if (values != null && values.Count > 0) {
                    rCount = 0;
                    _MyHierarchyList = new List<MyHierarchy>();
                    MyHierarchyList = new ObservableCollection<MyHierarchy>();
                    ObservableCollection<int> parentIdList = new ObservableCollection<int>();
                    foreach (var row in values) {
                        rCount++;
                        if (1 < rCount) {
                            dbMsg += "\r\n[R" + rCount;
                            MyHierarchy mh = new MyHierarchy();
                            cCount = 0;
                            foreach (var col in row) {
                                cCount++;
                                if (col != null) {
                                    string rStr = col.ToString();
                                    dbMsg += ",C" + cCount + "]" ;
                                    //          ColList.Add(col.ToString());
                                    switch (cCount) {
                                        case 1:
                                            if (!String.IsNullOrEmpty(rStr)) {
                                                mh.id = int.Parse(rStr);
                                                dbMsg += "[" + mh.id + "]";
                                            }
                                            break;
                                        case 2:
                                            if (!String.IsNullOrEmpty(rStr)) {
                                                mh.parent_iD = int.Parse(rStr);
                                                dbMsg += ",親=" + mh.parent_iD;
                                            }
                                            break;
                                        case 3:
                                            if (!String.IsNullOrEmpty(rStr)) {
                                                mh.order = int.Parse(rStr);
                                                dbMsg += "(" + mh.order + ")";
                                            }
                                            break;
                                        case 4:
                                            if (!String.IsNullOrEmpty(rStr)) {
                                                mh.name = rStr;
                                                dbMsg += mh.name;
                                            }
                                            break;
                                        case 5:
                                            if (!String.IsNullOrEmpty(rStr)) {
                                                mh.hierarchy = int.Parse(rStr);
                                                dbMsg += "[" + mh.order + "]";
                                            }
                                            break;
                                    }
                                    if (!String.IsNullOrEmpty(mh.parent_iD.ToString())) {
                                        if (0 < parentIdList.Count) {
                                            bool isAdd = true;
                                            foreach (int pId in parentIdList) {
                                                if (pId == mh.parent_iD) {
                                                    isAdd = false;
                                                }
                                            }
                                            if (isAdd) {
                                                parentIdList.Add(mh.parent_iD);
                                            }
                                        } else {
                                            parentIdList.Add(mh.parent_iD);
                                        }
                                    }

                                }
                            }
                            MyHierarchyList.Add(mh);
                        }
                        //https://docs.google.com/spreadsheets/d/1M7eq9P9Gyi26vU9qVE5jak7tcLeSvZ-7NmlYwCFIPUU/edit#gid=0
                    }

                    dbMsg += "\r\n[R" + rCount + ",C" + cCount + "]";
                    NotifyPropertyChanged("MyHierarchyList");
                    dbMsg += ",parentIdList" + parentIdList.Count + "件";

                    ObservableCollection<MyHierarchy> parentList = new ObservableCollection<MyHierarchy>();
                    foreach (int pId in parentIdList) {
                        dbMsg += "[pID:" + pId + "]";
                        foreach (MyHierarchy pH in MyHierarchyList) {
                            if (pId== pH.id) {
                                parentList.Add(pH);
                                break;
                            }
                        }
                    }
                        dbMsg += ",parentList" + parentList.Count + "件";

                    HierarchyTreeList = new ObservableCollection<MyHierarchy>();
                    foreach (MyHierarchy pMH in parentList) {
                        dbMsg += "\r\n[pID:" + pMH.id + "]";
                   //     var dto1 = new Dto(pMH.name);
                        foreach (MyHierarchy mH in MyHierarchyList) {
                            if (mH.parent_iD == pMH.id) {
                                if (pMH.Child == null) {
                                    pMH.Child = new List<MyHierarchy>();
                                }
                                dbMsg += ":" + mH.id + "]" + mH.name;
                                pMH.Child.Add(mH);
                            }
                        }
                        HierarchyTreeList.Add(pMH);
                    }
                    dbMsg += ",HierarchyTreeList" + HierarchyTreeList.Count + "件";
                    NotifyPropertyChanged("HierarchyTreeList");
                } else {
                    dbMsg += "No data found.";
                }
                ////Sheet1のセルA1の値取得
                //SpreadsheetsResource.ValuesResource.GetRequest req1 = service.Spreadsheets.Values.Get(Constant.HierarchyFileID, "Sheet1!A1");
                //IList<IList<Object>> values = req1.Execute().Values;
                //dbMsg = "," + values[0][0];
         //       Console.ReadKey(true);
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
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
