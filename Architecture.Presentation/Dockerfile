#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Architecture.Presentation/Architecture.Presentation.csproj", "Architecture.Presentation/"]
COPY ["Architecture.DataSource.MongoDb/Architecture.DataSource.MongoDb.csproj", "Architecture.DataSource.MongoDb/"]
COPY ["Architecture.Domain/Architecture.Domain.csproj", "Architecture.Domain/"]
COPY ["Architecture.DataSource.Cache/Architecture.DataSource.Cache.csproj", "Architecture.DataSource.Cache/"]
COPY ["Architecture.Utils/Architecture.Utils.csproj", "Architecture.Utils/"]
COPY ["Architecture.Application/Architecture.Application.csproj", "Architecture.Application/"]
COPY ["Architecture.Infrastructure/Architecture.Infrastructure.csproj", "Architecture.Infrastructure/"]
RUN dotnet restore "Architecture.Presentation/Architecture.Presentation.csproj"
COPY . .
WORKDIR "/src/Architecture.Presentation"
RUN dotnet build "Architecture.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Architecture.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Architecture.Presentation.dll"]