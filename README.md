# SCPSL Android CI（冒烟工程）

用 GitHub Actions + GameCI 构建 Unity 2019.4.16f1 Android APK。
当前是**冒烟版**：用 `Assets/Editor/CIBuild.cs` 现场生成一个「相机+灯光+立方体」场景并打 APK，用来验证：
- Unity Personal 许可在 CI 的激活
- GameCI 镜像 `unityci/editor:ubuntu-2019.4.16f1-android-3` 可用
- APK 能出、能下载、能装到手机

冒烟通过后，再把 AssetRipper 导出的完整工程（6 场景/2367 脚本/887MB）换进来。

## 一次性配置（手机浏览器可完成）

1. **创建 GitHub 仓库**（Public 或 Private 均可，Public 不消耗 Actions 分钟）
2. **Unity 账号**：https://id.unity.com 注册/登录（Personal 免费）
3. 仓库 Settings → Secrets and variables → Actions 添加三个 secret：
   - `UNITY_EMAIL`：Unity 账号邮箱
   - `UNITY_PASSWORD`：Unity 账号密码
   - `UNITY_LICENSE`：Unity 许可文件 `.ulf` 的**原文**（见下方获取步骤）
4. **获取 .ulf（激活文件）**：
   - 先推送本仓库代码（含 `.github/workflows/build.yml`）
   - 在 GitHub Actions 手动跑一次（workflow_dispatch）；若因许可证失败，按 GameCI 官方文档用 `game-ci/unity-request-activation-file` 生成 `.alf`，上传到 https://license.unity3d.com/manual 下载 `.ulf`
   - 打开 `.ulf`，把**完整内容**粘贴到 `UNITY_LICENSE` secret
5. 再次运行 workflow，等 10–20 分钟，在 Actions run 页面底部下载 `Smoke-APK` artifact

## 本地命令（可选）

```bash
# 推送到你自己的仓库（把 URL 换成你的）
git init && git add . && git commit -m "smoke" && git branch -M main
git remote add origin https://github.com/<你的用户名>/<仓库名>.git
git push -u origin main
```
