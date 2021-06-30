using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using ProductionSchedule.Models;


namespace ProductionSchedule
{
	class GoogleCalendarUtil {
		public Google.Apis.Calendar.v3.Data.Event listItems = null;
		CS_Util Util = new CS_Util();

		/// <summary>
		/// GoogleCalender・Eventの登録状況表示
		/// </summary>
		/// <param name="timeMin">開始日</param>
		/// <param name="timeMax">終了日</param>
		/// <returns></returns>
		public IList<Event> GEventsListUp(DateTime timeMin, DateTime timeMax)
		{
			string TAG = "GEventsListUp";
			string dbMsg = "[GoogleCalendarUtil]";
			IList<Google.Apis.Calendar.v3.Data.Event> retList = new List<Google.Apis.Calendar.v3.Data.Event>();
			try {
				dbMsg += ",対象期間=" + timeMin + "～" + timeMax;
				// Create Google Calendar API service.
				var service = new CalendarService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyCalendarCredential,
					ApplicationName = Constant.ApplicationName,
				});
				dbMsg += ",HttpClient=" + service.HttpClient.ToString();

				// Define parameters of request.
				// ※終日・複数日のStartはDateTimeが無いのでStart.DateでソートしたいがOrderByEnumに該当カラムが無い
				EventsResource.ListRequest request = service.Events.List("primary");
				request.TimeMin = timeMin;                     //DateTime.Now;
				request.TimeMax = timeMax;
				request.ShowDeleted = false;
				request.SingleEvents = true;
				request.MaxResults = 1000;
				request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
				Task<Events> evRequest = Task.Run(() => {
					return request.ExecuteAsync();              //取得実行
				});
				evRequest.Wait();
				Events events = evRequest.Result;                           //作成結果が格納され戻される
																			//				Events events = request.Execute();              //取得実行
				Constant.CalenderSummary = events.Summary;
				if (events.Items != null && events.Items.Count > 0) {
					dbMsg += ",events=" + events.Items.Count() + "件";
					foreach (var eventItem in events.Items) {
						string startDT = eventItem.Start.DateTime.ToString();
						dbMsg += "\r\n" + startDT;
						string endDT = eventItem.End.DateTime.ToString();
						dbMsg += "～" + endDT;
						if (String.IsNullOrEmpty(startDT)) {
							startDT = eventItem.Start.Date;
						}
						string Summary = eventItem.Summary;
						dbMsg += "," + Summary;
						retList.Add(eventItem);
					}
					dbMsg += "," + retList.Count() + "件";
				} else {
					dbMsg += "まだ予定が登録されていません";
				}
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retList;
		}

		/// <summary>
		/// 指定されたスケジュールを検索する
		/// </summary>
		/// <param name="KetStr"></param>
		/// <param name="VarStr"></param>
		/// <param name="timeMin"></param>
		/// <param name="timeMax"></param>
		/// <returns></returns>
		public Event Pram2GEvents(string KetStr, string VarStr, DateTime timeMin, DateTime timeMax)
		{
			string TAG = "Pram2GEvents";
			string dbMsg = "[GoogleCalendarUtil]";
			//この戻り値はnullを返すだけ
			Google.Apis.Calendar.v3.Data.Event rEvent = new Google.Apis.Calendar.v3.Data.Event();
			try {
				dbMsg += ",検索対象=" + KetStr + " :  " + VarStr;
				if (KetStr.Equals("HtmlLink")) {
					if (VarStr.Contains("calendar/r/eventedit/")) {
						string[] delimiter = { "calendar/r/eventedit/" };
						string[] Strs = VarStr.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						VarStr = Strs[1];
						string[] delimiter2 = { "?pli=" };
						string[] Strs2 = VarStr.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);
						VarStr = Strs2[0];
					} else if (VarStr.Contains("www.google.com/calendar/event")) {
						string[] delimiter = { "eid=" };
						string[] Strs = VarStr.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						VarStr = Strs[1];
					}
					dbMsg += ">>" + VarStr;
				}

				dbMsg += " ,対象期間=" + timeMin + "～" + timeMax;
				var service = new CalendarService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyCalendarCredential,
					ApplicationName = Constant.ApplicationName,
				});
				dbMsg += ",HttpClient=" + service.HttpClient.ToString();
				EventsResource.ListRequest request = service.Events.List("primary");
				request.TimeMin = timeMin;                     //DateTime.Now;
				request.TimeMax = timeMax;
				request.ShowDeleted = false;
				request.SingleEvents = true;
				request.MaxResults = 1000;
				request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
				Task<Events> evRequest = Task.Run(() => {
					return request.ExecuteAsync();              //取得実行
				});
				evRequest.Wait();
				Events events = evRequest.Result;                           //作成結果が格納され戻される
																			//				Events events = request.Execute();              //取得実行
				Constant.CalenderSummary = events.Summary;
				if (events.Items != null && events.Items.Count > 0) {
					dbMsg += ",events=" + events.Items.Count() + "件";
					foreach (var eventItem in events.Items) {
						if (KetStr.Equals("Start")) {
							if (eventItem.Start.ToString().Equals(VarStr)) {
								rEvent = eventItem;
							}
						} else if (KetStr.Equals("HtmlLink")) {
							string HtmlLink = @eventItem.HtmlLink;
							string[] delimiter = { "eid=" };
							string[] Strs = HtmlLink.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
							HtmlLink = Strs[1];
							dbMsg += ", " + HtmlLink;
							if (HtmlLink.Equals(VarStr)) {
								rEvent = eventItem; 

						//		return eventItem;
								break;
							}
						}
						//					retList.Add(eventItem);
					}
					string startDT = rEvent.Start.DateTime.ToString();
					dbMsg += "\r\n" + startDT;
					string endDT = rEvent.End.DateTime.ToString();
					dbMsg += "～" + endDT;
					if (String.IsNullOrEmpty(startDT)) {
						startDT = rEvent.Start.Date;
					}
					string Summary = rEvent.Summary;
					dbMsg += "," + Summary;
					//				dbMsg += "," + retList.Count() + "件";
				} else {
					dbMsg += "まだ予定が登録されていません";
				}
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return rEvent;
		}

		/// <summary>
		/// GoogleCalenderならtrueを返す
		/// </summary>
		/// <param name="CurrentUrl"></param>
		/// <returns></returns>
		public bool IsGoogleCalender(string CurrentUrl)
		{
			string TAG = "IsGoogleCalender";
			string dbMsg = "[GoogleCalendarUtil]";
			bool retBool = false;
			try {
				dbMsg += ",CurrentUrl= 　" + CurrentUrl;
				if (CurrentUrl.Contains("calendar.google.com/calendar/r/") ||
					CurrentUrl.Contains("www.google.com/calendar/")) {
					retBool = true;
				}
				dbMsg += ",retBool= " + retBool;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retBool;
		}

		public bool IsGoogleEvent(string CurrentUrl)
		{
			string TAG = "IsGoogleEvent";
			string dbMsg = "[GoogleCalendarUtil]";
			bool retBool = false;
			try {
				dbMsg += ",CurrentUrl= 　" + CurrentUrl;
				if (CurrentUrl.Contains("calendar.google.com/calendar/r/") &&
					CurrentUrl.Contains("eid=")) {
					retBool = true;
				}
				dbMsg += ",retBool= " + retBool;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retBool;
		}

		public bool IsEventEditEnd(string CurrentUrl)
		{
			string TAG = "IsGoogleEvent";
			string dbMsg = "[GoogleCalendarUtil]";
			bool retBool = false;
			try {
				dbMsg += ",CurrentUrl= 　" + CurrentUrl;
				if (CurrentUrl.Contains("calendar.google.com/calendar/r/") &&
					CurrentUrl.Contains("sf=false")) {
					retBool = true;
				}
				dbMsg += ",retBool= " + retBool;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retBool;
		}

		/// <summary>
		/// Googleカレンダーから表示している日付を返す
		/// 日付部分が無ければ本日と判定する
		/// </summary>
		/// <param name="CurrentUrl"></param>
		public DateTime GoogleWebCurentDate(string CurrentUrl)
		{
			string TAG = "GoogleWebCurentDate";
			string dbMsg = "[GoogleCalendarUtil]";
			DateTime timeCurrent = DateTime.Now;
			try {
				dbMsg += ",CurrentUrl= 　" + CurrentUrl;
				string DatePram = "";
				if (IsGoogleCalender(CurrentUrl)) {
					if (CurrentUrl.Contains("r/year")) {
						dbMsg += ">>年";
						string[] delimiter = { "r/year" };
						string[] Strs = CurrentUrl.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						DatePram = Strs[1];
					} else if (CurrentUrl.Contains("r/month")) {
						dbMsg += ">>月";
						string[] delimiter = { "r/month" };
						string[] Strs = CurrentUrl.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						DatePram = Strs[1];
					} else if (CurrentUrl.Contains("r/week")) {
						dbMsg += ">>週";
						string[] delimiter = { "r/week" };
						string[] Strs = CurrentUrl.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						DatePram = Strs[1];
					} else if (CurrentUrl.Contains("r/day")) {
						dbMsg += ">>日";
						string[] delimiter = { "r/day" };
						string[] Strs = CurrentUrl.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						DatePram = Strs[1];
					} else if (CurrentUrl.Contains("r/agenda")) {
						dbMsg += ">>スケジュール";
						string[] delimiter = { "r/agenda" };
						string[] Strs = CurrentUrl.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
						DatePram = Strs[1];
					}
				}
				dbMsg += ",DatePram= " + DatePram;
				string[] dStrs = DatePram.Split('/');
				if (2 < dStrs.Length) {
					int yearInt = int.Parse(dStrs[dStrs.LongLength - 3]);
					int monthInt = int.Parse(dStrs[dStrs.LongLength - 2]);
					int dayInt = int.Parse(dStrs[dStrs.LongLength - 1]);
					timeCurrent = new DateTime(yearInt, monthInt, dayInt);
				}
				dbMsg += " ,対象期間=" + timeCurrent;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return timeCurrent;
		}


		/// <summary>
		/// EventDateTimeを20211231などの数値で返す
		/// </summary>
		/// <param name="TEventDateTime">EventのStartやEnd</param>
		/// <returns>20211231などのint</returns>
		public int EventDate2Int(EventDateTime TEventDateTime)
		{
			string TAG = "EventDate2Str";
			string dbMsg = "[GoogleCalendarUtil]";
			int retInt = 0;
			try {
				DateTime tDateTime = EventDateTime2DT(TEventDateTime);
				string retStr = String.Format("{0:yyyyMMdd}", tDateTime);
				retInt = int.Parse(retStr);
				dbMsg += ">>" + retInt;
				//		Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retInt;
		}

		public long EventDateTime2Long(EventDateTime TEventDateTime)
		{
			string TAG = "EventDateTime2Long";
			string dbMsg = "[GoogleCalendarUtil]";
			long retLong = 0;
			try {
				DateTime tDateTime = EventDateTime2DT(TEventDateTime);
				string retStr = String.Format("{0:yyyyMMddHHmm}", tDateTime);
				retLong = long.Parse(retStr);
				dbMsg += ">>" + retLong;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retLong;
		}

		/// <summary>
		///  EventDateTimeをyyyyMMddの文字列で返す
		/// </summary>
		/// <param name="TEventDateTime">EventのStartやEnd</param>
		/// <returns>yyyyMMddのstring</returns>
		public string EventDateTime2Str(EventDateTime TEventDateTime)
		{
			string TAG = "EventDateTime2Str";
			string dbMsg = "[GoogleCalendarUtil]";
			string retStr = null;
			try {
				DateTime tDateTime = EventDateTime2DT(TEventDateTime);
				if (tDateTime == null) {
					tDateTime = DateTime.Now;
				}
				retStr = String.Format("{0:yyyyMMdd}", tDateTime);
				dbMsg += ">>" + retStr;
				//			Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}

		/// <summary>
		/// Google.EventのStart・Endでデータが入っているところからDateTimeを返す
		/// 何も使えるものが無ければ現在日時が返る
		/// </summary>
		/// <param name="TEventDateTime"></param>
		/// <returns></returns>
		public DateTime EventDateTime2DT(EventDateTime TEventDateTime)
		{
			string TAG = "EventDateTime2DT";
			string dbMsg = "[GoogleCalendarUtil]";
			DateTime tDateTime = DateTime.Now;
			try {
				if (TEventDateTime.Date != null) {
					dbMsg += "Date=" + TEventDateTime.Date;
					string todayItemStartDate = TEventDateTime.Date;
					string[] sStr = todayItemStartDate.Split('-');
					tDateTime = new DateTime(int.Parse(sStr[0]), int.Parse(sStr[1]), int.Parse(sStr[2]));
				} else if (TEventDateTime.DateTime != null) {
					dbMsg += "Date=" + TEventDateTime.DateTime;
					int sYear = TEventDateTime.DateTime.Value.Year;
					int sMonth = TEventDateTime.DateTime.Value.Month;
					int sDay = TEventDateTime.DateTime.Value.Day;
					int sHour = TEventDateTime.DateTime.Value.Hour;
					int sMinute = TEventDateTime.DateTime.Value.Minute;
					int sSecond = TEventDateTime.DateTime.Value.Second;
					tDateTime = new DateTime(sYear, sMonth, sDay, sHour, sMinute, sSecond);
				}
				dbMsg += ">>" + tDateTime;
				//			Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return tDateTime;
		}


		public Color ColorId2RGB(string colorId)
		{
			string TAG = "ColorId2RGB";
			string dbMsg = "[GoogleCalendarUtil]";
			Color reColor = Color.FromRgb(0x00, 0xFF, 0x00);
			try {
				dbMsg += "colorId=" + colorId;
				/*
								Google.Apis.Calendar.v3.Data.Colors colors = Constant.MyCalendarService.Colors.Get().Fetch();

								// Print available calendarListEntry colors.
								foreach (KeyValuePair<String, ColorDefinition> color in colors.Calendar) {
									System.out.println("ColorId : " + color.Key);
									System.out.println("  Background: " + color.Value.Background);
									System.out.println("  Foreground: " + color.Value.Foreground);
								}
								// Print available event colors.
								foreach (KeyValuePair<String, ColorDefinition> color in colors.Event) {
									System.out.println("ColorId : " + color.Key);
									System.out.println("  Background: " + color.Value.Background);
									System.out.println("  Foreground: " + color.Value.Foreground);
								}
				*/

				switch (colorId) {
					case "1":
						dbMsg += ":ラベンダー";
						reColor = Color.FromRgb(121, 134, 203);
						break;
					case "2":
						dbMsg += ":セージ";
						reColor = Color.FromRgb(51, 182, 121);
						break;
					case "3":
						dbMsg += ":ブドウ";
						reColor = Color.FromRgb(142, 36, 170);
						break;
					case "4":
						dbMsg += ":フラミンゴ";
						reColor = Color.FromRgb(230, 124, 115);
						break;
					case "5":
						dbMsg += ":バナナ";
						reColor = Color.FromRgb(246, 192, 40);
						break;
					case "6":
						dbMsg += ":ミカン";
						reColor = Color.FromRgb(244, 81, 30);
						break;
					case "7":
						dbMsg += ":不明";
						break;
					case "8":
						dbMsg += ":ブルーベリー";
						reColor = Color.FromRgb(63, 81, 181);
						break;
					case "9":
						dbMsg += ":グラファイト";
						reColor = Color.FromRgb(97, 97, 97);
						break;
					case "10":
						dbMsg += ":バジル";
						reColor = Color.FromRgb(11, 128, 67);
						break;
					case "11":
						dbMsg += ":トマト";
						reColor = Color.FromRgb(213, 0, 0);
						break;
					default:
						dbMsg += ":無指定：ピーコック";
						reColor = Color.FromRgb(121, 134, 203);
						break;
				}
				dbMsg += ">>" + reColor;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return reColor;
		}
		//https://developers.google.com/calendar/v3/reference/colors/get#.net

		/// <summary>
		/// GoogleのIDで定義されたEventColor	を設定する	
		/// </summary>
		public void setGoogleEventColor()
		{
			string TAG = "setGoogleEventColor";
			string dbMsg = "[GCalender]";
			try {
				Constant.googleEventColor = new List<Constant.GoogleEventColor>();
				dbMsg += ":googleEventColor=" + Constant.googleEventColor.Count + "件";
		//		Constant.googleEventColor.Add(new Constant.GoogleEventColor("0", "文字", Color.FromRgb(255, 255, 255)));       //,#FF7986CB
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("1", "ラベンダー", Color.FromRgb(121, 134, 203)));       //,#FF7986CB
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("2", "セージ", Color.FromRgb(1, 182, 121)));                   //#FF01B679
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("3", "ブドウ", Color.FromRgb(142, 36, 170)));                  //#FF8E24AA
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("4", "フラミンゴ", Color.FromRgb(230, 124, 115)));       //#FFE67C73
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("5", "バナナ", Color.FromRgb(246, 192, 40)));               //#FFF6C028
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("6", "ミカン", Color.FromRgb(244, 81, 30)));                   //#FFF4511E
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("7", "ピーコック", Color.FromRgb(121, 134, 203)));           //#7986cb
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("8", "ブルーベリー", Color.FromRgb(63, 81, 181)));            //#FF3F51B5
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("9", "グラファイト", Color.FromRgb(97, 97, 97)));         //#FF616161
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("10", "バジル", Color.FromRgb(11, 128, 67)));                //#FF0B8043
				Constant.googleEventColor.Add(new Constant.GoogleEventColor("11", "トマト", Color.FromRgb(213, 0, 0)));                        //#FFD50000				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
		}

		/// <summary>
		/// Eventの更新
		/// 成功したらUrlを返す
		/// </summary>
		/// <returns>Url</returns>
		public string UpDateGEvents(Google.Apis.Calendar.v3.Data.Event eventItem)
		{
			string TAG = "UpDateGEvents";
			string dbMsg = "[GoogleCalendarUtil]";
			string retLink = null;
			try {
				// 予定を追加登録
				String eventId = eventItem.Id;
				Event body = eventItem;
				dbMsg += "[" + eventId;
				dbMsg += "}" + eventItem.Start.ToString();
				dbMsg += "," + eventItem.Summary;
				dbMsg += "\r\n" + eventItem.Description;
				CalendarService service = new CalendarService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyCalendarCredential,
					ApplicationName = Constant.ApplicationName,
				});
				string calendarId = "primary";

				EventsResource.UpdateRequest request = service.Events.Update(body, calendarId, eventId);
				Event createdEvent = request.Execute();
				retLink = createdEvent.HtmlLink;
				dbMsg += ">>" + retLink;
				//添付許可を出す Optional query parameters	supportsAttachments　をtrueにする
				EventsResource.PatchRequest EPatch = service.Events.Patch(eventItem, calendarId, eventId);
				EPatch.SupportsAttachments = true;
				EPatch.Execute();
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retLink;
		}

		/// <summary>
		/// 予定を新規登録
		/// </summary>
		/// <returns></returns>
		public string InsertGEventsAsync(Google.Apis.Calendar.v3.Data.Event eventItem)
		{
			string TAG = "InsertGEvents";
			string dbMsg = "[GoogleCalendarUtil]";
			string retLink = null;
			try {
				dbMsg += ",Token= " + Constant.MyCalendarCredential.Token;
				String eventId = eventItem.Id;
				Event body = eventItem;
				dbMsg += eventItem.Start.ToString();
				//OriginalStartTimeを強制的に作る
				eventItem.OriginalStartTime = new Google.Apis.Calendar.v3.Data.EventDateTime();
				DateTime startDT = EventDateTime2DT(eventItem.Start);
				eventItem.OriginalStartTime.DateTime = startDT;
				dbMsg += ",OriginalStartTime=" + eventItem.OriginalStartTime;

				dbMsg += "," + eventItem.Summary;
				dbMsg += "\r\n" + eventItem.Description;
				CalendarService service = new CalendarService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyCalendarCredential,
					ApplicationName = Constant.ApplicationName,
				});
				string calendarId = "primary";
				EventsResource.InsertRequest request = service.Events.Insert(body, calendarId);

                Event createdEvent =  request.Execute();
             //   Event createdEvent = await request.ExecuteAsync(); でも失敗する
                 eventId = createdEvent.Id;
                dbMsg += "," + eventId + ")";
                retLink = createdEvent.HtmlLink;
				dbMsg += ">>" + retLink;
				//添付許可を出す Optional query parameters	supportsAttachments　をtrueにする
				EventsResource.PatchRequest EPatch = service.Events.Patch(eventItem, calendarId, eventId);
				EPatch.SupportsAttachments = true;
				EPatch.Execute();

				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retLink;
		}

		/// <summary>
		/// 予定を削除
		/// ※成功/失敗の判別不明
		/// </summary>
		/// <param name="eventItem"></param>
		/// <returns></returns>
		public void DeleteGEvents(Google.Apis.Calendar.v3.Data.Event eventItem)
		{
			string TAG = "DeleteGEvents";
			string dbMsg = "[GoogleCalendarUtil]";
			string retStr = null;
			try {
				dbMsg += ",Token= " + Constant.MyCalendarCredential.Token;
				string calendarId = "primary";
				dbMsg += "[calendarId:" + calendarId + "]";
				String eventId = eventItem.Id;
				dbMsg += "[eventId:" + eventId +"]";
				CalendarService service = new CalendarService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyCalendarCredential,
					ApplicationName = Constant.ApplicationName,
				});
				EventsResource.DeleteRequest request = service.Events.Delete(calendarId, eventId);
				retStr = request.Execute();
				//retLink = request.;
				//dbMsg += ">>" + retLink;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
		}


		///webからのスケジュール操作////////////////////////////////////////////////////////
		/// <summary>
		/// 選択された予定へ
		/// WebWindow.MakeEventから
		/// </summary>
		public string ModifyingEvent(Google.Apis.Calendar.v3.Data.Event taregetEvent, DateTime timeCurrent)
		{
			string TAG = "ModifyingEvent";
			string dbMsg = "[WebWindow]";
			string retLink = "";
			try {
				dbMsg += ",timeCurrent= " + timeCurrent;
				if(taregetEvent == null) {
					taregetEvent = MakeNewEvent( timeCurrent);
				}
				dbMsg += "  ,targetEvent=" + taregetEvent.Id;
				dbMsg += " ; " + taregetEvent.Summary;
				if(taregetEvent.Summary == null) {
					taregetEvent.Summary = "新規案件対応会議";
					dbMsg += "taregetEvent=" + taregetEvent.Summary;
				}

				//追加する項目
				Constant.orderNumber = "abc987654321DEF";                  //受注No（参照項目）
				Constant.managementNumber = timeCurrent.ToString();     //管理番号（参照項目）
				Constant.customerName = "(株)TEST建設";             //得意先（参照項目）
				string addText = "<table><tbody>";
				addText += "<tr><td>受注No</td>" + "<td> : " + Constant.orderNumber + "</td></tr>";
				addText += "<tr><td>管理番号</td>" + "<td> : " + Constant.managementNumber + "</td></tr>";
				addText += "<tr><td>得意先</td>" + "<td> : " + Constant.customerName + "</td></tr>";
				if(0 < taregetEvent.Attachments.Count) {
					addText += "<tr><td>添付ファイル</td>" + "<td> : ";
					foreach (Google.Apis.Calendar.v3.Data.EventAttachment tA in taregetEvent.Attachments) {
						addText += "<a href=" + tA.FileUrl + ">" + tA.Title + "<a></br>";
					}
					addText += Constant.customerName + "</td></tr>";
				}
				addText += "</tbody >";

				string description = taregetEvent.Description;
				dbMsg += ",Description=" + description;
				string memoStr = description;
				//Descriptionのエディターから入力できないタグで既に追記が有るかどうかを判定
				if ( description == null) {
					description = "添付ファイルを参照できる様、準備して参加をお願いします。</p>" + addText;
				} else if (description.Contains("<table>") ) {
					string[] delimiter = { "<table>" };
					string[] memoStrs = description.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
					string prefix = memoStrs[0];
					string suffix = memoStrs[memoStrs.Length-1];
					string[] delimiter2 = { "</table>" };
					string[] prefixs = suffix.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);
					suffix = memoStrs[memoStrs.Length - 1];
					description = "< pre > " + prefix + " </ pre >" + addText  + "<br><pre>" + suffix + "</pre>";
				}else{
					description = description +addText;
				}
				dbMsg += ",description=" + description;
				taregetEvent.Description = description;
				retLink = UpDateGEvents(taregetEvent);
				dbMsg += "\r\nretLink" + retLink;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retLink;
		}

		/// <summary>
		/// スケジュールに情報追加
		/// </summary>
		/// <param name="taregetEvent">作成もしくは変更する予定</param>
		/// <param name="addInfo">追加する情報</param>
		/// <returns></returns>
		public string AddEventInfo(Google.Apis.Calendar.v3.Data.Event taregetEvent, t_events tEvents)
		{
			string TAG = "AddEventInfo";
			string dbMsg = "[GoogleCalendarUtil]";
			string retLink = "";
			try {

				if (taregetEvent == null) {
					DateTime timeCurrent = DateTime.Now;
					dbMsg += ",timeCurrent= " + timeCurrent;
					taregetEvent = MakeNewEvent(timeCurrent);
				}
				dbMsg += ",tEvents：" + tEvents.event_date_start + "(" + tEvents.event_time_start + ")～" + tEvents.event_date_end + "(" + tEvents.event_time_end + ")" + tEvents.event_title;
				dbMsg += "  ,targetEvent=" + taregetEvent.Id;
				DateTime dateTime = DateTime.Parse(tEvents.event_date_start.ToString());
				dateTime.AddHours((int)tEvents.event_time_start);
				taregetEvent.Start.DateTime = dateTime;
				//taregetEvent.Start.DateTime = taregetEvent.Start.DateTime.        DateTime.Parse(tEvents.event_date_start.ToString() + " " + tEvents.event_time_start.ToString() + ":00");
				dbMsg += "  ," + taregetEvent.Start.DateTime;
				dateTime = DateTime.Parse(tEvents.event_date_end.ToString());
				dateTime.AddHours((int)tEvents.event_time_end);
				taregetEvent.End.DateTime = dateTime;				// DateTime.Parse(tEvents.event_date_end.ToString() + " " + tEvents.event_date_end.ToString() + ":00");
				dbMsg += "～" + taregetEvent.End.DateTime;
				taregetEvent.OriginalStartTime.DateTime = taregetEvent.Start.DateTime;
				dbMsg += ",OriginalStartTime=" + taregetEvent.OriginalStartTime.DateTime;
				dbMsg += ",終日=" + tEvents.event_is_daylong;
				if (tEvents.event_is_daylong == false) {
					taregetEvent.Start.Date = String.Format("yyyy-MM-dd" , taregetEvent.Start.DateTime);
					dbMsg += ">>" + taregetEvent.Start.Date;
				}else{

				}

				taregetEvent.Summary = tEvents.event_title;
				dbMsg += " ; " + taregetEvent.Summary;
				if (taregetEvent.Summary == null) {
					taregetEvent.Summary = "予定";
					dbMsg += ">>" + taregetEvent.Summary;
				}
				taregetEvent.Location = tEvents.event_place;
				taregetEvent.ColorId = tEvents.event_bg_color;


				int AttachmentsCount = taregetEvent.Attachments.Count;
				dbMsg += ",添付=" + AttachmentsCount + "件";

				//追加する項目
				string addText = "<table><tbody valign=" + "\"" + "top" + "\"" +">";
				if(tEvents.event_type ==1) {
					//using (var context = new EventEntities()) {
					//	var rProject = context.t_project_base.Single(x => x.Id == tEvents.t_project_base_id);
					//	dbMsg += "[" + rProject.Id + "]" + rProject.project_name;
					//	addText += "<tr><td>案件番号</td>" + "<td> : " + rProject.project_manage_code + "</td></tr>";                 // selectedAriadneData.ItemNumber
					//	addText += "<tr><td>受注No</td>" + "<td> : " + rProject.order_number + "</td></tr>";                    //OrderNumber
					//	addText += "<tr><td>管理番号</td>" + "<td> : " + rProject.management_number + "</td></tr>";       //ManagementNumber
					//	addText += "<tr><td>得意先</td>" + "<td> : " + rProject.owner_name + "</td></tr>";                        //CustomerName
					//}

				}
				if (0 < taregetEvent.Attachments.Count) {
					addText += "<tr valign=" + "\"" + "top" + "\"" + "><td>添付ファイル</td><td> ";
					int RowCount = 1;
					foreach (Google.Apis.Calendar.v3.Data.EventAttachment tA in taregetEvent.Attachments) {
						addText += " :<a href=" + tA.FileUrl + ">" + tA.Title + "</a>";
						if(RowCount < AttachmentsCount) {
							addText += "<br>";
						}
						RowCount++;
					}
					addText +=  "</td></tr>";
				}

				addText += "<tr valign = " + "\"" + "top" + "\"" + "><td>メモ</td>" + "<td> : <pre>" + tEvents.event_memo + "</pre></td></tr>";             //Memo

				addText += "</tbody ></table>";

				string description = addText;
				//dbMsg += ",Description=" + description;
				//string memoStr = description;
				////Descriptionのエディターから入力できないタグで既に追記が有るかどうかを判定
				//if (description == null) {
				//	description = addText + "</p>" +;
				//} else if (description.Contains("<table>")) {
				//	string[] delimiter = { "<table>" };
				//	string[] memoStrs = description.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
				//	string prefix = memoStrs[0];
				//	string suffix = memoStrs[memoStrs.Length - 1];
				//	string[] delimiter2 = { "</table>" };
				//	string[] prefixs = suffix.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);
				//	suffix = memoStrs[memoStrs.Length - 1];
				//	description = "< pre > " + prefix + " </ pre >" + addText + "<br><pre>" + suffix + "</pre>";
				//} else {
				//	description = description + addText;
				//}
				dbMsg += ",description=" + description;
				taregetEvent.Description = description;
				if (taregetEvent.Id == null) {
					dbMsg += "新規";
					retLink = InsertGEventsAsync(taregetEvent);
				} else {
					dbMsg += "変更";
					retLink = UpDateGEvents(taregetEvent);
				}

				dbMsg += "\r\nretLink" + retLink;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retLink;
		}

		//public AriadneData ReedEventInfo(Google.Apis.Calendar.v3.Data.Event taregetEvent)
		//{
		//	string TAG = "ReedEventInfo";
		//	string dbMsg = "[GoogleCalendarUtil]";
		//	 AriadneData selectedAriadneData = new AriadneData();
		//	try {
		//		string[] delimiterStart= { "</td>" + "<td> : " };
		//		string[] delimiterEnd = { "</td></tr>" };

		//		string description = taregetEvent.Description;
		//		dbMsg += ",description= " + description + "\r\n";
		//		string[] delimiter = { "<table>" };
		//		string[] memoStrs = description.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
		//		string prefix = memoStrs[0];
		//		string suffix = memoStrs[memoStrs.Length - 1];
		//		dbMsg += ",suffix= " + suffix + "\r\n";
		//		//末尾から分離
		//		string[] delimiter3 = { "得意先" };
		//		memoStrs = suffix.Split(delimiter3, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		string CustomerNameStr = memoStrs[memoStrs.Length - 1];
		//		string[] delimiter2 = { "管理番号" };
		//		memoStrs = prefix.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		string ManagementNumberStr = memoStrs[memoStrs.Length - 1];
		//		string[] delimiter1 = { "受注No" };
		//		memoStrs = prefix.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		string OrderNumberStr = memoStrs[memoStrs.Length - 1];
		//		string[] delimiter4 = { "案件番号" };
		//		memoStrs = prefix.Split(delimiter4, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		string ItemNumberStr = memoStrs[memoStrs.Length - 1];

		//		//先頭から読込み
		//		dbMsg += "\r\n案件番号= " + ItemNumberStr;
		//		memoStrs = ItemNumberStr.Split(delimiterEnd, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		suffix = memoStrs[memoStrs.Length - 1];

		//		dbMsg += ",prefix= " + prefix;
		//		memoStrs = prefix.Split(delimiterStart, StringSplitOptions.RemoveEmptyEntries);
		//		selectedAriadneData.ItemNumber = memoStrs[memoStrs.Length - 1];
		//		dbMsg += ">>" + selectedAriadneData.ItemNumber;

		//		dbMsg += "\r\n受注No= " + OrderNumberStr;
		//		memoStrs = OrderNumberStr.Split(delimiterEnd, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		suffix = memoStrs[memoStrs.Length - 1];

		//		dbMsg += ",prefix= " + prefix;
		//		memoStrs = prefix.Split(delimiterStart, StringSplitOptions.RemoveEmptyEntries);
		//		selectedAriadneData.ManagementNumber = memoStrs[memoStrs.Length - 1];
		//		dbMsg += ">>" + selectedAriadneData.ManagementNumber;
		//		Constant.orderNumber = selectedAriadneData.ManagementNumber;           //受注No

		//		dbMsg += "\r\n管理番号= " + ManagementNumberStr;
		//		memoStrs = ManagementNumberStr.Split(delimiterEnd, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		suffix = memoStrs[memoStrs.Length - 1];

		//		dbMsg += ",prefix= " + prefix;
		//		memoStrs = prefix.Split(delimiterStart, StringSplitOptions.RemoveEmptyEntries);
		//		selectedAriadneData.ManagementNumber = memoStrs[memoStrs.Length - 1];
		//		dbMsg += ">>" + selectedAriadneData.ManagementNumber;
		//		Constant.managementNumber = selectedAriadneData.ManagementNumber;           //管理番号

		//		dbMsg += "\r\n得意先= " + CustomerNameStr;
		//		memoStrs = CustomerNameStr.Split(delimiterEnd, StringSplitOptions.RemoveEmptyEntries);
		//		prefix = memoStrs[0];
		//		suffix = memoStrs[memoStrs.Length - 1];

		//		dbMsg += ",prefix= " + prefix;
		//		memoStrs = prefix.Split(delimiterStart, StringSplitOptions.RemoveEmptyEntries);
		//		selectedAriadneData.CustomerName = memoStrs[memoStrs.Length - 1];
		//		dbMsg += ">>" + selectedAriadneData.CustomerName;
		//		Constant.customerName = selectedAriadneData.CustomerName;                                      //得意先
		//		Util.MyLog(TAG, dbMsg);
		//	} catch (Exception er) {
		//		Util.MyErrorLog(TAG, dbMsg, er);
		//	}
		//	return selectedAriadneData;
		//}


		private Google.Apis.Calendar.v3.Data.Event MakeNewEvent(DateTime timeCurrent)
		{
			string TAG = "MakeNewEvent";
			string dbMsg = "[WebWindow]";
			Google.Apis.Calendar.v3.Data.Event taregetEvent = new Google.Apis.Calendar.v3.Data.Event();
			try {
				dbMsg += ",timeCurrent= " + timeCurrent;
				//作成直後はNullなので生成が必要
				taregetEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime();
				taregetEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime();
				taregetEvent.OriginalStartTime = new Google.Apis.Calendar.v3.Data.EventDateTime();
				taregetEvent.Attendees = new List<Google.Apis.Calendar.v3.Data.EventAttendee>();
				taregetEvent.Attachments = new List<Google.Apis.Calendar.v3.Data.EventAttachment>();
				//			taregetEvent.Reminders = new List<Google.Apis.Calendar.v3.Data.RemindersData>();

				//Eventにセットできる項目
				DateTime startDT = timeCurrent;
				DateTime now = DateTime.Now;
				TimeSpan nowTime = new TimeSpan(now.Hour, 0, 0);
				startDT = startDT.Add(nowTime);
				dbMsg += "startDT=" + startDT;
				taregetEvent.Start.DateTime = startDT;
				dbMsg += "Start=" + taregetEvent.Start.DateTime;
				//終了は一時間後を仮設定
				taregetEvent.End.DateTime = startDT.AddHours(1);
				dbMsg += "～" + taregetEvent.End.DateTime;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return taregetEvent;
		}

	}
}

/*
 * https://developers.google.com/calendar/v3/reference/events
 追加							http://sloppy-content.blog.jp/archives/16488560.html
 イベントを作成する		https://developers.google.com/calendar/create-events#java	
Events: insert				https://pentan.info/doc/google_developers/calendar/v3/reference/events/insert.html
Create Events				https://developers.google.com/calendar/create-events?utm_campaign=calendarAPI-615&utm_medium=blog&utm_source=gadbc#python_1

移植時の不明
EventEntities

 */
