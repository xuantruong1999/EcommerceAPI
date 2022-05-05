#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EcommerceWEB/EcommerceWEB.csproj", "EcommerceWEB/"]
COPY ["EcommerceAPI.Model/EcommerceAPI.Model.csproj", "EcommerceAPI.Model/"]
COPY ["EcommerceAPI.Services/EcommerceAPI.Services.csproj", "EcommerceAPI.Services/"]
COPY ["EcommerceAPI.DataAccess/EcommerceAPI.DataAccess.csproj", "EcommerceAPI.DataAccess/"]
RUN dotnet restore "EcommerceWEB/EcommerceWEB.csproj"
COPY . .
WORKDIR "/src/EcommerceWEB"
RUN dotnet build "EcommerceWEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EcommerceWEB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet EcommerceWEB.dll

