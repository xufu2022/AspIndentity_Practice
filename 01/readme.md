dotnet new globaljson --sdk-version 7.0.100 --output IdentityTodo
dotnet new webapp --auth Individual --use-local-db true --output IdentityTodo --framework net7.0
dotnet new sln -o IdentityTodo
dotnet sln IdentityTodo add IdentityTodo

cd IdentityTodo
dotnet build
dotnet run --watch

dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 7.0.0

cd .\IdentityTodo\

dotnet ef migrations add AddTodos
dotnet ef database drop --force
dotnet ef database update