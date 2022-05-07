FROM mcr.microsoft.com/dotnet/aspnet:6.0
# Expose default port for App Engine
EXPOSE 8080
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "MovieApp.dll"]
