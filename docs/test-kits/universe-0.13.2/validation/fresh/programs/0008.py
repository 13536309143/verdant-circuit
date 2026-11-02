clear()
while num_items(Items.Hay)<50:
    while get_pos_x()>0:
        move(West)
    while get_pos_y()>0:
        move(South)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if col+1<get_world_size():
                move(East)
        while get_pos_x()>0:
            move(West)
        if row+1<get_world_size():
            move(North)
    while get_pos_y()>0:
        move(South)
