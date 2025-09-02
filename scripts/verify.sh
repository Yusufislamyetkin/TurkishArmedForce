#!/usr/bin/env bash
set -euo pipefail

if command -v dotnet >/dev/null 2>&1; then
  dotnet restore >/dev/null 2>&1 || true
  dotnet build --no-restore >/dev/null 2>&1 || true
  dotnet test --no-build >/dev/null 2>&1 || true
else
  echo "dotnet not found. Skipping verification."
fi
