#!/usr/bin/env bash
set -euo pipefail

(cd ops/compose && docker compose up -d)
