FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FileWatcher/FileWatcher.csproj", "FileWatcher/"]
RUN dotnet restore "FileWatcher/FileWatcher.csproj"
COPY . .
WORKDIR "/src/FileWatcher"
RUN dotnet build "FileWatcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileWatcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileWatcher.dll"]
