# 禾序 0.11.3：382步真实基准回放

种子731，382步，状态 `passed`，总主农场游戏时钟 03:01:46。

这是逐步骤参考记录。推荐使用同包 README 的分段开局与自动控制器，更适合人工测试。以下代码需逐步执行，购买步骤需在科技树点击，不是一个可拼接运行的程序。库存为该固定回放的结果，随机种子和人工等待会造成差异。普通模式保持未解锁语法关闭；竞速预设拥有独立的语法权限。

## 001 · 起步：手动收获五份干草

类型：`bootstrap`；状态：`passed`；主农场 00:00:00 → 00:00:16。

重复运行 harvest()；等待自然成熟，不启用任何语法或成长加成。

```python
harvest()
```

结束库存：hay=5.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：``。

## 002 · 研究 loops → 1

类型：`research`；状态：`passed`；主农场 00:00:16 → 00:00:16。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=0.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1`。

## 003 · 起步循环攒干草至 60

类型：`bootstrap-loop`；状态：`passed`；主农场 00:00:16 → 00:03:18。

玩家运行 while True 收获，在目标达成后点击停止。

```python
while True:
    harvest()
```

结束库存：hay=60.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1`。

## 004 · 研究 operators → 1

类型：`research`；状态：`passed`；主农场 00:03:18 → 00:03:18。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=50.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1`。

## 005 · 研究 variables → 1

类型：`research`；状态：`passed`；主农场 00:03:18 → 00:03:18。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=35.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1`。

## 006 · 研究 senses → 1

类型：`research`；状态：`passed`；主农场 00:03:18 → 00:03:18。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=25.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1`。

## 007 · 研究 plant → 1

类型：`research`；状态：`passed`；主农场 00:03:18 → 00:03:18。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=20.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1`。

## 008 · 研究 expand → 1

类型：`research`；状态：`passed`；主农场 00:03:18 → 00:03:18。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=12.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1`。

## 009 · 生产 hay 至 50

类型：`production`；状态：`passed`；主农场 00:03:18 → 00:04:13。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<50:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=52.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1`。

## 010 · 研究 grass → 1

类型：`research`；状态：`passed`；主农场 00:04:13 → 00:04:13。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=2.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1, grass:1`。

## 011 · 生产 hay 至 57

类型：`production`；状态：`passed`；主农场 00:04:13 → 00:04:53。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<57:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=58.000，wood=0.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1, grass:1`。

## 012 · 生产 wood 至 30

类型：`production`；状态：`passed`；主农场 00:04:53 → 00:06:01。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<30:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Bush)
            move(East)
        move(North)
```

结束库存：hay=58.000，wood=32.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1, grass:1`。

## 013 · 研究 trees → 1

类型：`research`；状态：`passed`；主农场 00:06:01 → 00:06:01。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=58.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:1, grass:1, trees:1`。

## 014 · 研究 expand → 2

类型：`research`；状态：`passed`；主农场 00:06:01 → 00:06:01。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=34.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:1, trees:1`。

## 015 · 生产 hay 至 145

类型：`production`；状态：`passed`；主农场 00:06:01 → 00:07:13。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<145:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=154.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:1, trees:1`。

## 016 · 研究 grass → 2

类型：`research`；状态：`passed`；主农场 00:07:13 → 00:07:13。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=9.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1`。

## 017 · 生产 hay 至 20

类型：`production`；状态：`passed`；主农场 00:07:13 → 00:07:30。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<20:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=57.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1`。

## 018 · 研究 speed → 1

类型：`research`；状态：`passed`；主农场 00:07:30 → 00:07:30。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=2.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1`。

## 019 · 生产 wood 至 20

类型：`production`；状态：`passed`；主农场 00:07:30 → 00:08:06。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<20:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

结束库存：hay=37.000，wood=50.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1`。

## 020 · 研究 functions → 1

类型：`research`；状态：`passed`；主农场 00:08:06 → 00:08:06。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=30.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1`。

## 021 · 生产 wood 至 50

类型：`production`；状态：`passed`；主农场 00:08:06 → 00:08:42。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<50:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

结束库存：hay=37.000，wood=78.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1`。

## 022 · 研究 lists → 1

类型：`research`；状态：`passed`；主农场 00:08:42 → 00:08:42。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=28.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1`。

## 023 · 研究 costs → 1

类型：`research`；状态：`passed`；主农场 00:08:42 → 00:08:42。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=18.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1`。

## 024 · 生产 wood 至 20

类型：`production`；状态：`passed`；主农场 00:08:42 → 00:09:17。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<20:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

结束库存：hay=37.000，wood=66.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1`。

## 025 · 研究 timing → 1

类型：`research`；状态：`passed`；主农场 00:09:17 → 00:09:17。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=46.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1`。

## 026 · 研究 hats → 1

类型：`research`；状态：`passed`；主农场 00:09:17 → 00:09:17。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=21.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1`。

## 027 · 生产 wood 至 445

类型：`production`；状态：`passed`；主农场 00:09:17 → 00:11:53。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<445:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

结束库存：hay=37.000，wood=453.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1`。

## 028 · 研究 carrots → 1

类型：`research`；状态：`passed`；主农场 00:11:53 → 00:11:53。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=37.000，wood=413.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 029 · 生产 hay 至 421

类型：`production`；状态：`passed`；主农场 00:11:53 → 00:13:03。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<421:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=453.000，wood=413.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:2, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 030 · 研究 grass → 3

类型：`research`；状态：`passed`；主农场 00:13:03 → 00:13:03。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=32.000，wood=413.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 031 · 生产 hay 至 58

类型：`production`；状态：`passed`；主农场 00:13:03 → 00:13:11。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<58:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

结束库存：hay=96.000，wood=413.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:1, speed:1, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 032 · 研究 speed → 2

类型：`research`；状态：`passed`；主农场 00:13:11 → 00:13:11。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=38.000，wood=413.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:1, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 033 · 研究 trees → 2

类型：`research`；状态：`passed`；主农场 00:13:11 → 00:13:11。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=38.000，wood=326.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1`。

## 034 · 研究 watering → 1

类型：`research`；状态：`passed`；主农场 00:13:11 → 00:13:11。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=38.000，wood=246.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1`。

## 035 · 生产 hay 至 96

类型：`production`；状态：`passed`；主农场 00:13:11 → 00:13:20。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<96:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=174.000，wood=246.000，carrot=0.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1`。

## 036 · 生产 carrot 至 30

类型：`production`；状态：`passed`；主农场 00:13:20 → 00:13:53。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<30:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=74.000，wood=146.000，carrot=34.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1`。

## 037 · 研究 dicts → 1

类型：`research`；状态：`passed`；主农场 00:13:53 → 00:13:53。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=74.000，wood=146.000，carrot=4.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1`。

## 038 · 生产 hay 至 128

类型：`production`；状态：`passed`；主农场 00:13:53 → 00:14:02。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<128:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=210.000，wood=146.000，carrot=4.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1`。

## 039 · 生产 carrot 至 50

类型：`production`；状态：`passed`；主农场 00:14:02 → 00:14:43。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<50:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=80.000，wood=16.000，carrot=53.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1`。

## 040 · 研究 import → 1

类型：`research`；状态：`passed`；主农场 00:14:43 → 00:14:43。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=80.000，wood=16.000，carrot=3.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1`。

## 041 · 生产 hay 至 160

类型：`production`；状态：`passed`；主农场 00:14:43 → 00:14:53。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<160:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=216.000，wood=16.000，carrot=3.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1`。

## 042 · 生产 wood 至 160

类型：`production`；状态：`passed`；主农场 00:14:53 → 00:15:22。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<160:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=216.000，wood=208.000，carrot=3.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1`。

## 043 · 生产 carrot 至 60

类型：`production`；状态：`passed`；主农场 00:15:22 → 00:16:11。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<60:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=54.000，wood=46.000，carrot=68.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=1.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1`。

## 044 · 研究 auto_unlock → 1

类型：`research`；状态：`passed`；主农场 00:16:11 → 00:16:11。

点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。

在科技树完成本步骤标题对应的购买。

结束库存：hay=54.000，wood=46.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=1.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1`。

## 045 · 研究 debug → 1

类型：`research`；状态：`passed`；主农场 00:16:11 → 00:16:11。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Debug)
```

结束库存：hay=49.000，wood=46.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=1.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1`。

## 046 · 生产 wood 至 100

类型：`production`；状态：`passed`；主农场 00:16:11 → 00:16:32。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<100:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=49.000，wood=142.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1`。

## 047 · 研究 debug2 → 1

类型：`research`；状态：`passed`；主农场 00:16:32 → 00:16:32。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Debug_2)
```

结束库存：hay=49.000，wood=42.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1`。

## 048 · 生产 wood 至 60

类型：`production`；状态：`passed`；主农场 00:16:32 → 00:16:52。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<60:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=49.000，wood=138.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1`。

## 049 · 研究 utilities → 1

类型：`research`；状态：`passed`；主农场 00:16:52 → 00:16:52。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Utilities)
```

结束库存：hay=49.000，wood=78.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1`。

## 050 · 生产 hay 至 500

类型：`production`；状态：`passed`；主农场 00:16:52 → 00:17:19。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<500:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=569.000，wood=78.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1`。

## 051 · 研究 polyculture → 1

类型：`research`；状态：`passed`；主农场 00:17:19 → 00:17:19。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=69.000，wood=78.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1`。

## 052 · 生产 hay 至 2752

类型：`production`；状态：`passed`；主农场 00:17:19 → 00:19:26。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<2752:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=2773.000，wood=78.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1`。

## 053 · 生产 wood 至 2752

类型：`production`；状态：`passed`；主农场 00:19:26 → 00:22:54。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<2752:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=2773.000，wood=2792.000，carrot=8.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1`。

## 054 · 生产 carrot 至 1360

类型：`production`；状态：`passed`；主农场 00:22:54 → 00:35:07。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<1360:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=19.000，wood=38.000，carrot=1369.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1`。

## 055 · 研究 pumpkins → 1

类型：`research`；状态：`passed`；主农场 00:35:07 → 00:35:07。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=19.000，wood=38.000，carrot=1269.000，pumpkin=0.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1`。

## 056 · 生产 pumpkin 至 100

类型：`production`；状态：`passed`；主农场 00:35:07 → 00:36:23。

反复扫描并补种枯死南瓜；整块成熟后收获。

```python
clear()
while num_items(Items.Pumpkin)<100:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        harvest()
```

结束库存：hay=19.000，wood=38.000，carrot=1232.000，pumpkin=128.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1`。

## 057 · 研究 fertilizer → 1

类型：`research`；状态：`passed`；主农场 00:36:23 → 00:36:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=19.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 058 · 生产 hay 至 68

类型：`production`；状态：`passed`；主农场 00:36:23 → 00:36:33。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<68:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=155.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:2, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 059 · 研究 expand → 3

类型：`research`；状态：`passed`；主农场 00:36:33 → 00:36:33。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Expand)
```

结束库存：hay=87.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=0.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 060 · 生产 hay 至 1220

类型：`production`；状态：`passed`；主农场 00:36:33 → 00:37:37。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<1220:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1495.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=7.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:3, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 061 · 研究 grass → 4

类型：`research`；状态：`passed`；主农场 00:37:37 → 00:37:37。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Grass)
```

结束库存：hay=275.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=7.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:2, speed:2, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 062 · 研究 speed → 3

类型：`research`；状态：`passed`；主农场 00:37:37 → 00:37:37。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=106.000，wood=38.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=7.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:2, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 063 · 生产 wood 至 253

类型：`production`；状态：`passed`；主农场 00:37:37 → 00:38:19。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<253:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=106.000，wood=518.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=11.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:2, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 064 · 研究 trees → 3

类型：`research`；状态：`passed`；主农场 00:38:19 → 00:38:20。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=106.000，wood=265.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=11.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:1, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 065 · 研究 carrots → 2

类型：`research`；状态：`passed`；主农场 00:38:20 → 00:38:20。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=106.000，wood=149.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=11.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 066 · 生产 wood 至 232

类型：`production`；状态：`passed`；主农场 00:38:20 → 00:39:02。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<232:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=106.000，wood=1017.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=15.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:1, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 067 · 研究 watering → 2

类型：`research`；状态：`passed`；主农场 00:39:02 → 00:39:02。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Watering)
```

结束库存：hay=106.000，wood=785.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=15.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 068 · 生产 hay 至 1450

类型：`production`；状态：`passed`；主农场 00:39:02 → 00:39:25。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<1450:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1674.000，wood=785.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=18.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:1, pumpkins:1, fertilizer:1`。

## 069 · 研究 polyculture → 2

类型：`research`；状态：`passed`；主农场 00:39:25 → 00:39:25。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=224.000，wood=785.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=18.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1`。

## 070 · 生产 hay 至 3840

类型：`production`；状态：`passed`；主农场 00:39:25 → 00:40:11。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<3840:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=3840.000，wood=785.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=22.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1`。

## 071 · 生产 wood 至 3840

类型：`production`；状态：`passed`；主农场 00:40:11 → 00:41:31。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<3840:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=3840.000，wood=3971.000，carrot=1232.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=30.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1`。

## 072 · 生产 carrot 至 2957

类型：`production`；状态：`passed`；主农场 00:41:31 → 00:46:08。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<2957:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=0.000，wood=131.000，carrot=3024.000，pumpkin=28.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=58.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1`。

## 073 · 生产 pumpkin 至 7900

类型：`production`；状态：`passed`；主农场 00:46:08 → 01:05:01。

反复扫描并补种枯死南瓜；整块成熟后收获。

```python
clear()
while num_items(Items.Pumpkin)<7900:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        harvest()
```

结束库存：hay=0.000，wood=131.000，carrot=1352.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=171.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1`。

## 074 · 研究 sunflowers → 1

类型：`research`；状态：`passed`；主农场 01:05:01 → 01:05:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=0.000，wood=131.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=171.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 075 · 生产 hay 至 3537

类型：`production`；状态：`passed`；主农场 01:05:01 → 01:05:48。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<3537:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=3616.000，wood=131.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=176.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:4, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 076 · 研究 grass → 5

类型：`research`；状态：`passed`；主农场 01:05:48 → 01:05:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Grass)
```

结束库存：hay=79.000，wood=131.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=176.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 077 · 生产 hay 至 488

类型：`production`；状态：`passed`；主农场 01:05:48 → 01:05:58。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<488:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1231.000，wood=131.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=177.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:3, speed:3, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 078 · 研究 speed → 4

类型：`research`；状态：`passed`；主农场 01:05:58 → 01:05:58。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=743.000，wood=131.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=177.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:3, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 079 · 生产 wood 至 732

类型：`production`；状态：`passed`；主农场 01:05:58 → 01:06:22。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<732:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=743.000，wood=1097.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=179.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:3, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 080 · 研究 trees → 4

类型：`research`；状态：`passed`；主农场 01:06:22 → 01:06:22。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=743.000，wood=365.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=179.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:2, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 081 · 研究 carrots → 3

类型：`research`；状态：`passed`；主农场 01:06:22 → 01:06:22。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=743.000，wood=28.000，carrot=1202.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=179.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:1, fertilizer:1, sunflowers:1`。

## 082 · 研究 pumpkins → 2

类型：`research`；状态：`passed`；主农场 01:06:22 → 01:06:22。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=743.000，wood=28.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=179.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:2, fertilizer:1, sunflowers:1`。

## 083 · 生产 wood 至 673

类型：`production`；状态：`passed`；主农场 01:06:22 → 01:06:46。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<673:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=743.000，wood=1996.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=182.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:2, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:2, fertilizer:1, sunflowers:1`。

## 084 · 研究 watering → 3

类型：`research`；状态：`passed`；主农场 01:06:46 → 01:06:46。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Watering)
```

结束库存：hay=743.000，wood=1323.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=182.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:2, fertilizer:1, sunflowers:1`。

## 085 · 生产 hay 至 4205

类型：`production`；状态：`passed`；主农场 01:06:46 → 01:07:04。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<4205:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=5191.000，wood=1323.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=184.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:2, pumpkins:2, fertilizer:1, sunflowers:1`。

## 086 · 研究 polyculture → 3

类型：`research`；状态：`passed`；主农场 01:07:04 → 01:07:04。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=986.000，wood=1323.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=184.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1`。

## 087 · 生产 hay 至 2048

类型：`production`；状态：`passed`；主农场 01:07:04 → 01:07:15。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<2048:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=3386.000，wood=1323.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=185.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1`。

## 088 · 生产 wood 至 2048

类型：`production`；状态：`passed`；主农场 01:07:15 → 01:07:39。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<2048:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=3386.000，wood=3659.000，carrot=912.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=187.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1`。

## 089 · 生产 carrot 至 1536

类型：`production`；状态：`passed`；主农场 01:07:39 → 01:08:23。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<1536:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1338.000，wood=1611.000，carrot=1680.000，pumpkin=8092.000，power=0.000，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=191.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1`。

## 090 · 生产 power 至 6000

类型：`production`；状态：`passed`；主农场 01:08:23 → 01:17:08。

先等待整田成熟，再按 15→7 花瓣分成九档扫描；每档顺序收获，避免逐株全田查找的 O(n⁴) 扫描与能量开销。

```python
clear()
while num_items(Items.Power)<6000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Sunflower)
            move(East)
        move(North)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            while not can_harvest():
                pass
            move(East)
        move(North)
    for petals in range(15,6,-1):
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and measure()==petals:
                    harvest()
                move(East)
            move(North)
```

结束库存：hay=1338.000，wood=1611.000，carrot=720.000，pumpkin=8092.000，power=6166.831，cactus=0.000，bone=0.000，gold=0.000，water=208.000，fertilizer=244.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1`。

## 091 · 研究 cactus → 1

类型：`research`；状态：`passed`；主农场 01:17:08 → 01:17:08。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=1338.000，wood=1611.000，carrot=720.000，pumpkin=7692.000，power=6166.797，cactus=0.000，bone=0.000，gold=0.000，water=208.000，fertilizer=244.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:3, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 092 · 研究 expand → 4

类型：`research`；状态：`passed`；主农场 01:17:08 → 01:17:08。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Expand)
```

结束库存：hay=1142.000，wood=1611.000，carrot=720.000，pumpkin=7692.000，power=6166.764，cactus=0.000，bone=0.000，gold=0.000，water=208.000，fertilizer=244.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 093 · 生产 hay 至 10256

类型：`production`；状态：`passed`；主农场 01:17:08 → 01:17:46。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<10256:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=15990.000，wood=1611.000，carrot=720.000，pumpkin=7692.000，power=6124.290，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=248.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:5, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 094 · 研究 grass → 6

类型：`research`；状态：`passed`；主农场 01:17:46 → 01:17:46。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Grass)
```

结束库存：hay=5734.000，wood=1611.000，carrot=720.000，pumpkin=7692.000，power=6124.257，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=248.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:4, speed:4, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 095 · 研究 speed → 5

类型：`research`；状态：`passed`；主农场 01:17:46 → 01:17:46。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=4319.000，wood=1611.000，carrot=720.000，pumpkin=7692.000，power=6124.223，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=248.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:4, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 096 · 生产 wood 至 2122

类型：`production`；状态：`passed`；主农场 01:17:46 → 01:18:18。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<2122:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=4319.000，wood=10123.000，carrot=720.000，pumpkin=7692.000，power=6059.884，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=251.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:4, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 097 · 研究 trees → 5

类型：`research`；状态：`passed`；主农场 01:18:18 → 01:18:18。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=4319.000，wood=8001.000，carrot=720.000，pumpkin=7692.000，power=6059.850，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=251.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:3, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 098 · 研究 carrots → 4

类型：`research`；状态：`passed`；主农场 01:18:18 → 01:18:18。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=4319.000，wood=7025.000，carrot=720.000，pumpkin=7692.000，power=6059.816，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=251.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 099 · 生产 hay 至 8192

类型：`production`；状态：`passed`；主农场 01:18:18 → 01:18:25。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<8192:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=10591.000，wood=7025.000，carrot=720.000，pumpkin=7692.000，power=6046.710，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=252.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 100 · 生产 wood 至 8192

类型：`production`；状态：`passed`；主农场 01:18:25 → 01:18:57。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<8192:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=10591.000，wood=25105.000，carrot=720.000，pumpkin=7692.000，power=5982.370，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=255.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 101 · 生产 carrot 至 841

类型：`production`；状态：`passed`；主农场 01:18:57 → 01:19:29。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<841:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=2399.000，wood=16913.000，carrot=2768.000，pumpkin=7692.000，power=5918.628，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=258.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:2, fertilizer:1, sunflowers:1, cactus:1`。

## 102 · 研究 pumpkins → 3

类型：`research`；状态：`passed`；主农场 01:19:29 → 01:19:29。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=2399.000，wood=16913.000，carrot=1927.000，pumpkin=7692.000，power=5918.595，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=258.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:3, fertilizer:1, sunflowers:1, cactus:1`。

## 103 · 研究 sunflowers → 2

类型：`research`；状态：`passed`；主农场 01:19:29 → 01:19:29。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=2399.000，wood=16913.000，carrot=1492.000，pumpkin=7692.000，power=5918.561，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=258.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:3, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:3, fertilizer:1, sunflowers:2, cactus:1`。

## 104 · 研究 watering → 4

类型：`research`；状态：`passed`；主农场 01:19:29 → 01:19:29。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Watering)
```

结束库存：hay=2399.000，wood=14961.000，carrot=1492.000，pumpkin=7692.000，power=5918.527，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=258.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:3, fertilizer:1, sunflowers:2, cactus:1`。

## 105 · 研究 fertilizer → 2

类型：`research`；状态：`passed`；主农场 01:19:29 → 01:19:29。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=2399.000，wood=14961.000，carrot=1492.000，pumpkin=7402.000，power=5918.494，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=258.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 106 · 生产 hay 至 12195

类型：`production`；状态：`passed`；主农场 01:19:29 → 01:19:45。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<12195:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=25311.000，wood=14961.000，carrot=1492.000，pumpkin=7402.000，power=5886.592，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=262.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:3, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 107 · 研究 polyculture → 4

类型：`research`；状态：`passed`；主农场 01:19:45 → 01:19:45。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=13116.000，wood=14961.000，carrot=1492.000，pumpkin=7402.000，power=5886.558，cactus=0.000，bone=0.000，gold=0.000，water=0.000，fertilizer=262.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 108 · 生产 cactus 至 30000

类型：`production`；状态：`passed`；主农场 01:19:45 → 01:45:16。

行冒泡排序后列冒泡排序；二维有序后连锁收获。比较复杂度 O(n³)，输出按连锁株数与封顶倍率计算。

```python
clear()
while num_items(Items.Cactus)<30000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    complete=False
    while not complete:
        complete=True
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                move(East)
            move(North)
    # Run on a field of mature cacti. Bubble each row, then each column.
    for axis in [East, North]:
        for sweep in range(get_world_size()):
            for row in range(get_world_size()):
                for col in range(get_world_size()-1):
                    if measure() > measure(axis):
                        swap(axis)
                    move(axis)
                move(axis)
                if axis == East:
                    move(North)
                else:
                    move(East)
    harvest()
```

结束库存：hay=13116.000，wood=14961.000，carrot=1492.000，pumpkin=5354.000，power=2800.324，cactus=32768.000，bone=0.000，gold=0.000，water=144.000，fertilizer=568.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 109 · 生产 carrot 至 4506

类型：`production`；状态：`passed`；主农场 01:45:16 → 01:46:04。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<4506:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=828.000，wood=2673.000，carrot=5588.000，pumpkin=5354.000，power=2703.749，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=578.000，weird_substance=0.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 110 · 生产 weird_substance 至 30

类型：`production`；状态：`passed`；主农场 01:46:04 → 01:47:20。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<30:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((30-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=828.000，wood=2673.000，carrot=4344.000，pumpkin=11462.000，power=2549.853，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=589.000，weird_substance=36.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1`。

## 111 · 研究 mazes → 1

类型：`research`；状态：`passed`；主农场 01:47:20 → 01:47:20。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=828.000，wood=2673.000，carrot=4344.000，pumpkin=11462.000，power=2549.820，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=589.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 112 · 生产 hay 至 29742

类型：`production`；状态：`passed`；主农场 01:47:20 → 01:47:45。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<29742:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=40124.000，wood=2673.000，carrot=4344.000，pumpkin=11462.000，power=2499.391，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=595.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:6, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 113 · 研究 grass → 7

类型：`research`；状态：`passed`；主农场 01:47:45 → 01:47:45。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Grass)
```

结束库存：hay=10382.000，wood=2673.000，carrot=4344.000，pumpkin=11462.000，power=2499.357，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=595.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:5, speed:5, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 114 · 研究 speed → 6

类型：`research`；状态：`passed`；主农场 01:47:45 → 01:47:45。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=6279.000，wood=2673.000，carrot=4344.000，pumpkin=11462.000，power=2499.324，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=595.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:5, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 115 · 生产 wood 至 6154

类型：`production`；状态：`passed`；主农场 01:47:45 → 01:48:01。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<6154:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=6279.000，wood=6281.000，carrot=4344.000，pumpkin=11462.000，power=2443.613，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=597.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:5, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 116 · 研究 trees → 6

类型：`research`；状态：`passed`；主农场 01:48:01 → 01:48:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=6279.000，wood=127.000，carrot=4344.000，pumpkin=11462.000，power=2443.579，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=597.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 117 · 生产 wood 至 2830

类型：`production`；状态：`passed`；主农场 01:48:01 → 01:48:16。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<2830:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=6279.000，wood=7663.000，carrot=4344.000，pumpkin=11462.000，power=2387.599，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=601.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:4, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 118 · 研究 carrots → 5

类型：`research`；状态：`passed`；主农场 01:48:16 → 01:48:16。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=6279.000，wood=4833.000，carrot=4344.000，pumpkin=11462.000，power=2387.566，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=601.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:3, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 119 · 研究 pumpkins → 4

类型：`research`；状态：`passed`；主农场 01:48:16 → 01:48:16。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=6279.000，wood=4833.000，carrot=1905.000，pumpkin=11462.000，power=2387.532，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=601.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:2, sunflowers:2, cactus:1, mazes:1`。

## 120 · 研究 sunflowers → 3

类型：`research`；状态：`passed`；主农场 01:48:16 → 01:48:16。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=6279.000，wood=4833.000，carrot=643.000，pumpkin=11462.000，power=2387.498，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=601.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:2, sunflowers:3, cactus:1, mazes:1`。

## 121 · 生产 wood 至 5659

类型：`production`；状态：`passed`；主农场 01:48:16 → 01:48:31。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<5659:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=6279.000，wood=12609.000，carrot=643.000，pumpkin=11462.000，power=2331.518，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=603.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:4, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:2, sunflowers:3, cactus:1, mazes:1`。

## 122 · 研究 watering → 5

类型：`research`；状态：`passed`；主农场 01:48:31 → 01:48:32。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Watering)
```

结束库存：hay=6279.000，wood=6950.000，carrot=643.000，pumpkin=11462.000，power=2331.485，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=603.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:2, sunflowers:3, cactus:1, mazes:1`。

## 123 · 研究 fertilizer → 3

类型：`research`；状态：`passed`；主农场 01:48:32 → 01:48:32。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=6279.000，wood=6950.000，carrot=643.000，pumpkin=10621.000，power=2331.451，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=603.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:3, sunflowers:3, cactus:1, mazes:1`。

## 124 · 生产 hay 至 35365

类型：`production`；状态：`passed`；主农场 01:48:32 → 01:48:44。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<35365:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=67463.000，wood=6950.000，carrot=643.000，pumpkin=10621.000，power=2285.871，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=611.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:4, pumpkins:4, fertilizer:3, sunflowers:3, cactus:1, mazes:1`。

## 125 · 研究 polyculture → 5

类型：`research`；状态：`passed`；主农场 01:48:44 → 01:48:44。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=32098.000，wood=6950.000，carrot=643.000，pumpkin=10621.000，power=2285.837，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=611.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:1, mazes:1`。

## 126 · 研究 cactus → 2

类型：`research`；状态：`passed`；主农场 01:48:44 → 01:48:44。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=32098.000，wood=6950.000，carrot=643.000，pumpkin=9461.000，power=2285.803，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=611.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 127 · 生产 hay 至 32768

类型：`production`；状态：`passed`；主农场 01:48:44 → 01:48:52。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<32768:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=60514.000，wood=6950.000，carrot=643.000，pumpkin=9461.000，power=2258.480，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=611.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 128 · 生产 wood 至 32768

类型：`production`；状态：`passed`；主农场 01:48:52 → 01:49:15。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<32768:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=60514.000，wood=53414.000，carrot=643.000，pumpkin=9461.000，power=2174.342，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=623.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 129 · 生产 carrot 至 9012

类型：`production`；状态：`passed`；主农场 01:49:15 → 01:49:43。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<9012:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=35426.000，wood=28326.000，carrot=9091.000，pumpkin=9461.000，power=2071.455，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=635.000，weird_substance=6.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 130 · 生产 weird_substance 至 1888

类型：`production`；状态：`passed`；主农场 01:49:43 → 01:50:50。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<1888:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((1888-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1830.760，cactus=32768.000，bone=0.000，gold=0.000，water=0.000，fertilizer=580.000，weird_substance=1902.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 131 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:50:50 → 01:50:51。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1824.408，cactus=32768.000，bone=0.000，gold=256.000，water=16.000，fertilizer=580.000，weird_substance=1886.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 132 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:50:51 → 01:50:55。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1812.637，cactus=32768.000，bone=0.000，gold=512.000，water=16.000，fertilizer=584.000，weird_substance=1870.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 133 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:50:55 → 01:50:57。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1804.045，cactus=32768.000，bone=0.000，gold=768.000，water=16.000，fertilizer=584.000，weird_substance=1854.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 134 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:50:57 → 01:51:00。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1791.470，cactus=32768.000，bone=0.000，gold=1024.000，water=16.000，fertilizer=584.000，weird_substance=1838.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 135 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:00 → 01:51:08。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1764.259，cactus=32768.000，bone=0.000，gold=1280.000，water=32.000，fertilizer=588.000，weird_substance=1822.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 136 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:08 → 01:51:16。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1736.412，cactus=32768.000，bone=0.000，gold=1536.000，water=48.000，fertilizer=592.000，weird_substance=1806.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 137 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:16 → 01:51:26。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1700.495，cactus=32768.000，bone=0.000，gold=1792.000，water=64.000，fertilizer=596.000，weird_substance=1790.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 138 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:26 → 01:51:29。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1688.011，cactus=32768.000，bone=0.000，gold=2048.000，water=64.000，fertilizer=596.000，weird_substance=1774.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 139 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:29 → 01:51:32。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1678.201，cactus=32768.000，bone=0.000，gold=2304.000，water=80.000，fertilizer=596.000，weird_substance=1758.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 140 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:32 → 01:51:35。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1665.719，cactus=32768.000，bone=0.000，gold=2560.000，water=80.000，fertilizer=600.000，weird_substance=1742.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 141 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:35 → 01:51:38。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1654.795，cactus=32768.000，bone=0.000，gold=2816.000，water=80.000，fertilizer=600.000，weird_substance=1726.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 142 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:38 → 01:51:43。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1638.446，cactus=32768.000，bone=0.000，gold=3072.000，water=96.000，fertilizer=600.000，weird_substance=1710.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 143 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:43 → 01:51:55。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1594.201，cactus=32768.000，bone=0.000，gold=3328.000，water=112.000，fertilizer=608.000，weird_substance=1694.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 144 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:51:55 → 01:52:02。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1569.839，cactus=32768.000，bone=0.000，gold=3584.000，water=128.000，fertilizer=608.000，weird_substance=1678.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 145 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:02 → 01:52:10。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1538.418，cactus=32768.000，bone=0.000，gold=3840.000，water=128.000，fertilizer=612.000，weird_substance=1662.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 146 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:10 → 01:52:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1525.455，cactus=32768.000，bone=0.000，gold=4096.000，water=144.000，fertilizer=616.000，weird_substance=1646.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 147 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:14 → 01:52:24。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1489.390，cactus=32768.000，bone=0.000，gold=4352.000，water=160.000，fertilizer=620.000，weird_substance=1630.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 148 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:24 → 01:52:28。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1474.428，cactus=32768.000，bone=0.000，gold=4608.000，water=160.000，fertilizer=620.000，weird_substance=1614.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 149 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:28 → 01:52:31。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1462.835，cactus=32768.000，bone=0.000，gold=4864.000，water=176.000，fertilizer=620.000，weird_substance=1598.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 150 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:31 → 01:52:34。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1451.376，cactus=32768.000，bone=0.000，gold=5120.000，water=176.000，fertilizer=624.000，weird_substance=1582.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 151 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:34 → 01:52:45。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1412.431，cactus=32768.000，bone=0.000，gold=5376.000，water=192.000，fertilizer=628.000，weird_substance=1566.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 152 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:45 → 01:52:50。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1395.870，cactus=32768.000，bone=0.000，gold=5632.000，water=192.000，fertilizer=628.000，weird_substance=1550.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 153 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:52:50 → 01:53:00。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1357.362，cactus=32768.000，bone=0.000，gold=5888.000，water=208.000，fertilizer=632.000，weird_substance=1534.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 154 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:00 → 01:53:04。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1342.415，cactus=32768.000，bone=0.000，gold=6144.000，water=224.000，fertilizer=636.000，weird_substance=1518.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 155 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:04 → 01:53:10。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1320.695，cactus=32768.000，bone=0.000，gold=6400.000，water=224.000，fertilizer=636.000，weird_substance=1502.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 156 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:10 → 01:53:23。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1275.770，cactus=32768.000，bone=0.000，gold=6656.000，water=256.000，fertilizer=640.000，weird_substance=1486.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 157 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:23 → 01:53:30。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1250.883，cactus=32768.000，bone=0.000，gold=6912.000，water=256.000，fertilizer=644.000，weird_substance=1470.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 158 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:30 → 01:53:36。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1226.607，cactus=32768.000，bone=0.000，gold=7168.000，water=272.000，fertilizer=648.000，weird_substance=1454.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 159 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:36 → 01:53:39。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1215.812，cactus=32768.000，bone=0.000，gold=7424.000，water=272.000，fertilizer=648.000，weird_substance=1438.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 160 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:39 → 01:53:41。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1208.386，cactus=32768.000，bone=0.000，gold=7680.000，water=288.000，fertilizer=648.000，weird_substance=1422.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 161 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:41 → 01:53:54。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1162.493，cactus=32768.000，bone=0.000，gold=7936.000，water=304.000，fertilizer=656.000，weird_substance=1406.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 162 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:53:54 → 01:54:00。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1140.740，cactus=32768.000，bone=0.000，gold=8192.000，water=304.000，fertilizer=656.000，weird_substance=1390.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 163 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:00 → 01:54:03。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1131.332，cactus=32768.000，bone=0.000，gold=8448.000，water=320.000，fertilizer=656.000，weird_substance=1374.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 164 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:03 → 01:54:09。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1108.102，cactus=32768.000，bone=0.000，gold=8704.000，water=320.000，fertilizer=660.000，weird_substance=1358.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 165 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:09 → 01:54:16。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1083.148，cactus=32768.000，bone=0.000，gold=8960.000，water=336.000，fertilizer=664.000，weird_substance=1342.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 166 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:16 → 01:54:22。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1060.252，cactus=32768.000，bone=0.000，gold=9216.000，water=352.000，fertilizer=664.000，weird_substance=1326.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 167 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:22 → 01:54:30。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1032.356，cactus=32768.000，bone=0.000，gold=9472.000，water=352.000，fertilizer=668.000，weird_substance=1310.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 168 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:30 → 01:54:35。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=1015.044，cactus=32768.000，bone=0.000，gold=9728.000，water=368.000，fertilizer=672.000，weird_substance=1294.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 169 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:35 → 01:54:40。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=997.208，cactus=32768.000，bone=0.000，gold=9984.000，water=368.000，fertilizer=672.000，weird_substance=1278.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 170 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:40 → 01:54:42。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=988.614，cactus=32768.000，bone=0.000，gold=10240.000，water=384.000，fertilizer=672.000，weird_substance=1262.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 171 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:42 → 01:54:45。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=977.324，cactus=32768.000，bone=0.000，gold=10496.000，water=384.000，fertilizer=676.000，weird_substance=1246.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 172 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:45 → 01:54:50。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=958.304，cactus=32768.000，bone=0.000，gold=10752.000，water=384.000，fertilizer=676.000，weird_substance=1230.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 173 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:50 → 01:54:54。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=944.303，cactus=32768.000，bone=0.000，gold=11008.000，water=400.000，fertilizer=680.000，weird_substance=1214.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 174 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:54 → 01:54:59。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=925.844，cactus=32768.000，bone=0.000，gold=11264.000，water=400.000，fertilizer=680.000，weird_substance=1198.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 175 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:54:59 → 01:55:03。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=913.344，cactus=32768.000，bone=0.000，gold=11520.000，water=416.000，fertilizer=680.000，weird_substance=1182.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 176 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:03 → 01:55:11。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=882.364，cactus=32768.000，bone=0.000，gold=11776.000，water=432.000，fertilizer=684.000，weird_substance=1166.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 177 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:11 → 01:55:17。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=863.295，cactus=32768.000，bone=0.000，gold=12032.000，water=432.000，fertilizer=688.000，weird_substance=1150.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 178 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:17 → 01:55:23。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=838.380，cactus=32768.000，bone=0.000，gold=12288.000，water=448.000，fertilizer=692.000，weird_substance=1134.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 179 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:23 → 01:55:31。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=809.957，cactus=32768.000，bone=0.000，gold=12544.000，water=464.000，fertilizer=692.000，weird_substance=1118.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 180 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:31 → 01:55:36。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=794.734，cactus=32768.000，bone=0.000，gold=12800.000，water=464.000，fertilizer=696.000，weird_substance=1102.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 181 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:36 → 01:55:42。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=771.854，cactus=32768.000，bone=0.000，gold=13056.000，water=480.000，fertilizer=696.000，weird_substance=1086.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 182 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:42 → 01:55:45。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=760.590，cactus=32768.000，bone=0.000，gold=13312.000，water=480.000，fertilizer=700.000，weird_substance=1070.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 183 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:45 → 01:55:53。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=733.309，cactus=32768.000，bone=0.000，gold=13568.000，water=496.000，fertilizer=700.000，weird_substance=1054.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 184 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:53 → 01:55:59。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=708.447，cactus=32768.000，bone=0.000，gold=13824.000，water=496.000，fertilizer=704.000，weird_substance=1038.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 185 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:55:59 → 01:56:07。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=680.896，cactus=32768.000，bone=0.000，gold=14080.000，water=512.000，fertilizer=708.000，weird_substance=1022.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 186 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:07 → 01:56:11。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=667.753，cactus=32768.000，bone=0.000，gold=14336.000，water=512.000，fertilizer=708.000，weird_substance=1006.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 187 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:11 → 01:56:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=656.667，cactus=32768.000，bone=0.000，gold=14592.000，water=528.000，fertilizer=712.000，weird_substance=990.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 188 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:14 → 01:56:21。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=628.818，cactus=32768.000，bone=0.000，gold=14848.000，water=544.000，fertilizer=712.000，weird_substance=974.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 189 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:21 → 01:56:28。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=604.056，cactus=32768.000，bone=0.000，gold=15104.000，water=544.000，fertilizer=716.000，weird_substance=958.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 190 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:28 → 01:56:31。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=593.140，cactus=32768.000，bone=0.000，gold=15360.000，water=560.000，fertilizer=716.000，weird_substance=942.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 191 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:31 → 01:56:35。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=580.726，cactus=32768.000，bone=0.000，gold=15616.000，water=560.000，fertilizer=720.000，weird_substance=926.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 192 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:35 → 01:56:38。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=567.956，cactus=32768.000，bone=0.000，gold=15872.000，water=560.000，fertilizer=720.000，weird_substance=910.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 193 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:38 → 01:56:44。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=545.798，cactus=32768.000，bone=0.000，gold=16128.000，water=576.000，fertilizer=724.000，weird_substance=894.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 194 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:44 → 01:56:47。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=534.390，cactus=32768.000，bone=0.000，gold=16384.000，water=576.000，fertilizer=724.000，weird_substance=878.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 195 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:47 → 01:56:55。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=507.021，cactus=32768.000，bone=0.000，gold=16640.000，water=592.000，fertilizer=728.000，weird_substance=862.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 196 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:56:55 → 01:57:01。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=484.900，cactus=32768.000，bone=0.000，gold=16896.000，water=608.000，fertilizer=728.000，weird_substance=846.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 197 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:01 → 01:57:08。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=459.070，cactus=32768.000，bone=0.000，gold=17152.000，water=608.000，fertilizer=732.000，weird_substance=830.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 198 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:08 → 01:57:12。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=445.069，cactus=32768.000，bone=0.000，gold=17408.000，water=624.000，fertilizer=732.000，weird_substance=814.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 199 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:12 → 01:57:16。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=431.660，cactus=32768.000，bone=0.000，gold=17664.000，water=624.000，fertilizer=736.000，weird_substance=798.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 200 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:16 → 01:57:23。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=406.427，cactus=32768.000，bone=0.000，gold=17920.000，water=640.000，fertilizer=736.000，weird_substance=782.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 201 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:23 → 01:57:27。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=389.803，cactus=32768.000，bone=0.000，gold=18176.000，water=640.000，fertilizer=740.000，weird_substance=766.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 202 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:27 → 01:57:33。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=367.781，cactus=32768.000，bone=0.000，gold=18432.000，water=656.000，fertilizer=744.000，weird_substance=750.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 203 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:33 → 01:57:37。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=355.919，cactus=32768.000，bone=0.000，gold=18688.000，water=656.000，fertilizer=744.000，weird_substance=734.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 204 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:37 → 01:57:39。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=347.276，cactus=32768.000，bone=0.000，gold=18944.000，water=656.000，fertilizer=744.000，weird_substance=718.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 205 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:39 → 01:57:42。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=335.552，cactus=32768.000，bone=0.000，gold=19200.000，water=672.000，fertilizer=744.000，weird_substance=702.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 206 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:42 → 01:57:54。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=293.583，cactus=32768.000，bone=0.000，gold=19456.000，water=688.000，fertilizer=752.000，weird_substance=686.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 207 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:57:54 → 01:58:04。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=255.224，cactus=32768.000，bone=0.000，gold=19712.000，water=704.000，fertilizer=756.000，weird_substance=670.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 208 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:04 → 01:58:09。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=239.014，cactus=32768.000，bone=0.000，gold=19968.000，water=704.000，fertilizer=756.000，weird_substance=654.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 209 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:09 → 01:58:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=219.452，cactus=32768.000，bone=0.000，gold=20224.000，water=720.000，fertilizer=760.000，weird_substance=638.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 210 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:14 → 01:58:19。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=204.171，cactus=32768.000，bone=0.000，gold=20480.000，water=720.000，fertilizer=760.000，weird_substance=622.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 211 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:19 → 01:58:22。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=191.399，cactus=32768.000，bone=0.000，gold=20736.000，water=736.000，fertilizer=760.000，weird_substance=606.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 212 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:22 → 01:58:27。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=172.987，cactus=32768.000，bone=0.000，gold=20992.000，water=736.000，fertilizer=764.000，weird_substance=590.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 213 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:27 → 01:58:36。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=139.484，cactus=32768.000，bone=0.000，gold=21248.000，water=752.000，fertilizer=768.000，weird_substance=574.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 214 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:36 → 01:58:39。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=130.655，cactus=32768.000，bone=0.000，gold=21504.000，water=752.000，fertilizer=768.000，weird_substance=558.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 215 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:39 → 01:58:49。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=95.370，cactus=32768.000，bone=0.000，gold=21760.000，water=768.000，fertilizer=772.000，weird_substance=542.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 216 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:49 → 01:58:56。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=69.637，cactus=32768.000，bone=0.000，gold=22016.000，water=784.000，fertilizer=776.000，weird_substance=526.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 217 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:58:56 → 01:59:00。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=55.580，cactus=32768.000，bone=0.000，gold=22272.000，water=784.000，fertilizer=776.000，weird_substance=510.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 218 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:00 → 01:59:11。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=15.935，cactus=32768.000，bone=0.000，gold=22528.000，water=800.000，fertilizer=780.000，weird_substance=494.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 219 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:11 → 01:59:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=4.609，cactus=32768.000，bone=0.000，gold=22784.000，water=816.000，fertilizer=784.000，weird_substance=478.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 220 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:14 → 01:59:24。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=23040.000，water=832.000，fertilizer=788.000，weird_substance=462.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 221 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:24 → 01:59:33。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=23296.000，water=848.000，fertilizer=788.000，weird_substance=446.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 222 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:33 → 01:59:40。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=23552.000，water=848.000，fertilizer=792.000，weird_substance=430.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 223 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:40 → 01:59:51。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=23808.000，water=864.000，fertilizer=796.000，weird_substance=414.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 224 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 01:59:51 → 02:00:03。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=24064.000，water=896.000，fertilizer=804.000，weird_substance=398.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 225 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:03 → 02:00:08。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=24320.000，water=896.000，fertilizer=804.000，weird_substance=382.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 226 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:08 → 02:00:17。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=24576.000，water=912.000，fertilizer=808.000，weird_substance=366.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 227 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:17 → 02:00:24。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=24832.000，water=928.000，fertilizer=812.000，weird_substance=350.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 228 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:24 → 02:00:37。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=25088.000，water=944.000，fertilizer=816.000，weird_substance=334.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 229 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:37 → 02:00:47。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=25344.000，water=960.000，fertilizer=820.000，weird_substance=318.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 230 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:47 → 02:00:58。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=25600.000，water=976.000，fertilizer=824.000，weird_substance=302.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 231 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:00:58 → 02:01:01。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=25856.000，water=992.000，fertilizer=824.000，weird_substance=286.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 232 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:01 → 02:01:07。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=26112.000，water=992.000，fertilizer=828.000，weird_substance=270.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 233 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:07 → 02:01:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=26368.000，water=1008.000，fertilizer=832.000，weird_substance=254.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 234 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:14 → 02:01:22。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=26624.000，water=1024.000，fertilizer=832.000，weird_substance=238.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 235 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:22 → 02:01:26。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=26880.000，water=1024.000，fertilizer=836.000，weird_substance=222.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 236 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:26 → 02:01:30。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=27136.000，water=1024.000，fertilizer=836.000，weird_substance=206.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 237 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:30 → 02:01:39。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=27392.000，water=1040.000，fertilizer=840.000，weird_substance=190.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 238 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:39 → 02:01:46。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=27648.000，water=1056.000，fertilizer=844.000，weird_substance=174.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 239 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:46 → 02:01:59。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=27904.000，water=1072.000，fertilizer=848.000，weird_substance=158.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 240 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:01:59 → 02:02:03。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=28160.000，water=1088.000，fertilizer=852.000，weird_substance=142.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 241 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:03 → 02:02:07。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=28416.000，water=1088.000，fertilizer=852.000，weird_substance=126.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 242 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:07 → 02:02:22。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=28672.000，water=1120.000，fertilizer=856.000，weird_substance=110.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 243 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:22 → 02:02:29。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=28928.000，water=1120.000，fertilizer=860.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 244 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:29 → 02:02:34。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=29184.000，water=1136.000，fertilizer=864.000，weird_substance=78.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 245 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:34 → 02:02:51。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=29440.000，water=1152.000，fertilizer=868.000，weird_substance=62.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 246 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:02:51 → 02:03:01。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=29696.000，water=1184.000，fertilizer=872.000，weird_substance=46.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 247 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:03:01 → 02:03:06。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=29952.000，water=1184.000，fertilizer=876.000，weird_substance=30.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 248 · 迷宫生产 gold 至 30000

类型：`production`；状态：`passed`；主农场 02:03:06 → 02:03:14。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32768.000，bone=0.000，gold=30208.000，water=1200.000，fertilizer=880.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1`。

## 249 · 研究 dinosaurs → 1

类型：`research`；状态：`passed`；主农场 02:03:14 → 02:03:14。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=35426.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=1200.000，fertilizer=880.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:4, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 250 · 研究 expand → 5

类型：`research`；状态：`passed`；主农场 02:03:14 → 02:03:14。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Expand)
```

结束库存：hay=34860.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=1200.000，fertilizer=880.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 251 · 生产 hay 至 86250

类型：`production`；状态：`passed`；主农场 02:03:14 → 02:04:00。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<86250:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=153516.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=240.000，fertilizer=896.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:7, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 252 · 研究 grass → 8

类型：`research`；状态：`passed`；主农场 02:04:00 → 02:04:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Grass)
```

结束库存：hay=67266.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=240.000，fertilizer=896.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:6, speed:6, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 253 · 研究 speed → 7

类型：`research`；状态：`passed`；主农场 02:04:00 → 02:04:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=55369.000，wood=28326.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=240.000，fertilizer=896.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:6, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 254 · 研究 trees → 7

类型：`research`；状态：`passed`；主农场 02:04:00 → 02:04:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=55369.000，wood=10481.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=240.000，fertilizer=896.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:5, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 255 · 研究 carrots → 6

类型：`research`；状态：`passed`；主农场 02:04:00 → 02:04:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=55369.000，wood=2276.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=240.000，fertilizer=896.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 256 · 生产 hay 至 131072

类型：`production`；状态：`passed`；主农场 02:04:00 → 02:04:18。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<131072:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=272457.000，wood=2276.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=904.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 257 · 生产 wood 至 131072

类型：`production`；状态：`passed`；主农场 02:04:18 → 02:05:21。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<131072:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=272457.000，wood=348452.000，carrot=6507.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=5.000，fertilizer=928.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 258 · 生产 carrot 至 7073

类型：`production`；状态：`passed`；主农场 02:05:21 → 02:06:23。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<7073:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=141385.000，wood=217380.000，carrot=39275.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:4, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 259 · 研究 pumpkins → 5

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=141385.000，wood=217380.000，carrot=32202.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:5, fertilizer:3, sunflowers:3, cactus:2, mazes:1, dinosaurs:1`。

## 260 · 研究 sunflowers → 4

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=141385.000，wood=217380.000，carrot=28543.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:5, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:5, fertilizer:3, sunflowers:4, cactus:2, mazes:1, dinosaurs:1`。

## 261 · 研究 watering → 6

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Watering)
```

结束库存：hay=141385.000，wood=200971.000，carrot=28543.000，pumpkin=19853.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:5, fertilizer:3, sunflowers:4, cactus:2, mazes:1, dinosaurs:1`。

## 262 · 研究 fertilizer → 4

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=141385.000，wood=200971.000，carrot=28543.000，pumpkin=17414.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:5, pumpkins:5, fertilizer:4, sunflowers:4, cactus:2, mazes:1, dinosaurs:1`。

## 263 · 研究 polyculture → 6

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Polyculture)
```

结束库存：hay=38829.000，wood=200971.000，carrot=28543.000，pumpkin=17414.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:2, mazes:1, dinosaurs:1`。

## 264 · 研究 cactus → 3

类型：`research`；状态：`passed`；主农场 02:06:23 → 02:06:23。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=38829.000，wood=200971.000，carrot=28543.000，pumpkin=14050.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=956.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:1, dinosaurs:1`。

## 265 · 生产 hay 至 196608

类型：`production`；状态：`passed`；主农场 02:06:23 → 02:06:38。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<196608:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=214701.000，wood=200971.000，carrot=28543.000，pumpkin=14050.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=964.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:1, dinosaurs:1`。

## 266 · 生产 carrot 至 72090

类型：`production`；状态：`passed`；主农场 02:06:38 → 02:08:08。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<72090:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=18093.000，wood=4363.000，carrot=94079.000，pumpkin=14050.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=0.000，fertilizer=1036.000，weird_substance=14.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:1, dinosaurs:1`。

## 267 · 生产 weird_substance 至 87

类型：`production`；状态：`passed`；主农场 02:08:08 → 02:09:57。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<87:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((87-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=18093.000，wood=4363.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=32.000，fertilizer=1122.000，weird_substance=110.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:1, dinosaurs:1`。

## 268 · 研究 mazes → 2

类型：`research`；状态：`passed`；主农场 02:09:57 → 02:09:57。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=18093.000，wood=4363.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30208.000，water=32.000，fertilizer=1122.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1`。

## 269 · 研究 megafarm → 1

类型：`research`；状态：`passed`；主农场 02:09:57 → 02:09:57。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Megafarm)
```

结束库存：hay=18093.000，wood=4363.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=32.000，fertilizer=1122.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 270 · 生产 hay 至 34500

类型：`production`；状态：`passed`；主农场 02:09:57 → 02:10:13。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<34500:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=201645.000，wood=4363.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1138.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:7, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 271 · 研究 speed → 8

类型：`research`；状态：`passed`；主农场 02:10:13 → 02:10:13。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Speed)
```

结束库存：hay=167145.000，wood=4363.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1138.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 272 · 生产 wood 至 51750

类型：`production`；状态：`passed`；主农场 02:10:13 → 02:10:48。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<51750:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=167145.000，wood=344779.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:7, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 273 · 研究 trees → 8

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Trees)
```

结束库存：hay=167145.000，wood=293029.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:6, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 274 · 研究 carrots → 7

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=167145.000，wood=269236.000，carrot=73311.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:5, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 275 · 研究 pumpkins → 6

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=167145.000，wood=269236.000，carrot=52799.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:4, sunflowers:4, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 276 · 研究 sunflowers → 5

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=167145.000，wood=269236.000，carrot=42189.000，pumpkin=112258.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:4, sunflowers:5, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 277 · 研究 fertilizer → 5

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=167145.000，wood=269236.000，carrot=42189.000，pumpkin=105185.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:3, mazes:2, dinosaurs:1, megafarm:1`。

## 278 · 研究 cactus → 4

类型：`research`；状态：`passed`；主农场 02:10:48 → 02:10:48。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=167145.000，wood=269236.000，carrot=42189.000，pumpkin=95429.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1162.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:2, dinosaurs:1, megafarm:1`。

## 279 · 生产 hay 至 393216

类型：`production`；状态：`passed`；主农场 02:10:48 → 02:11:06。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<393216:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=543977.000，wood=269236.000，carrot=42189.000，pumpkin=95429.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1194.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:2, dinosaurs:1, megafarm:1`。

## 280 · 生产 wood 至 393216

类型：`production`；状态：`passed`；主农场 02:11:06 → 02:11:40。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<393216:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=543977.000，wood=965748.000，carrot=42189.000，pumpkin=95429.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1242.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:2, dinosaurs:1, megafarm:1`。

## 281 · 生产 carrot 至 144180

类型：`production`；状态：`passed`；主农场 02:11:40 → 02:12:30。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<144180:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=150761.000，wood=572532.000，carrot=173261.000，pumpkin=95429.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=0.000，fertilizer=1322.000，weird_substance=23.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:2, dinosaurs:1, megafarm:1`。

## 282 · 生产 weird_substance 至 253

类型：`production`；状态：`passed`；主农场 02:12:30 → 02:13:52。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<253:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((253-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=32.000，fertilizer=1447.000，weird_substance=311.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:2, dinosaurs:1, megafarm:1`。

## 283 · 研究 mazes → 3

类型：`research`；状态：`passed`；主农场 02:13:52 → 02:13:52。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=32268.000，bone=0.000，gold=30108.000，water=32.000，fertilizer=1447.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:1, megafarm:1`。

## 284 · 研究 dinosaurs → 2

类型：`research`；状态：`passed`；主农场 02:13:52 → 02:13:52。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=30818.000，bone=0.000，gold=30108.000，water=32.000，fertilizer=1447.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:1`。

## 285 · 研究 megafarm → 2

类型：`research`；状态：`passed`；主农场 02:13:52 → 02:13:52。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Megafarm)
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=30818.000，bone=0.000，gold=29818.000，water=32.000，fertilizer=1447.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:2`。

## 286 · 长尾生产 bone 至 100

类型：`production`；状态：`passed`；主农场 02:13:52 → 02:21:01。

偶数网格上的哈密顿回路；只在边界受阻时结束，换帽结算尾长收益；倍率在长度 64 封顶，继续增长仍增加产量。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
# Use an even-sized empty field and keep cactus in inventory.
change_hat(Hats.Dinosaur_Hat)
while True:
    direction=East
    if get_pos_x()==0:
        if get_pos_y()>0:
            direction=South
    elif get_pos_y()%2==0:
        if get_pos_x()==get_world_size()-1:
            direction=North
    else:
        direction=West
        if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
            direction=North
    if not move(direction):
        break
change_hat(Hats.Straw_Hat)
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=29794.000，bone=81840.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:2`。

## 287 · 研究 simulation → 1

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Simulation)
```

结束库存：hay=150761.000，wood=572532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:7, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 288 · 研究 carrots → 8

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Carrots)
```

结束库存：hay=150761.000，wood=503532.000，carrot=132653.000，pumpkin=291749.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:6, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 289 · 研究 pumpkins → 7

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=150761.000，wood=503532.000，carrot=73170.000，pumpkin=291749.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:5, sunflowers:5, cactus:4, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 290 · 研究 sunflowers → 6

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=150761.000，wood=503532.000，carrot=42403.000，pumpkin=291749.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:5, sunflowers:6, cactus:4, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 291 · 研究 fertilizer → 6

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Fertilizer)
```

结束库存：hay=150761.000，wood=503532.000，carrot=42403.000，pumpkin=271237.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:4, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 292 · 研究 cactus → 5

类型：`research`；状态：`passed`；主农场 02:21:01 → 02:21:01。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=150761.000，wood=503532.000，carrot=42403.000，pumpkin=242945.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=1408.000，fertilizer=2135.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 293 · 生产 hay 至 786432

类型：`production`；状态：`passed`；主农场 02:21:01 → 02:21:36。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<786432:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=856297.000，wood=503532.000，carrot=42403.000，pumpkin=242945.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=0.000，fertilizer=2263.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 294 · 生产 wood 至 786432

类型：`production`；状态：`passed`；主农场 02:21:36 → 02:22:11。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<786432:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=856297.000，wood=1199148.000，carrot=42403.000，pumpkin=242945.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=30.000，fertilizer=2359.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 295 · 生产 carrot 至 288359

类型：`production`；状态：`passed`；主农场 02:22:11 → 02:23:00。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<288359:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=69865.000，wood=412716.000，carrot=304547.000，pumpkin=242945.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=0.000，fertilizer=2519.000，weird_substance=58.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 296 · 生产 weird_substance 至 732

类型：`production`；状态：`passed`；主农场 02:23:00 → 02:24:05。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<732:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((732-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=69865.000，wood=412716.000，carrot=224675.000，pumpkin=635393.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=32.000，fertilizer=2739.000，weird_substance=826.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:3, dinosaurs:2, megafarm:2, simulation:1`。

## 297 · 研究 mazes → 4

类型：`research`；状态：`passed`；主农场 02:24:05 → 02:24:05。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=69865.000，wood=412716.000，carrot=224675.000，pumpkin=635393.000，power=0.000，cactus=29794.000，bone=81740.000，gold=29818.000，water=32.000，fertilizer=2739.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:2, megafarm:2, simulation:1`。

## 298 · 研究 dinosaurs → 3

类型：`research`；状态：`passed`；主农场 02:24:05 → 02:24:05。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=69865.000，wood=412716.000，carrot=224675.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=29818.000，water=32.000，fertilizer=2739.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:2, simulation:1`。

## 299 · 研究 megafarm → 3

类型：`research`；状态：`passed`；主农场 02:24:05 → 02:24:05。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Megafarm)
```

结束库存：hay=69865.000，wood=412716.000，carrot=224675.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=28977.000，water=32.000，fertilizer=2739.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1`。

## 300 · 研究 leaderboard → 1

类型：`research`；状态：`passed`；主农场 02:24:05 → 02:24:06。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Leaderboard)
```

结束库存：hay=69865.000，wood=412716.000，carrot=224675.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=32.000，fertilizer=2739.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:7, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 301 · 研究 pumpkins → 8

类型：`research`；状态：`passed`；主农场 02:24:06 → 02:24:06。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Pumpkins)
```

结束库存：hay=69865.000，wood=412716.000，carrot=52176.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=32.000，fertilizer=2739.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 302 · 生产 hay 至 524288

类型：`production`；状态：`passed`；主农场 02:24:06 → 02:24:33。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=708841.000，wood=412716.000，carrot=52176.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=2835.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 303 · 生产 wood 至 524288

类型：`production`；状态：`passed`；主农场 02:24:33 → 02:25:08。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=708841.000，wood=1108780.000，carrot=52176.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=2931.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 304 · 生产 carrot 至 89224

类型：`production`；状态：`passed`；主农场 02:25:08 → 02:25:42。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<89224:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=184553.000，wood=584492.000，carrot=183248.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3027.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:6, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 305 · 研究 sunflowers → 7

类型：`research`；状态：`passed`；主农场 02:25:42 → 02:25:42。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=184553.000，wood=584492.000，carrot=94024.000，pumpkin=635393.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3027.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:5, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 306 · 研究 cactus → 6

类型：`research`；状态：`passed`；主农场 02:25:42 → 02:25:42。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=184553.000，wood=584492.000，carrot=94024.000，pumpkin=553348.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3027.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 307 · 生产 hay 至 1310720

类型：`production`；状态：`passed`；主农场 02:25:42 → 02:26:29。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<1310720:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1340137.000，wood=584492.000，carrot=94024.000，pumpkin=553348.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3187.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 308 · 生产 wood 至 1310720

类型：`production`；状态：`passed`；主农场 02:26:29 → 02:27:19。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<1310720:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1340137.000，wood=1981100.000，carrot=94024.000，pumpkin=553348.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3347.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 309 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:27:19 → 02:28:38。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=29417.000，wood=670380.000，carrot=618312.000，pumpkin=553348.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=0.000，fertilizer=3603.000，weird_substance=94.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 310 · 生产 weird_substance 至 2122

类型：`production`；状态：`passed`；主农场 02:28:38 → 02:30:11。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<2122:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((2122-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=29417.000，wood=670380.000，carrot=449992.000，pumpkin=1337476.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=32.000，fertilizer=3885.000，weird_substance=2398.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:4, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 311 · 研究 mazes → 5

类型：`research`；状态：`passed`；主农场 02:30:11 → 02:30:11。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=29417.000，wood=670380.000，carrot=449992.000，pumpkin=1337476.000，power=0.000，cactus=25589.000，bone=81740.000，gold=27977.000，water=32.000，fertilizer=3885.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:3, megafarm:3, simulation:1, leaderboard:1`。

## 312 · 研究 dinosaurs → 4

类型：`research`；状态：`passed`；主农场 02:30:11 → 02:30:11。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=29417.000，wood=670380.000，carrot=449992.000，pumpkin=1337476.000，power=0.000，cactus=13394.000，bone=81740.000，gold=27977.000，water=32.000，fertilizer=3885.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:3, simulation:1, leaderboard:1`。

## 313 · 研究 megafarm → 4

类型：`research`；状态：`passed`；主农场 02:30:11 → 02:30:11。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Megafarm)
```

结束库存：hay=29417.000，wood=670380.000，carrot=449992.000，pumpkin=1337476.000，power=0.000，cactus=13394.000，bone=81740.000，gold=25538.000，water=32.000，fertilizer=3885.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 314 · 生产 power 至 1000

类型：`production`；状态：`passed`；主农场 02:30:11 → 02:31:03。

先等待整田成熟，再按 15→7 花瓣分成九档扫描；每档顺序收获，避免逐株全田查找的 O(n⁴) 扫描与能量开销。

```python
clear()
while num_items(Items.Power)<1000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Sunflower)
            move(East)
        move(North)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            while not can_harvest():
                pass
            move(East)
        move(North)
    for petals in range(15,6,-1):
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and measure()==petals:
                    harvest()
                move(East)
            move(North)
```

结束库存：hay=29417.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=519891.161，cactus=13394.000，bone=81740.000，gold=25538.000，water=192.000，fertilizer=4077.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 315 · 验证随机伴生目标

类型：`production`；状态：`passed`；主农场 02:31:03 → 02:31:06。

读取伴生类型与坐标，布置邻作，回原地收获；检查实际额外收益。

```python
clear()
entity,position=get_companion()
x,y=position
while get_pos_x()!=x:
    move(East)
while get_pos_y()!=y:
    move(North)
till()
plant(entity)
while get_pos_x()!=0:
    move(East)
while get_pos_y()!=0:
    move(North)
while not can_harvest():
    do_a_flip()
harvest()
```

结束库存：hay=30569.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=519855.661，cactus=13394.000，bone=81740.000，gold=25538.000，water=192.000，fertilizer=4077.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 316 · 验证独立工作机与等待

类型：`production`；状态：`passed`；主农场 02:31:06 → 02:31:11。

按列派生任务；槽满时主机回退执行；等待所有句柄结束。

```python
clear()
while not can_harvest():
    pass
def column():
    for row in range(get_world_size()):
        if can_harvest():
            harvest()
        move(North)
    return True
workers=[]
for col in range(get_world_size()):
    handle=spawn_drone(column)
    if handle == None:
        column()
    else:
        workers.append(handle)
    move(East)
for worker in workers:
    wait_for(worker)
```

结束库存：hay=292713.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=519738.192，cactus=13394.000，bone=81740.000，gold=25538.000，water=224.000，fertilizer=4077.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 317 · 验证固定种子模拟

类型：`simulation`；状态：`passed`；主农场 02:31:11 → 02:31:11。

两个相同种子的隔离农场执行相同实验，返回游戏时间应相等。

```python
a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
b=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
c=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)
print(a==b)
```

结束库存：hay=292713.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=519728.593，cactus=13394.000，bone=81740.000，gold=25538.000，water=224.000，fertilizer=4077.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 318 · 长尾生产 bone 至 170000

类型：`production`；状态：`passed`；主农场 02:31:11 → 02:37:36。

偶数网格上的哈密顿回路；只在边界受阻时结束，换帽结算尾长收益；倍率在长度 64 封顶，继续增长仍增加产量。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
# Use an even-sized empty field and keep cactus in inventory.
change_hat(Hats.Dinosaur_Hat)
while True:
    direction=East
    if get_pos_x()==0:
        if get_pos_y()>0:
            direction=South
    elif get_pos_y()%2==0:
        if get_pos_x()==get_world_size()-1:
            direction=North
    else:
        direction=West
        if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
            direction=North
    if not move(direction):
        break
change_hat(Hats.Straw_Hat)
```

结束库存：hay=292713.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=517422.029，cactus=12370.000，bone=196316.000，gold=25538.000，water=1440.000，fertilizer=5325.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1`。

## 319 · 研究 the_farmers_remains → 1

类型：`research`；状态：`passed`；主农场 02:37:36 → 02:37:36。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.The_Farmers_Remains)
```

结束库存：hay=292713.000，wood=670380.000，carrot=384456.000，pumpkin=1337476.000，power=517421.996，cactus=12370.000，bone=26316.000，gold=25538.000，water=1440.000，fertilizer=5325.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:7, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 320 · 研究 sunflowers → 8

类型：`research`；状态：`passed`；主农场 02:37:36 → 02:37:36。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Sunflowers)
```

结束库存：hay=292713.000，wood=670380.000，carrot=125707.000，pumpkin=1337476.000，power=517421.962，cactus=12370.000，bone=26316.000，gold=25538.000，water=1440.000，fertilizer=5325.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:6, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 321 · 研究 cactus → 7

类型：`research`；状态：`passed`；主农场 02:37:36 → 02:37:36。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=292713.000，wood=670380.000，carrot=125707.000，pumpkin=1099546.000，power=517421.928，cactus=12370.000，bone=26316.000，gold=25538.000，water=1440.000，fertilizer=5325.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 322 · 生产 hay 至 1310720

类型：`production`；状态：`passed`；主农场 02:37:36 → 02:38:10。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<1310720:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1474153.000，wood=670380.000，carrot=125707.000，pumpkin=1099546.000，power=517028.288，cactus=12370.000，bone=26316.000，gold=25538.000，water=0.000，fertilizer=5421.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 323 · 生产 wood 至 1310720

类型：`production`；状态：`passed`；主农场 02:38:10 → 02:38:32。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<1310720:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=1474153.000，wood=1388844.000，carrot=125707.000，pumpkin=1099546.000，power=516772.721，cactus=12370.000，bone=26316.000，gold=25538.000，water=0.000，fertilizer=5485.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 324 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:38:32 → 02:39:24。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=155241.000，wood=69932.000，carrot=654091.000，pumpkin=1099546.000，power=516155.848，cactus=12370.000，bone=26316.000，gold=25538.000，water=0.000，fertilizer=5677.000，weird_substance=276.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 325 · 生产 weird_substance 至 6154

类型：`production`；状态：`passed`；主农场 02:39:24 → 02:40:53。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<6154:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((6154-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=155241.000，wood=69932.000，carrot=491915.000，pumpkin=1879834.000，power=515112.230，cactus=12370.000，bone=26316.000，gold=25538.000，water=32.000，fertilizer=5949.000，weird_substance=6420.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:5, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 326 · 研究 mazes → 6

类型：`research`；状态：`passed`；主农场 02:40:53 → 02:40:53。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=155241.000，wood=69932.000，carrot=491915.000，pumpkin=1879834.000，power=515112.196，cactus=12370.000，bone=26316.000，gold=25538.000，water=32.000，fertilizer=5949.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 327 · 生产 cactus 至 35365

类型：`production`；状态：`passed`；主农场 02:40:53 → 02:45:00。

行冒泡排序后列冒泡排序；二维有序后连锁收获。比较复杂度 O(n³)，输出按连锁株数与封顶倍率计算。

```python
clear()
while num_items(Items.Cactus)<35365:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    complete=False
    while not complete:
        complete=True
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                move(East)
            move(North)
    # Run on a field of mature cacti. Bubble each row, then each column.
    for axis in [East, North]:
        for sweep in range(get_world_size()):
            for row in range(get_world_size()):
                for col in range(get_world_size()-1):
                    if measure() > measure(axis):
                        swap(axis)
                    move(axis)
                move(axis)
                if axis == East:
                    move(North)
                else:
                    move(East)
    harvest()
```

结束库存：hay=155241.000，wood=69932.000，carrot=491915.000，pumpkin=1814298.000，power=512211.744，cactus=1060946.000，bone=26316.000，gold=25538.000，water=736.000，fertilizer=6717.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:4, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 328 · 研究 dinosaurs → 5

类型：`research`；状态：`passed`；主农场 02:45:00 → 02:45:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=155241.000，wood=69932.000，carrot=491915.000，pumpkin=1814298.000，power=512211.711，cactus=1025581.000，bone=26316.000，gold=25538.000，water=736.000，fertilizer=6717.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:4, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 329 · 研究 megafarm → 5

类型：`research`；状态：`passed`；主农场 02:45:00 → 02:45:00。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Megafarm)
```

结束库存：hay=155241.000，wood=69932.000，carrot=491915.000，pumpkin=1814298.000，power=512211.677，cactus=1025581.000，bone=26316.000，gold=18465.000，water=736.000，fertilizer=6717.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 330 · 生产 hay 至 524288

类型：`production`；状态：`passed`；主农场 02:45:00 → 02:45:13。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=550249.000，wood=69932.000，carrot=491915.000，pumpkin=1814298.000，power=512057.858，cactus=1025581.000，bone=26316.000，gold=18465.000，water=0.000，fertilizer=6749.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 331 · 生产 wood 至 524288

类型：`production`；状态：`passed`；主农场 02:45:13 → 02:45:34。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=550249.000，wood=770028.000，carrot=491915.000，pumpkin=1814298.000，power=511803.368，cactus=1025581.000，bone=26316.000，gold=18465.000，water=0.000，fertilizer=6845.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 332 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:45:34 → 02:45:59。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=17769.000，wood=237548.000，carrot=627083.000，pumpkin=1814298.000，power=511510.979，cactus=1025581.000，bone=26316.000，gold=18465.000，water=0.000，fertilizer=6909.000，weird_substance=266.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 333 · 生产 weird_substance 至 10240

类型：`production`；状态：`passed`；主农场 02:45:59 → 02:47:42。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<10240:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((10240-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=510305.313，cactus=1025581.000，bone=26316.000，gold=18465.000，water=32.000，fertilizer=7203.000，weird_substance=10250.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 334 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:47:42 → 02:47:50。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=510214.080，cactus=1025581.000，bone=26316.000，gold=51233.000，water=32.000，fertilizer=7235.000，weird_substance=9226.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 335 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:47:50 → 02:47:58。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=510114.863，cactus=1025581.000，bone=26316.000，gold=84001.000，water=64.000，fertilizer=7267.000，weird_substance=8202.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 336 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:47:58 → 02:48:06。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=510022.273，cactus=1025581.000，bone=26316.000，gold=116769.000，water=96.000，fertilizer=7299.000，weird_substance=7178.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 337 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:48:06 → 02:48:17。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509891.073，cactus=1025581.000，bone=26316.000，gold=149537.000，water=128.000，fertilizer=7331.000，weird_substance=6154.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 338 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:48:17 → 02:48:24。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509810.960，cactus=1025581.000，bone=26316.000，gold=182305.000，water=160.000，fertilizer=7363.000，weird_substance=5130.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 339 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:48:24 → 02:48:30。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509737.633，cactus=1025581.000，bone=26316.000，gold=215073.000，water=160.000，fertilizer=7363.000，weird_substance=4106.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 340 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:48:30 → 02:48:48。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509531.951，cactus=1025581.000，bone=26316.000，gold=247841.000，water=224.000，fertilizer=7427.000，weird_substance=3082.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 341 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:48:48 → 02:49:01。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509381.663，cactus=1025581.000，bone=26316.000，gold=280609.000，water=256.000，fertilizer=7459.000，weird_substance=2058.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 342 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:49:01 → 02:49:13。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509240.657，cactus=1025581.000，bone=26316.000，gold=313377.000，water=320.000，fertilizer=7491.000，weird_substance=1034.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 343 · 迷宫生产 gold 至 320000

类型：`production`；状态：`passed`；主农场 02:49:13 → 02:49:19。

迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509165.991，cactus=1025581.000，bone=26316.000，gold=346145.000，water=320.000，fertilizer=7523.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1`。

## 344 · 研究 top_hat → 1

类型：`research`；状态：`passed`；主农场 02:49:19 → 02:49:19。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Top_Hat)
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=2590746.000，power=509165.958，cactus=1025581.000，bone=26316.000，gold=26145.000，water=320.000，fertilizer=7523.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:7, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 345 · 研究 cactus → 8

类型：`research`；状态：`passed`；主农场 02:49:19 → 02:49:19。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Cactus)
```

结束库存：hay=17769.000，wood=237548.000，carrot=466443.000，pumpkin=1900750.000，power=509165.924，cactus=1025581.000，bone=26316.000，gold=26145.000，water=320.000，fertilizer=7523.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 346 · 生产 hay 至 524288

类型：`production`；状态：`passed`；主农场 02:49:19 → 02:49:37。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=639081.000，wood=237548.000，carrot=466443.000，pumpkin=1900750.000，power=508958.898，cactus=1025581.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7587.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 347 · 生产 wood 至 524288

类型：`production`；状态：`passed`；主农场 02:49:37 → 02:49:58。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=639081.000，wood=910764.000，carrot=466443.000，pumpkin=1900750.000，power=508704.408，cactus=1025581.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7651.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 348 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:49:58 → 02:50:23。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=114793.000，wood=386476.000，carrot=597515.000，pumpkin=1900750.000，power=508413.095，cactus=1025581.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7747.000，weird_substance=10.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 349 · 生产 weird_substance 至 17845

类型：`production`；状态：`passed`；主农场 02:50:23 → 02:51:55。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<17845:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((17845-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=114793.000，wood=386476.000，carrot=433419.000，pumpkin=2669134.000，power=507326.794，cactus=1025581.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7988.000，weird_substance=18058.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:6, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 350 · 研究 mazes → 7

类型：`research`；状态：`passed`；主农场 02:51:55 → 02:51:55。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=114793.000，wood=386476.000，carrot=433419.000，pumpkin=2669134.000，power=507326.760，cactus=1025581.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7988.000，weird_substance=213.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:5, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 351 · 研究 dinosaurs → 6

类型：`research`；状态：`passed`；主农场 02:51:55 → 02:51:55。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=114793.000，wood=386476.000，carrot=433419.000，pumpkin=2669134.000，power=507326.727，cactus=923025.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=7988.000，weird_substance=213.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 352 · 生产 hay 至 786432

类型：`production`；状态：`passed`；主农场 02:51:55 → 02:52:17。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<786432:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=915305.000，wood=386476.000，carrot=433419.000，pumpkin=2669134.000，power=507069.455，cactus=923025.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=8052.000，weird_substance=213.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 353 · 生产 wood 至 786432

类型：`production`；状态：`passed`；主农场 02:52:17 → 02:52:39。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<786432:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=915305.000，wood=1074476.000，carrot=433419.000，pumpkin=2669134.000，power=506814.965，cactus=923025.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=8116.000，weird_substance=213.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 354 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:52:39 → 02:53:13。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=120681.000，wood=279852.000，carrot=699659.000，pumpkin=2669134.000，power=506413.337，cactus=923025.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=8244.000，weird_substance=213.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 355 · 生产 weird_substance 至 51750

类型：`production`；状态：`passed`；主农场 02:53:13 → 02:54:32。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<51750:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((51750-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3403726.000，power=505486.289，cactus=923025.000，bone=26316.000，gold=26145.000，water=32.000，fertilizer=8333.000，weird_substance=52053.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:7, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 356 · 研究 mazes → 8

类型：`research`；状态：`passed`；主农场 02:54:32 → 02:54:32。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Mazes)
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3403726.000，power=505486.255，cactus=923025.000，bone=26316.000，gold=26145.000，water=32.000，fertilizer=8333.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:6, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 357 · 研究 dinosaurs → 7

类型：`research`；状态：`passed`；主农场 02:54:32 → 02:54:32。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3403726.000，power=505486.222，cactus=625613.000，bone=26316.000，gold=26145.000，water=32.000，fertilizer=8333.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:7, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 358 · 生产 cactus 至 862494

类型：`production`；状态：`passed`；主农场 02:54:32 → 02:58:37。

行冒泡排序后列冒泡排序；二维有序后连锁收获。比较复杂度 O(n³)，输出按连锁株数与封顶倍率计算。

```python
clear()
while num_items(Items.Cactus)<862494:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    complete=False
    while not complete:
        complete=True
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                move(East)
            move(North)
    # Run on a field of mature cacti. Bubble each row, then each column.
    for axis in [East, North]:
        for sweep in range(get_world_size()):
            for row in range(get_world_size()):
                for col in range(get_world_size()-1):
                    if measure() > measure(axis):
                        swap(axis)
                    move(axis)
                move(axis)
                if axis == East:
                    move(North)
                else:
                    move(East)
    harvest()
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3272654.000，power=502607.889，cactus=2722765.000，bone=26316.000，gold=26145.000，water=736.000，fertilizer=9133.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:7, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 359 · 研究 dinosaurs → 8

类型：`research`；状态：`passed`；主农场 02:58:37 → 02:58:37。

unlock() 按真实前置与库存扣费；无资源注入。

```python
unlock(Unlocks.Dinosaurs)
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3272654.000，power=502607.855，cactus=1860271.000，bone=26316.000，gold=26145.000，water=736.000，fertilizer=9133.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 360 · 验证大师礼帽外观接口

类型：`feature`；状态：`passed`；主农场 02:58:37 → 02:58:37。

研究奖励解锁后更换礼帽，再恢复草帽。

```python
change_hat(Hats.Top_Hat)
change_hat(Hats.Straw_Hat)
```

结束库存：hay=120681.000，wood=279852.000，carrot=535691.000，pumpkin=3272654.000，power=502607.788，cactus=1860271.000，bone=26316.000，gold=26145.000，water=736.000，fertilizer=9133.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 361 · 生产 hay 至 524288

类型：`production`；状态：`passed`；主农场 02:58:37 → 02:58:56。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
while num_items(Items.Hay)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=777833.000，wood=279852.000，carrot=535691.000，pumpkin=3272654.000，power=502382.043，cactus=1860271.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=9197.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 362 · 生产 wood 至 524288

类型：`production`；状态：`passed`；主农场 02:58:56 → 02:59:18。

棋盘树木与灌木；避免四邻树木减速，自动补种。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<524288:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=777833.000，wood=970092.000，carrot=535691.000，pumpkin=3272654.000，power=502127.553，cactus=1860271.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=9261.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 363 · 生产 carrot 至 576717

类型：`production`；状态：`passed`；主农场 02:59:18 → 02:59:43。

逐行遍历、成熟判断与循环补种；根据实际库存结束。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<576717:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

结束库存：hay=253545.000，wood=445804.000，carrot=666763.000，pumpkin=3272654.000，power=501836.241，cactus=1860271.000，bone=26316.000，gold=26145.000，water=0.000，fertilizer=9325.000，weird_substance=303.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 364 · 生产 weird_substance 至 8192

类型：`production`；状态：`passed`；主农场 02:59:43 → 03:01:01。

反复扫描并补种枯死南瓜；整块成熟后收获。按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。

```python
clear()
while num_items(Items.Weird_Substance)<8192:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((8192-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
```

结束库存：hay=253545.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=500912.969，cactus=1860271.000，bone=26316.000，gold=26145.000，water=32.000，fertilizer=9560.000，weird_substance=8367.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 365 · 验证迷宫复用与开墙

类型：`production`；状态：`passed`；主农场 03:01:01 → 03:01:33。

第一次寻宝后复用，重新 DFS 搜索随机新目标，最后收获；算法允许地图出现环。

```python
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))

use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()
```

结束库存：hay=253545.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=500536.162，cactus=1860271.000，bone=26316.000，gold=288289.000，water=128.000，fertilizer=9688.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 366 · 验证 32 台工作机同时生产

类型：`production`；状态：`passed`；主农场 03:01:33 → 03:01:45。

按列派生任务；槽满时主机回退执行；等待所有句柄结束。工作机持续工作八游戏秒，保证同时占满 32 个槽位。

```python
clear()
while not can_harvest():
    pass
def column(seconds):
    start=get_time()
    while get_time()-start<seconds:
        if can_harvest():
            harvest()
        move(North)
    return True
workers=[]
for col in range(get_world_size()):
    handle=spawn_drone(column,8)
    if handle==None:
        column(8)
    else:
        workers.append(handle)
    move(East)
for worker in workers:
    wait_for(worker)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.581，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 367 · 本地竞速 Hay

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Hay,'race_hay',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.574，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 368 · 单机竞速 Hay

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Hay_Single,'race_hay',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.567，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 369 · 本地竞速 Wood

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Wood,'race_wood',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.558，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 370 · 单机竞速 Wood

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Wood_Single,'race_wood',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.549，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 371 · 本地竞速 Carrots

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Carrots,'race_carrots',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.538，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 372 · 单机竞速 Carrots

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Carrots_Single,'race_carrots',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.527，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 373 · 本地竞速 Pumpkins

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Pumpkins,'race_pumpkins',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.503，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 374 · 单机竞速 Pumpkins

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Pumpkins_Single,'race_pumpkins',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.478，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 375 · 本地竞速 Sunflowers

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Sunflowers,'race_sunflowers',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.449，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 376 · 单机竞速 Sunflowers

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Sunflowers_Single,'race_sunflowers',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.419，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 377 · 本地竞速 Cactus

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Cactus,'race_cactus',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.375，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 378 · 单机竞速 Cactus

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:45。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Cactus_Single,'race_cactus',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.332，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 379 · 本地竞速 Maze

类型：`race`；状态：`passed`；主农场 03:01:45 → 03:01:46。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Maze,'race_maze',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.320，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 380 · 单机竞速 Maze

类型：`race`；状态：`passed`；主农场 03:01:46 → 03:01:46。

相同官方本地预设，限制为一台无人机；仍使用实际解释器。

```python
leaderboard_run(Leaderboards.Maze_Single,'race_maze',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.308，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 381 · 本地竞速 Dinosaur

类型：`race`；状态：`passed`；主农场 03:01:46 → 03:01:46。

游戏自带的种子 41、8×8、中阶科技及分类生产目标，达标后所有任务终止。

```python
leaderboard_run(Leaderboards.Dinosaur,'race_dinosaur',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497473.262，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

## 382 · 最快重置完整竞速

类型：`race`；状态：`passed`；主农场 03:01:46 → 03:01:46。

从一格零物资开始，遵循竞速预设允许的语言功能，通过真实生产支付科技成本，直到解锁本地竞速。

```python
leaderboard_run(Leaderboards.Fastest_Reset,'race_reset',64)
```

结束库存：hay=1039977.000，wood=445804.000，carrot=502283.000，pumpkin=4051022.000，power=497472.531，cactus=1860271.000，bone=26316.000，gold=288289.000，water=160.000，fertilizer=9720.000，weird_substance=175.000。

结束研究：`loops:1, operators:1, variables:1, senses:1, plant:1, expand:5, grass:8, trees:8, speed:8, functions:1, lists:1, costs:1, timing:1, hats:1, carrots:8, watering:6, dicts:1, import:1, auto_unlock:1, debug:1, debug2:1, utilities:1, polyculture:6, pumpkins:8, fertilizer:6, sunflowers:8, cactus:8, mazes:8, dinosaurs:8, megafarm:5, simulation:1, leaderboard:1, the_farmers_remains:1, top_hat:1`。

