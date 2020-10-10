# Running migrations

First, install fluent migrator CLI tool

```
dotnet tool install -g FluentMigrator.DotNet.Cli
```

build the migrations project:

```
dotnet build src/KanbanBoard.Migrations
```

Then, run:

dotnet-fm migrate -p Postgres -c "{connection string here}" -a "src/KanbanBoard.Migrations/bin/Debug/netstandard2.0/KanbanBoard.Migrations"
