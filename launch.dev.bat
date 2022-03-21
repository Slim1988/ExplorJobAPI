if not exist appsettings.Development.json (
    gpg -o appsettings.Development.json -d appsettings.Development.json.gpg
)

dotnet watch run --environment "Development"
