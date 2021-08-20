using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ProductionSchedule.Models
{
    /// <summary>
    /// 階層管理モデル
    /// </summary>
    class MyHierarchy : INotifyPropertyChanged {

        //TreeView上での状態//////////////////////////////////////////////////////////////////
        private bool _IsSelected = false;
        public bool IsSelected {
            get { return _IsSelected; }
            set {
                _IsSelected = value; OnPropertyChanged("IsSelected");
            }
        }

        private bool _IsExpanded = false;
        public bool IsExpanded {
            get { return _IsExpanded; }
            set {
                _IsExpanded = value; OnPropertyChanged("IsExpanded");
            }
        }

        private Brush _Background = Brushes.Transparent;
        public Brush Background {
            get { return _Background; }
            set {
                _Background = value; OnPropertyChanged("Background");
            }
        }

        private Visibility _beforeSeparatorVisibility = Visibility.Hidden;
        public Visibility BeforeSeparatorVisibility {
            get { return _beforeSeparatorVisibility; }
            set {
                _beforeSeparatorVisibility = value; OnPropertyChanged("BeforeSeparatorVisibility");
            }
        }

        private Visibility _afterSeparatorVisibility = Visibility.Hidden;
        public Visibility AfterSeparatorVisibility {
            get { return _afterSeparatorVisibility; }
            set {
                _afterSeparatorVisibility = value; OnPropertyChanged("AfterSeparatorVisibility");
            }
        }
        //Drag&Drop動作////////////////////////////////////////////////////////TreeView上での状態//
        public MyHierarchy TreeParent { get; set; }

        public ObservableCollection<MyHierarchy> TreeChildren { get; set; } = new ObservableCollection<MyHierarchy>();


        //-- 地震の親を指定されたTreeViewItemInfoオブジェクトにし、子要素の親を自身として設定します
        public void SetParentToChildren(MyHierarchy parent = null) {
            TreeParent = parent;

            if (TreeChildren == null)
                return;
            foreach (var child in TreeChildren) {
                child.SetParentToChildren(this);
            }
        }

        //-- 既存の子要素アイテムの前に新しいアイテムを挿入します
        public void InsertBeforeChildren(MyHierarchy from, MyHierarchy newItem) {
            var index = TreeChildren.IndexOf(newItem);
            if (index < 0)
                return;

            TreeChildren.Insert(index, from);
        }

        //-- 既存の子要素アイテムの後ろに新しいアイテムを挿入します
        public void InsertAfterChildren(MyHierarchy from, MyHierarchy newItem) {
            var index = TreeChildren.IndexOf(newItem);
            if (index < 0)
                return;

            TreeChildren.Insert(index + 1, from);
        }

        //-- 子要素の末尾に新しいアイテムを追加します
        public void AddChildren(MyHierarchy info) {
            TreeChildren.Add(info);
        }

        //-- 子要素から指定されたアイテムを削除します
        public void RemoveChildren(MyHierarchy info) {
            TreeChildren.Remove(info);
        }

        //-- 親要素に指定されたアイテムが存在するかどうかをチェックします
        public bool ContainsParent(MyHierarchy info) {
            if (TreeParent == null)
                return false;
            return TreeParent == info || TreeParent.ContainsParent(info);
        }

        //GoogleSpredへの書き込み///////////////////////////////////////////////////Drag&Drop動作//
        /// <summary>
        /// 使用するパラメータ
        /// </summary>
        private string _Value = "";
        public string Value {
            get { return _Value; }
            set { _Value = value; OnPropertyChanged("Value"); }
        }

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
            //  if (null == Child) Child = new List<MyHierarchy>();  ここだと呼出し元で オブジェクト参照がオブジェクト インスタンスに設定されていません 
            child.parent = this;
            child.Add(child);
        }

        //-- 子要素から指定されたアイテムを削除します
        public void Remove(MyHierarchy child) {
            child.parent = this;
            child.Remove(child);
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
        public object Clone(MyHierarchy cl) {
            return new MyHierarchy() {
                id = cl.id,
                parent_iD = cl.parent_iD,
                order = cl.order,
                name = cl.name,
                parent = cl.parent,
                Child = cl.Child,
                hierarchy = cl.hierarchy
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
