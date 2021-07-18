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
        public int id {
            get { return _ID; }
            set {
                if (_ID == value)
                    return;
                _ID = value;
                OnPropertyChanged("id");
            }
        }

        /// <summary>
        /// 表示名称
        /// </summary>
        private string _Name = "";
        public string name {
            get { return _Name; }
            set {
                if (_Name == value)
                    return;
                _Name = value;
                OnPropertyChanged("name"); }
        }

        private int _ParentID;
        /// <summary>
        /// 直上のID
        /// </summary>
        public int parent_iD {
            get { return _ParentID; }
            set {
                if (_ParentID == value)
                    return;
                _ParentID = value;
                OnPropertyChanged("parent_iD");
            }
        }

        private int _order;
        /// <summary>
        /// 同階層の何番目か
        /// </summary>
        public int order {
            get { return _order; }
            set {
                if (_order == value)
                    return;
                _order = value;
                OnPropertyChanged("order");
            }
        }


        private int _Hierarchy;
        /// <summary>
        /// 何階層目か
        /// </summary>
        public int hierarchy {
            get { return _Hierarchy; }
            set {
                if (_Hierarchy == value)
                    return;
                _Hierarchy = value;
                OnPropertyChanged("hierarchy");
            }
        }


        public MyHierarchy _Parent;
        public MyHierarchy parent {
            get { return _Parent; }
            set {
                if (_Parent == value)
                    return;
                _Parent = value;
                OnPropertyChanged("parent");
                if (_ParentID == parent.id)
                    return;
                parent_iD = _Parent.id;

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
            child.parent = this;
            child.Add(child);
        }

        /// <summary>
        /// 自身のコピーを生成します。
        /// </summary>
        public object Clone() {
            return new MyHierarchy() {
                id = this.id,
                parent_iD = this.parent_iD,
                order = this.order,
                name = this.name,
                hierarchy = this.hierarchy
            };
        }

        ///////////////////////////////////////////////
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


    }
}
