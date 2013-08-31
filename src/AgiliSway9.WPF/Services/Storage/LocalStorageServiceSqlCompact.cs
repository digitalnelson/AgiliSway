using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AgiliSway9.WPF.Models;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Services.Storage
{
	public class LocalStorageServiceSqlCompact : IDbStorage
	{
		private readonly LocalStorageContext _context;

		public LocalStorageServiceSqlCompact()
		{
			var prefs = IoC.Get<IAppPreferences>();

			Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0", prefs.DataStorePath, "");
			Database.SetInitializer(new LocalStorageContextInitializer());

			_context = new LocalStorageContext();
		}

		// STUDIES

		public Study[] LoadStudies()
		{	
			return _context.Studies.ToArray();
		}

		public Study NewStudy()
		{
			return _context.Studies.Add(new Study() { Title = "New Study" });
		}

		public void DeleteStudy(Study study)
		{
			_context.Studies.Remove(study);
		}

		// SUBJECTS

		public Subject[] LoadSubjects()
		{
			return _context.Subjects.ToArray();
		}

		public Subject[] LoadSubjects(Study study)
		{
			return study.Subjects.ToArray();
		}

		public Subject NewSubject(Study study)
		{
			return _context.Subjects.Add(new Subject { Study = study, Birthdate = "" });
		}

		public void DeleteSubject(Subject subj)
		{
			_context.Subjects.Remove(subj);
		}

		// GROUPS

		public Group NewGroup(Study study)
		{
			return _context.Groups.Add(new Group { Study = study });
		}

		public Group[] LoadGroups(Study study)
		{
			return study.Groups.ToArray();
		}

		public void DeleteGroup(Group group)
		{
			_context.Groups.Remove(group);
		}

		// TASKS

		public Task NewTask(Study study)
		{
			return _context.Tasks.Add(new Task { Study = study });
		}

		public Task[] LoadTasks(Study study)
		{
			return study.Tasks.ToArray();
		}

		public void DeleteTask(Task tsk)
		{
			_context.Tasks.Remove(tsk);
		}

		// COLLECTIONS

		public Collection NewCollection(Subject subject)
		{
			return _context.Collections.Add(new Collection { Subject = subject, Timestamp = DateTime.UtcNow });
		}

		public Collection[] LoadCollections()
		{
			return _context.Collections.ToArray();
		}

		public Collection[] LoadCollections(Study study)
		{
			return _context.Collections.Where(c => c.Subject.StudyId == study.StudyId).ToArray();
		}

		public Collection[] LoadCollections(Subject subject)
		{
			return subject.Collections.Where(c => c.Subject == subject).ToArray();
		}

		public void DeleteCollection(Collection collection)
		{
			_context.Collections.Remove(collection);
		}

		// COMMON

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Close()
		{
			_context.Dispose();
		}
	}
}
