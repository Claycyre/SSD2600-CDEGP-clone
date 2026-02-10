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
COPY --chmod=755 deploy/dev/mssql /usr/config/

# Change back to default image user
USER mssql

ENV ACCEPT_EULA=Y
EXPOSE 1433

ENTRYPOINT ["./entrypoint.sh"]

##########################
# Dev App Intermediate 
##########################
FROM app-common AS app-dev-intermediate

RUN apt-get install -y socat

COPY --chmod=755 deploy/dev/app /usr/config/

##########################
# Dev Application Runtime 
##########################
FROM app-dev-intermediate AS app-dev

WORKDIR /App
COPY ./app ./

RUN dotnet restore
RUN dotnet tool restore
RUN npm install

ENV ASPNETCORE_URLS=http://*:5202
ENV DOTNET_ENVIRONMENT=Development

ENV DOTNET_WATCH_AUTO_RELOAD_WS_PORT=42042

EXPOSE 5202

ENTRYPOINT ["/usr/config/entrypoint.sh"]
