# Use target 30 before Trees; use 900 after the 4x4 expansion.
target = 900
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                if num_unlocked(Unlocks.Trees) > 0 and (get_pos_x()+get_pos_y())%2 == 0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)

