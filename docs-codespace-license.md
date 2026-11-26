# 用 Codespaces 云端桌面拿 Unity 个人版许可（.ulf）

1. 仓库页 → Code → Codespaces → **Create codespace on main**（2 核/8GB 即可）
2. 等 3–5 分钟创建完成；首次会自动装云端桌面 + Unity Hub（`scripts/setup-unityhub.sh`）
3. 打开 **Ports** 面板 → 端口 **6080** → 点地球图标在浏览器打开（密码 `vscode`）
4. 云端桌面里双击 **Unity Hub** 图标 → 登录 Unity 账号 → `Preferences → Licenses → Add → Get a free personal license`
5. 回到 Codespaces 终端（VS Code 下方 Terminal）运行：
   ```bash
   bash scripts/finish-license.sh
   ```
6. 脚本会找到 `~/.local/share/unity3d/Unity/Unity_lic.ulf` 并尝试自动写入 `UNITY_LICENSE` secret；
   若失败，按提示复制文件内容，在 GitHub 仓库 Settings → Secrets → Actions 新建 `UNITY_LICENSE` 粘贴。
7. 用完记得 **停止/删除 Codespace**（否则占用免费额度）。
