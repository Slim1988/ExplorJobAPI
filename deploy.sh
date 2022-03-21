#!/bin/bash

# Versioning
git checkout -f master

# Config
[ ! -f appsettings.Production.json ] \
&& gpg -o appsettings.Production.json -d appsettings.Production.json.gpg

# Resources
[ ! -d Resources ] \
&& cp -Rf ../resources Resources \
&& chown -R www-data:www-data Resources/ \
&& chmod -R 775 Resources/

# Build
export ASPNETCORE_ENVIRONMENT=Production \
&& dotnet restore \
&& dotnet ef database update \
&& dotnet publish --configuration Production

# Deployment
cp -f Deployment/nginx/explorjob-api.conf /etc/nginx/sites-enabled
cp -f Deployment/nginx/proxy.conf /etc/nginx/conf.d
service nginx restart

# Service
systemctl stop explorjob-api.service
systemctl disable explorjob-api.service
cp -f Deployment/service/explorjob-api.service /etc/systemd/system
systemctl enable explorjob-api.service
systemctl start explorjob-api.service
