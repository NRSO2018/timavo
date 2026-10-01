# syntax=docker/dockerfile:1.7

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src

# csproj-only first → restore layer caches well
COPY Timavo.sln ./
COPY Timavo/Server/Timavo.Server.csproj  Timavo/Server/
COPY Timavo/Client/Timavo.Client.csproj  Timavo/Client/
COPY Timavo/Shared/Timavo.Shared.csproj  Timavo/Shared/
RUN dotnet restore Timavo/Server/Timavo.Server.csproj -a $TARGETARCH

COPY . .
RUN dotnet publish Timavo/Server/Timavo.Server.csproj \
        -c Release -a $TARGETARCH --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
RUN mkdir -p /keys && chown app:app /keys
USER app
COPY --from=build --chown=app:app /app/publish .
ENV ASPNETCORE_URLS=http://+:8080 \
    DatabaseMigrationOnStartup=true \
    DataProtection__KeyPath=/keys/data-protection \
    IdentityServer__KeyManagement__KeyPath=/keys \
    ForwardedHeaders__AllowAnyProxy=true
EXPOSE 8080
# VOLUME is metadata; the actual /keys directory was created and chowned
# earlier (as root) before the USER switch.
VOLUME ["/keys"]
ENTRYPOINT ["dotnet", "Timavo.Server.dll"]
