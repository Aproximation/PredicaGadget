#!/bin/sh
### BEGIN INIT INFO
# Provides: Predica Gadget
# Required-Start: bluetooth
# Required-Stop:
# Default-Start: 2 3 4 5
# Default-Stop: 0 1 6
# Short-Description: Start Predica Gadget daemons
### END INIT INFO

# Add script to /etc/init.d/
# IF adding it first time then also run
#	update-rc.d BootScript.sh defaults


echo 'Starting Predica Gadget...'
Pfp="/home/pi/Desktop/Predica/PredicaGadget" #Predica Gadget Folder Path
bash -c "echo Y > /sys/module/i2c_bcm2708/parameters/combined"
python $Pfp/bluetoothEmiter.py > $Pfp/Logs/btEmiter.log 2>&1 & #stdout and stderr move to *.log file
python $Pfp/bluetoothListener.py > $Pfp/Logs/btListener.log 2>&1 & #as above
echo 'Predica Gadget started'
