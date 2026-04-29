 Commande pour créer une migration

`dotnet ef migrations add <NOM_DE_LA_MIGRATION> --output-dir "Infrastructure/Migrations"`

Ensuite pour l'appliquer à la BDD :

`dotnet ef database update`


Pour que l'application détecte la BDD en locale, il faut :
- Avoir installé PostGreSQL (et PgAdmin) en local
- Avoir une base de données nommée "mojii"
- Avoir un utilisateur nommé "postgres" (user de base) avec le mot de passe que vous voulez
- Remplacer le mot de passe dans le fichier appsettings.json (à la fin de `DefaultConnection`)

## Run with Docker

Lancer API + PostgreSQL:

`docker compose up --build`

ou 

`docker compose watch`
pour que ça rebuild à chaque changement de code efficacement

API disponible sur:

`http://localhost:7200`

PostgreSQL disponible sur:

`localhost:5533`

URL que j'ai utilisé pour me connecter à la BDD depuis mon IDE:

`jdbc:postgresql://127.0.0.1:5533/mojii?user=postgres&password=password`

Arreter:

`docker compose down`

Arreter et supprimer les donnees postgres:

`docker compose down -v`

### Migrations avec Docker

Les migrations EF Core sont appliquees automatiquement au demarrage de l'API via `Database.MigrateAsync()`.
Quand le conteneur `api` demarre, il attend que PostgreSQL soit healthy puis applique les migrations en attente.

### Variables de configuration en Docker

Le conteneur `api` utilise:

- `ASPNETCORE_ENVIRONMENT=Development` (donc `appsettings.Development.json` est charge)
- `ConnectionStrings__DefaultConnection=...` pour pointer vers le service Docker `db`

Identifiants postgres standard utilises dans `docker-compose.yml`:

- database: `mojii`
- username: `postgres`
- password: `password`


Le SMTP Brevo fonctionne en Docker comme en local, tant que:

- les champs `BrevoSettings` sont presents (host/port/user/password/sender)
- le conteneur a un acces sortant internet (vers `smtp-relay.brevo.com:587`)


Pas nécessaire (peu servir si y a trop d'erreurs liées à ça):
`docker pull mcr.microsoft.com/dotnet/sdk:10.0`