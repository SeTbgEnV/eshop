# eShop API (Lektion 14)

### Detta är start projektet för lektion 14

Projektet är konfigurerat för att använda Sqlite som databas för enkelhetens skull.
Det finns konfiguration för att kunna köra mot en MySql databasen. Då måste först vår container startas för att kunna kommunicera mot den.

Så jag kommer att använda Sqlite för detta projekt.

### Komma igång

Kör följande kommando i terminalen, det är viktigt att vi står i roten av projektet _eshop.api_

`dotnet ef migrations add InitialSqlite -o Data/Migrations`

Nästa steg är att köra migreringen så kör följande kommando i samma terminalfönster.
`dotnet ef database update`

### Dummy data

Det finns test data som vi kan fylla databasen med.
Data som finns just nu är leverantörer och adresser.

Starta applikationen med kommandot `dotnet run` då körs _seed_ skriptet
