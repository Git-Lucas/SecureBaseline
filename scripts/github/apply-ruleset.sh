#!/usr/bin/env bash
# Applies the versioned ruleset in .github/rulesets/default-branch.json to the
# GitHub repository's default branch. Run locally by the maintainer only, with
# a short-lived fine-grained token (Administration: read and write). See
# docs/procedimentos/configuracao-do-repositorio-github.md for the full procedure.
set -euo pipefail

REPO_OWNER="Git-Lucas"
REPO_NAME="SecureBaseline"
RULESET_FILE="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)/.github/rulesets/default-branch.json"
API_VERSION="2022-11-28"
API_BASE="https://api.github.com/repos/${REPO_OWNER}/${REPO_NAME}"

WITHOUT_REQUIRED_CHECKS=false
for arg in "$@"; do
  case "$arg" in
    --without-required-checks)
      WITHOUT_REQUIRED_CHECKS=true
      ;;
    *)
      echo "Usage: $0 [--without-required-checks]" >&2
      exit 1
      ;;
  esac
done

if [[ ! -f "$RULESET_FILE" ]]; then
  echo "Ruleset file not found: $RULESET_FILE" >&2
  exit 1
fi

command -v curl >/dev/null 2>&1 || { echo "curl is required" >&2; exit 1; }
command -v python3 >/dev/null 2>&1 || { echo "python3 is required" >&2; exit 1; }

read -r -s -p "GitHub fine-grained token (Administration: read and write, this repo only): " GITHUB_TOKEN
echo
if [[ -z "$GITHUB_TOKEN" ]]; then
  echo "No token entered." >&2
  exit 1
fi

# Build the request body: the versioned JSON, optionally stripped of the
# required_status_checks rule for the bootstrap run (--without-required-checks).
REQUEST_BODY="$(WITHOUT_REQUIRED_CHECKS="$WITHOUT_REQUIRED_CHECKS" python3 - "$RULESET_FILE" <<'PY'
import json
import os
import sys

with open(sys.argv[1], encoding="utf-8") as f:
    ruleset = json.load(f)

if os.environ.get("WITHOUT_REQUIRED_CHECKS") == "true":
    ruleset["rules"] = [r for r in ruleset["rules"] if r.get("type") != "required_status_checks"]

print(json.dumps(ruleset))
PY
)"
RULESET_NAME="$(python3 -c 'import json,sys; print(json.loads(sys.argv[1])["name"])' "$REQUEST_BODY")"

# curl helper: performs the request, prints the response body on stdout and
# fails loudly (with the response body on stderr) on a non-2xx status.
call_api() {
  local method="$1" url="$2" body="${3:-}"
  local response status http_body
  local -a curl_args=(
    -sS -o - -w '\n%{http_code}'
    -X "$method"
    -H "Authorization: Bearer ${GITHUB_TOKEN}"
    -H "Accept: application/vnd.github+json"
    -H "X-GitHub-Api-Version: ${API_VERSION}"
  )
  if [[ -n "$body" ]]; then
    curl_args+=(-H "Content-Type: application/json" -d "$body")
  fi
  response="$(curl "${curl_args[@]}" "$url")"
  status="${response##*$'\n'}"
  http_body="${response%$'\n'*}"
  if [[ "$status" -lt 200 || "$status" -ge 300 ]]; then
    echo "GitHub API request failed (HTTP $status): $method $url" >&2
    echo "$http_body" >&2
    exit 1
  fi
  echo "$http_body"
}

# Find an existing ruleset with the same name so the script stays idempotent:
# create it on the first run, update it in place on every later run.
EXISTING_ID="$(call_api GET "${API_BASE}/rulesets" | python3 -c '
import json, sys
name = sys.argv[1]
rulesets = json.load(sys.stdin)
for r in rulesets:
    if r.get("name") == name:
        print(r["id"])
        break
' "$RULESET_NAME")"

if [[ -n "$EXISTING_ID" ]]; then
  echo "Updating existing ruleset '${RULESET_NAME}' (id ${EXISTING_ID})..."
  call_api PUT "${API_BASE}/rulesets/${EXISTING_ID}" "$REQUEST_BODY" >/dev/null
else
  echo "Creating ruleset '${RULESET_NAME}'..."
  call_api POST "${API_BASE}/rulesets" "$REQUEST_BODY" >/dev/null
fi

unset GITHUB_TOKEN
echo "Done. Confirm the ruleset in the GitHub UI (Settings > Rules > Rulesets)."
