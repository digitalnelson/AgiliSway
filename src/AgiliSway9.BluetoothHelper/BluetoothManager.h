#pragma once

#include <windows.h>
#include <bthsdpdef.h>
#include <bthdef.h>
#include <BluetoothAPIs.h>
#include <strsafe.h>

#pragma comment(lib, "Bthprops.lib")

using namespace System;
using namespace System::Collections::Generic;

namespace AgiliSway9 { namespace BluetoothHelper
{
	public ref class BluetoothRadio
	{
	public:
		HANDLE RadioHandle;
		String^ Name;
		List<Byte>^ Address;
		BLUETOOTH_RADIO_INFO *Info;

		~BluetoothRadio()
		{
			if(Info)
				delete Info;
		}
	};

	public ref class BluetoothDevice
	{
	public:
		String^ Name;
		List<Byte>^ Address;
		BLUETOOTH_DEVICE_INFO *Info;

		property bool Authenticated
		{
			bool get()
			{
				return Info->fAuthenticated;
			}
		}

		property bool Connected
		{
			bool get()
			{
				return Info->fConnected;
			}
		}

		property bool Remembered
		{
			bool get()
			{
				return Info->fRemembered;
			}
		}

		~BluetoothDevice()
		{
			if(Info)
				delete Info;
		}
	};

	public ref class BluetoothManager
	{
	public:
		List<BluetoothRadio^>^ GetRadios()
		{
			List<BluetoothRadio^>^ radios = gcnew List<BluetoothRadio^>();

			HANDLE hRadio;
			HBLUETOOTH_RADIO_FIND hFindRadio;
			BLUETOOTH_FIND_RADIO_PARAMS radioParam;
			radioParam.dwSize = sizeof(BLUETOOTH_FIND_RADIO_PARAMS);

			hFindRadio = BluetoothFindFirstRadio(&radioParam, &hRadio);
			try
			{
				if (hFindRadio)
				{
					do
					{
						BluetoothRadio^ radio = gcnew BluetoothRadio();
						radio->RadioHandle = hRadio;

						BLUETOOTH_RADIO_INFO radioInfo;
						radioInfo.dwSize = sizeof(radioInfo);
						BluetoothGetRadioInfo(radio->RadioHandle, &radioInfo);

						radio->Name = gcnew String(radioInfo.szName);
						radio->Address = gcnew List<Byte>();
						radio->Address->Add(radioInfo.address.rgBytes[0]);
						radio->Address->Add(radioInfo.address.rgBytes[1]);
						radio->Address->Add(radioInfo.address.rgBytes[2]);
						radio->Address->Add(radioInfo.address.rgBytes[3]);
						radio->Address->Add(radioInfo.address.rgBytes[4]);
						radio->Address->Add(radioInfo.address.rgBytes[5]);

						// TODO: Clean this up
						
						radio->Info = new BLUETOOTH_RADIO_INFO();
						memset(radio->Info, 0, sizeof(BLUETOOTH_RADIO_INFO));
						memcpy(radio->Info, &radioInfo, sizeof(BLUETOOTH_RADIO_INFO));

						radios->Add(radio);
					}
					while (BluetoothFindNextRadio(&radioParam, &hRadio));
				}
			}
			finally
			{
				BluetoothFindRadioClose(hFindRadio);
			}

			return radios;
		}

		List<BluetoothDevice^>^ DiscoverDevices(BluetoothRadio^ radio)
		{
			List<BluetoothDevice^>^ devices = gcnew List<BluetoothDevice^>();

			BLUETOOTH_DEVICE_SEARCH_PARAMS srch;
			srch.dwSize = sizeof(BLUETOOTH_DEVICE_SEARCH_PARAMS);
			srch.fReturnAuthenticated = TRUE;
			srch.fReturnRemembered = TRUE;
			srch.fReturnConnected = TRUE;
			srch.fReturnUnknown = TRUE;
			srch.fIssueInquiry = TRUE;
			srch.cTimeoutMultiplier = 2;
			srch.hRadio = radio->RadioHandle;

			BLUETOOTH_DEVICE_INFO btdi;
			btdi.dwSize = sizeof(btdi);

			HBLUETOOTH_DEVICE_FIND hFind = BluetoothFindFirstDevice(&srch, &btdi);

			try
			{
				do
				{
					if(hFind)
					{
						BluetoothDevice^ dev = gcnew BluetoothDevice();
						dev->Name = gcnew String(btdi.szName);
						dev->Address = gcnew List<Byte>();
						dev->Address->Add(btdi.Address.rgBytes[0]);
						dev->Address->Add(btdi.Address.rgBytes[1]);
						dev->Address->Add(btdi.Address.rgBytes[2]);
						dev->Address->Add(btdi.Address.rgBytes[3]);
						dev->Address->Add(btdi.Address.rgBytes[4]);
						dev->Address->Add(btdi.Address.rgBytes[5]);

						dev->Info = new BLUETOOTH_DEVICE_INFO();
						memset(dev->Info, 0, sizeof(BLUETOOTH_DEVICE_INFO));
						memcpy(dev->Info, &btdi, sizeof(BLUETOOTH_DEVICE_INFO));

						devices->Add(dev);
					}
				}
				while (BluetoothFindNextDevice(hFind, &btdi));
			}
			finally
			{
				BluetoothFindDeviceClose(hFind);
			}

			return devices;
		}

		void PairWithDevice(BluetoothRadio^ radio, BluetoothDevice^ device, array<Byte>^ pin)
		{
			WCHAR pass[6];

			// TODO: make buf dynamic and loop through pin array

			pass[0] = pin[0];
			pass[1] = pin[1];
			pass[2] = pin[2];
			pass[3] = pin[3];
			pass[4] = pin[4];
			pass[5] = pin[5];

			DWORD pcServices = 16;
			GUID guids[16];

			CheckForError(L"BluetoothAuthenticateDevice", BluetoothAuthenticateDevice(NULL, radio->RadioHandle, device->Info, pass, pin->Length));
			CheckForError(L"BluetoothEnumerateInstalledServices", BluetoothEnumerateInstalledServices(radio->RadioHandle, device->Info, &pcServices, guids));
			CheckForError(L"BluetoothSetServiceState", BluetoothSetServiceState (radio->RadioHandle, device->Info, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE));
		}

		void ConnectDevice(BluetoothRadio^ radio, BluetoothDevice^ device)
		{
			CheckForError(L"BluetoothSetServiceState", BluetoothSetServiceState (radio->RadioHandle, device->Info, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE));
		}

		void DisconnectDevice(BluetoothRadio^ radio, BluetoothDevice^ device)
		{
			CheckForError(L"BluetoothSetServiceState", BluetoothSetServiceState (radio->RadioHandle, device->Info, &HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_DISABLE));
		}

		void CheckForError(LPWSTR msg, DWORD dw)
		{
			if(dw != ERROR_SUCCESS)
			{
				LPVOID lpMsgBuf;

				FormatMessage(
					FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
					NULL,
					dw,
					MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
					(LPTSTR) &lpMsgBuf,
					0,
					NULL
				);

				String^ strError = String::Format(gcnew String("{0}: {1}"), gcnew String(msg), gcnew String((LPTSTR)lpMsgBuf)); 

				LocalFree(lpMsgBuf);

				throw gcnew Exception(strError);
			}
		}
	};

}}
