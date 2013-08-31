using AgiliSway9.WPF.Models;
using AgiliSway9.WPF.Models.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Services
{
	public interface IFileStorage
	{
		string GenerateFilePath(Study study, Subject subject, string fileTag);

		CollectionDataSesssion LoadCollectionDataSesssionFile(string filePath);
		SamplingSession LoadSamplingSessionFile(string filePath);

		void SaveFile(CollectionDataSesssion dataSet, string filePath);
	}
}
