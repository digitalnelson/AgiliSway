using AgiliSway.vNext.Services;
using AgiliSway.vNext.Services.Devices;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway.vNext.Sections.Device
{
	public class WBBViewModel : DeviceConfigScreen
	{
		public WBBViewModel()
		{
			this.DisplayName = "WBB";
			this.DeviceType = DeviceTypes.WBB;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			//WBBLocator.All();
		}

		public BindableCollection<WBBDefViewModel> Boards { get { return _inlBoards; } set { _inlBoards = value; NotifyOfPropertyChange(() => Boards); } } private BindableCollection<WBBDefViewModel> _inlBoards;
	}
}
