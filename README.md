# ExplorJob API

## Prerequisites

### Essentials to install

- git  
- [dotnet 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)  
- dotnet-ef (Entity Framework) : cf. Persistence Section for installation  
- database ProstgreSQL 11.7  
- [redis 5.0.5](https://redis.io)
- openssl

### Essentials to process

> This are essentials to process but were managed by launch scripts (dev & prod).

- appsettings env decryption : cf. Security Section  
- dev Cert : cf. Security Section  

### Recommended to install

- [Visual Studio Code](https://code.visualstudio.com)  
## Démarrage
- [Télécharger](https://www.postgresql.org/) et Installer PostgreSql (renseigner mot de passe et nom d'utilisateur  - ils seront utile pour la connectionString).
- installer [GnuPG](https://gnupg.org/download/index.html), pour décrypter les différents fichiers. 
- Décrypter le fichier de configuration à l'aide de cette commande `gpg -o appsettings.Development.json -d appsettings.Development.json.gpg`. (renseigner le mot de passe qu'il faudra demander à l'administrateur).
- Accéder au fichier de configuration et remplacer le mot de passe et le userId renseignés pendant l'installatiion de postgresql.
- Télécharger et installer [Redis-server](https://redis.io/download).
- Lancer redis-server.
- Installer la CLI d'entity framework(ef) `dotnet tool install --global dotnet-ef `.
- Lancer la commande `dotnet ef database update` pour créer la base de donnee.
- Lancer la commande `dotnet run --environment "Development"` pour lancer l'application.
## Environnements

| Env | Url | Git branches | Databases |
| - | - | - | - |
| **Development** | https://127.0.0.1:9000 | *feature branch / develop* | <center>Local</center> |
| **Production** | https://api.explorjob.com | *master* | <center>Prod</center> |

## Versioning

Managed with [Git](https://git-scm.com)

> Repository url  
> https://gitlab.com/explorjob/explorjob-api.git

## Commands

### Run

```bash
dotnet run --environment "Development"
dotnet run --environment "Production"
```  

### Run with watch

```bash
dotnet watch run --environment "Development"
dotnet watch run --environment "Production"
sh launch.dev.sh
sh launch.prod.sh
```  

### Production deployment

```bash
sh deploy.sh
```  

> ---  
> nginx conf (/etc/nginx/nginx.conf) must have :
> ```bash
> http {
>   ...
>   limit_req_zone $binary_remote_addr zone=one:10m rate=5r/s;
>   server_tokens off;
>   ...
> }
> ```  
> ---  

## Persistence

### Prerequisites

```bash
dotnet tool install --global dotnet-ef
```  

### Migrations

Add migration

```bash
dotnet ef migrations add MyMigration
```  

Remove specific migration

```bash
dotnet ef migrations remove MyMigration
```  

Remove last migration

```bash
dotnet ef migrations remove
```  

Update database

```bash
dotnet ef database update
```  

## Security

Follow up dotnet security [OWASP](https://cheatsheetseries.owasp.org/cheatsheets/DotNet_Security_Cheat_Sheet.html) recommandations.

Encrypt settings files

```bash
gpg -c appsettings.Development.json
gpg -c appsettings.Production.json
```  

Decrypt settings files

```bash
gpg -o appsettings.Development.json -d appsettings.Development.json.gpg
gpg -o appsettings.Production.json -d appsettings.Production.json.gpg
```  

Generate Dev Cert

```bash
sh generate-cert-pfx.sh
```  

## Utils

### Swagger

> https://127.0.0.1:9000/swagger  

### API Health

> https://127.0.0.1:9000/health  

### Hangfire dashboard

> https://127.0.0.1:9000/hangfire  

### Redis-Server

> https://redis.io/documentation

### Packages

```bash
dotnet add package MyPackage
dotnet add package MyPackage -v 3.0.0-rc2
```  
