#Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

#Copias csproj y resturar dependencias
COPY *.csproj ./
RUN dotnet restore

#Copiar el resto del codigo
COPY . ./
RUN dotnet publish -c Release -o out

#Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .


EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT [ "dotnet", "SentimentAPI.dll"]