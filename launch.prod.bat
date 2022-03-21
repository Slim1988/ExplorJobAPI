if not exist appsettings.Production.json (
    gpg -o appsettings.Production.json -d appsettings.Production.json.gpg
)

dotnet watch run --environment "Production"
