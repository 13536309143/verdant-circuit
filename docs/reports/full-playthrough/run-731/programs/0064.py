clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<5734:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
