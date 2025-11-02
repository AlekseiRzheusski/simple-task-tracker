# миграция
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add Init
dotnet ef database update

# запуск
dotnet run --launch-profile https