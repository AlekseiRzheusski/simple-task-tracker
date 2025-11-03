# миграция
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add Init
dotnet ef database update

# запуск
dotnet run --launch-profile https

#добавлеие JsonPatchDocument<>
dotnet add package Microsoft.AspNetCore.JsonPatch
#для работы ApplyTo(dto, ModelState);
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
