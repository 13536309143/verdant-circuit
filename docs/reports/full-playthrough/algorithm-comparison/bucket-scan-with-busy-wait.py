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
    complete=False
    while not complete:
        complete=True
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                move(East)
            move(North)
    for petals in range(15,6,-1):
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and measure()==petals:
                    harvest()
                move(East)
            move(North)
