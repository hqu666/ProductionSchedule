using ProductionSchedule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //TreeVrew用////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ドラッグ & ドロップの際にマウスカーソル上にあるアイテムの上か下に挿入するのか、あるいは子要素に追加するのかを判定するのに使う
        /// </summary>
        private enum InsertType {
            After,
            Before,
            Children
        }
        /// <summary>
        /// 背景色やセパレータの表示を変更したTreeViewItemInfoオブジェクトを記憶する
        /// </summary>
        private readonly HashSet<MyHierarchy> _changedBlocks = new HashSet<MyHierarchy>();
        private InsertType _insertType;
        /// <summary>
        /// 開始地点を記録
        /// </summary>
        private Point? _startPos;
        ////////////////////////////////////////////////////////////////////////////////TreeVrew用//

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
            VM.MyView = this;                   //渡らない？
            VM.MytreeView = MyTree;            //渡らない？
            VM.SelItemID = this.SelItemID;
            VM.SelItemName = this.SelItemName;
            VM.SelItemHierarchy = this.SelItemHierarchy;
            VM.SelParentID = this.SelParentID;
            VM.SelParentName = this.SelParentName;

            //// ウィンドウのサイズを復元
            //RecoverWindowBounds();
            //VM.MainWindowWidth = this.Width;
            //VM.MainWindowHight = this.Height;
            ////VM.CalenderTop = ContorolSP.Height + ContorolSP.Margin.Top+ContorolSP.Margin.Bottom;
            ////CalenderSV.Height = this.Height- (ContorolSP.Height + ContorolSP.Margin.Top + ContorolSP.Margin.Bottom);
            //VM.MakeCalenderBase();

            MyTree.AllowDrop = true;
            MyTree.PreviewMouseLeftButtonDown += MyTreeOnPreviewMouseLeftButtonDown;
            MyTree.PreviewMouseLeftButtonUp += MyTreeOnPreviewMouseLeftButtonUp;
            MyTree.PreviewMouseMove += MyTreeOnPreviewMouseMove;
            MyTree.Drop += MyTreeOnDrop;
            MyTree.DragOver += MyTreeOnDragOver;

        }

        //Drag&Drop////////////////////////////////////////////////////////////////////////////////
        public double itemSpan=0;
        public double bPosY=0;
        public int bDropId=0;

        /// <summary>
        /// Drag移動中
        /// ※ VM.DragOverTreeに移動済み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeOnDragOver(object sender, DragEventArgs e) {
            string TAG = "MyTreeOnDragOver";
            string dbMsg = "";
            try {
                VM.DragOverTree(sender,e, HierarchyGrid);

                //以降、移設済み
                // 背景色やセパレータを元に戻します
                ResetSeparator(_changedBlocks);
                //    if (!(sender is ItemsControl itemsControl) || !e.Data.GetDataPresent(typeof(MyHierarchy)))
                //    return;
                ItemsControl itemsControl = sender as ItemsControl;
                if (itemsControl ==null){
                    dbMsg += "itemsControl ==null";
                } else {
                    if (!e.Data.GetDataPresent(typeof(MyHierarchy))) {
                        dbMsg += "MyHierarchyではない";
                    } else {
                        // 画面上部/下部にドラッグした際にスクロールします
                        DragScroll(itemsControl, e);
                        // ドラッグ中のアイテムとマウスカーソルの位置にある要素を取得します
                        // HitTestで取れる要素は大体TextBlockなので、次でTreeViewItemInfoを取得します
                        MyHierarchy sourceItem = (MyHierarchy)e.Data.GetData(typeof(MyHierarchy));
                        dbMsg += "ドラッグ中のアイテム[" + sourceItem.id + "]" + sourceItem.name;
                        FrameworkElement targetElement = HitTest<FrameworkElement>(itemsControl, e.GetPosition);

                        // カーソル要素から直近のGridを取得します(後の範囲計算で必要)
                        // カーソル要素からTreeViewItemInfoを取得するにはDataContextを変換します
                        // カーソル要素とドラッグ要素が同じ場合は何もする必要がないのでreturnしておきます
                        var parentGrid = HierarchyGrid; // targetElement?.GetParent<Grid>();
                        MyHierarchy targetElementInfo = targetElement.DataContext as MyHierarchy;
                        if (parentGrid == null) {
                            // || targetElementInfo == sourceItem || !(targetElement.DataContext is MyHierarchy targetElementInfo)
                            dbMsg += "parentGrid == null";
                        } else {
                            if (targetElementInfo == sourceItem) {
                                dbMsg += ",Info == sourceItem";
                            } else {
                                if (targetElementInfo == null) {
                                    dbMsg += ",Info ==null";
                                } else {
                                    // カーソル要素がドラッグ中の要素の子要素にある時は何もする必要がないのでreturnします
                                    // 独自の処理をするならこれは不要、今回のコードではこれがないと要素が消えます
                                    if (targetElementInfo.ContainsParent(sourceItem)) {
                                        dbMsg += "targetElementInfo.ContainsParent(sourceItem)";
                                    } else {
                                        //TreeView sendTree = sender as TreeView;
                                        //TreeViewItem dragItem = sendTree.SelectedItem as TreeViewItem;

                                        dbMsg += ",Info[" + targetElementInfo.id + "]" + targetElementInfo.name;
                                        e.Effects = DragDropEffects.Move;
                                        // 挿入するか子要素に追加するかの判定処理
                                        // 基本的には0 ~ boundaryの位置なら上部に挿入、それ以外なら子要素に追加します
                                        // それだけでは末尾に追加できなくなるので子要素の最後だけ末尾に追加できるようにします
                                        const int boundary = 10;
                                        Point pos = e.GetPosition(parentGrid);
                                        dbMsg += ",グリッド上(" + pos.X + "," + pos.Y + ")ActualHeight=" + parentGrid.ActualHeight;
                                        MyHierarchy targetParentLast = GetParentLastChild(targetElementInfo);
                                        if (targetParentLast != null) {
                                            dbMsg += ",targetParentLast[" + targetParentLast.id + "]" + targetParentLast.name;
                                        }
                                        if (bDropId != targetElementInfo.id) {
                                            dbMsg += ">>Drop先変化";
                                            bDropId = targetElementInfo.id;
                                            if ( bPosY != 0) {
                                                itemSpan = bPosY - pos.Y;
                                            }
                                            dbMsg += ",itemSpan=" + itemSpan;
                                            bPosY = pos.Y;
                                            dbMsg += ",bPosY=" + bPosY;
                                        }
                                        ///経過；Childrenが出ていない
                                        if (Math.Abs(bPosY - pos.Y) < 10) {            //間隔は仮設定
                                            if (0 < (bPosY - pos.Y)) {
                                                targetElementInfo.BeforeSeparatorVisibility = Visibility.Visible;
                                                _insertType = InsertType.Before;
                                            } else {
                                                targetElementInfo.AfterSeparatorVisibility = Visibility.Visible;
                                                _insertType = InsertType.After;
                                            }
                                        } else {
                                            targetElementInfo.AfterSeparatorVisibility = Visibility.Hidden;
                                            targetElementInfo.BeforeSeparatorVisibility = Visibility.Hidden;
                                            _insertType = InsertType.Children;
                                            targetElementInfo.Background = Brushes.Gray;
                                        }

                                        dbMsg += ",_insertType=" + _insertType;
                                        dbMsg += ",BeforeSeparator=" + targetElementInfo.BeforeSeparatorVisibility;
                                        dbMsg += ",AfterSeparator=" + targetElementInfo.AfterSeparatorVisibility;
                                        // 背景色などを変更したTreeViewItemInfoオブジェクトを_changedBlocksに追加しておきます
                                        if (!_changedBlocks.Contains(targetElementInfo))
                                            _changedBlocks.Add(targetElementInfo);

                                    }
                                }
                            }
                        }
                    }
                }
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// Dropした時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeOnDrop(object sender, DragEventArgs e) {
            string TAG = "MyTreeOnDrop";
            string dbMsg = "";
            try {
                VM.DropInTree(sender, e);




                TreeView TV = sender as TreeView;

                // 背景色やセパレータを元に戻します
                ResetSeparator(_changedBlocks);

                if (!(sender is ItemsControl itemsControl))
                    return;

                // ドラッグ中の要素(source)とマウス位置の要素(target)を取得します
                MyHierarchy sourceItem = (MyHierarchy)e.Data.GetData(typeof(MyHierarchy));
                dbMsg += "[" + sourceItem.id + "]" + sourceItem.name + "をDrop";
                MyHierarchy targetItem = HitTest<FrameworkElement>(itemsControl, e.GetPosition)?.DataContext as MyHierarchy;
                dbMsg += ",(targetItem[" + targetItem.id + "]" + targetItem.name + "に)";
                // それぞれの要素がnullならreturn
                // もしくはsourceとtargetが同一の場合もreturn
                if (targetItem == null || sourceItem == null || sourceItem == targetItem)
                    return;

                // カーソル要素がドラッグ中の要素の子要素にある時は何もする必要がないのでreturnします
                if (targetItem.ContainsParent(sourceItem))
                    return;


                // それぞれの要素の親要素を取得しておきます
                // Childrenの場合はtargetの子要素に追加します
                MyHierarchy targetItemParent = targetItem.parent;
                dbMsg += ",Drop先の親";
                if (targetItemParent != null) {
                    dbMsg += "[" + targetItemParent.id + "]" + targetItemParent.name + "に";
                } else {
                    dbMsg += "無し";
                }
                MyHierarchy sourceItemParent = sourceItem.parent;
                dbMsg += ",Dragされたアイテムの親";
                if (sourceItemParent ==null) {
                    dbMsg += "無し";
                } else {
                    dbMsg += ",Dragされたアイテムの親[" + sourceItemParent.id + "]" + sourceItemParent.name + "に";
                }
                // 次にsourceを現在の位置から削除しておきます
                RemoveCurrentItem(sourceItemParent, sourceItem);
                // あとはBefore, Afterの場合はtargetの前後にsourceを挿入
                dbMsg += ",_insertType=" + _insertType;
                int dropPosition = 0;               //末尾に追加
                switch (_insertType) {
                    case InsertType.Before:
                        dropPosition = -1;               //前に追加
                        break;
                    case InsertType.After:
                        dropPosition = 1;               //後ろに追加
                        break;
                    default:
                        break;
                }
                VM.Drop2Tree(targetItem, sourceItem, dropPosition, (ObservableCollection<MyHierarchy>)TV.ItemsSource, TV);
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        private void MyTreeOnPreviewMouseMove(object sender, MouseEventArgs e) {
            string TAG = "MyTreeOnPreviewMouseMove";
            string dbMsg = "";
            try {
                dbMsg += ",_startPos=" + _startPos;
                if (!(sender is TreeView treeView) || treeView.SelectedItem == null || _startPos == null)
                    return;

                dbMsg += ",SelectedItem=" + MyTree.SelectedItem.ToString();
                Point cursorPoint = treeView.PointToScreen(e.GetPosition(treeView));
                dbMsg += ",cursorPoint(" + cursorPoint.X+ "," + cursorPoint.Y + ")";
                Vector diff = cursorPoint - (Point)_startPos;
                dbMsg += ",移動距離=" + diff;
                if (!CanDrag(diff))
                    return;

                DragDrop.DoDragDrop(treeView, treeView.SelectedItem, DragDropEffects.Move);

                _startPos = null;
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// マウス左ボタンが離された時
        /// 開始点を破棄する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            string TAG = "MyTreeOnPreviewMouseLeftButtonUp";
            string dbMsg = "";
            try {
                dbMsg += ",_startPos=" + _startPos;
                _startPos = null;
                dbMsg += ">>" + _startPos;
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// マウス左ボタンがクリックされた時
        /// Tree内のアイテムなら開始点を取得し、異なれば破棄する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTreeOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            string TAG = "MyTreeOnPreviewMouseLeftButtonDown";
            string dbMsg = "";
            try {
                if (!(sender is ItemsControl itemsControl)) {
                    return;
                }
                //TreeView TV = sender as TreeView;
                //VM.ResetSelect((ObservableCollection<MyHierarchy>)TV.ItemsSource);

                Point pos = e.GetPosition(itemsControl);
                dbMsg += ",pos=" + pos;
                FrameworkElement hit = HitTest<FrameworkElement>(itemsControl, e.GetPosition);
                if (hit.DataContext is MyHierarchy) { 
                    _startPos = itemsControl.PointToScreen(pos);
                    dbMsg += ",_startPos(" + _startPos.Value.X + "," + _startPos.Value.Y + ")";
                } else {
                    _startPos = null;
                    dbMsg += ",_startPos破棄";
                }
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        private void MyTree_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            string TAG = "MyTree_MouseDoubleClick";
            string dbMsg = "";
            try {
                VM.TreeLClick(sender, e);
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        /// <summary>
        /// 親要素から子要素郡の末尾を取得します
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static MyHierarchy GetParentLastChild(MyHierarchy info) {
            string TAG = "MyTreeOnPreviewMouseMove";
            string dbMsg = "";
            MyHierarchy last=new MyHierarchy();
            try {
                dbMsg += ",info[" + info.id + "]" + info.name;
                MyHierarchy targetParent = info.parent;
                if (targetParent == null) {
                    dbMsg += "targetParent=null";
                    last = null;
                } else {
                    dbMsg += "targetParent[" + targetParent.id + "]" + targetParent.name;
                    if (targetParent.Child != null) {
                        foreach (MyHierarchy child in targetParent.Child) {
                            last = child;
                        }
                    }
                }
           //     last = targetParent?.TreeChildren.LastOrDefault();
 //               MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
            return last;
        }

        /// <summary>
        /// 親要素から指定した要素を削除します
        /// </summary>
        /// <param name="sourceItemParent"></param>
        /// <param name="sourceItem"></param>
        private static void RemoveCurrentItem(MyHierarchy sourceItemParent, MyHierarchy sourceItem) {
            string TAG = "RemoveCurrentItem";
            string dbMsg = "";
            try {
                if (sourceItemParent == null) {
                    ///経過；Root＝親が無ければ消去されない
                } else {
                    sourceItemParent.RemoveChildren(sourceItem);
                }
                MyLog(TAG, dbMsg);
            } catch (Exception er) {
                MyErrorLog(TAG, dbMsg, er);
            }
        }

        //--- 変更されたセパレータ、背景色を元に戻します
        private static void ResetSeparator(ICollection<MyHierarchy> collection) {
            var list = collection.ToList();
            foreach (var pair in list) {
                ResetSeparator(pair);
                collection.Remove(pair);
            }
        }
        //--- 背景色を元に戻します
        private static void ResetSeparator(MyHierarchy info) {
            info.Background = Brushes.Transparent;
            info.BeforeSeparatorVisibility = Visibility.Hidden;
            info.AfterSeparatorVisibility = Visibility.Hidden;
        }

        //--- 上部・下部にドラッグした際にスクロールします
        private static void DragScroll(FrameworkElement itemsControl, DragEventArgs e) {
            //var scrollViewer = itemsControl.Descendants<ScrollViewer>().FirstOrDefault();
            //const double tolerance = 10d;
            //const double offset = 3d;
            //var verticalPos = e.GetPosition(itemsControl).Y;
            //if (verticalPos < tolerance)
            //    scrollViewer?.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            //else if (verticalPos > itemsControl.ActualHeight - tolerance)
            //    scrollViewer?.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
        }

        //--- カーソルポジションとUIElementからカーソル上の要素を取得します
        private static T HitTest<T>(UIElement itemsControl, Func<IInputElement, Point> getPosition) where T : class {
            Point pt = getPosition(itemsControl);
            DependencyObject result = itemsControl.InputHitTest(pt) as DependencyObject;
            if (result is T ret)
                return ret;
            return null;
        }

        //--- ドラッグ可能かどうかを判定します
        private static bool CanDrag(Vector delta) {
            return (SystemParameters.MinimumHorizontalDragDistance < Math.Abs(delta.X)) ||
                    (SystemParameters.MinimumVerticalDragDistance < Math.Abs(delta.Y));
        }


        ////////////////////////////////////////////////////////////////////////////////Drag&Drop//
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
