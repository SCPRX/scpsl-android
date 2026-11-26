#!/usr/bin/env bash
# 找到 Unity Hub 生成的 .ulf 并尝试直接写入 GitHub secret
set -uo pipefail
ULF=$(find "$HOME" /root -name 'Unity_lic.ulf' 2>/dev/null | head -1)
if [ -z "${ULF:-}" ]; then
  echo "❌ 没找到 Unity_lic.ulf。请先在 Unity Hub 里完成：Preferences -> Licenses -> Add -> Get a free personal license"
  exit 1
fi
echo "✅ 找到许可证: $ULF  ($(wc -c < "$ULF") 字节)"
echo "--- 内容前 3 行 ---"
head -3 "$ULF"
echo "---------------------"
if command -v gh >/dev/null 2>&1; then
  if gh secret set UNITY_LICENSE < "$ULF"; then
    echo "✅ 已直接写入 GitHub secret: UNITY_LICENSE"
    exit 0
  fi
fi
echo "⚠️ 自动写入失败（codespace token 无 secrets 权限属正常）。"
echo "请用 VS Code 打开这个文件，Ctrl+A 全选、Ctrl+C 复制，然后到 GitHub 仓库 Settings -> Secrets -> UNITY_LICENSE 粘贴："
echo "   $ULF"
