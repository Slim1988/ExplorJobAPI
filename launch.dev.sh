#!/bin/bash

# Certificate
[ ! -f Auth/Cert/cert.pfx ] \
&& openssl pkcs12 -in Auth/Cert/Dev/cert.pem -inkey Auth/Cert/Dev/privatekey.pem -export -out Auth/Cert/cert.pfx

# Config
[ ! -f appsettings.Development.json ] \
&& gpg -o appsettings.Development.json -d appsettings.Development.json.gpg

# Resources
[ ! -d Resources ] \
&& cp -rf ../resources Resources

# Launch
dotnet watch run --environment "Development"
