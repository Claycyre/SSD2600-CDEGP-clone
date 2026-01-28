#!/bin/bash

# Update database to latest migration
dotnet ef database update &&

# Watch dotnet project
dotnet watch --no-launch-profile --non-interactive
