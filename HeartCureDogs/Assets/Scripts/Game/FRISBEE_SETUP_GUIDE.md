# 飞盘接取小游戏设置指南

## 概述
这个飞盘接取小游戏系统包括以下脚本：
- `FrisbeeGame.cs` - 主游戏管理器
- `Frisbee.cs` - 飞盘脚本

## 需要创建的资源

### 1. 飞盘预制体 (Frisbee Prefab)

#### 在 Unity Editor 中创建飞盘预制体：
1. 在 Hierarchy 中右键 → Create Empty → 命名为 "Frisbee"
2. 添加组件：
   - **SpriteRenderer** - 显示飞盘图片
     - 在 Sprite 字段中选择一个圆形或飞盘形状的图片
     - 可以在 DogSprites 或 Image 文件夹中找
   - **CircleCollider 2D** - 用于碰撞检测
     - Radius 设置为合适的大小（如 0.5）
3. 在 Inspector 中将此 GameObject 拖入 `Assets/Prefabs` 文件夹，创建预制体
4. 删除 Hierarchy 中的 Frisbee GameObject

### 2. 心脏 UI 预制体 (Heart Prefab)

#### 在 Unity Editor 中创建心脏 UI 预制体：
1. 在 Hierarchy 中右键 → UI → Image → 命名为 "Heart"
2. 设置 Image 组件：
   - 从 Image 文件夹中选择一个心形图片作为 Sprite
   - 设置合适的大小（如 50x50）
3. 在 Inspector 中将此 GameObject 拖入 `Assets/Prefabs` 文件夹，创建预制体
4. 删除 Hierarchy 中的 Heart GameObject

### 3. 在 GameCanvas 中创建 UI 结构

1. **创建心脏容器 (Hearts Container)**
   - 在 GameCanvas 中右键 → UI → Panel 或 Horizontal Layout Group → 命名为 "HeartsContainer"
   - 放在左上角
   - 添加 **Horizontal Layout Group** 组件，设置间距

2. **创建游戏结束面板 (Game Over Panel)**
   - 在 GameCanvas 中右键 → UI → Panel → 命名为 "GameOverPanel"
   - 设置为全屏或大面板，用半透明黑色背景
   - 添加子元素：
     - Text "游戏结束" (标题)
     - Button "重新开始" - 绑定到 `FrisbeeGame.RestartGame()`
     - Button "返回公园" - 绑定到 `FrisbeeGame.BackToPark()`
   - 初始状态设置为不活跃 (Unchecked "Active")

## 在 Unity Inspector 中配置 FrisbeeGame

1. **创建一个空 GameObject 存放脚本**
   - 在 GameCanvas 下右键 → Create Empty → 命名为 "FrisbeeGameManager"
   - 添加组件：`FrisbeeGame.cs` (Add Component → FrisbeeGame)

2. **在 Inspector 中填充以下字段**：

| 字段 | 值 | 描述 |
|------|-----|------|
| Max Hearts | 3 | 初始生命值 |
| Frisbee Spawn Interval | 2 | 初始飞盘生成间隔（秒） |
| Difficulty Multiplier | 1.05 | 难度递增系数（>1 表示越来越难） |
| Frisbee Prefab | (拖入 Frisbee 预制体) | 要生成的飞盘 |
| Frisbee Speed | 5 | 初始飞盘速度 |
| Spawn X Position | 10 | 飞盘生成时的 X 坐标（屏幕右边） |
| Spawn Y Range | (2, 6) | 飞盘 Y 坐标的随机范围 |
| Dog Transform | (拖入小狗 Transform) | Corgi(Clone) 的 Transform |
| Catch Distance | 0.5 | 接住飞盘的距离阈值 |
| Hearts Container | (拖入 HeartsContainer) | 放置心脏 UI 的父容器 |
| Heart Prefab | (拖入 Heart 预制体) | 单个心脏 UI |
| Game Over Panel | (拖入 GameOverPanel) | 游戏结束时显示的面板 |
| Dog Controller | (拖入 DogControllerr) | Dog 上的 DogControllerr 脚本 |

## 启动小游戏

### 选项 A：通过按钮启动
1. 在 GameCanvas 中创建一个 Button "开始飞盘游戏"
2. 在 Button 的 On Click 事件中：
   - 拖入 FrisbeeGameManager GameObject
   - 选择函数：`FrisbeeGame → InitializeGame()`

### 选项 B：自动启动
在 FrisbeeGame.cs 的 Start() 方法中，`InitializeGame()` 会自动被调用。

## 游戏流程

1. **游戏开始**：
   - 显示 3 颗心
   - 启用小狗的移动功能
   - 开始定时生成飞盘

2. **飞盘飞行**：
   - 从屏幕右边随机高度飞出
   - 从右向左移动
   - 玩家点击地图位置让小狗移动去接

3. **碰撞检测**：
   - 如果小狗与飞盘足够接近（<= Catch Distance）：
     - 飞盘被接住
     - 难度提升（生成间隔变短，飞盘速度变快）
   - 如果飞盘飞出左边界：
     - 失误一次，扣除 1 颗心
     - 显示剩余心数

4. **游戏结束**：
   - 当心数为 0 时，游戏结束
   - 禁用小狗移动
   - 显示"游戏结束"面板
   - 提供"重新开始"和"返回公园"选项

## 调整游戏难度

修改以下参数：
- `frisbeeSpawnInterval` - 生成间隔（更小 = 更频繁）
- `frisbeeSpeed` - 飞盘速度
- `difficultyMultiplier` - 难度递增速率（推荐 1.05 - 1.1）
- `catchDistance` - 接住距离（更大 = 更容易）

## 常见问题

**Q: 为什么没有显示心脏？**
A: 检查 Hearts Container 和 Heart Prefab 是否正确设置，Heart Prefab 中是否有 Image 组件。

**Q: 小狗不显示/飞盘无法生成？**
A: 检查 Dog Transform 和 Frisbee Prefab 是否正确赋值。

**Q: 飞盘飞出屏幕边界后没有扣心？**
A: 检查 Spawn X Position 的值是否超出相机范围，调整 Catch Distance 的值。

## 脚本 API

### FrisbeeGame
- `InitializeGame()` - 初始化和开始游戏
- `RestartGame()` - 重新开始游戏
- `BackToPark()` - 返回公园并隐藏游戏UI
- `OnFrisbeeCaught()` - 接住飞盘时的内部回调
- `OnFrisbeeMissed()` - 失误时的内部回调

### Frisbee
- `SetSpeed(float speed)` - 设置飞盘速度
- `Catch()` - 标记飞盘为已接住并销毁
