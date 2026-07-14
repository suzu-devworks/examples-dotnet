#!/bin/sh
script_dir=$(realpath "$(dirname "$0")")
password_file="${script_dir}/../.db_password"

cat /dev/urandom | LC_CTYPE=C tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | fold -w 24 | head -n 1 > "${password_file}"
