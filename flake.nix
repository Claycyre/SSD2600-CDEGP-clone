# flake.nix - Nix Flake configuration file
# This file defines a reproducible development environment using Nix flakes
# Documentation: https://nixos.wiki/wiki/Flakes
{
  description = "SSDP 2600 Project environment";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    # This function creates outputs for each system (x86_64-linux, aarch64-darwin, etc.)
    flake-utils.lib.eachDefaultSystem (system:
      let pkgs = import nixpkgs { system = "x86_64-linux"; };
      in {
        devShells.default = pkgs.mkShell {
          name = "SSD2600";
          nativeBuildInputs = with pkgs; [ zsh ];

          buildInputs = with pkgs; [ git dotnet-sdk jre_minimal ];
        };
      });
}
