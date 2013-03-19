using AgiliSway.vNext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway.vNext.Services
{
	public interface IDbStorage
	{
		Study[] LoadStudies();
		Study NewStudy();
		void DeleteStudy(Study study);

		Subject[] LoadSubjects();
		Subject[] LoadSubjects(Study study);
		Subject NewSubject(Study study);
		void DeleteSubject(Subject subj);

		Group NewGroup(Study study);
		Group[] LoadGroups(Study study);
		void DeleteGroup(Group grp);

		Task NewTask(Study study);
		Task[] LoadTasks(Study study);
		void DeleteTask(Task tsk);

		Collection NewCollection(Subject subject);
		Collection[] LoadCollections();
		Collection[] LoadCollections(Study study);
		Collection[] LoadCollections(Subject subject);
		void DeleteCollection(Collection collection);

		void Save();
		void Close();
	}
}
