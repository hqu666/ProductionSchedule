using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Collections.ObjectModel;
using Livet;
using Livet.Commands;
using Livet.Messaging;

using ProductionSchedule.Views;
using ProductionSchedule.Models;
//using ProductionSchedule.Enums;
using System.ComponentModel;
using System.Reflection;
//using Infragistics.Windows.Controls;

namespace ProductionSchedule.ViewModels
{
    public class MainViewModel : ViewModel, INotifyPropertyChanged {

		public Views.MainWindow MyView { get; set; }
		public List<Models.MyMenu> _MyMenu { get; set; }

		//	public TabControl MainTab { get; set; }

		public System.Collections.IEnumerable TabItems { get; set; }

		public string InfoLavel { get; set; }
		public string ReTitle="";
		//	public TreeView MenuTree { get; set; }
		/// <summary>TreeViewItemが選択されているかを取得・設定します。</summary>
		//	public ReactivePropertySlim<bool> IsSelected { get; set; }
		//public bool NeedHideOwner { get; set; }

		//		int AddCount;

		public MainViewModel()
		{
			//			AddCount = 0;
			//			MainTab = FindName("MainTab") as TabControl;
			////閉じるコマンドを生成
			//var Closing = new Livet.Commands.ListenerCommand<Window>((w) => {
			//	if (NeedHideOwner && w.Owner != null) {
			//		w.Owner.Show();
			//	}
			//});

			Initialize();
		}
	//	private int AddCount;

		//	Authorizer authorizer = new Authorizer();
		public void Initialize()
		{
		//	if (!authorizer.IsAuthorized) {
				var message = new InteractionMessage();
				Messenger.Raise(message);
			//	}

			//_MyMenu = new List<MyMenu>()
			//   {
			//	new MyMenu()
			//	{
			//		Name = "MVVMテスト",
			//		Child = new List<MyMenu>()
			//		{
			//			new MyMenu() { Name = "MySQL",Value="MySQL" },
			//			new MyMenu() { Name = "Googleドライブ",Value="W-1" },
			//			new MyMenu()
			//			{
			//				Name = "スケジュール",
			//				Child = new List<MyMenu>()
			//				{
			//					new MyMenu() { Name = "日別表示",Value="X-1-1" },
			//					new MyMenu() { Name = "週別表示",Value="X-1-2" },
			//					new MyMenu() { Name = "月別表示",Value="X-1-3" },
			//					new MyMenu() { Name = "スケジュール",Value="X-1-4" },
			//				}
			//			},
			//			new MyMenu()
			//			{
			//				Name = "システム管理",
			//				Child = new List<MyMenu>()
			//				{
			//					new MyMenu() { Name = "Googleアカウント認証",Value="Z-1-4" },
			//				}
			//			},
			//		}
			//	},
			//	new MyMenu() { Name = "パーツテスト",Value="ParrtsTest" },
			//	new MyMenu()
			//	{
			//		Name = "簡易テスト",
			//		Child = new List<Models.MyMenu>()
			//		{
			//			new MyMenu() {
			//				Name = "TabConから操作",
			//				Child = new List<Models.MyMenu>()
			//				{
			//					new MyMenu() { Name = "ViewをTabConに読み込む",Value="TabCon_AddTab" },
			//					new MyMenu() { Name = "Viewを読み込んだTabを削除",Value="TabCon_DrelTabItem" },
			//					new MyMenu() { Name = "ViewをNavigationWindowで開く",Value="TabCon_ShowNaniWindow" },
			//					new MyMenu() { Name = "ViewをWindowで開く",Value="TabCon_OpenWindow" },
			//					new MyMenu() { Name = "Windowクラスをダイアログ表示",Value="TabCon_OpenDialog" },
			//				}
			//		},
			//			new MyMenu()
			//			{
			//				Name = "このViewから操作",
			//				Child = new List<Models.MyMenu>()
			//				{
			//					new MyMenu() { Name = "ViewをWindowで開く",Value="GotoCommand2" },
			//					new MyMenu() { Name = "Windowクラスをダイアログ表示",Value="OpenDialogCommand" },
			//					new MyMenu() { Name = "閉じる",Value="CloseCommand" },
			//				}
			//			}



			//		}
			//	}

			//};

		}
/*
		/// <summary>
		/// Treeの選択動作
		/// </summary>
		public void MenuSelected()
		{
			string dbMsg = "";
			string selectedValue =  MenuselectLoop(_MyMenu);
			if (selectedValue == "CloseCommand") {
				Close();
			}else if(selectedValue == "GotoCommand2") {
				Goto2();
			} else if (selectedValue == "OpenDialogCommand") {
				OpenDialog();
			} else if (selectedValue == "TabCon_OpenDialog") {
				MyView.ViewTab.OpenDialog();
				MyView.Info_lv.Content = "ViewTabControl : UserControlからWindowクラスのViewをダイアログを表示させています。\nダイアログを閉じるまで他の操作はできません";
			} else if (selectedValue == "TabCon_OpenWindow") {
				MyView.ViewTab.OpenWindow();
				MyView.Info_lv.Content = "ViewTabControl : UserControlからWindowクラスのViewをWindowを表示させています。";
			} else if (selectedValue == "TabCon_ShowNaniWindow") {
				MyView.ViewTab.ShowNaniWindow();
				MyView.Info_lv.Content = "ViewTabControl : UserControlからPageクラスのViewをNavigationWindowで表示させています。";
			} else if (selectedValue == "TabCon_DrelTabItem") {
				MyView.ViewTab.DrelTabItem();
				MyView.Info_lv.Content = "ViewTabControl : UserControlからViewを読み込んだTabを削除。";
			} else if (selectedValue == "TabCon_AddTab") {
				MyView.ViewTab.AddTab();
				MyView.Info_lv.Content = "ViewTabControl : UserControlからPageクラスのViewをTabに読込みます";

			} else if (selectedValue == "ParrtsTest") {
				dbMsg = "カスタムコントロールのテストです\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.ParrtsTestView rContent = new Views.ParrtsTestView();
				//読込んだページを操作
				rContent.VM.RootViewModel = this;
				Add2Tab(rContent);
			} else if (selectedValue == "W-1") {
				if (Constant.MyDriveService == null) {
					String msgStr = "システム設定のGoogleアカウント認証で認証を行ってください。";
					String titolStr = "Googleアカウント";
					MessageShowWPF(titolStr, msgStr, MessageBoxButton.OK, MessageBoxImage.Error);
				}else{
					dbMsg = "GoogleDriveを表示します\r\n";
					MyView.Info_lv.Content = dbMsg;
					Views.W_1 rContent = new Views.W_1();
					Add2Tab(rContent);
				}
			} else if (selectedValue == "X-1-1") {
				dbMsg = "MySQLデータベースのスケジュールテーブルからカレンダに日別スケジュールを書き込みます\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.X_1_1 rContent = new Views.X_1_1();
				rContent.VM.RootViewModel = this;
				rContent.VM.weekDisplayMode = "None";
				Add2Tab(rContent);
			} else if (selectedValue == "X-1-2") {
				dbMsg = "MySQLデータベースのスケジュールテーブルからカレンダに週別スケジュールを書き込みます\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.X_1_1 rContent = new Views.X_1_1();
				rContent.VM.RootViewModel = this;
				rContent.VM.weekDisplayMode = "Week";
				ReTitle = "週別表示";
				Add2Tab(rContent);
			} else if (selectedValue == "X-1-3") {
				dbMsg = "MySQLデータベースのスケジュールテーブルからカレンダに月別スケジュールを書き込みます\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.X_1_3 rContent = new Views.X_1_3();
				//読込んだページを操作
				rContent.VM.RootViewModel = this;
				Add2Tab(rContent);
			} else if (selectedValue == "X-1-4") {
				dbMsg = "MySQLデータベースのスケジュールテーブルからスケジュールをリストアップします\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.X_1_4 rContent = new Views.X_1_4();
				//rContent.VM.RootViewModel = this;
				//rContent.VM.weekDisplayMode = "Week";
				//ReTitle = "週別表示";
				Add2Tab(rContent);
			} else if (selectedValue == "Z-1-4") {
				dbMsg = "Googleサービスを利用します\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.Z_1_4 rContent = new Views.Z_1_4();
				Add2Tab(rContent);
			} else if (selectedValue == "MySQL") {
				dbMsg = "MySQLデータベースに接続し、コンボボックスで選択したテーブルを操作します\r\n";
				MyView.Info_lv.Content = dbMsg;
				Views.MySQLBase rContent = new Views.MySQLBase();
				//読込んだページを操作
				rContent.VM.RootViewModel = this;
				//rContent.CInfo_lb.Content = dbMsg + (MyView.ViewTab.MainTab.Items.Count + 1) + "番目に追加したTabItemです";
				Add2Tab(rContent);
			}
		}
		public string MenuselectLoop(List<Models.MyMenu> tMenu)
		{
			string selectedValue = "";
			foreach (Models.MyMenu sele in tMenu) {
				string rName = sele.Name;
				bool rIsSelected = sele.IsSelected;
				 if ( sele.Child != null) {
					selectedValue = MenuselectLoop(sele.Child);
					if(selectedValue != "") {
						break;
					}
				} else if (rIsSelected) {
					selectedValue = sele.Value;
					break;
				}
			}
			return selectedValue;
		}

		/// <summary>
		/// View名で指定した画面をタブに読込む
		/// </summary>
		/// <param name="ViewName"></param>
		public void Add2Tab(Page rContent)
		{
			//MyView.ViewTab.MainTab.Height = rContent.MaxHeight;
			//MyView.ViewTab.MainTab.Width = rContent.MaxWidth;
			//MyView.Info_lv.Content += "サイズは" + MyView.ViewTab.MainTab.Width + "×" + MyView.ViewTab.MainTab.Height + "]です";

			TabItemEx tab = new TabItemEx();
			//タブの表示名はコントロールのTitleから取得・別名の指定が有れば書き換え
			tab.Header = rContent.Title;                        ///"Tab" + (MyView.ViewTab.MainTab.Items.Count + 1);
			if(!ReTitle.Equals("")) {
				tab.Header = ReTitle;
				ReTitle = "";
			}
			////フレームを生成して設置したタブコントロールのContentにする場合
			////var frame = new Frame();
			//②Viewの読込ができるTabContentを生成する
			WindowTabContentUC tabContent = new WindowTabContentUC();
	//		dbMsg += "PageクラスのXAMLを\r\n";
			////frame.Navigate(rContent);
			tabContent.TabContent.Navigate(rContent);
			tab.Content = tabContent;
			//追加した物を選択状態にしてタブコントロールに追加
			tab.IsSelected = true;
			MyView.ViewTab.MainTab.Items.Add(tab);
			
		}
        */

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

		//public ViewModelCommand GotoCommand2 {
		//	get { return new Livet.Commands.ViewModelCommand(Goto2); }
		//}

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

        private ViewModelCommand _ConnectCommand;

        public ViewModelCommand ConnectCommand {
            get {
                if (_ConnectCommand == null)
                {
                    _ConnectCommand = new ViewModelCommand(Connect);
                }
                return _ConnectCommand;
            }
        }

        /// <summary>
        /// 接続前の状況確認
        /// </summary>
        public void Connect()
        {
            string TAG = "Connect";
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

        /// <summary>
        /// このウィンドウを閉じる
        /// </summary>
        public void Close()
		{
//			MyView.Info_lv.Content = "お疲れさまでした";
			var window = Application.Current.Windows.OfType<Window>().SingleOrDefault((w) =>w.IsActive);
			window.Close();
		}

		public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
																		MessageBoxButton buttns,
																		MessageBoxImage icon
																		)
		{
			CS_Util Util = new CS_Util();
			return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
		}
        //Livet Messenger用///////////////////////
        new public void Dispose()
        {
            // 基本クラスのDispose()でCompositeDisposableに登録されたイベントを解放する。
            base.Dispose();
            Dispose(true);
        }
        ///////////////////////Livet Messenger用//
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

        //public MessageBoxResult MessageShowWPF(String titolStr, String msgStr,
        //                                                                MessageBoxButton buttns,
        //                                                                MessageBoxImage icon
        //                                                                )
        //{
        //    CS_Util Util = new CS_Util();
        //    return Util.MessageShowWPF(msgStr, titolStr, buttns, icon);
        //}


    }
}
