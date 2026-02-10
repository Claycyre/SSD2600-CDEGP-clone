##########################
# Common Application Image
##########################
FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:25d14b400b75fa4e89d5bd4487a92a604a4e409ab65becb91821e7dc4ac7f81f AS app-common

RUN curl -fsSL https://deb.nodesource.com/setup_24.x | bash -

RUN apt-get update && \
  apt-get install -y nodejs
