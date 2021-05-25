using System;
using System.Collections.Generic;

namespace ProductionSchedule.Models {
	/// <summary>
	/// GoogleのOAuthから取得したJSON
	/// </summary>
	public class GOAuthModel {
		/// <summary>
		/// "912719822179-n9hvcs7tr9pqgn8mns7pdl5njo54gpe1.apps.googleusercontent.com",
		/// </summary>
		public String client_id;

		/// <summary>
		/// "aGVZ_mfTKJq8WFf5spDOOiHi",		/// </summary>
		public String client_secret;

		/// <summary>
		///  "kyokuto4",
		/// </summary>
		public String project_id;

		/// <summary>
		///  "https://accounts.google.com/o/oauth2/auth",
		/// </summary>
		public String auth_uri;

		/// <summary>
		/// "https://oauth2.googleapis.com/token",
		/// </summary>
		public String token_uri;

		/// <summary>
		///  "https://www.googleapis.com/oauth2/v1/certs",
		/// </summary>
		public String auth_provider_x509_cert_url;

		private IDictionary<string, object> ivalue1;        //機構なので消せない？
		public IDictionary<string, object> installed {
			get {
				return ivalue1;		//機構なので消せない？
				/*
				 IDictionary<string, Menbers>などModelの場合
				  Error converting value "912719822179-n9hvcs7tr9pqgn8mns7pdl5njo54gpe1.apps.googleusercontent.com" to type 'GoogleOSD.Menbers'. Path 'installed.client_id', line 3, position 91.			
				 ＞＞ /JsonConvert.DeserializeObjectエラー   https://stackoverrun.com/ja/q/11520492
				 List< Menbers>の場合
				 Cannot deserialize the current JSON object (e.g. {"name":"value"}) into type 'System.Collections.Generic.List`1[GoogleOSD.Menbers]' because the type requires a JSON array (e.g. [1,2,3]) to deserialize correctly.
To fix this error either change the JSON to a JSON array (e.g. [1,2,3]) or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.
Path 'installed.client_id', line 3, position 16.
				 */
			}
			set {
				ivalue1 = value;        //機構なので消せない？
				foreach (var item in value) {
					switch (item.Key) {
						case "client_id":
							client_id = item.Value.ToString();
							break;
						case "client_secret":
							client_secret = item.Value.ToString();
							break;
						case "project_id":
							project_id = item.Value.ToString();
							break;
						case "auth_uri":
							auth_uri = item.Value.ToString();
							break;
						case "token_uri":
							token_uri = item.Value.ToString();
							break;
						case "auth_provider_x509_cert_url":
							auth_provider_x509_cert_url = item.Value.ToString();
							break;
					}
				}
			}
		}
	}
}
