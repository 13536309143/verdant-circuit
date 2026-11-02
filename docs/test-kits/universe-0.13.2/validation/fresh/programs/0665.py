clear()
while num_items(Items.Pumpkin)<28292:
    complete=True
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
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
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
    if complete:
        harvest()
