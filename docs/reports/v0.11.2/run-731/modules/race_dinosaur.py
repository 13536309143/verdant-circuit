while num_items(Items.Bone)<30000:
    clear()
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            till()
            move(East)
        move(North)
    # Use an even-sized empty field and keep cactus in inventory.
    change_hat(Hats.Dinosaur_Hat)
    while True:
        direction=East
        if get_pos_x()==0:
            if get_pos_y()>0:
                direction=South
        elif get_pos_y()%2==0:
            if get_pos_x()==get_world_size()-1:
                direction=North
        else:
            direction=West
            if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
                direction=North
        if not move(direction):
            break
    change_hat(Hats.Straw_Hat)
