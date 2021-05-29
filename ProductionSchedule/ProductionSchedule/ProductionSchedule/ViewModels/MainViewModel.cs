using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using ProductionSchedule.Views;
using ProductionSchedule.Models;
//using ProductionSchedule.Enums;
using System.Reflection;
using System.Windows.Input;

namespace ProductionSchedule.ViewModels
{
    public class MainViewModel  {
        // :ViewModel, INotifyPropertyChanged

        public Views.MainWindow MyView { get; set; }
		public List<Models.MyMenu> _MyMenu { get; set; }

		public System.Collections.IEnumerable TabItems { get; set; }

		public string InfoLavel { get; set; }
		public string ReTitle="";

		public MainViewModel()
		{
			Initialize();
		}

        public void Initialize()
        {
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

        /// <summary>
        /// このウィンドウを閉じる
        /// </summary>
        public void Close()
		{
//			MyView.Info_lv.Content = "お疲れさまでした";
			var window = Application.Current.Windows.OfType<Window>().SingleOrDefault((w) =>w.IsActive);
			window.Close();
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

        private class DelegateCommand : ICommand
        {
            private Action MyAction;
            private Action webStart;

            public DelegateCommand(Action _action)
            {
                this.MyAction = _action;
            }

            event EventHandler ICommand.CanExecuteChanged {
                add {
                    //          throw new NotImplementedException();
                }

                remove {
                    //          throw new NotImplementedException();
                }
            }

            bool ICommand.CanExecute(object parameter)
            {
                return true;
//                throw new NotImplementedException();
            }

            void ICommand.Execute(object parameter)
            {
                //if (MyAction.Method.Name.Equals("WebStart"))
                //{
                MyAction();
                //}
            }
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
