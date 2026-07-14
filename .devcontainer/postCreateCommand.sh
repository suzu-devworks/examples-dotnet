#!/bin/sh
script_user=`whoami`
script_dir=$(realpath "$(dirname "$0")")

echo "USER:" ${script_user}
echo "DIR:" ${script_dir}
echo

sh ${script_dir}/scripts/install-xunit3template.sh

dotnet tool restore --ignore-failed-sources
