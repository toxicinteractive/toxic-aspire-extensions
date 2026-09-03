#!/usr/bin/env bash

# this script is run on "postCreateCommand" in the container AFTER the container is CREATED for the first time
# https://containers.dev/implementors/json_reference/

# if the image doesn't load ip_tables modules dockerd will fail to launch (docker-in-docker)
# https://github.com/devcontainers/features/issues/1235
sudo update-alternatives --set iptables /usr/sbin/iptables-nft

sudo chown -R $USER $AZURE_CONFIG_DIR

dotnet new install Aspire.ProjectTemplates
dotnet tool install -g Aspire.Cli
dotnet tool install -g dotnet-outdated-tool
