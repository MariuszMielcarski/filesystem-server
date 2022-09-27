#!/bin/bash

wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update

sudo apt-get install -y dotnet-sdk-6.0

sudo dotnet dev-certs https --trust

sudo rm filesystem-server-main/ -r -f

wget https://github.com/MariuszMielcarski/filesystem-server/archive/refs/heads/main.zip

sudo apt-get install -y unzip

unzip main.zip

rm main.zip -f

cd filesystem-server-main/FilesystemServer

sudo dotnet run
