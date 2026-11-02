# Change target to 50, 200 or 500 as directed in the guide.
target = 50
clear()
while num_items(Items.Hay) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)

