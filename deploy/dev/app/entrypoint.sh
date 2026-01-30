#!/bin/bash

socat TCP-LISTEN:42000,fork,reuseaddr TCP:127.0.0.1:42042 &

# Update database to latest migration
dotnet ef database update &&

# Watch dotnet project
dotnet watch --no-launch-profile --non-interactive
