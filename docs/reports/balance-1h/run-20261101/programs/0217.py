clear()
while num_items(Items.Cactus)<458872:
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
