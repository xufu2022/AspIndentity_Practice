# ch2

dotnet new globaljson --sdk-version 7.0.100 --output  IdentityApp
dotnet new web --no-https --output IdentityApp --framework net7.0
dotnet new sln -o IdentityApp
dotnet sln IdentityApp add IdentityApp

cd .\IdentityApp\  

dotnet tool uninstall --global Microsoft.Web.LibraryManager.Cli
dotnet tool install --global Microsoft.Web.LibraryManager.Cli --version 2.1.175
libman init -p cdnjs
libman install twitter-bootstrap@4.5.0 -d wwwroot/lib/twitter-bootstrap


dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0

dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 7.0.0

dotnet dev-certs https --clean
dotnet dev-certs https --trust
<!-- cd IdentityTodo
dotnet build
dotnet run --watch

dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 7.0.0

cd .\IdentityTodo\

dotnet ef migrations add AddTodos
dotnet ef database drop --force
dotnet ef database update -->