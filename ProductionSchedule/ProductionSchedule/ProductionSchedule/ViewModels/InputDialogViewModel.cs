using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
//using MySql.Data.MySqlClient;
using ProductionSchedule.Models;

namespace ProductionSchedule.ViewModels {
	public class InputDialogViewModel : ViewModel {
		public string titolStr = "入力ダイアログ";
        /// <summary>
        /// 呼出し元
        /// </summary>
        //	public Z_1_4ViewModel ParentVM;

        private InputDialogModel _inputDialogModel;
		public InputDialogModel InputDlogModel {
			get { return _inputDialogModel; }
			set {
				if (_inputDialogModel == value)
					return;
				_inputDialogModel = value;
				RaisePropertyChanged("InputDlogModel");
			}
		}

		private string _InputStr;
		public string InputStr {
			get { return _InputStr; }
			set {
				if (_InputStr == value)
					return;
				_InputStr = value;
				RaisePropertyChanged();
			}
		}
		private string _PromptStr;
		public string PromptStr {
			get { return _PromptStr; }
			set {
				if (_PromptStr == value)
					return;
				_PromptStr = value;
				RaisePropertyChanged();
			}
		}

		private string _TitleStr;
		public string TitleStr {
			get { return _TitleStr; }
			set {
				if (_TitleStr == value)
					return;
				_TitleStr = value;
				RaisePropertyChanged();
			}
		}

		public InputDialogViewModel(InputDialogModel inputDialogModel)
		{
			// , object parent	ParentVM = (Z_1_4ViewModel)parent;
			Initialize(inputDialogModel);
		}

		public InputDialogViewModel()
		{
			string TAG = "InputDialogViewModel";
			string dbMsg = "パラメータ無し";
			MyLog(TAG, dbMsg);
		}

		public void Initialize(InputDialogModel inputDialogModel)
		{
			string TAG = "Initialize";
			string dbMsg = "";
			try {
				this.InputDlogModel = inputDialogModel;
				TitleStr = inputDialogModel.TitolStr;
				PromptStr = inputDialogModel.MessegeStr;
				InputStr= inputDialogModel.InputStr;
				RaisePropertyChanged();
				dbMsg += ",TitolStr=" + TitleStr + ",MessegeStr=" + PromptStr + ",InputStr=" + InputStr;
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}

		//////////////////////////////////////////////////登録//
		#region 確定
		private ViewModelCommand _ConnectCommand;

		public ViewModelCommand ConnectCommand {
			get {
				if (_ConnectCommand == null) {
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
			try {
				_inputDialogModel.InputStr = InputStr;
				RaisePropertyChanged("InputDlogModel");

				//if(ParentVM != null) {
				//	ParentVM.FromInput(InputStr);
				//}
				MyLog(TAG, dbMsg);
		//		Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
		}
		#endregion

		#region キャンセル
		private ViewModelCommand _CancelCommand;

		public ViewModelCommand CancelCommand {
			get {
				if (_CancelCommand == null) {
					_CancelCommand = new ViewModelCommand(Cancel);
				}
				return _CancelCommand;
			}
		}

		public void Cancel()
		{
			string TAG = "Cancel";
			string dbMsg = "";
			try {
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));

				//	Messenger.Raise(new TransitionMessage(new InputDialogViewModel(), "Cancel"));
				//			Messenger.Raise(new WindowActionMessage(WindowAction.Close, "InputClose"));
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}

		}
		#endregion

		private ViewModelCommand _nextCommand;
		public ViewModelCommand NextCommand => _nextCommand = _nextCommand ?? new ViewModelCommand(Next);

		public void Next()
		{
			Messenger.Raise(new TransitionMessage(new InputDialogViewModel(), "Next"));
		}

		//Livet Messenger用///////////////////////
		new public void Dispose()
		{
			// 基本クラスのDispose()でCompositeDisposableに登録されたイベントを解放する。
			base.Dispose();
			Dispose(true);
		}
		///////////////////////Livet Messenger用//

		//////////////////////////////////////////////////登録//
		public static void MyLog(string TAG, string dbMsg)
		{
			dbMsg = "[InputDialogViewModel]" + dbMsg;
			//TabCon.exe = MethodBase.GetCurrentMethod().Module.Name
			CS_Util Util = new CS_Util();
			Util.MyLog(TAG, dbMsg);
		}

		public static void MyErrorLog(string TAG, string dbMsg, Exception err)
		{
			dbMsg = "[InputDialogViewModel]" + dbMsg;
			CS_Util Util = new CS_Util();
			Util.MyErrorLog(TAG, dbMsg, err);
		}

	}
}
