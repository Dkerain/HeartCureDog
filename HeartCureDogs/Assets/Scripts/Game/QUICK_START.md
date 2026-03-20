# 飞盘小游戏 - 快速开始指南

## 文件清单
已创建的脚本文件：
- ✅ `Assets/Scripts/Game/FrisbeeGame.cs` - 主游戏管理器
- ✅ `Assets/Scripts/Game/Frisbee.cs` - 飞盘脚本
- ✅ `Assets/Scripts/Game/FrisbeeGameUIManager.cs` - UI管理器
- 📄 `Assets/Scripts/Game/FRISBEE_SETUP_GUIDE.md` - 详细设置指南

## 5分钟快速设置步骤

### Step 1: 创建飞盘预制体（1分钟）
1. 打开 Unity Editor
2. 在 Hierarchy 中右键 → Create Empty → 命名为 `Frisbee`
3. 给 Frisbee 添加组件：
   - **SpriteRenderer**（选择一个圆形图片，如果没有可以使用单色圆点）
   - **CircleCollider 2D**（Radius 设为 0.5）
4. 将 Frisbee 从 Hierarchy 拖到 `Assets/Prefabs` 创建预制体
5. 删除 Hierarchy 中的 Frisbee

### Step 2: 创建心脏UI预制体（1分钟）
1. 在 Hierarchy 中右键 → UI → Image → 命名为 `Heart`
2. 选择心形图片作为 Sprite（或使用任何图片）
3. 调整大小为 50x50
4. 将 Heart 从 Hierarchy 拖到 `Assets/Prefabs` 创建预制体
5. 删除 Hierarchy 中的 Heart

### Step 3: 设置GameCanvas UI（2分钟）
1. 在 GameCanvas 中创建以下结构：
   ```
   GameCanvas
   ├── HeartsContainer (Panel)
   │   └── [心脏UI会动态创建]
   ├── GameOverPanel (Panel)
   │   ├── Text "游戏结束"
   │   ├── Button "重新开始"
   │   └── Button "返回公园"
   └── StartGameButton (Button) - 可选
   ```

2. **HeartsContainer 设置**：
   - 放在左上角
   - 添加组件：**Horizontal Layout Group**

3. **GameOverPanel 设置**：
   - 设为全屏或大面板
   - 初始状态：取消勾选 Active（不显示）
   - 背景色设为半透明黑色

### Step 4: 配置脚本（1分钟）
1. 在 GameCanvas 中创建空 GameObject → 命名为 `FrisbeeGameManager`
2. 添加组件 `FrisbeeGame.cs`
3. 在 Inspector 中填充以下字段（按优先级）：
   - **Dog Transform** → 拖入 Corgi(Clone)
   - **Dog Controller** → 拖入 Dog 上的 DogControllerr 脚本
   - **Frisbee Prefab** → 拖入之前创建的 Frisbee 预制体
   - **Hearts Container** → 拖入 HeartsContainer
   - **Heart Prefab** → 拖入 Heart 预制体
   - **Game Over Panel** → 拖入 GameOverPanel

4. 其他字段保持默认值即可

### Step 5: 绑定按钮事件（自动化）
1. **GameOverPanel 中的"重新开始"按钮**：
   - On Click 事件 → 拖入 FrisbeeGameManager
   - 选择函数：`FrisbeeGame → RestartGame()`

2. **GameOverPanel 中的"返回公园"按钮**：
   - On Click 事件 → 拖入 FrisbeeGameManager
   - 选择函数：`FrisbeeGame → BackToPark()`

3. **（可选）启动游戏按钮**：
   - 在 GameCanvas 中创建一个 Button
   - On Click 事件 → 拖入 FrisbeeGameManager
   - 选择函数：`FrisbeeGame → InitializeGame()`

## 测试游戏

1. 按 Play 启动场景
2. 点击"启动游戏"按钮（或在 Inspector 中调用 InitializeGame()）
3. 看到 3 颗心后游戏开始
4. 点击地图位置移动狗去接飞盘
5. 接住或错过 3 次后游戏结束

## 常见错误排查

| 问题 | 原因 | 解决 |
|------|------|------|
| 没有显示心脏 | Hearts Container 或 Heart Prefab 未设置 | 检查 Inspector 中的字段 |
| 飞盘不显示 | Frisbee Prefab 未设置 | 在 Prefabs 中创建 Frisbee 预制体 |
| 小狗不动 | Dog Controller 未正确配置 | 拖入正确的 DogControllerr 脚本 |
| 游戏不能开始 | FrisbeeGame 未找到 DogController | 检查 Dog Controller 字段是否为空 |

## 下一步优化（可选）

- 添加音效：在 `FrisbeeGame.OnFrisbeeCaught()` 中添加声音
- 添加动画：在 `Frisbee.Catch()` 中添加消失动画
- 显示分数：在 FrisbeeGame 中添加分数逻辑
- 难度曲线：调整 `difficultyMultiplier` 和初始参数
- 游戏统计：保存玩家的最高分到 PlayerPrefs

## 需要帮助？

如果出现问题，请参考详细的 `FRISBEE_SETUP_GUIDE.md` 文件。
