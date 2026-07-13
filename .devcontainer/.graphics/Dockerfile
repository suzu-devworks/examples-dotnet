FROM mcr.microsoft.com/devcontainers/dotnet:10.0 AS develop-stage

# Avoid warnings by switching to noninteractive
ENV DEBIAN_FRONTEND=noninteractive

# Configure apt and install packages
RUN set -x \
    && apt update \
    && apt -y install --no-install-recommends apt-utils dialog 2>&1 \
    # Install font libraries
    # && apt -y install fonts-ubuntu \
    # && apt -y install fonts-roboto-mono \
    && apt -y install fonts-noto-cjk \
    && apt -y install fonts-noto-color-emoji \
    # Use system fontconfig to avoid warnings
    # && apt -y install fontconfig libfontconfig1 \
    # && fc-cache -fv \
    ;

# Clean up
RUN set -x \
    && apt autoremove -y \
    && apt clean -y \
    && rm -rf /var/lib/apt/lists/* \
    ;

# Switch back to dialog for any ad-hoc use of apt-get
ENV DEBIAN_FRONTEND=dialog
