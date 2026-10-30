#!/usr/bin/env bash

set -euo pipefail

script_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd -- "${script_dir}/.." && pwd)"
src_dir="${repo_root}/src"

shared_files=(
    .github
    .markdownlint-cli2.yaml
    .prettierignore
    .prettierrc
    AGENTS.md
)

# Add category-specific links as "workspace-directory/file-name" entries.
workspace_files=(
    compiler_and_roslyn/dotnet-tools.json
    databases/dotnet-tools.json
    serialization/buf.yaml
    web/buf.yaml
    web/dotnet-tools.json
)

ensure_link() {
    local workspace_dir="$1"
    local name="$2"
    local link_path="${workspace_dir}/${name}" link_target="../../${name}"

    [[ -e "${repo_root}/${name}" ]] || {
        printf 'Error: root target not found: %s\n' "${repo_root}/${name}" >&2
        return 1
    }
    [[ -d "${repo_root}/${name}" ]] && link_target+='/'

    if [[ -L "${link_path}" ]]; then
        [[ "$(readlink -- "${link_path}")" == "${link_target}" ]] && return 0
        printf 'Error: unexpected symlink target: %s -> %s\n' \
            "${link_path}" "$(readlink -- "${link_path}")" >&2
        return 1
    elif [[ -e "${link_path}" ]]; then
        printf 'Error: refusing to replace existing path: %s\n' "${link_path}" >&2
        return 1
    else
        ln -s "${link_target}" "${link_path}"
        printf 'Created %s -> %s\n' "${link_path}" "${link_target}"
    fi
}

workspace_count=0
for workspace_file in "${src_dir}"/*/*.code-workspace; do
    [[ -f "${workspace_file}" ]] || continue
    workspace_dir="$(dirname -- "${workspace_file}")"
    workspace_name="$(basename -- "${workspace_dir}")"
    workspace_count=$((workspace_count + 1))

    for shared_file in "${shared_files[@]}"; do ensure_link "${workspace_dir}" "${shared_file}"; done

    for entry in "${workspace_files[@]}"; do
        [[ "${entry}" == "${workspace_name}/"* ]] && ensure_link "${workspace_dir}" "${entry#*/}"
    done
done

if (( workspace_count == 0 )); then
    printf 'Error: no workspace files found under %s\n' "${src_dir}" >&2
    exit 1
fi

printf 'Workspace links are up to date for %d workspace(s).\n' "${workspace_count}"
