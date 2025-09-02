#!/usr/bin/env bash
set -euo pipefail

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet not found, skip build/test"
  exit 0
fi

dotnet restore
dotnet build -warnaserror
dotnet test
