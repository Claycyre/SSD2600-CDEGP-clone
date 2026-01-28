##########################
# Database 
##########################
FROM mcr.microsoft.com/mssql/server:2022-latest AS mssql 

# Change to root user for config
USER root

# Create a config directory
RUN mkdir -p /usr/config
WORKDIR /usr/config

# Bundle config source
COPY --chmod=755 --chown=mmsql deploy/dev/mssql /usr/config/

# Change back to default image user
USER mssql

ENV ACCEPT_EULA=Y
EXPOSE 1433

ENTRYPOINT ["./entrypoint.sh"]

##########################
# Application Build/Restore
##########################
FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:25d14b400b75fa4e89d5bd4487a92a604a4e409ab65becb91821e7dc4ac7f81f AS app-build

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef

COPY --chmod=755 deploy/dev/app /usr/config/

WORKDIR /App
COPY *.csproj ./

RUN dotnet restore

##########################
# Application Runtime 
##########################
FROM app-build AS app

WORKDIR /App
COPY . ./

RUN dotnet restore

ENV ASPNETCORE_URLS=http://*:5202

EXPOSE 5202

ENTRYPOINT ["/usr/config/entrypoint.sh"]
