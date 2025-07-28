FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY *.csproj .
RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /app/publish --no-restore
RUN dotnet tool install dotnet-ef --tool-path /tools


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

COPY --from=build /tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

EXPOSE 8080

ENTRYPOINT ["dotnet", "ImaginedWorlds.dll"]