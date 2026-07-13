# Examples.Graphics.SkiaSharp

## Use Font in Container

### When using `SkiaSharp.NativeAssets.Linux` package

It uses the Linux OS font system.

```bash
# Install fontconfig package
sudo sudo apt update
sudo apt install -y fontconfig libfontconfig1
sudo apt install -y fonts-ubuntu fonts-noto-cjk fonts-noto-color-emoji
```

### When using `SkiaSharp.NativeAssets.Linux.NoDependencies` package

Specify the font file explicitly in the program.

download font file:

```bash
mkdir -p ~/.local/share/fonts
curl -o ~/.local/share/fonts/NotoSansJP-Regular.otf -LO https://fonts.google.com/download?family=Noto%20Sans%20JP
curl -o ~/.local/share/fonts/NotoSerifJP-Regular.otf -LO https://fonts.google.com/download?family=Noto%20Serif%20JP
```
