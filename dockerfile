FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /Publish
COPY . .
RUN dotnet publish src/ESProj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /Publish/out .
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT dotnet ESProj.Application.dll