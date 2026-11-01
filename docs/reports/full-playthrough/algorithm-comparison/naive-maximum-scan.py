clear()
while num_items(Items.Power)<1000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Sunflower)
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
    remaining=get_world_size()*get_world_size()
    while remaining>0:
        best=-1
        bx=0
        by=0
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and can_harvest():
                    petals=measure()
                    if petals>best:
                        best=petals
                        bx=get_pos_x()
                        by=get_pos_y()
                move(East)
            move(North)
        if best>=0:
            while get_pos_x()!=bx:
                move(East)
            while get_pos_y()!=by:
                move(North)
            harvest()
            remaining-=1
