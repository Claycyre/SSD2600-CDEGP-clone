# https://hub.docker.com/_/microsoft-dotnet
FROM app-common AS app-prod-intermediate
WORKDIR /source

# copy csproj and restore as distinct layers
COPY app/*.sln .
COPY app/*.csproj .

# copy everything else and build app
COPY app/ .
RUN dotnet restore
RUN npm install
RUN dotnet publish SSD2600_CDEGP.csproj -c Release -o ./app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=app-prod-intermediate /source/app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SSD2600_CDEGP.dll"]
