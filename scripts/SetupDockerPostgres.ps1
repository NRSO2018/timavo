docker network create timinute-database

if("$(docker ps -a | Select-string -Pattern timinute.postgres)") {
  docker rm -f timinute.postgres
}

docker pull postgres:16-alpine

# Single knob for the password: honor POSTGRES_PASSWORD (same var docker-compose
# uses) and fall back to the built-in dev default so `dotnet run` works out of the box.
$postgresPassword = if ($env:POSTGRES_PASSWORD) { $env:POSTGRES_PASSWORD } else { "TiminuteAdmin." }

docker run `
  -e "POSTGRES_USER=postgres" `
  -e "POSTGRES_PASSWORD=$postgresPassword" `
  -e "POSTGRES_DB=Timinute" `
  -p 5432:5432 `
  --name timinute.postgres `
  --hostname timinute.postgres `
  --restart unless-stopped `
  -d `
  postgres:16-alpine

docker network connect timinute-database timinute.postgres
