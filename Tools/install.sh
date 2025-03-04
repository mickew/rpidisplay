#!/usr/bin/env bash

#Check if script is being run as root
if [ "$(id -u)" != "0" ]; then
   echo "This script must be run as root" 1>&2
   exit 1
fi

if [ ! $? = 0 ]; then
   exit 1
else
   BASEDIR=${PWD}

   sh ./getlatest.sh

   chmod +x /usr/local/bin/rpidisplay/rpidisplay
   
   cp rpidisplay.service /etc/systemd/system
   if [ ! -f /etc/systemd/system/rpidisplay.service ]; then
     whiptail --title "Installation aborted" --msgbox "There was a problem writing the rpidisplay.service file" 8 78
    exit
   fi

   systemctl enable rpidisplay.service
   systemctl start rpidisplay.service
   whiptail --title "Installation complete" --msgbox "Raspberry PI Info Display installation complete." 8 78

   #reboot
   #poweroff
fi
