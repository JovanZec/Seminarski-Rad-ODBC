# ODBC: pojam, uloga u razvoju informacionih sistema

Seminarski projekat za predmet **Arhitektura informacionih sistema**. Projekat demonstrira upotrebu ODBC-a u troslojnoj ASP.NET Core MVC aplikaciji za evidenciju studenata.

## Arhitektura

- `src/OdbcIS.Web` - prezentacioni sloj (ASP.NET Core MVC, ViewModel-i, tanki kontroleri)
- `src/OdbcIS.BLL` - poslovna logika (validacija, mapiranje, tok aplikacije)
- `src/OdbcIS.DAL` - pristup podacima (entiteti, repository, `System.Data.Odbc`)
- `database` - SQL skripta za kreiranje baze
- `docs` - dokumentacija seminarskog rada

Tok poziva:

`Browser -> MVC/ViewModel -> BLL service -> DAL repository -> ODBC Driver Manager -> ODBC Driver 17 -> SQL Server`

## Potrebno

1. Visual Studio 2022/2026 sa workload-om **ASP.NET and web development**.
2. .NET 8 SDK.
3. SQL Server Express (ili druga SQL Server instanca).
4. Microsoft ODBC Driver 17 for SQL Server.

## Pokretanje

1. Pokrenuti `database/CreateDatabase.sql` u SQL Server Management Studio-u.
2. Po potrebi izmeniti `ConnectionStrings:OdbcDatabase` u `src/OdbcIS.Web/appsettings.json`.
3. Otvoriti `OdbcSeminarski.sln` u Visual Studio-u.
4. Desni klik na `OdbcIS.Web` -> **Set as Startup Project**.
5. `Build > Build Solution`.
6. Pokrenuti sa `Ctrl+F5`.

Ako je SQL Server instanca drugačija od `localhost\\SQLEXPRESS`, prilagoditi deo `Server=` u connection string-u.

## Napomena o ODBC parametrima

U `System.Data.Odbc` SQL upiti koriste `?` kao placeholder. Parametri se vezuju **po redosledu**, pa redosled `command.Parameters.Add(...)` mora odgovarati redosledu `?` u SQL izrazu.
