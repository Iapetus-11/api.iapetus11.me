# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY api.iapetus11.me/*.csproj ./api.iapetus11.me/
COPY Fractals/*.csproj ./Fractals/
RUN dotnet restore

# copy everything else and build app
COPY api.iapetus11.me/. ./api.iapetus11.me/
COPY Fractals/. ./Fractals/
WORKDIR /source/api.iapetus11.me
# RUN dotnet publish -c release -o /app --no-restore
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
COPY api.iapetus11.me/Content Content
COPY api.iapetus11.me/Properties Properties
ENTRYPOINT ["dotnet", "api.iapetus11.me.dll"]