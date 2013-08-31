using System;
using AgiliSway9.WPF.Events;
using Caliburn.Micro;

namespace AgiliSway9.WPF.Common
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
