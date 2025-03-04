#!/usr/bin/env bash


CHOICE=$(
whiptail --title "Raspberry PI Info Display" --menu "Choose an option" 25 78 16 \
	"1)" "Install Raspberry PI Info Display on system." \
	"2)" "Update Raspberry PI Info Display on system." \
	"3)" "Remove Raspberry PI Info Display from the system." \
	"9)" "Exit."  3>&2 2>&1 1>&3	
)

case $CHOICE in
	"1)")   
		result="Install"
		echo "User selected $result."
		sh ./install.sh
	;;
	"2)")   
		result="Update"
		echo "User selected $result."
		sh ./update.sh
	;;

	"3)")   
		result="Uninstall"
		echo "User selected $result."
		if whiptail --title "Raspberry PI Info Display - Uninstall" --yesno "Do you realy want to uninstall" 8 78; then
			result="Uninstall"
			echo "User selected Yes."
			sh ./uninstall.sh
		else
			result=""
			echo "User selected No."
		fi
        ;;

	"9)") exit
        ;;
esac

exit