Step 1: Create an empty database in MsSQLServer

Step 2 : Open appsettings.json file

Step 3: Find line: "DefaultConnectionString": "Data Source=SQLSERVERNAME;Initial Catalog=DATABASENAME;Integrated Security=True;Pooling=False",

Replace 'SQLSERVERNAME' with your SQL server and 'DATABASENAME' with database name
that you created in step 1

Step 4: Open NuGet Package Manager Console and write command: 'add-migration "MIGRATION_NAME"', then write command 'update-database'

Step 5: Run project
