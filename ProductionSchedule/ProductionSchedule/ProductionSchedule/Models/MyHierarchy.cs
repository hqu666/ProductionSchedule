using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionSchedule.Models
{
    class MyHierarchy : INotifyPropertyChanged {

        private int _ID;
        /// <summary>
        /// 自身のID
        /// </summary>
        public int ID {
            get { return _ID; }
            set {
                if (_ID == value)
                    return;
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// 表示名称
        /// </summary>
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set {
                if (_Name == value)
                    return;
                _Name = value;
                OnPropertyChanged("Name"); }
        }

        private int _ParentID;
        /// <summary>
        /// 直上のID
        /// </summary>
        public int ParentID {
            get { return _ParentID; }
            set {
                if (_ParentID == value)
                    return;
                _ParentID = value;
                OnPropertyChanged("ParentID");
            }
        }


        private int _Hierarchy;
        /// <summary>
        /// 何階層目か
        /// </summary>
        public int Hierarchy {
            get { return _Hierarchy; }
            set {
                if (_Hierarchy == value)
                    return;
                _Hierarchy = value;
                OnPropertyChanged("Hierarchy");
            }
        }


        public MyHierarchy _Parent;
        public MyHierarchy Parent {
            get { return _Parent; }
            set {
                if (_Parent == value)
                    return;
                _Parent = value;
                OnPropertyChanged("Parent");
                if (_ParentID == _Parent.ID)
                    return;
                ParentID = _Parent.ID;

            }
        }

        public List<MyHierarchy> _Child;
        /// <summary>
        /// サブメニュー配列
        /// </summary>
        public List<MyHierarchy> Child {
            get { return _Child; }
            set { _Child = value; OnPropertyChanged("Child"); }
        }

        public void Add(MyHierarchy child) {
            if (null == Child) Child = new List<MyHierarchy>();
            child.Parent = this;
            child.Add(child);
        }


///////////////////////////////////////////////
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


    }
}
