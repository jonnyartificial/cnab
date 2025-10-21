FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./Cnab.WebApi/Cnab.WebApi.csproj
RUN dotnet publish ./Cnab.WebApi/Cnab.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 9910
ENTRYPOINT ["dotnet", "Cnab.WebApi.dll"]
