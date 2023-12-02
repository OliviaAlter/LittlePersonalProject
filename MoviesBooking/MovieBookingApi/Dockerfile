FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MovieBookingApi/MovieBookingApi.csproj", "MovieBookingApi/"]
RUN dotnet restore "MovieBookingApi/MovieBookingApi.csproj"
COPY . .
WORKDIR "/src/MovieBookingApi"
RUN dotnet build "MovieBookingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieBookingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieBookingApi.dll"]
