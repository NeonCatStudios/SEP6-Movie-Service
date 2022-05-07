FROM mcr.microsoft.com/dotnet/sdk:6.0
# Expose default port for App Engine
EXPOSE 8080
# Add app source
ADD . /
WORKDIR /app
ENTRYPOINT ["dotnet", "MovieApp.dll"]
