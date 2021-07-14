using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace ProductionSchedule
{
	class GoogleDriveUtil {
		CS_Util Util = new CS_Util();
		LocalFileUtil LFUtil = new LocalFileUtil();

		public string GoogleDriveMime_Folder = Constant.GoogleDriveMime_Folder;

		/// <summary>
		/// parents無しでもドライブ内ファイルをリストアップ
		/// </summary>
		/// <param name="pFolder"></param>
		/// <returns>IList<Google.Apis.Drive.v3.Data.File> </returns>
		public IList<Google.Apis.Drive.v3.Data.File> GDAllFolderListUp(string pFolder = null, bool isOnlyFolder = true)
		{
			string TAG = "GDFolderListUp";
			string dbMsg = "[GoogleDriveUtil]";
			//		string pFolder = Constant.RootFolderName;
			IList<Google.Apis.Drive.v3.Data.File> retList = new List<Google.Apis.Drive.v3.Data.File>();
			try {
				string pageToken = null;
				do {
					FilesResource.ListRequest request = Constant.MyDriveService.Files.List();
					request.Q = "(trashed = false)";
					if(isOnlyFolder) {
						request.Q += "and (mimeType = 'application/vnd.google-apps.folder')";
					}
					if(pFolder != null) {
						request.Q += "(parents = '" + pFolder + "')";
					}
					request.Q = "(mimeType = 'application/vnd.google-apps.folder') and (trashed = false)";
					request.Spaces = "drive";
					request.Fields = "nextPageToken, files(id, name , parents ,driveId )";
					request.PageToken = pageToken;
					var result = request.Execute();
					foreach (Google.Apis.Drive.v3.Data.File file in result.Files) {
						retList.Add(file);
						dbMsg += "\r\n" + file.DriveId + " / " + file.Parents[0] + " / " + file.Id + " = " + file.Name;
						//Console.WriteLine(String.Format(
						//		"Found file: {0} ({1})", file.Name, file.Id));
					}
					pageToken = result.NextPageToken;
				} while (pageToken != null);
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retList;
		}
		//ファイルとフォルダを検索する	https://www.milk-island.net/translate/ggd/drive/api/v3/search-files.html

		/// <summary>
		/// 指定されたフォルダ以下の全フォルダ取得
		/// passTreeに使用
		/// </summary>
		/// <param name="pFolder"></param>
		/// <returns></returns>
		public IList<Google.Apis.Drive.v3.Data.File> GDFolderListUp(string pFolder= null)
		{
			string TAG = "GDFolderListUp";
			string dbMsg = "[GoogleDriveUtil]";
	//		string pFolder = Constant.RootFolderName;
			IList<Google.Apis.Drive.v3.Data.File> retList = null;
			try {
				//			Constant.CurrentFolder = pFolder;
				// フォルダIDの取得
				FilesResource.ListRequest listRequest = Constant.MyDriveService.Files.List();
				listRequest.PageSize = 1;   // 取得するフォルダの条件をクエリ構文で指定
				listRequest.Q = "(name = '" + pFolder + "') and (mimeType = 'application/vnd.google-apps.folder') and (trashed = false)";
				if (pFolder == null) {
					listRequest.Q =  "(mimeType = 'application/vnd.google-apps.folder') and (trashed = false)";
				}
				listRequest.Fields = "nextPageToken, files(id)";

				var folderId = listRequest.Execute().Files.First().Id;
				dbMsg += "[" + folderId + "]" + pFolder;
				Task<IList<Google.Apis.Drive.v3.Data.File>> rFolders = Task.Run(() => GDFolderListUpAsyncBody(pFolder));
				IList<Google.Apis.Drive.v3.Data.File> pFolders = rFolders.Result;
				if (pFolders != null && pFolders.Count > 0) {
					dbMsg += "の直下に" + pFolders.Count + "件";
					foreach (var folder in pFolders) {
						retList.Add(folder);
						string folderName = folder.Name;
						dbMsg += "\r\n" + retList.Count + ")" + folderName;
						Task<IList<Google.Apis.Drive.v3.Data.File>> rr = Task.Run(() => GDFolderListUpAsyncBody(folderName));
						IList<Google.Apis.Drive.v3.Data.File> rrFolders = rr.Result;
					}
				} else {
					dbMsg += "の直下にフォルダは有りません";
				}
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retList;
		}

		public async Task<IList<File>> GDFolderListUpAsyncBody(string pFolder)
		{
			string TAG = "GDFolderListUpAsyncBody";
			string dbMsg = "[GoogleDriveUtil]";
			IList<Google.Apis.Drive.v3.Data.File> retList = new List<Google.Apis.Drive.v3.Data.File>();
			try {
				dbMsg += ",pFolder=" + pFolder;
				Constant.CurrentFolder = pFolder;
				// フォルダIDの取得
				FilesResource.ListRequest listRequest = Constant.MyDriveService.Files.List();
				listRequest.PageSize = 1;   // 取得するフォルダの条件をクエリ構文で指定
				listRequest.Q = "(name = '" + pFolder + "') and (mimeType = 'application/vnd.google-apps.folder') and (trashed = false)";
				listRequest.Fields = "nextPageToken, files(id)";

				var folderId = listRequest.Execute().Files.First().Id;
				dbMsg += "[" + folderId + "]";
				// フォルダの内容
				listRequest = Constant.MyDriveService.Files.List();
				listRequest.PageSize = 12;                      //返される共有ドライブの最大数。許容値は1〜100です。（デフォルト：10）
																//※19で表示されなくなった
				listRequest.Q = $"('{folderId}' in parents) and (trashed = false) and (mimeType = 'application/vnd.google-apps.folder')";
				listRequest.OrderBy = "name";
				listRequest.Fields = "nextPageToken, files(id, name,modifiedTime,size,parents,trashed, mimeType,webContentLink)";
				FileList ret = await listRequest.ExecuteAsync();
				if (ret != null) {
					int fCount = ret.Files.Count();
					if (0 < fCount) {
						dbMsg += "の直下に" + fCount + "件";
						foreach (var folder in ret.Files) {
							string folderName = folder.Name;
							dbMsg += "\r\n" + retList.Count + ")" + folderName;
							retList.Add(folder);
							//Task<IList<File>> rr = GDFolderListUpAsyncBody(folderName);
							//if(rr == null) {
							//	return retList;
							//}
							////Task<IList<Google.Apis.Drive.v3.Data.File>> rr = Task.Run(() => GDFolderListUpAsyncBody(folderName));
							////IList<Google.Apis.Drive.v3.Data.File> rrFolders = rr.Result;

						}
					} else {
						dbMsg += "の直下にフォルダは有りません";
						//			return retList;
					}
				}
				dbMsg = ">>" + retList.Count() + "件";
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retList;
		}

		/// <summary>
		/// 対象フォルダ内の登録状況表示
		/// </summary>
		/// <returns></returns>
		public IList<Google.Apis.Drive.v3.Data.File> GDFileListUp(string pFolder, bool isOnlyFolder, bool isOnlyFile = true)
		{
			string TAG = "GDFileListUp";
			string dbMsg = "[GoogleDriveUtil]";
			IList<Google.Apis.Drive.v3.Data.File> retList = null;
			try {
				dbMsg += ",pFolder=" + pFolder;
				Constant.CurrentFolder = pFolder;
				//親フォルダのid取得
				string pFolderId;
				// フォルダIDの取得
				FilesResource.ListRequest listRequest = Constant.MyDriveService.Files.List();
				listRequest.PageSize = 1;   // 取得するフォルダの条件をクエリ構文で指定
											//if(isOnlyFolder) {
				listRequest.Q = "(name = '" + pFolder + "') and (mimeType = 'application/vnd.google-apps.folder') and (trashed = false)";
				//}else{
				//	listRequest.Q = "(name = '" + pFolder + "')  and (trashed = false)";
				//}
				listRequest.Fields = "nextPageToken, files(id)";
				var folderId = listRequest.Execute().Files.First().Id;
				dbMsg += "[" + folderId + "]";
				// フォルダの内容
				listRequest = Constant.MyDriveService.Files.List();
				listRequest.PageSize = 12;                      //返される共有ドライブの最大数。許容値は1〜100です。（デフォルト：10）
																//※19で表示されなくなった
				listRequest.Q = $"('{folderId}' in parents) and (trashed = false)";
				if (isOnlyFolder) {
					listRequest.Q += " and (mimeType = 'application/vnd.google-apps.folder')";
				}
				listRequest.OrderBy = "name";
				listRequest.Fields = "nextPageToken, files(id, name,modifiedTime,size,parents,trashed, mimeType,webContentLink)";
				IList<Google.Apis.Drive.v3.Data.File> ret = listRequest.Execute().Files;
				if (ret != null) {
					retList = ret;
					dbMsg = "," + retList.Count() + "件";
				}
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retList;
		}

		/// <summary>
		/// フォルダを作り、IDを返す
		/// 既に同名フォルダが有ればIDを返す
		/// </summary>
		/// <param name="name">表示される名前</param>
		/// <param name="parentFolderId">作成する位置</param>
		/// <param name="driveId">できれば指定</param>
		/// <returns>作成したフォルダのID</returns>
		public File CreateFolder(string name, string parentFolderId = null, string driveId = null)
		{
			string TAG = "CreateFolder";
			string dbMsg = "[GoogleDriveUtil]";
			string retStr = null;
			File newFolder = new File();
			try {
				dbMsg += "[drive:" + driveId + "][parent:" + parentFolderId + "]" + name;
				SearchFilter filter = SearchFilter.NONE;
				var queries = new List<string>() { $"name = '{ name }'" };
				if (filter != SearchFilter.NONE) queries.Add(filter.ToQuery());
				Task<File> folder = Task.Run(() => {
		//			return FindByName( name);
					return FindFile(queries);
				});

				folder.Wait();
				newFolder = folder.Result;
				string folderId = null;
				if (newFolder != null) {
					 folderId = newFolder.Id;
					dbMsg += ">既存>[" + folderId + "]";
				}
				if (folderId == null) {
					File meta = new File();
					meta.Name = name;
					meta.MimeType = GoogleDriveMime_Folder;
					if (driveId != null) meta.DriveId = driveId;
					meta.Parents = new List<string> { parentFolderId }; //特定のフォルダのサブフォルダ
	//				dbMsg += ",meta=" + meta.Parents[0];
					var request = Constant.MyDriveService.Files.Create(meta);
					request.Fields = "id, name,parents";
					dbMsg += ",request=" + request.MethodName;
					Task<File> rr = Task.Run(() => {
						return request.ExecuteAsync();
					});
					rr.Wait();			//
					dbMsg += ",IsCompleted=" + rr.IsCompleted;
					newFolder = rr.Result;
					retStr = newFolder.Id;
					dbMsg += ">作成>[";
				} else {
					retStr = folderId;
					dbMsg += ">既存>[";
				}
				dbMsg += retStr + "]" + newFolder.Name;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return newFolder;
		}

		/// <summary>
		/// ファイルを一つ登録し、作成したGooglrFileIDを返す
		/// </summary>
		/// <param name="fileName">単独のファイル名</param>
		/// <param name="filePath">PC上のフルパス</param>
		/// <param name="parentId">書込み先のフォルダID</param>
		/// <returns>作成したファイルのID</returns>
		public string UploadFile(string fileName, string filePath, string parentId)
		{
			string TAG = "UploadFile";
			string dbMsg = "[GoogleDriveUtil]";
			string retStr = null;
			try {
				//検索して同じファイルが無い事を確認
				File rParent = FindById(parentId);
				string parentName = rParent.Name;
				dbMsg += "[" + parentName + "]" + fileName + "(" + filePath + ")";
				Task<File> tFile = Task<string>.Run(() => {
					return FindByNameParent(fileName, parentName);
				});
				tFile.Wait();
				File targetF = tFile.Result;
				if (targetF != null) {
					string fileId = targetF.Id;
					dbMsg += ",削除[" + fileId + "]";
					Task<string> delItem = Task.Run(() => {
						return DelteItem(fileId);
					});
					delItem.Wait();
					dbMsg += ",消去=" + delItem.Result;
				}
				LocalFileUtil LFUtil = new LocalFileUtil();
				dbMsg += " ,parentId=" + parentId;
				dbMsg += " ,Name=" + System.IO.Path.GetFileName(filePath);
				String MimeStr = LFUtil.GetMimeType(filePath);
				dbMsg += " ,Mime=" + MimeStr;
				var meta = new File() {
					Name = System.IO.Path.GetFileName(filePath),
					MimeType = MimeStr,
					Parents = new List<string> { parentId }
				};
				//		FilesResource.CreateMediaUpload request;				//InsertMediaUpload は廃止
				FilesResource.CreateMediaUpload request;
				using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open)) {
					// Create:新規追加
					request = Constant.MyDriveService.Files.Create(meta, stream, MimeStr);
			//		request.Fields = "id, name";
					//					request.Fields = "id, name,etag,webcontentlink,iconlink";
					Task<Google.Apis.Upload.IUploadProgress> uploadProgress = Task.Run(() => {
						return request.Upload();
						//			return request.UploadAsync();
					});
					uploadProgress.Wait();
					IUploadProgress rProgress = uploadProgress.Result;      //		Status	Completed	Google.Apis.Upload.UploadStatus くらい
					dbMsg += " ,uploadProgress=" + rProgress.Status;
				}
				var rFile = request.ResponseBody;                           //作成結果が格納され戻される
				retStr = rFile.Id;
				dbMsg += ">作成したファイルID>" + retStr;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}

        /// <summary>
        /// 新規ファイル作成
        /// </summary>
        /// <param name="fileName">string</param>
        /// <param name="MimeStr">string　https://developers.google.com/apps-script/reference/base/mime-type　</param>
        /// <param name="parentId">string</param>
        /// <returns></returns>
        public string MakeFile(string fileName, string MimeStr, string parentId) {
            string TAG = "MakeFile";
            string dbMsg = "[GoogleDriveUtil]";
            string retStr = null;
            try {
                File rParent = FindById(parentId);
                string parentName = rParent.Name;
                dbMsg += "[" + parentName + "]" + fileName;
                dbMsg += " ,Mime=" + MimeStr;
                var meta = new File() {
                    Name = System.IO.Path.GetFileName(fileName),
                    MimeType = MimeStr,
                    Parents = new List<string> { parentId }
                };
                var request = Constant.MyDriveService.Files.Create(meta);
                request.Fields = "id, name,parents";
                dbMsg += ",request=" + request.MethodName;
                Task<File> rr = Task.Run(() => {
                    return request.ExecuteAsync();
                });
                rr.Wait();          //
                dbMsg += ",IsCompleted=" + rr.IsCompleted;
                File rFile = rr.Result;
                retStr = rFile.Id;
                dbMsg += ">作成>[";
                dbMsg += ">作成したファイルID>" + retStr;
                Util.MyLog(TAG, dbMsg);
            } catch (Exception er) {
                Util.MyErrorLog(TAG, dbMsg, er);
            }
            return retStr;
        }






        /// <summary>
        /// 指定されたIDのファイルを削除する
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<string> DelteItem(string fileId)
		{
			string TAG = "DelteItem";
			string dbMsg = "[GoogleUtil]";
			string retStr = null;
			try {
				dbMsg += fileId;
				dbMsg += fileId;
				Google.Apis.Drive.v3.FilesResource.DeleteRequest request = Constant.MyDriveService.Files.Delete(fileId);
				retStr = await request.ExecuteAsync();
				//Task<Google.Apis.Requests.ClientServiceRequest> csRequest = Task.Run(() => {
				//	return request.ExecuteAsync();
				//});
				//csRequest.Wait();
				//var rFile = request.ResponseBody;                           //作成結果が格納され戻される
				//retStr = rFile.Id;
				dbMsg += ">削除したファイルID>" + retStr;
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}

		/// <summary>
		/// 検索対象の名称と親フォルダの名称で検索
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parentName"></param>
		/// <param name="isFolder">検索対象の種類</param>
		/// <returns>Google.Apis.Drive.v3.Data.File</returns>
		public File FindByNameParent(string name, string parentName)
		{
			string TAG = "FindByNameParent";
			string dbMsg = "[GoogleUtil]";
			File retFile = null;
			try {
				dbMsg = "[" + parentName + "]" + name;
				IList<File> retList = GDFileListUp(parentName, false);
				if(retList != null) {
					dbMsg += ">該当＞" + retList.Count + "件";
					foreach (File rItem in retList) {
						string itemName = rItem.Name;
						string mimeType = rItem.MimeType;
						dbMsg += "\r\n" + itemName + ")" + mimeType;
						if (@itemName.Equals(@name)) {
							retFile = rItem;
							dbMsg += ">＞的中";
							break;
						}
					}
				}
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retFile;
		}

		/// <summary>
		/// フォルダもしくはファイル名称で検索してファイルIDを返す
		/// </summary>
		/// <param name="name"></param>
		/// <param name="filter"></param>
		/// <returns>Task<File></returns>
		public async Task<string> FindByName(string name, SearchFilter filter = SearchFilter.NONE)
		{
			string TAG = "FindByName";
			string dbMsg = "[GoogleUtil]";
			string retStr = null;
			File retFile = null;
			try {
				dbMsg = "name=" + name;
				var queries = new List<string>() { $"name = '{ name }'" };
				if (filter != SearchFilter.NONE) queries.Add(filter.ToQuery());
				retFile = await FindFile(queries);
				if(retFile != null) {
					retStr = retFile.Id;
				}
				dbMsg = ">>[" + retStr + "]";
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}


		/// <summary>
		/// idに該当するファイル・フォルダの情報を返す
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public File FindById(string id)
		{
			string TAG = "FindById";
			string dbMsg = "[GoogleDriveUtil]";
			string retStr = null;
			File retFile = null;
			try {
				dbMsg += "[id=" + id + "]";
				retFile = Constant.MyDriveService.Files.Get(id).Execute();

				//var queries = new List<string>() { $"id = '{ id }'" };           //	 , $"id = '{ fileId }'"
				//Task<File> rr = Task<File>.Run(() => {
				//	//SearchFilter filter = SearchFilter.NONE;
				//	//queries.Add(filter.ToQuery());
				//	return FindFile(queries);
				//});
				//rr.Wait();
				//if (rr != null) {
				//	retFile = rr.Result;
				//}

					retStr = retFile.Name;
				dbMsg += retStr;
				dbMsg += ">>[" + retStr + "]";
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return retFile;
		}

		/// <summary>
		/// ファイルを一件検索する
		/// </summary>
		/// <param name="queries"></param>
		/// <returns></returns>
		public static async Task<File> FindFile(List<string> queries)
		{
			string TAG = "FindFile";
			string dbMsg = "[GoogleUtil]";
			File newFolder = new File();
			try {
				Constant.MyDriveService = new DriveService(new BaseClientService.Initializer() {
					HttpClientInitializer = Constant.MyDriveCredential,
					ApplicationName = Constant.ApplicationName,
				});

				dbMsg += "queries=" + queries.ToString();
				var result = await FindFilesCore(queries);
				MyLog(TAG, dbMsg);
				return (result.Count > 0) ? result[0] : null;
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
				return null; 
			}
		}

		/// <summary>
		/// リストで渡されたファイルを検索する
		/// </summary>
		/// <param name="queries"></param>
		/// <param name="fileFields"></param>
		/// <returns>List<File></returns>
		public static async Task<List<File>> FindFiles(List<string> queries, string fileFields)
		{
			string TAG = "FindFiles";
			string dbMsg = "[GoogleUtil]";
			try {
				dbMsg = "queries=" + queries.ToString();
				dbMsg = "fileFields=" + fileFields;
				MyLog(TAG, dbMsg);
				return await FindFilesCore(queries, $"nextPageToken, files({fileFields})");
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
				return null;
			}
		}

		/// <summary>
		/// ファイル検索の本体
		/// </summary>
		/// <param name="queries"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		static async Task<List<File>> FindFilesCore(List<string> queries, string fields = "nextPageToken, files(id, name)")
		{
			string TAG = "FindFilesCore";
			string dbMsg = "[GoogleUtil]";
			try {
				dbMsg += "queries=" + queries.ToString();
				dbMsg += "fields=" + fields;
				string nextPageToken = null;
				queries.Add("trashed = false");

				do {
					FilesResource.ListRequest request = Constant.MyDriveService.Files.List();
					request.PageToken = nextPageToken;
					request.Q = queries.ToQuery();
					request.Fields = fields;
					var result = await request.ExecuteAsync();
					if (result.Files.Count > 0) return result.Files.ToList();
					nextPageToken = result.NextPageToken;
				} while (!string.IsNullOrEmpty(nextPageToken));
				MyLog(TAG, dbMsg);
				return new List<File>();
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
				return new List<File>();
			}

		}
		////////////////////////////////////////////////////	Waitの使い方？
		/// <summary>
		/// このアプリ用フォルダの作成
		/// </summary>
		/// <param name="pcFilePath"></param>
		/// <param name="rootFolderName"></param>
		/// <returns></returns>
		public string MakeMyeGoogleFolder()
		{
			string TAG = "MakeMyeGoogleFolder";
			string dbMsg = "[GoogleDriveUtil]";
			string rootFolderId = "";
			try {
				Constant.GDriveFiles = new List<Google.Apis.Drive.v3.Data.File>();
				Constant.GDriveFiles = GDAllFolderListUp();
				int fCount = Constant.GDriveFiles.Count;
				dbMsg += " , " + fCount + " フォルダ登録";
				if (0 < fCount) {
					dbMsg += "[" + Constant.GDriveFiles.First().Id + " ]" + Constant.GDriveFiles.First().Name + " ;Parents;" + Constant.GDriveFiles.First().Parents[0] + " ;DriveId;" + Constant.GDriveFiles.First().DriveId;
					dbMsg += "～[" + Constant.GDriveFiles.Last().Id + " ]" + Constant.GDriveFiles.Last().Name + " ;Parents;" + Constant.GDriveFiles.First().Parents[0] + " ;DriveId;" + Constant.GDriveFiles.First().DriveId;
				}
				//仮処理；最初に拾えたファイルのParentsをdriveIdとする
				Constant.DriveId = Constant.GDriveFiles.First().Parents[0];
				dbMsg += ">>" + Constant.DriveId + "にrootFolder:　"+ Constant.RootFolderName  + "　を作成";
                Task<File> rr = Task<File>.Run(() => {
					return CreateFolder(Constant.RootFolderName, Constant.DriveId);
				});
				rr.Wait();
				if (rr == null) {
					dbMsg += ">>失敗";
					Util.MyLog(TAG, dbMsg);
					return null;
				}
				Constant.RootFolderID = rr.Result.Id;
                dbMsg += "[" + Constant.RootFolderID + "]" + rr.Result.Name;    // + "("+ rr.Result.Parents. + ")";
				Constant.RootFolderURL = "https://drive.google.com/drive/folders/" + Constant.RootFolderID; 
				dbMsg +=  ". " + Constant.RootFolderURL;
				//dbMsg += "[" + Constant.AriadneEventNames.Count() + "件";

				////案件、工程、一般　のAriadneEventフォルダ作成
				//foreach (string topFolderName in Constant.AriadneEventNames) {
				//	string topFolderId = "";
				//	rr = Task.Run(() => {
				//		return CreateFolder(topFolderName, Constant.RootFolderID);
				//	});
				//	rr.Wait();
				//	if (rr == null) {
				//		dbMsg += ">>失敗";
				//		Util.MyLog(TAG, dbMsg);
				//	}
				//	topFolderId = rr.Result.Id;
				//	dbMsg += "[" + topFolderId + "]" + topFolderName;
				//	if(topFolderName.Equals(Constant.AriadneEventAnken)){
				//		Constant.AriadneAnkenFolderId = topFolderId;
				//		dbMsg += "[案件;" + Constant.AriadneAnkenFolderId + "]" ;
				//	} else if (topFolderName.Equals(Constant.AriadneEventKoutei)) {
				//		Constant.AriadneKouteiFolderId = topFolderId;
				//		dbMsg += "[工程;" + Constant.AriadneKouteiFolderId + "]";
				//	} else if (topFolderName.Equals(Constant.AriadneEventOther)) {
				//		Constant.AriadneOtherFolderId = topFolderId;
				//		dbMsg += "[一般;" + Constant.AriadneOtherFolderId + "]";
				//	}
				//	dbMsg += topFolderName;
				//}
				
				Util.MyLog(TAG, dbMsg);
			} catch (Exception er) {
				Util.MyErrorLog(TAG, dbMsg, er);
			}
			return Constant.RootFolderID;
		}

		/// <summary>
		/// Ariadnイベントのデータ登録定義に即したデータ登録
		/// </summary>
		/// <param name="pcFilePath">作成する案件名フォルダ名</param>
		/// <param name="parentoFolderId">Event相当の親フォルダId</param>
		/// <returns></returns>
		//public string AriadneDataPut(string pcFilePath, string parentoFolderId, string parentoFolderName)
		//{
		//	string TAG = "AriadneDataPut";
		//	string dbMsg = "[GoogleDriveUtil]";
		//	string retFileID = null;
		//	try {
		//		dbMsg += " , " + pcFilePath;
		//		string[] strs = pcFilePath.Split('\\');
		//		string targetFileName = strs[strs.Length - 1];
		//		dbMsg += ",name=" + targetFileName;
		//		string parentName = strs[strs.Length - 2];
		//		dbMsg += ",parent=" + parentName;
		//		//Ariadnイベントのフォルダの下にフォルダが必要な場合（見積もりなど）
		//		if (!parentName.Equals(parentoFolderName)) {
		//			File parentFolder = FindByNameParent(parentName, parentoFolderName);
		//			if (parentFolder == null) {
		//				dbMsg += ">>targetFolder作成";
		//				Task<string> rr = CreateFolder(parentName, parentoFolderId);
		//				if (rr == null) {
		//					return null;
		//				} else {
		//					parentoFolderId = rr.Result;
		//				}
		//			} else {
		//				parentoFolderId = parentFolder.Id;
		//			}
		//		}

		//		dbMsg += " ,[" + parentoFolderId + "]" + parentoFolderName;
		//		File gFile = FindByNameParent(targetFileName, parentoFolderName);
		//		if (gFile == null) {
		//			dbMsg += ">>targetFile作成";
		//			retFileID = UploadFile(targetFileName, pcFilePath, parentoFolderId);
		//		}else{
		//			retFileID = gFile.Id;
		//		}
		//		dbMsg += " ,[" + retFileID + "]" + targetFileName;
		//		Util.MyLog(TAG, dbMsg);
		//	} catch (Exception er) {
		//		Util.MyErrorLog(TAG, dbMsg, er);
		//	}
		//	return retFileID;
		//}

		////////////////////////////////////////////////////
		public static void MyLog(string TAG, string dbMsg)
		{
			CS_Util Util = new CS_Util();
			Util.MyLog(TAG, dbMsg);
		}

		public static void MyErrorLog(string TAG, string dbMsg, Exception err)
		{
			CS_Util Util = new CS_Util();
			Util.MyErrorLog(TAG, dbMsg, err);
		}
	}


	////////////////////////////////////////////////////
	//	https://github.com/iwatuki/JuscoBot から
	public enum SearchFilter {
		/// <summary>フィルターなし</summary>
		NONE,
		/// <summary>フォルダのみ</summary>
		FOLDER,
		/// <summary>ファイルのみ</summary>
		FILE,
	}

	internal static class GoogleDriveQueryExt {
		public const string IGNORE_TRASH = "trashed = false";

		static List<string> queries = new List<string> {
			null,
			"mimeType = 'application/vnd.google-apps.folder'", // フォルダのみ
			"mimeType != 'application/vnd.google-apps.folder'", // ファイルのみ
		};

		public static string ToQuery(this SearchFilter type)
		{
			return queries[(int)type];
		}

		public static string ToQuery(this List<string> queries)
		{
			//while(queries.Contains("")) queries.Remove("");
			//while (queries.Contains(null)) queries.Remove(null);
			return string.Join(" and ", queries);
		}
	}
}

/*
Google Drive APIv3リファレンス	 https://developers.google.com/drive/api/v3/reference/files
https://www.apps-gcp.com/google-deive-api-v2-to-v3-4points/
 */
