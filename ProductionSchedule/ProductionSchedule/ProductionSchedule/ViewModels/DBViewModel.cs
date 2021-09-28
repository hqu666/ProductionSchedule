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
        public TreeView MytreeView { get; set; }
        public MyHierarchy SelectedTreeItem { get; set; }

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

        public List<MyHierarchy> MHCopyList;

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
                                        // 親が有ればIDリストに追加
                                        if (0 < parentIdList.Count) {
                                            //既に登録されていなければ登録
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
                                            //最初の1件目は
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

                    // IDのリストからMyHierarchyのリストに
                    MHCopyList = new List<MyHierarchy>();
                    foreach (MyHierarchy pH in MyHierarchyList) {
                        foreach (int pId in parentIdList) {
                            dbMsg += "[pID:" + pId + "]";
                            if (pId == pH.id) {
                                //親が無い　：　ルートだけを登録する
                                // Model内で作成できないのでここで作成
                                pH.Child = new List<MyHierarchy>();
                                break;
                            }
                        }
                        MHCopyList.Add(pH);
                    }

                    //　親リストに子を登録
                    HierarchyTreeList = new ObservableCollection<MyHierarchy>();
                    foreach (MyHierarchy pMH in MHCopyList) {
             //           dbMsg += "\r\n[pID:" + pMH.id + "]";
                        //子の入れ場が有る物に子を追加
                        if (pMH.Child != null) {  
                            //     親ごとの子リスト作成
                            foreach (MyHierarchy mH in MyHierarchyList) {
                                if (mH.parent_iD == pMH.id) {
                                    dbMsg += ":" + mH.id + "]" + mH.name;
                                    pMH.Child.Add(mH);
                                }
                            }
                            if (pMH.parent_iD == 0) {
                                //ルートはそのまま追記
                                HierarchyTreeList.Add(pMH);
                            } else {
                                //親が有れば検索して追記
                                //※前提：親より先に子は作られない
                                foreach (MyHierarchy addedH in HierarchyTreeList) {
                                    foreach (MyHierarchy child in addedH.Child) {
                                        if (child.id == pMH.parent_iD) {
                                            child.Child = pMH.Child;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dbMsg += ",HierarchyTreeList" + HierarchyTreeList.Count + "件";
                    //課題；parentを設定する
                    foreach (MyHierarchy tmh in MyHierarchyList) {
                        if (0 < tmh.parent_iD) {
                            foreach (MyHierarchy pMh in MyHierarchyList) {
                                if (pMh.id == tmh.parent_iD) {
                                    dbMsg += "\r\n[pID:" + tmh.id + "]"+ tmh.name+"の親は[" + pMh.id + "]" + pMh.name+ "の" + tmh.order +"番目";
                                    tmh.parent = pMh;
                                    break;
                                }
                            }
                        }
                    }
                    NotifyPropertyChanged("HierarchyTreeList");
            //nullになる        MytreeView.AddHandler(TreeViewItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TreeDoubleClick), true);
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


        ////TreeView//////////////////////////////////////////////////////////////////////////////////////////////
        public MyHierarchy FindTreeItem(int id, List<MyHierarchy> tItems) {
            string TAG = "FindTreeItem";
            string dbMsg = "";
            MyHierarchy retItem = new MyHierarchy();
            try {
                dbMsg += "検索対象[" + id + "]";
                foreach (MyHierarchy item in tItems) {
                    if ( item.id == id) {
                        retItem = item;
                        dbMsg += retItem.name + "{親:" + retItem.parent_iD + "}の" + SelectedTreeItem.order + "番目";
                        break;
                    }
                }

                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
            return retItem;
        }

        public MyHierarchy FindTreeItem(int id, ObservableCollection<MyHierarchy> tItems) {
            string TAG = "FindTreeItem:ObservableCollection";
            string dbMsg = "";
            MyHierarchy retItem = new MyHierarchy();
            try {
                dbMsg += "検索対象[" + id + "]";
                foreach (MyHierarchy item in tItems) {
                    if (item.id == id) {
                        retItem = item;
                        dbMsg += retItem.name + "{親:" + retItem.parent_iD + "}の" + SelectedTreeItem.order + "番目";
                        break;
                    }
                }

                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
            return retItem;
        }




        /// <summary>
        /// アイテムクリック
        /// </summary>
        public void TreeLClick(object sender, MouseButtonEventArgs e) {
            string TAG = "TreeLClick";
            string dbMsg = "";
            try {
                foreach (MyHierarchy item in MHCopyList) {
                    if (item.IsSelected) {
                        SelectedTreeItem = item;
                        dbMsg += "selected[" + SelectedTreeItem.id + "]" + SelectedTreeItem.name + "{" + SelectedTreeItem.parent_iD + "}の" + SelectedTreeItem.order;
                        int nowHierarchy = 1;
                        if (0< SelectedTreeItem.parent_iD) {
                            nowHierarchy++;
                            MyHierarchy parent = FindTreeItem(SelectedTreeItem.parent_iD, MHCopyList);
                            if (parent != null) {
                                SelParentName = parent.name;
                                dbMsg += ".SelParentName=" + SelParentName;
                                NotifyPropertyChanged("SelParentName");
                            }
                            while (0< parent.parent_iD) {
                                dbMsg += ".SelParentの親[" + parent.parent_iD + "]";
                                nowHierarchy++;
                                parent = FindTreeItem(parent.parent_iD, MHCopyList);
                                dbMsg += ".その親[" + parent.parent_iD + "]";
                                //if (parent.parent_iD == 0) {
                                //    break;
                                ////} else {
                                ////    nowHierarchy++;
                                //}
                            }
                        }
                        SelectedTreeItem.hierarchy= nowHierarchy;
                        dbMsg += ".hierarchy=" + SelectedTreeItem.hierarchy;
                        NotifyPropertyChanged("SelectedTreeItem");
                        break;
                    }
                }

                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// Dropされたアイテムの移動
        /// </summary>
        /// <param name="dropTo">追加先</param>
        /// <param name="souceItem">Dropされたアイテム</param>
        /// <param name="souceOrder">追加先での順番</param>
        /// <param name="treeItemsSource">TreeVeiwのItemsSource</param>
        public void Drop2Tree(MyHierarchy dropTo , MyHierarchy souceItem,int souceOrder, ObservableCollection<MyHierarchy> treeItemsSource , TreeView tv) {
            string TAG = "Drop2Tree";
            string dbMsg = "";
            try {
                dbMsg += "[" + dropTo.id + "]" + dropTo.name +"　に["+ souceItem.id + "]"+ souceItem.name +"をDrop";
                MyHierarchy parentMH = souceItem.parent;
                dbMsg += ",元の親";
                if (parentMH == null) {
                    dbMsg += "無し";
                } else {
                    dbMsg += "親[" + parentMH.id + "]" + parentMH.name + ":" + parentMH.Child.Count + "件";
                    parentMH.Child.Remove(souceItem);
                }
                dbMsg += "から削除して" + parentMH.Child.Count + "件";
                souceItem.parent = dropTo;
                dbMsg += ">>[" + souceItem.parent.id + "]" + souceItem.parent.name;
                dbMsg += ",追加先Child";
                if (dropTo.Child == null) {
                    dbMsg += "無し";
                    dropTo.Child = new List<MyHierarchy>();
                } else { 
                    dbMsg += dropTo.Child.Count + "件";
                }
                if (souceOrder==0) {
                    dbMsg += ">末尾へ>";
                    dropTo.Child.Add(souceItem);
                    HierarchyTreeList = new ObservableCollection<MyHierarchy>(treeItemsSource);
                } else {
                    HierarchyTreeList = new ObservableCollection<MyHierarchy>();
                    int dropToPosiotion = 0;
                    foreach (MyHierarchy ti in treeItemsSource) {
                        dbMsg += "\r\n[" + ti .id+ "]"+ ti.name;
                        if (ti.Equals(dropTo.parent)) {
                            List<MyHierarchy> childList=new List<MyHierarchy>();
                            foreach (MyHierarchy dtc in ti.Child) {
                                if (dtc.Equals(dropTo)) {
                                    dbMsg += "の[" + dtc.id + "]" + dtc.name;
                                    if (0 < souceOrder) {
                                        dbMsg += "の後ろ";
                                        childList.Add(dtc);
                                        childList.Add(souceItem);
                                        souceItem.id = dtc.id + 1;
                                    } else {
                                        dbMsg += "の前";
                                        childList.Add(souceItem);
                                        childList.Add(dtc);
                                        souceItem.id = dtc.id ;
                                        dtc.id ++;
                                    }
                                } else {
                                    childList.Add(dtc);
                                }
                                dropToPosiotion++;
                            }
                            souceItem.parent = ti;
                            ti.Child = childList;
                            HierarchyTreeList.Add(ti);
                        } else if (ti.Equals(dropTo)) {
                            if (0 < souceOrder) {
                                HierarchyTreeList.Add(ti);
                                HierarchyTreeList.Add(souceItem);
                            } else {
                                HierarchyTreeList.Add(souceItem);
                                HierarchyTreeList.Add(ti);
                            }
                        } else {
                            HierarchyTreeList.Add(ti);
                        }
                        dropToPosiotion++;
                    }
                }
                dbMsg += ">>" + dropTo.Child.Count + "件";
                souceItem.order = dropTo.Child.Count;
                dbMsg += "の" + souceItem.order + "番目に移動";
                NotifyPropertyChanged("HierarchyTreeList");
                ExpandSerlect(souceItem.id, HierarchyTreeList, tv);
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// 移動後など移動した状態を見せる
        /// </summary>
        /// <param name="id">見せる対象</param>
        /// <param name="tItems">ItemSouce</param>
        /// <returns></returns>
        public void ExpandSerlect(int id, ObservableCollection<MyHierarchy> tItems, TreeView tv) {
            string TAG = "ExpandSerlect";
            string dbMsg = "";
            MyHierarchy tItem = FindTreeItem( id,tItems);
            try {
                dbMsg += "検索対象[" + id + "]";    // + tItem.name + ";親[" + tItem.parent_iD + "]" ;

                foreach (MyHierarchy item in tItems) {
                    //  if (item.IsSelected) {
                    if (id == item.id) {
                        dbMsg += ",選択" + item.IsSelected;
                     //   item.IsSelected = true;
                        dbMsg += "[parent:" + item.parent_iD + "]";
                            int pCount = 0;
                            foreach (MyHierarchy pItem in tv.ItemsSource) {
                                if (pItem.id == item.parent_iD) {
                                    dbMsg += ",ノード" + pCount + "を開く";
                                    pItem.IsExpanded = true;
                                    //         tv.Nodes[pCount].Expand();
                                } else {
                                    pItem.IsExpanded = false;
                                }
                                pCount++;
                            }

                        } else if (item.IsSelected) {
                            dbMsg += "選択されていたのは[" + item.id + "]" + item.name;
                            item.IsSelected = false;
                        }
                  //  }
                }

                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// 選択フラグをすべて解除
        /// </summary>
        public void ResetSelect(ObservableCollection<MyHierarchy>itemsSource) {
            string TAG = "ResetSelect";
            string dbMsg = "";
            try {
                foreach (MyHierarchy pair in itemsSource) {
                    if (pair.IsSelected) {
                        dbMsg = "選択されていたのは[" + pair.id + "]" + pair.name;
                        pair.IsSelected = false;
                    }
                }
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
