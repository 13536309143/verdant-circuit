clear()
while num_items(Items.Solar_Energy)<6000:
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Sunflower)
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            while not can_harvest():
                pass
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
    for petals in range(15,6,-1):
        while get_pos_x()>0:
            move(West)
        while get_pos_y()>0:
            move(South)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and measure()==petals:
                    harvest()
                if col+1<get_world_size():
                    move(East)
            while get_pos_x()>0:
                move(West)
            if row+1<get_world_size():
                move(North)
        while get_pos_y()>0:
            move(South)
