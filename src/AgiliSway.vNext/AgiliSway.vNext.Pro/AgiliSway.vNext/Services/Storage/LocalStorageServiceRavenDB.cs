using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgiliSway.vNext.Models;

namespace AgiliSway.vNext.Services.Storage
{
    public class LocalStorageServiceRavenDB //: ILocalStorage
    {
		//private EmbeddableDocumentStore _eds;
		//private IDocumentSession _sess;

		//public LocalStorageServiceRavenDB()
		//{
		//	_eds = new EmbeddableDocumentStore() { DataDirectory = "Data" };
		//	_eds.Initialize();
		//	_sess = _eds.OpenSession();
		//}

		//#region Study Methods
		//public Study[] LoadStudies()
		//{
		//	return _sess.Query<Study>().ToArray();
		//}

		//public Study NewStudy()
		//{
		//	var study = new Study() { Title = "New Study" };

		//	_sess.Store(study);

		//	return study;
		//}

		//public void DeleteStudy(Study study)
		//{
		//	_sess.Delete<Study>(study);
		//}
		//#endregion

		//#region Subject Methods
		//public Subject[] LoadSubjects()
		//{
		//	return _sess.Query<Subject>().ToArray();
		//}

		//public Subject[] LoadSubjects(Study study)
		//{
		//	var subjects = from sub in _sess.Query<Subject>()
		//				   where sub.Study.StudyId == study.StudyId
		//				   select sub;

		//	return subjects.ToArray();
		//}

		//public Subject NewSubject(Study study)
		//{
		//	var subj = new Subject() { FirstName = "New", LastName = "Subject", Study = study };

		//	_sess.Store(subj);

		//	return subj;
		//}

		//public void DeleteSubject(Subject subj)
		//{
		//	_sess.Delete<Subject>(subj);
		//}
		//#endregion

		//#region Group Methods
		//public Group NewGroup(Study study)
		//{
		//	var group = new Group(){ StudyId = study.Id };

		//	_sess.Store(group);

		//	return group;
		//}

		//public Group[] LoadGroups(Study study)
		//{
		//	var groups = from grp in _sess.Query<Group>()
		//				 where grp.StudyId == study.Id
		//				 select grp;

		//	return groups.ToArray();
		//}

		//public void DeleteGroup(Group grp)
		//{
		//	_sess.Delete<Group>(grp);
		//}
		//#endregion

		//#region Task Methods
		//public Task NewTask(Study study)
		//{
		//	var tsk = new Task() { StudyId = study.Id };

		//	_sess.Store(tsk);

		//	return tsk;
		//}

		//public Task[] LoadTasks(Study study)
		//{
		//	var tsks = from tsk in _sess.Query<Task>()
		//				 where tsk.StudyId == study.Id
		//				 select tsk;

		//	return tsks.ToArray();
		//}

		//public void DeleteTask(Task tsk)
		//{
		//	_sess.Delete<Task>(tsk);
		//}
		//#endregion

		//#region Common Methods
		//public void Save()
		//{
		//	_sess.SaveChanges();
		//}

		//public void Close()
		//{
		//	_sess.Dispose();
		//}
		//#endregion


		//public Collection NewCollection(Subject subject)
		//{
		//	throw new NotImplementedException();
		//}

		//public Collection[] LoadCollections(Subject subject)
		//{
		//	throw new NotImplementedException();
		//}

		//public void DeleteCollection(Collection collection)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
