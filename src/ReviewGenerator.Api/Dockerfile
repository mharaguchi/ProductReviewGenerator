#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReviewGenerator.Api/ReviewGenerator.Api.csproj", "ReviewGenerator.Api/"]
COPY ["ReviewGenerator.Core/ReviewGenerator.Core.csproj", "ReviewGenerator.Core/"]
COPY ["ReviewGenerator.Data/ReviewGenerator.Data.csproj", "ReviewGenerator.Data/"]
RUN dotnet restore "ReviewGenerator.Api/ReviewGenerator.Api.csproj"
COPY . .
WORKDIR "/src/ReviewGenerator.Api"
RUN dotnet build "ReviewGenerator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReviewGenerator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewGenerator.Api.dll"]