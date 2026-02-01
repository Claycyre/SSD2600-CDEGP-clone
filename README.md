# Prometheus Atomics Procurement Industries
Asp.NET Core Web Application \
SSD2600 & SSD3300 Group Project

## Group Members
* Clay
* Daniil
* Ethan
* Geoff
* Pawel

---

# Getting Started

## Prerequisites
### Dotnet
[Install Dotnet 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Make sure to have Dotnet SDK 10 installed and accessible via your system PATH 
variable.\
You can check your current Dotnet SDK version using `dotnet --version`

### Docker
[Install Docker](https://docs.docker.com/engine/install/)

### Node/NPM
Check if NPM is available: `npm --version`

[Install using NVM](https://github.com/nvm-sh/nvm?tab=readme-ov-file#installing-and-updating)\
[Install using NVM for Windows](https://github.com/coreybutler/nvm-windows)

## Development
It is strongly recommended to use Docker for development. Using docker compose, you
will benefit from:

* a fresh ephemeral instance of Microsoft SQL Server Express,
* database migrations running automatically when they are created, 
* dotnet refreshing your browser on code changes, and
* automatic dotnet rebuilds/restarts when necessary.

### Development using Docker
Run `docker compose watch` inside a console window to start the development environment.

To watch dotnet application console output, run 
`docker compose logs -f | grep ^app` in a git bash or shell. 
If you are using a Windows terminal such as `cmd` or `powershell`, `grep` is
not available; in this case, run `docker compose logs -f` to watch all container
logs.

> [!IMPORTANT]
> When you are not working, run `docker compose down` to ensure development
> containers are shut down and not taking resources/battery.

If your docker containers do not start for whatever reason, try first tearing
them down using `docker compose down --remove-orphans` and then running
`docker compose build --no-cache` to rebuild them. You might also want to try
deleting the `bin` and `obj` directories inside `<project>/app/` before running
`docker compose build --no-cache`.

> [!CAUTION]
> Do NOT use NuGet Package Manager inside Visual Studio to run docker commands.
> Commands that are designed to run indefinitely will hijack the NuGet console
> and you will need to manually terminate the command process. (painful)

---

# Contributing
## Branching policy

Create branches from the latest commit on the `main` branch. (remember to `git pull`)\
Name branches in this pattern `<your name>/<feature-fix-or-change>`.

ex. `pawel/add-button-styles`

The branch name after the first `/` should generally convey what you want to 
change when the branch is merged with `main`.

> [!IMPORTANT]
> Once you have completed work on the branch, submit a pull request for review.
> Changes on your branch will not be reviewed until a pull request is submitted.

## Semantic commit messages
Try writing your commit messages according to [this specification](https://www.conventionalcommits.org/en/v1.0.0/).
This is an industry standard specification for commit messages used by many
organizations.

ex. `feat: add button to CTA`\
ex. `fix(nav): link not clickable`\
ex. `chore: rewrite article copy`

## CSS Style Guide

Use Tailwind utility classes to style your HTML:
[Tailwind Docs](https://tailwindcss.com/docs/styling-with-utility-classes)

Example:
```html
<button class="px-2 py-1 bg-gray-100 text-black hover:bg-gray-50">
    Click here
</button>
```
