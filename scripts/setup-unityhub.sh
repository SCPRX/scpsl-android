#!/usr/bin/env bash
# 在 Codespaces 云端 Linux 桌面里安装 Unity Hub（AppImage 解包方式，免 FUSE）
set -euxo pipefail
export DEBIAN_FRONTEND=noninteractive
sudo apt-get update -y
sudo apt-get install -y --no-install-recommends \
  wget ca-certificates unzip xz-utils dbus-x11 xdotool x11-utils \
  libnss3 libgbm1 libasound2t64 libxss1 libgtk-3-0 libatk-bridge2.0-0 \
  libdrm2 libxkbcommon0 libxcomposite1 libxdamage1 libxrandr2 libpango-1.0-0 libcups2 || true

mkdir -p "$HOME/unityhub"
cd "$HOME/unityhub"
if [ ! -x squashfs-root/unityhub ]; then
  wget -q -O UnityHub.AppImage "https://public-cdn.cloud.unity3d.com/hub/prod/UnityHub.AppImage"
  chmod +x UnityHub.AppImage
  ./UnityHub.AppImage --appimage-extract >/dev/null
fi

# 启动脚本（Electron 在容器里需要 --no-sandbox，指定虚拟显示 :1）
cat > "$HOME/start-unityhub.sh" <<'INNER'
#!/bin/bash
export DISPLAY=:1
cd "$HOME/unityhub"
if command -v dbus-run-session >/dev/null 2>&1; then
  exec dbus-run-session -- ./squashfs-root/unityhub --no-sandbox --disable-gpu "$@"
else
  exec ./squashfs-root/unityhub --no-sandbox --disable-gpu "$@"
fi
INNER
chmod +x "$HOME/start-unityhub.sh"

# 桌面快捷方式 + unityhub:// 协议处理器（登录回跳用）
mkdir -p "$HOME/Desktop" "$HOME/.local/share/applications"
cat > "$HOME/.local/share/applications/unityhub.desktop" <<INNER
[Desktop Entry]
Name=Unity Hub
Comment=Unity Hub (cloud desktop)
Exec=$HOME/start-unityhub.sh %u
Type=Application
Terminal=false
MimeType=x-scheme-handler/unityhub;
INNER
update-desktop-database "$HOME/.local/share/applications" 2>/dev/null || true
xdg-mime default unityhub.desktop x-scheme-handler/unityhub 2>/dev/null || true
cp "$HOME/.local/share/applications/unityhub.desktop" "$HOME/Desktop/UnityHub.desktop"
chmod +x "$HOME/Desktop/UnityHub.desktop" 2>/dev/null || true
echo "SETUP_DONE: Unity Hub 已安装。双击桌面 Unity Hub 图标即可登录激活。"
