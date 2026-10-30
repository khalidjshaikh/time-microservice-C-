#!/usr/bin/env bash
# Build the Docker image for time-microservice-csharp.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

IMAGE="${IMAGE:-time-microservice-csharp}"
TAG="${TAG:-latest}"
PLATFORM="${PLATFORM:-}"

BUILD_ARGS=()
if [[ -n "$PLATFORM" ]]; then
  BUILD_ARGS+=(--platform "$PLATFORM")
fi

docker build "${BUILD_ARGS[@]}" -t "${IMAGE}:${TAG}" .
echo "Built image ${IMAGE}:${TAG}"