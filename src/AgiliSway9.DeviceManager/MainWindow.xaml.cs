using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AgiliSway9.BluetoothHelper;

namespace AgiliSway9.DeviceManager
{
	public partial class MainWindow : Window
	{
		BluetoothManager _btHelper = new BluetoothManager();

		public MainWindow()
		{
			InitializeComponent();

			Devices = new ObservableCollection<Device>();

			this.DataContext = this;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Devices.Clear();
			var radios = _btHelper.GetRadios();
			foreach (var radio in radios)
			{
				List<BluetoothDevice> devices = _btHelper.DiscoverDevices(radio);
				foreach (var device in devices)
				{
					var dev = new Device() { Name = device.Name, BluetoothRadio = radio, BluetoothDevice = device };
					Devices.Add(dev);
				}
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (SelectedDevice != null)
			{
				if (SelectedDevice.BluetoothDevice.Authenticated)
					MessageBox.Show("Device already paired");
				if (SelectedDevice.Name == "Nintendo RVL-WBC-01" || String.IsNullOrEmpty(SelectedDevice.Name))
				{
					try
					{
						_btHelper.PairWithDevice(SelectedDevice.BluetoothRadio, SelectedDevice.BluetoothDevice, SelectedDevice.BluetoothRadio.Address.ToArray());
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), "Pairing Error");
					}
				}
				else
					MessageBox.Show("Please select a Will Balance Board.");
			}
			else
				MessageBox.Show("Please Select A Device.");
		}

		public ObservableCollection<Device> Devices { get; set; }
		public Device SelectedDevice { get; set; }
	}

	public class Radio
	{
		public string Name {get; set;}
	}

	public class Device
	{
		public string Name { get; set; }
		public BluetoothRadio BluetoothRadio { get; set; }
		public BluetoothDevice BluetoothDevice { get; set; }
	}
}
