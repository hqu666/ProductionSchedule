using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// GoogleDriveに転送するファイル
/// </summary>
namespace ProductionSchedule.Models  {
	public class GAttachFile {
		public string fullPass;
		public string gFileId;

		public string name;
		public string parent;
		public bool isFolder;

		public GAttachFile(string fullPass, string gFileId, string name, string parent, bool isFolder)
		{
			this.fullPass = fullPass;
			this.gFileId = gFileId;
			this.name = name;
			this.parent = parent;
			this.isFolder = isFolder;
		}
	}
}
