#!/bin/sh
echo "USER:" `whoami`

# add xunit3 template
dotnet new install xunit.v3.templates

# Get fonts for PDF generation
file_dir=$(dirname "$0")
sh ${file_dir}/scripts/download-fonts.sh
