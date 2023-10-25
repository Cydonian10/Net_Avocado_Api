
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /src

COPY *.sln .
COPY *.csproj .

RUN dotnet restore

COPY . .
RUN dotnet build "Api.csproj" -c Release -o /app/build
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:7.0 

ENV ASPNETCORE_URLS http://+:3000

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet","Api.dll" ]