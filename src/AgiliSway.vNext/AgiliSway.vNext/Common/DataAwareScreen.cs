using System;
using AgiliSway.vNext.Event;
using Caliburn.Micro;

namespace AgiliSway.vNext.Common
{
	public class DataAwareScreen : Screen
	{
		#region Service Injection
		protected readonly IEventAggregator _eventAggregator = IoC.Get<IEventAggregator>();
		#endregion

		public DataAwareScreen()
		{}

		public void DataChanged()
		{
			_eventAggregator.Publish(new DataChangedEvent { });
		}
	}
}
