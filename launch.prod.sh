#!/bin/bash

# Config
[ ! -f appsettings.Production.json ] \
&& gpg -o appsettings.Production.json -d appsettings.Production.json.gpg

# Resources
[ ! -d Resources ] \
&& cp -rf ../resources Resources

# Launch
dotnet watch run --environment "Production"
