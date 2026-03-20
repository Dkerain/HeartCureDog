# 飞盘碰撞检测调试清单

## 最新修复内容（2026-03-20 ver2）

### 架构改变
✅ **碰撞检测已从 FrisbeeGame 移到 Frisbee 脚本**
- 飞盘脚本现在实现 OnTriggerEnter2D()
- 直接检测与 DogControllerr 的碰撞
- 不再依赖距离计算

### 协程启动修复
✅ **添加了多重检查确保协程启动**
- gameObject.SetActive(true)
- this.enabled = true
- 清理旧飞盘防止冲突

---

## 🔍 逐一检查以下配置

### 小狗（Corgi(Clone)）配置

**检查 1: 脚本检查**
```
在Inspector查看"Corgi"或"Corgi(Clone)"：
☐ 是否有 DogControllerr 脚本（必须）
☐ DogControllerr 脚本是否启用（Component 启用）
```

**检查 2: 碰撞器设置**
```
在 Corgi(Clone) 上找到 CircleCollider 2D：
☐ "Is Trigger" = ✓ 勾选（重要！）
☐ Radius 调整为合适大小（如 0.3-0.5）
☐ 碰撞器位置与小狗图片对齐
```

**检查 3: 物理组件**
```
☑ 必须有 Rigidbody 2D 组件（碰撞检测必需）
   - Body Type = Dynamic
   - Gravity Scale = 0
   - Constraints: 冻结所有位置和旋转（Freeze Position X/Y/Z）
```

---

### 飞盘预制体配置

**检查 4: 碰撞器设置**
```
在飞盘预制体上找到 CircleCollider 2D：
☐ "Is Trigger" = ✗ 不勾选（重要！）
☐ Radius 调整为合适大小（如 0.3-0.5）
```

**检查 5: 物理组件**
```
☑ 必须有 Rigidbody 2D 组件
   - Body Type = Dynamic
   - Gravity Scale = 0（飞盘不受重力影响）
   - Constraints = 可以冻结旋转
```

**检查 6: 脚本检查**
```
☐ 飞盘预制体上是否有 Frisbee.cs 脚本
   - 如果没有，FrisbeeGame 会在生成时动态添加
```

---

### FrisbeeGameManager 配置

**检查 7: 组件和脚本**
```
在 Inspector 查看 FrisbeeGameManager：
☐ 添加了 FrisbeeGame.cs 脚本
☐ 脚本组件是否打开/启用
☐ 所有字段都有正确的值赋予
```

**检查 8: 引用检查**
```
在 FrisbeeGame 的 Inspector 中：
☐ Dog Transform → 拖入 Corgi(Clone)
☐ Dog Controller → 拖入 Corgi(Clone) 上的 DogControllerr 脚本
☐ Frisbee Prefab → 拖入飞盘预制体
☐ Hearts Container → 拖入 HeartsContainer
☐ Heart Prefab → 拖入 Heart UI 预制体
☐ Game Over Panel → 拖入 GameOverPanel
```

---

## 🧪 测试调试步骤

### Step 1: Console 日志检查
启动游戏后，在 Unity Console 中查看是否有以下日志：
```
✓ "游戏已初始化！开始生成飞盘..."
✓ "生成了一个新飞盘！"
✓ "飞盘与狗发生碰撞！接住了！" （当狗碰到飞盘时）
✗ "未接住飞盘！剩余生命值: X" （当飞盘飞出左边时）
```

### Step 2: 物理碰撞调试
1. 在 Edit → Physics2D Settings 中确认：
   - 物理引擎正常运行
   - 没有被暂停

2. 在 Game 窗口启用 Gizmos：
   - 顶部工具栏 → Gizmos 下拉 → 勾选显示碰撞器

### Step 3: 逐帧调试
1. 按下 Play 启动游戏
2. 点击"启动游戏"按钮
3. 在 Scene 视图中观察：
   - 小狗的碰撞器边界是否与图片对齐
   - 飞盘生成时是否有碰撞器
4. 让小狗走向飞盘，观察是否触发碰撞事件

---

## ⚠️ 常见问题和解决方案

### 问题 1: "Coroutine couldn't be started..."
**原因**: FrisbeeGameManager GameObject 非活跃
**解决**:
1. 检查 FrisbeeGameManager 初始状态是否为激活
2. 确保"启动游戏"按钮能够正确调用 InitializeGame()
3. 查看 Console 是否有 "游戏已初始化！" 的日志

### 问题 2: 飞盘飞出但没有碰撞检测
**原因**: 碰撞器配置错误（最常见）
**解决**:
1. 使用 Gizmos 查看碰撞器边界
2. 確認：
   - 小狗: CircleCollider2D 且 Is Trigger = ✓
   - 飞盘: CircleCollider2D 且 Is Trigger = ✗
3. 検查 Console 是否有 "飞盘与狗发生碰撞！" 的日志

### 问题 3: 没有生成飞盘
**原因**: Frisbee Prefab 未设置或无效
**解决**:
1. 在 Inspector 中确认 Frisbee Prefab 已正确赋值
2. 查看 Console 中是否有 "飞盘预制体未设置！" 的错误
3. 检查飞盘预制体是否包含必要的组件

### 问题 4: 游戏第二次启动失败
**原因**: 旧飞盘或协程未清理
**解决**:
1. InitializeGame() 已添加了清理逻辑
2. 确保点击按钮而不是直接运行 Start()
3. 查看 Console 中是否有相关错误

---

## 📊 预期行为

### 正常游戏流程
```
点击启动按钮
  ↓
显示 3 颗心
小狗可移动
定期生成飞盘
  ↓
小狗走向飞盘
碰撞检测触发 → "飞盘与狗发生碰撞！接住了！"
难度提升
  ↓
重复或失误
失误 → "未接住飞盘！剩余生命值: X"
  ↓
3 次失误后游戏结束
显示"游戏结束"面板
```

---

## 💡 如果问题仍未解决

1. **保存所有修改并重新加载场景**
   - Ctrl+S 保存
   - Ctrl+X 或 重新打开场景

2. **检查 Unity 版本兼容性**
   - 确保使用支持 Collider2D.OnTriggerEnter2D 的 Unity 版本
   - 建议 Unity 2020 LTS 及以上

3. **完整重新导入脚本**
   - 删除 Assets/Scripts/Game 中的 .cs 文件
   - 从最新版本重新复制

4. **提供关键信息以便排查**
   - Console 中的完整错误日志
   - Unity 版本号
   - 相关 Prefab 的 Inspector 截图（显示所有组件）
