#!/usr/bin/env bash

set -euo pipefail

SERVICE_NAME="${1:-}"

if [[ -z "$SERVICE_NAME" ]]; then
  echo "Usage: $0 <ServiceName>"
  echo "Example: $0 Identity"
  exit 1
fi

if [[ ! "$SERVICE_NAME" =~ ^[A-Z][A-Za-z0-9]*$ ]]; then
  echo "Service name must use PascalCase, for example: Identity"
  exit 1
fi

PROJECT_NAME="Vendora"
SOLUTION_FILE="${PROJECT_NAME}.slnx"

if [[ ! -f "$SOLUTION_FILE" ]]; then
  echo "Cannot find $SOLUTION_FILE."
  echo "Run this script from the repository root."
  exit 1
fi

SERVICE_DIR="src/Services/${SERVICE_NAME}"

if [[ -e "$SERVICE_DIR" ]]; then
  echo "Service directory already exists: $SERVICE_DIR"
  exit 1
fi

PROJECT_PREFIX="${PROJECT_NAME}.Services.${SERVICE_NAME}"

DOMAIN_PROJECT_NAME="${PROJECT_PREFIX}.Domain"
APPLICATION_PROJECT_NAME="${PROJECT_PREFIX}.Application"
INFRASTRUCTURE_PROJECT_NAME="${PROJECT_PREFIX}.Infrastructure"
API_PROJECT_NAME="${PROJECT_PREFIX}.Api"

DOMAIN_DIR="${SERVICE_DIR}/Domain"
APPLICATION_DIR="${SERVICE_DIR}/Application"
INFRASTRUCTURE_DIR="${SERVICE_DIR}/Infrastructure"
API_DIR="${SERVICE_DIR}/Api"

DOMAIN_PROJECT="${DOMAIN_DIR}/${DOMAIN_PROJECT_NAME}.csproj"
APPLICATION_PROJECT="${APPLICATION_DIR}/${APPLICATION_PROJECT_NAME}.csproj"
INFRASTRUCTURE_PROJECT="${INFRASTRUCTURE_DIR}/${INFRASTRUCTURE_PROJECT_NAME}.csproj"
API_PROJECT="${API_DIR}/${API_PROJECT_NAME}.csproj"

dotnet new classlib \
  --name "$DOMAIN_PROJECT_NAME" \
  --output "$DOMAIN_DIR" \
  --framework net10.0 \
  --no-restore

dotnet new classlib \
  --name "$APPLICATION_PROJECT_NAME" \
  --output "$APPLICATION_DIR" \
  --framework net10.0 \
  --no-restore

dotnet new classlib \
  --name "$INFRASTRUCTURE_PROJECT_NAME" \
  --output "$INFRASTRUCTURE_DIR" \
  --framework net10.0 \
  --no-restore

dotnet new webapi \
  --name "$API_PROJECT_NAME" \
  --output "$API_DIR" \
  --framework net10.0 \
  --no-restore

dotnet sln "$SOLUTION_FILE" add \
  --solution-folder "Services/${SERVICE_NAME}" \
  "$DOMAIN_PROJECT" \
  "$APPLICATION_PROJECT" \
  "$INFRASTRUCTURE_PROJECT" \
  "$API_PROJECT"

dotnet reference add \
  "$DOMAIN_PROJECT" \
  --project "$APPLICATION_PROJECT"

dotnet reference add \
  "$APPLICATION_PROJECT" \
  --project "$INFRASTRUCTURE_PROJECT"

dotnet reference add \
  "$APPLICATION_PROJECT" \
  "$INFRASTRUCTURE_PROJECT" \
  --project "$API_PROJECT"

dotnet restore "$SOLUTION_FILE"
dotnet build "$SOLUTION_FILE" --no-restore

echo "Created service: $SERVICE_NAME"