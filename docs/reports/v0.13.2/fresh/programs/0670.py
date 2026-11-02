clear()
while num_items(Items.Catalyst)<732:
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
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((732-num_items(Items.Catalyst)-1)/per_plant)+1)
        while get_pos_x()>0:
            move(West)
        while get_pos_y()>0:
            move(South)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                if col+1<get_world_size():
                    move(East)
            while get_pos_x()>0:
                move(West)
            if row+1<get_world_size():
                move(North)
        while get_pos_y()>0:
            move(South)
        harvest()
