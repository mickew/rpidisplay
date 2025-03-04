#!/usr/bin/env bash


version() { 
	echo "$@" | awk -F. '{ printf("%d%03d%03d%03d\n", $1,$2,$3,$4); }'; 
}

#Check if script is being run as root
if [ "$(id -u)" != "0" ]; then
   echo "This script must be run as root" 1>&2
   exit 1
fi

if [ ! $? = 0 ]; then
   exit 1
else
   BASEDIR=${PWD}
   latesturl="https://github.com/mickew/rpidisplay/releases/latest"
   location=$(curl -s -I $latesturl | grep -i ^Location: | cut -d: -f2- | sed 's/^ *\(.*\).*/\1/')
   ver="${location##*/}"
   ver=$(echo "$ver" | sed 's/.\{1\}$//')

   cd /usr/local/bin/rpidisplay
   oldver=$(/usr/local/bin/rpidisplay/rpidisplay --version)
   cd ${BASEDIR}
   oldver=$(echo "v$oldver")
   if [ $(version $oldver) -ge $(version $ver) ]; then
      echo "Version is up to date"
      whiptail --title "Version is up to date" --msgbox "Version $ver => $oldver ." 8 78
      exit
   fi

   systemctl stop rpidisplay.service
   sh ./getlatest.sh

   chmod +x /usr/local/bin/rpidisplay/rpidisplay

   cd ${BASEDIR}

   systemctl start rpidisplay.service
   whiptail --title "Update complete" --msgbox "Raspberry PI Info Display update complete." 8 78
fi
