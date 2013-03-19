using AgiliSway.vNext.Models;
using AgiliSway.vNext.Models.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Services
{
	public interface IFileStorage
	{
		string GenerateFilePath(Study study, Subject subject, string fileTag);

		CollectionDataSesssion LoadCollectionDataSesssionFile(string filePath);
		SamplingSession LoadSamplingSessionFile(string filePath);

		void SaveFile(CollectionDataSesssion dataSet, string filePath);
	}
}
