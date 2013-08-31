# AgiliSway

A thin WPF wrapper around the [WiiMote Lib][wiimotelib] (written by Brian Peek) intended to allow researchers to acquire data from the Wii Fit (R).  It collects raw data from the board and saves center of gravity coordinates to disk in an XML format.  It is also capable of exporting raw sensor values for those interested in generating custom analytics.

## Installation

1. Install [.net 4.5][net45]
2. Install [Visual C++ 11 redist][vc11]
3. Download [AgiliSway] from GitHub
  * Extract AgiliSway-master.zip
  * Navigate to AgiliSway-master/inst and extract AgiliSway.zip
4. Go to extracted folder and run AgiliSway.vNext.exe

## Running

1. First run a pop up will appear asking you to select a folder.  This will be the folder where AgiliSway stores the program database and all of the sway files.
2. AgiliSway will then start and default to the Study page.
3. Make sure your study has been created and selected.

## License
This software is released under the MIT license.

[net45]: http://www.microsoft.com/en-us/download/details.aspx?id=30653
[vc11]: http://www.microsoft.com/en-us/download/details.aspx?id=30679
[agilisway]: https://github.com/digitalnelson/AgiliSway/archive/master.zip
[wiimotelib]: http://wiimotelib.codeplex.com/