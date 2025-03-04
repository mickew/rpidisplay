#!/usr/bin/env bash

getlatesturl="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/getlatest.sh"
installurl="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/install.sh"
uninstallurl="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/uninstall.sh"
updateurl="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/update.sh"
rpidisplaysurl="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/rpidisplay.sh"
rpidisplayservice="https://raw.githubusercontent.com/mickew/rpidisplay/main/Tools/rpidisplay.service"

mkdir -p Tools
curl -o Tools/getlatest.sh $getlatesturl
curl -o Tools/install.sh $installurl
curl -o Tools/uninstall.sh $uninstallurl
curl -o Tools/update.sh $updateurl
curl -o Tools/rpidisplay.sh $rpidisplaysurl
curl -o Tools/rpidisplay.service $rpidisplayservice

cd Tools/
sudo sh rpidisplay.sh
