FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000                               

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/CommandAPI/CommandAPI.csproj", "src/CommandAPI/"]
RUN dotnet restore "src/CommandAPI/CommandAPI.csproj"
COPY . .
WORKDIR "/src/src/CommandAPI"
RUN dotnet build "CommandAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommandAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000                           
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandAPI.dll"]