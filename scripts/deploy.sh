#!/usr/bin/env bash
# Deploy time-microservice-csharp to Kubernetes.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

NAMESPACE="${NAMESPACE:-default}"
IMAGE="${IMAGE:-time-microservice-csharp}"
TAG="${TAG:-latest}"

echo "Applying manifests to namespace: ${NAMESPACE}"
kubectl apply -n "${NAMESPACE}" -f k8s/

# Optionally update the image used by the deployment.
if [[ -n "${IMAGE}" ]]; then
  kubectl set image deployment/time-microservice-csharp \
    time-microservice-csharp="${IMAGE}:${TAG}" -n "${NAMESPACE}" --record 2>/dev/null || true
fi

echo "Waiting for rollout to complete..."
kubectl rollout status deployment/time-microservice-csharp -n "${NAMESPACE}" --timeout=120s

echo "Deployment complete."
kubectl get pods,svc -n "${NAMESPACE}" -l app=time-microservice-csharp

echo "kubectl port-forward svc/time-microservice-csharp 8080:80"