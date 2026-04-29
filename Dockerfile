FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MojiiBackend.csproj", "./"]
RUN dotnet restore "MojiiBackend.csproj"

COPY . .
RUN dotnet publish "MojiiBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MojiiBackend.dll"]
