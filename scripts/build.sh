#!/usr/bin/env bash
# Build the time-microservice-csharp binary for the host platform.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

mkdir -p bin

VERSION="${VERSION:-$(git describe --tags --always --dirty 2>/dev/null || echo dev)}"
COMMIT="${COMMIT:-$(git rev-parse --short HEAD 2>/dev/null || echo none)}"
BUILD_DATE="${BUILD_DATE:-$(date -u +%Y-%m-%dT%H:%M:%SZ)}"

dotnet build -c Release 2>&1 | tail -n 20

# Publish output into bin/ for archiving.
dotnet publish -c Release -o bin/publish >/dev/null 2>&1 || true

# Launcher script so `bin/time-microservice-csharp` runs the service.
cat > bin/time-microservice-csharp <<'LAUNCHER'
#!/usr/bin/env bash
# Launcher for the time-microservice-csharp .NET application.
DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
exec dotnet "$DIR/publish/time-microservice-csharp.dll" "$@"
LAUNCHER
chmod +x bin/time-microservice-csharp

echo "Built bin/time-microservice-csharp"