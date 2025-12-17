 Commande pour créer une migration

`dotnet ef migrations add <NOM_DE_LA_MIGRATION> --output-dir "Infrastructure/Migrations"`

Ensuite pour l'appliquer à la BDD :

`dotnet ef database update`


Pour que l'application détecte la BDD en locale, il faut :
- Avoir installé PostGreSQL (et PgAdmin) en local
- Avoir une base de données nommée "mojii"
- Avoir un utilisateur nommé "postgres" (user de base) avec le mot de passe que vous voulez
- Remplacer le mot de passe dans le fichier appsettings.json (à la fin de `DefaultConnection`)