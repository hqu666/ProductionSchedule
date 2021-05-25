using Livet;
using System;
using System.Collections.ObjectModel;

namespace ProductionSchedule.Models
{
	public  class InputDialogModel : NotificationObject, ICloneable {


		#region TitolStr
		private string _TitolStr;
		public string TitolStr {
			get { return _TitolStr; }
			set {
				if (_TitolStr == value) return;
				_TitolStr = value;
				RaisePropertyChanged("TitolStr");
			}
		}
		#endregion

		#region MessegeStr
		private string _MessegeStr;
		public string MessegeStr {
			get { return _MessegeStr; }
			set {
				if (_MessegeStr == value) return;
				_MessegeStr = value;
				RaisePropertyChanged("MessegeStr");
			}
		}
		#endregion

		#region InputStr
		private string _InputStr;
		public string InputStr {
			get { return _InputStr; }
			set {
				if (_InputStr == value) return;
				_InputStr = value;
				RaisePropertyChanged("InputStr");
			}
		}
		#endregion

		/// <summary>
		/// 自身のコピーを生成します。
		/// </summary>
		public object Clone()
		{
			return new InputDialogModel() {
				TitolStr = this.TitolStr,
				MessegeStr = this.MessegeStr,
				InputStr = this.InputStr
			};
		}
	}

}