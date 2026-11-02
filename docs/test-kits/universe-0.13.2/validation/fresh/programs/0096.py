clear()
while num_items(Items.Cactus)<30000:
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
    complete=False
    while not complete:
        complete=True
        while get_pos_x()>0:
            move(West)
        while get_pos_y()>0:
            move(South)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                if col+1<get_world_size():
                    move(East)
            while get_pos_x()>0:
                move(West)
            if row+1<get_world_size():
                move(North)
        while get_pos_y()>0:
            move(South)
    for pass_id in range(get_world_size()):
        while get_pos_x()>0:
            move(West)
        while get_pos_y()>0:
            move(South)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_pos_x()+1<get_world_size() and measure()>measure(East):
                    swap(East)
                if col+1<get_world_size():
                    move(East)
            while get_pos_x()>0:
                move(West)
            if row+1<get_world_size():
                move(North)
        while get_pos_y()>0:
            move(South)
    for pass_id in range(get_world_size()):
        while get_pos_x()>0:
            move(West)
        while get_pos_y()>0:
            move(South)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_pos_y()+1<get_world_size() and measure()>measure(North):
                    swap(North)
                if col+1<get_world_size():
                    move(East)
            while get_pos_x()>0:
                move(West)
            if row+1<get_world_size():
                move(North)
        while get_pos_y()>0:
            move(South)
    harvest()
