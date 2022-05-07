FROM mcr.microsoft.com/dotnet/aspnet:6.0
FROM mcr.microsoft.com/dotnet/sdk:6:0-alpine
# Expose default port for App Engine
EXPOSE 8080
# Add app source
ADD . /
WORKDIR /app
ENTRYPOINT ["dotnet", "MovieApp.dll"]
