# Creating the Example Project

-- dotnet --list-sdks
-- dotnet --list-runtimes

dotnet new globaljson --sdk-version 7.0.100 --output ExampleApp
dotnet new web --no-https --output ExampleApp --framework net7.0
dotnet new sln -o ExampleApp

dotnet sln ExampleApp add ExampleApp

dotnet tool uninstall --global Microsoft.Web.LibraryManager.Cli
dotnet tool install --global Microsoft.Web.LibraryManager.Cli --version 2.1.175
libman init -p cdnjs
libman install twitter-bootstrap@4.5.0 -d wwwroot/lib/twitter-bootstrap