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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductionSchedule.Views {
    /// <summary>
    /// DBWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DBWindow : Window {
        ViewModels.DBViewModel VM;

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


        public DBWindow() {
            InitializeComponent();
            VM = new ViewModels.DBViewModel();
            this.DataContext = VM;
            this.Loaded += this_loaded;
        }

        /// <summary>
        /// 読込み終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_loaded(object sender, RoutedEventArgs e) {
            //ViewとViewModelの紐付け
            VM.MyView = this;
            VM.SelItemID = this.SelItemID;
            VM.SelItemName = this.SelItemName;
            VM.SelItemHierarchy = this.SelItemHierarchy;
            VM.SelParentID = this.SelParentID;
            VM.SelParentName = this.SelParentName;

            //VM.CalenderGR = this.CalenderGR;
            ////     VM.CalenderDG = this.CalenderDG;
            //// ウィンドウのサイズを復元
            //RecoverWindowBounds();
            //VM.MainWindowWidth = this.Width;
            //VM.MainWindowHight = this.Height;
            ////VM.CalenderTop = ContorolSP.Height + ContorolSP.Margin.Top+ContorolSP.Margin.Bottom;
            ////CalenderSV.Height = this.Height- (ContorolSP.Height + ContorolSP.Margin.Top + ContorolSP.Margin.Bottom);
            //VM.MakeCalenderBase();
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
            dbMsg = "[DBWindow]" + dbMsg;
            //dbMsg = "[" + MethodBase.GetCurrentMethod().Name + "]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyLog(TAG, dbMsg);
        }

        public static void MyErrorLog(string TAG, string dbMsg, Exception err) {
            dbMsg = "[DBWindow]" + dbMsg;
            CS_Util Util = new CS_Util();
            Util.MyErrorLog(TAG, dbMsg, err);
        }
        //////////////////////////////////////


    }
}
