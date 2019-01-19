dotnet restore
dotnet ef database update -s "Drugstore\Drugstore.csproj" -p "Drugstore.Infrastructure\Drugstore.Infrastructure.csproj" -c "DrugstoreDbContext"