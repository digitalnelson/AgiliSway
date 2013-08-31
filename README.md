# AgiliSway

A thin WPF wrapper around the [WiiMote Lib][wiimotelib] (written by Brian Peek) intended to allow researchers to acquire data from the Wii Fit (R).  It collects raw data from the board and saves center of gravity coordinates to disk in an XML format.  It is also capable of exporting raw sensor values for those interested in generating custom analytics.

## Installation

1. Install [.net 4.5][net45]
1. Install [Visual C++ 11 redist][vc11]
1. Install [SqlCE redist][sqlce]
1. Download [AgiliSway] from GitHub
  * Extract AgiliSway-master.zip
  * Navigate to AgiliSway-master/inst and extract AgiliSway9.zip and AgiliSway9.DeviceManager.zip

## Connecting a WBB

1. Go to the AgiliSway9.DeviceManager folder and double click on AgiliSway9.DeviceManager.exe (ASDM)
1. Open the battery compartment of the WBB
1. Push the red button
1. Quickly hit the search button in ASDM
1. The device will appear in the list.  Select the device.
1. Click the pair button
1. Allow windows to install the HID driver for the device.
1. Open AgiliSway and select the devices tab.
1. Select the WBB option on the left.
1. Push the front button on the WBB.
1. The green light should blink a couple of times and then become constant.

## Running AgiliSway

1. First run a pop up will appear asking you to select a folder.  This will be the folder where AgiliSway stores the program database and all of the sway files.
1. AgiliSway will then start and default to the Study page.
1. Make sure your study has been created and selected.
1. Click devices and select the device you are interested and set options.
1. Go to subjects and create a new subject.
1. Click on collections and create a new collection.
1. Make sure to calibrate the board before the subject steps onto it and in between tasks.

## Troubleshooting
* Make sure to push the red WBB button at least every 10 sec to make sure the device stays in pairing mode while the paring and installation are occuring.
* If the pairing is unsuccessful, remove one battery for 10 sec, delete the device from windows bluetooth under the bluetooth control panel applet.

## License
This software is released under the MIT license.

[net45]: http://www.microsoft.com/en-us/download/details.aspx?id=30653
[vc11]: http://www.microsoft.com/en-us/download/details.aspx?id=30679
[sqlce]: http://www.microsoft.com/en-us/download/details.aspx?id=17876
[agilisway]: https://github.com/digitalnelson/AgiliSway/archive/master.zip
[wiimotelib]: http://wiimotelib.codeplex.com/