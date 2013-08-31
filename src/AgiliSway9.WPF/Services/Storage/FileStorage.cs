using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgiliSway9.WPF.Models;
using Ninject;
using AgiliSway9.WPF.Models.Legacy;
using System.Xml.Serialization;

namespace AgiliSway9.WPF.Services.Storage
{
	public class FileStorageLocal : IFileStorage
	{
		private readonly IAppPreferences _appPreferences;

		[Inject]
		public FileStorageLocal(IAppPreferences appPreferences)
		{
			_appPreferences = appPreferences;
		}

		public string GenerateFilePath(Study study, Subject subject, string fileTag)
		{
			var dataFolder = _appPreferences.DataStorePath;
			var stu = GenerateStudyPart(study);
			var sub = GenerateSubjectPart(subject);

			var folderPath = string.Format("{0}\\{1}\\{2}", dataFolder, stu, sub);
			var fileName = string.Format("{2}_{3}_{0}_{1}.xml", fileTag, GenerateGUID(), stu, sub);
			var fullPath = Path.Combine(folderPath, fileName);

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			return fullPath;
		}

		private string GenerateStudyPart(Study study)
		{
			return "Study" + study.StudyId;
		}

		private string GenerateSubjectPart(Subject subject)
		{
			return "Subject" + subject.SubjectId;
		}

		private string GenerateGUID()
		{
			return Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
		}

		public SamplingSession LoadSamplingSessionFile(string filePath)
		{
			try
			{
				using (var sw = new StreamReader(filePath))
				{
					// TODO: Detect different file formats here

					var ser = new XmlSerializer(typeof(SamplingSession));
					return (SamplingSession)ser.Deserialize(sw);
				}
			}
			catch (Exception)
			{ }

			return null;
		}

		public CollectionDataSesssion LoadCollectionDataSesssionFile(string filePath)
		{
			try
			{
				using (var sw = new StreamReader(filePath))
				{
					// TODO: Detect different file formats here

					var ser = new XmlSerializer(typeof(CollectionDataSesssion));
					return (CollectionDataSesssion)ser.Deserialize(sw);
				}
			}
			catch (Exception)
			{ }

			return null;
		}

		public void SaveFile(CollectionDataSesssion dataSet, string filePath)
		{
			try
			{
				using (var sw = new StreamWriter(filePath))
				{
					var ser = new XmlSerializer(typeof(CollectionDataSesssion));
					ser.Serialize(sw, dataSet);
				}
			}
			catch (Exception)
			{ }
		}

	}
}
