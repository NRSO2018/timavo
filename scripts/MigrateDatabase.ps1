$location = Get-Location;

dotnet tool install --global dotnet-ef

Set-Location "../Timavo";

$startupProject = Get-Childitem -Include Timavo.Server.csproj -Recurse
Write-Host -NoNewline "Applying migrations for ";
Write-Host -ForegroundColor Yellow $file.Name;

Set-Location $startupProject.Directory;
dotnet build /v:q $file.Name;

dotnet ef database update --no-build --project $startupProject --startup-project $startupProject;

Write-Host;

Set-Location $location;