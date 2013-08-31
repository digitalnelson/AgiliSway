using AgiliSway9.WPF.Events;
using AgiliSway9.WPF.Services;
using AgiliSway9.WPF.Services.Devices;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway9.WPF.Sections.Device
{
	public class WBBViewModel : DeviceConfigScreen, IHandle<WiiBBSearchEvent>, IHandle
	{
		private new IEventAggregator _eventAggregator;
		public new IDeviceManager _deviceManager;

		public WBBViewModel()
		{
			this.DisplayName = "WBB";
			this.DeviceType = DeviceTypes.WBB;

			this._eventAggregator = IoC.Get<IEventAggregator>();
			this._deviceManager = IoC.Get<IDeviceManager>();
			this._eventAggregator.Subscribe((object)this);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			//WBBLocator.All();
		}

		public override void CanClose(System.Action<bool> callback)
		{
			if (this._deviceManager != null && this._deviceManager.CurrentConnector != null && this._deviceManager.CurrentConnector.CurrentDevice != null)
				this._deviceManager.CurrentConnector.CurrentDevice.Disconnect();
			base.CanClose(callback);
		}

		public void Handle(WiiBBSearchEvent message)
		{
			this.SearchMessage = message.Message;
		}

		public void Reconnect()
		{
			this._deviceManager.CurrentConnector.CurrentDevice = (IDevice)null;
		}

		public string SearchMessage { get { return this._inlSearchMessage; } set { this._inlSearchMessage = value; this.NotifyOfPropertyChange(() => this.SearchMessage); } } private string _inlSearchMessage; 
		public BindableCollection<WBBDefViewModel> Boards { get { return _inlBoards; } set { _inlBoards = value; NotifyOfPropertyChange(() => Boards); } } private BindableCollection<WBBDefViewModel> _inlBoards;
	}
}
