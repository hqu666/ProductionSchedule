using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionSchedule.Models {

/// <summary>
/// MainWindowのTreeView用
/// </summary>
	public class MyMenu : INotifyPropertyChanged {

		private bool _IsSelected = false;
		public bool IsSelected {
			get { return _IsSelected; }
			set {
				_IsSelected = value; OnPropertyChanged("IsSelected");
			}
		}

		/// <summary>
		/// 表示名称
		/// </summary>
		private string _Name = "";
		public string Name {
			get { return _Name; }
			set { _Name = value; OnPropertyChanged("Name"); }
		}

		/// <summary>
		/// 使用するパラメータ
		/// </summary>
		private string _Value = "";
		public string Value {
			get { return _Value; }
			set { _Value = value; OnPropertyChanged("Value"); }
		}

		public MyMenu _Parent;
		public MyMenu Parent {
			get { return _Parent; }
			set { _Parent = value; OnPropertyChanged("Parent"); }
		}

		public List<MyMenu> _Child;
		/// <summary>
		/// サブメニュー配列
		/// </summary>
		public List<MyMenu> Child {
			get { return _Child; }
			set { _Child = value; OnPropertyChanged("Child"); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string name)
		{
			if (null == this.PropertyChanged) return;
			this.PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		public void Add(MyMenu child)
		{
			if (null == Child) Child = new List<MyMenu>();
			child.Parent = this;
			child.Add(child);
		}

	}
}
