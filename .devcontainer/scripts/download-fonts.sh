#!/bin/sh
# spell-checker: words Noto wght
font_dir="$HOME/.local/share/fonts"
mkdir -p "$font_dir"

if [ ! -f "${font_dir}/NotoSansJP[wght].ttf" ]; then
  echo "Downloading NotoSansJP[wght].ttf font..."
  curl -L -o "${font_dir}/NotoSansJP[wght].ttf" https://github.com/google/fonts/raw/refs/heads/main/ofl/notosansjp/NotoSansJP%5Bwght%5D.ttf
  echo
fi

if [ ! -f "${font_dir}/NotoSerifJP[wght].ttf" ]; then
  echo "Downloading NotoSerifJP[wght].ttf font..."
  curl -L -o "${font_dir}/NotoSerifJP[wght].ttf" https://github.com/google/fonts/raw/refs/heads/main/ofl/notoserifjp/NotoSerifJP%5Bwght%5D.ttf
fi
