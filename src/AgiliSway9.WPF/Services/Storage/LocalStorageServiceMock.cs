using System;
using System.Collections.Generic;
using System.Linq;
using AgiliSway9.WPF.Models;

namespace AgiliSway9.WPF.Services.Storage
{
	public class LocalStorageServiceMock //: ILocalStorage
	{
		//private List<Study> _studies;
		//private List<Subject> _subjects;

		//public LocalStorageServiceMock()
		//{


		//	_studies = new List<Study>()
		//	{
		//		new Study() { 
		//			StudyId = 1, 
		//			Title = "SZ_169" ,
		//			Groups = new List<Group>(){
		//				new Group(){ GroupId = 1, Title = "Controls" },
		//				new Group(){ GroupId = 2, Title = "Probands" },
		//			},
		//			Tasks = new List<Task>(){
		//				new Task(){ TaskId = 1, Title = "Eyes Closed FA", Duration = 60 },
		//				new Task(){ TaskId = 2, Title = "Inspection FA", Duration = 60 },
		//				new Task(){ TaskId = 3, Title = "Search FA", Duration = 60 },
		//			},
		//		},
		//	};

		//	_subjects = new List<Subject>()
		//	{
		//		new Subject() { SubjectId = 1, CustomId = "100", FirstName = "Subject", LastName = "One", Birthdate = new DateTime(1980, 1, 1) },
		//		new Subject() { SubjectId = 2, CustomId = "101", FirstName = "Subject", LastName = "Two", Birthdate = new DateTime(1981, 1, 1) },
		//	};
		//}

		//public Study[] LoadStudies()
		//{
		//	return _studies.ToArray();
		//}

		//public Study NewStudy()
		//{
		//	var study = new Study() { Title = "New Study" };

		//	_studies.Add(study);

		//	return study;
		//}

		//public void DeleteStudy(Study study)
		//{
		//	_studies.Remove(study);
		//}

		//public Subject[] LoadSubjects()
		//{
		//	List<Subject> subjects = new List<Subject>();

		//	foreach (var study in _studies)
		//		subjects.AddRange(study.Subjects);

		//	return subjects.ToArray();
		//}

		//public Subject[] LoadSubjects(Study study)
		//{
		//	return study.Subjects.ToArray();
		//}

		//public Subject NewSubject(Study study)
		//{
		//	var subj = new Subject() { FirstName = "New", LastName = "Subject", Study = study };

		//	study.Subjects.Add(subj);

		//	return subj;
		//}

		//public void DeleteSubject(Subject subj)
		//{
		//	//_sess.Delete<Subject>(subj);
		//}

		//public Group NewGroup(Study study)
		//{
		//	var group = new Group() { Study = study };

		//	study.Groups.Add(group);

		//	return group;
		//}

		//public Group[] LoadGroups()
		//{
		//	List<Group> groups = new List<Group>();

		//	foreach (var study in _studies)
		//		groups.AddRange(study.Groups);

		//	return groups.ToArray();
		//}

		//public Group[] LoadGroups(Study study)
		//{
		//	return study.Groups.ToArray();
		//}

		//public void DeleteGroup(Group grp)
		//{
		//	//_sess.Delete<Group>(grp);
		//}

		//public Task NewTask(Study study)
		//{
		//	var tsk = new Task() { Study = study };

		//	study.Tasks.Add(tsk);

		//	return tsk;
		//}

		//public Task[] LoadTasks(Study study)
		//{
		//	//var tsks = from tsk in _context.Tasks
		//	//			 where tsk.StudyId == study.Id
		//	//			 select tsk;

		//	return study.Tasks.ToArray();
		//}

		//public void DeleteTask(Task tsk)
		//{
		//	//_sess.Delete<Task>(tsk);
		//}

		//public Collection NewCollection(Subject subject)
		//{
		//	var collection = new Collection() { Subject = subject };

		//	subject.Collections.Add(collection);

		//	return collection;
		//}

		//public Collection[] LoadCollections(Subject subject)
		//{
		//	//var collections = from collection in _context.Collections
		//	//		   where collection.SubjectId == subject.Id
		//	//		   select collection;

		//	return subject.Collections.ToArray();
		//}

		//public void DeleteCollection(Collection collection)
		//{
		//	//_sess.Delete<Collection>(collection);
		//}

		//public void Save()
		//{}

		//public void Close()
		//{}
	}
}
