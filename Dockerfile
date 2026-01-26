FROM mcr.microsoft.com/mssql/server:2022-latest AS backend-dev

# Change to root user for config
USER root

# Create a config directory
RUN mkdir -p /usr/config
WORKDIR /usr/config

# Bundle config source
COPY --chmod=755 --chown=mmsql deploy/dev /usr/config/

# Change back to default image user
USER mssql

EXPOSE 1433

ENTRYPOINT ["./entrypoint.sh"]
