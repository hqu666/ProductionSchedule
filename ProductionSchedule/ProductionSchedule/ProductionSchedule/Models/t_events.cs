using System;
using System.Collections.ObjectModel;
using Livet;

namespace ProductionSchedule.Models {
	/// <summary>
	/// イベント
	/// </summary>
	public partial class t_events : NotificationObject, ICloneable {

		///<summary>
		///ID
		///</summary>
		private int _id;
		public int id
		{
			get => _id;
			set
			{
				if (_id == value)
					return;
				_id = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///契約ID :=契約マスタ.ID
		///</summary>
		private int _m_contract_id;
		public int m_contract_id
		{
			get => _m_contract_id;
			set
			{
				if (_m_contract_id == value)
					return;
				_m_contract_id = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///案件ID :=案件情報.ID
		///</summary>
		private int _t_project_base_id;
		public int t_project_base_id
		{
			get => _t_project_base_id;
			set
			{
				if (_t_project_base_id == value)
					return;
				_t_project_base_id = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///イベント種別 :※固定値：イベント種別
		///</summary>
		private int _event_type;
		public int event_type
		{
			get => _event_type;
			set
			{
				if (_event_type == value)
					return;
				_event_type = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///イベント開始日 :未登録案件はnull
		///</summary>
		private DateTime _event_date_start;
		public DateTime event_date_start
		{
			get => _event_date_start;
			set
			{
				if (_event_date_start == value)
					return;
				_event_date_start = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///イベント開始時刻 :※固定値：イベント時刻
		///</summary>
		private int _event_time_start;
		public int event_time_start
		{
			get => _event_time_start;
			set
			{
				if (_event_time_start == value)
					return;
				_event_time_start = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///イベント終了日
		///</summary>
		private DateTime _event_date_end;
		public DateTime event_date_end
		{
			get => _event_date_end;
			set
			{
				if (_event_date_end == value)
					return;
				_event_date_end = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///イベント終了時刻 :※固定値：イベント時刻
		///</summary>
		private int _event_time_end;
		public int event_time_end
		{
			get => _event_time_end;
			set
			{
				if (_event_time_end == value)
					return;
				_event_time_end = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///終日
		///</summary>
		private bool _event_is_daylong;
		public bool event_is_daylong
		{
			get => _event_is_daylong;
			set
			{
				if (_event_is_daylong == value)
					return;
				_event_is_daylong = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///タイトル
		///</summary>
		private string _event_title;
		public string event_title
		{
			get => _event_title;
			set
			{
				if (_event_title == value)
					return;
				_event_title = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///場所
		///</summary>
		private string _event_place;
		public string event_place
		{
			get => _event_place;
			set
			{
				if (_event_place == value)
					return;
				_event_place = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///メモ
		///</summary>
		private string _event_memo;
		public string event_memo
		{
			get => _event_memo;
			set
			{
				if (_event_memo == value)
					return;
				_event_memo = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///ステータス :※固定値：イベントステータス
		///</summary>
		private int _event_status;
		public int event_status
		{
			get => _event_status;
			set
			{
				if (_event_status == value)
					return;
				_event_status = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///GoogleイベントID
		///</summary>
		private string _google_id;
		public string google_id
		{
			get => _google_id;
			set
			{
				if (_google_id == value)
					return;
				_google_id = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///背景色 :※固定値：カラーもしくはARGB値（９桁）
		///</summary>
		private string _event_bg_color;
		public string event_bg_color
		{
			get => _event_bg_color;
			set
			{
				if (_event_bg_color == value)
					return;
				_event_bg_color = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///文字色 :ARGB値（９桁：カラーピッカーによっては透明度が付与される）
		///</summary>
		private string _event_font_color;
		public string event_font_color
		{
			get => _event_font_color;
			set
			{
				if (_event_font_color == value)
					return;
				_event_font_color = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///作成者
		///</summary>
		private int _created_user;
		public int created_user
		{
			get => _created_user;
			set
			{
				if (_created_user == value)
					return;
				_created_user = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///作成日時
		///</summary>
		private DateTime? _created_at;
		public DateTime? created_at
		{
			get => _created_at;
			set
			{
				if (_created_at == value)
					return;
				_created_at = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///更新者
		///</summary>
		private int _updated_user;
		public int updated_user
		{
			get => _updated_user;
			set
			{
				if (_updated_user == value)
					return;
				_updated_user = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///更新日時
		///</summary>
		private DateTime _updated_at;
		public DateTime updated_at
		{
			get => _updated_at;
			set
			{
				if (_updated_at == value)
					return;
				_updated_at = value;
				RaisePropertyChanged();
			}
		}

		///<summary>
		///削除日時
		///基本的にNullなのでnull 許容参照型に型指定する
		///</summary>
		private DateTime ? _deleted_at;
		public DateTime ? deleted_at
		{
			get => _deleted_at;
			set
			{
				if (_deleted_at == value)
					return;
				_deleted_at = value;
				RaisePropertyChanged();
			}
		}
		//生成以降の追記//////////////////////////////////////////////////////////////
		/// <summary>
		/// 表示用の文字列
		/// </summary>
		private string _summary;
		public string summary {
			get => _summary;
			set {
				if (_summary == value)
					return;
				_summary = value;
				//_summary = "終日";
				//if (!_event_is_daylong) {
				//	_summary = _event_time_start + "～" + _event_time_end;
				//}
				//_summary += ": " + _event_title + " : " + _event_place + " : " + _event_memo;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 同じ日で何番目の予定か
		/// </summary>
		private int _childIndex;
		public int childIndex {
			get => _childIndex;
			set {
				if (_childIndex == value)
					return;
				_childIndex = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 選択された
		/// </summary>
		private bool _isSetect;
		public bool isSetect {
			get => _isSetect;
			set {
				if (_isSetect == value)
					return;
				_isSetect = value;
				RaisePropertyChanged();
			}
		}

		public bool _IsChecked;
		/// <summary>
		/// リスト先頭のチェック
		/// </summary>
		public bool IsChecked {
			get { return _IsChecked; }
			set {
				if (_IsChecked == value)
					return;

				_IsChecked = value;
			}
		}

		object ICloneable.Clone()
		{
			return new t_events() {
				id = this.id,
				event_type = this.event_type,
				event_title = this.event_title,
				event_date_start = this.event_date_start,
				event_time_start = this.event_time_start,
				event_date_end = this.event_date_end,
				event_time_end = this.event_time_end,
				event_is_daylong = this.event_is_daylong,
				event_place = this.event_place,
				event_memo = this.event_memo,
				google_id = this.google_id,
				event_status = this.event_status,
				event_bg_color = this.event_bg_color,
				event_font_color = this.event_font_color,
				created_user = this.created_user,
				created_at = this.created_at,
				updated_user = this.updated_user,
				updated_at = this.updated_at,
				deleted_at = this.deleted_at
			};
		}
		/////////////////////////////////////////////////////////////生成以降の追記///
	}

	public class t_eventsCollection : ObservableCollection<t_events> {
		public t_eventsCollection(){
		}
	}
}
