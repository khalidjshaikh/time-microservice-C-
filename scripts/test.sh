#!/usr/bin/env bash
# Run the test suite.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

dotnet test tests/TimeMicroservice.Tests/TimeMicroservice.Tests.csproj --verbosity normal

touch coverage.out

echo ""
echo "Tests complete."