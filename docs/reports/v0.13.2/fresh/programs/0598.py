clear()
while get_pos_x()>0:
    move(West)
while get_pos_y()>0:
    move(South)
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        if col+1<get_world_size():
            move(East)
    while get_pos_x()>0:
        move(West)
    if row+1<get_world_size():
        move(North)
while get_pos_y()>0:
    move(South)
while num_items(Items.Wood)<17845:
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
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
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
