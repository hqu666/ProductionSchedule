using System;
using System.IO;
using System.Linq;

namespace ProductionSchedule
{
	class LocalFileUtil {
		///// <summary>
		///// OpenFileDialogで選択されたファイルのある全ファイル名を返す
		///// </summary>
		///// <param name="ofDialog"></param>
		//public void ListUpFolderFiles(System.Windows.Forms.OpenFileDialog ofDialog)
		//{
		//	string TAG = "Serect_bt_Click";
		//	string dbMsg = "[LocalFileUtil]";
		//	try {
		//		String FolderPath = System.IO.Path.GetDirectoryName(ofDialog.FileName);
		//		System.IO.DirectoryInfo dirInfo = System.IO.Directory.GetParent(ofDialog.FileName);
		//		Constant.MakeFolderName = dirInfo.Name;
		//		Constant.selectFiles = Directory.GetFiles(@FolderPath, "*");
		//		dbMsg += Constant.selectFiles.Count() + "件";
		//		MyLog(TAG, dbMsg);
		//	} catch (Exception er) {
		//		MyErrorLog(TAG, dbMsg + "でエラー発生;" + er);
		//	}
		//}

		/// <summary>
		/// rootName以下で一つ上のフォルダ名を返す
		/// フルパス名はConstantに記録する
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="rootName"></param>
		/// <returns>フォルダ名</returns>
		public string GetPearentPass(string fileName, string rootName)
		{
			string TAG = "GetPearentPass";
			string dbMsg = "[LocalFileUtil]";
			string retStr = "";
			dbMsg += "," + fileName;
			try {
				Constant.LocalPass = System.IO.Path.GetDirectoryName(fileName);     //
				dbMsg += ",一つ上のフォルダ名=" + Constant.LocalPass;
				string[] delimiter = { rootName };
				string[] strs = Constant.LocalPass.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
				retStr = strs[1];
				retStr = retStr.Substring(1, retStr.Length - 1);
				dbMsg += ">>" + retStr;
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}

		/// <summary>
		/// パスを抜いてファイル名を返す
		/// </summary>
		/// <param name="fileName">フルパスファイル名</param>
		/// <returns>パスを除去したファイル名</returns>
		public string GetFileNameExt(string fileName)
		{
			string TAG = "GetFileName";
			string dbMsg = "[LocalFileUtil]";
			dbMsg += "," + fileName;
			string retStr = "";
			try {
				retStr = System.IO.Path.GetFileName(fileName);
				dbMsg += ">>" + retStr;
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
			return retStr;
		}

		/// <summary>
		/// Mimeを返す
		/// </summary>
		/// <param name="fileName">調べるファイルのフルパス名</param>
		/// <returns>Mime</returns>
		public string GetMimeType(string fileName)
		{
			string TAG = "Serect_bt_Click";
			string dbMsg = "[LocalFileUtil]";
			string mimeType = "application/unknown";
			try {
				dbMsg += "," + fileName;
				string ext = System.IO.Path.GetExtension(fileName).ToLower();
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
				if (regKey != null && regKey.GetValue("Content Type") != null)
					mimeType = regKey.GetValue("Content Type").ToString();
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
			return mimeType;
		}

		/// <summary>
		/// 指定したファイルがフォルダならtrue
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public bool IsDirectory(string fileName)
		{
			string TAG = "IsFile";
			string dbMsg = "[LocalFileUtil]";
			dbMsg += "," + fileName;
			bool retBool = false;
			try {
				retBool = File.GetAttributes(fileName).HasFlag(FileAttributes.Directory);
				dbMsg += ">>" + retBool;
				MyLog(TAG, dbMsg);
			} catch (Exception er) {
				MyErrorLog(TAG, dbMsg, er);
			}
			return retBool;
		}

		////////////////////////////////////////////////////
		public void MyLog(string TAG, string dbMsg)
		{
			CS_Util Util = new CS_Util();
			Util.MyLog(TAG, dbMsg);
		}

		public void MyErrorLog(string TAG, string dbMsg, Exception err)
		{
			CS_Util Util = new CS_Util();
			Util.MyErrorLog(TAG, dbMsg, err);
		}

	}
}