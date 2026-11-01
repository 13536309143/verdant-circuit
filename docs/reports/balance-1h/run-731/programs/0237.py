clear()
while num_items(Items.Weird_Substance)<3921:
    complete=True
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
            move(East)
        move(North)
    if complete:
        per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
        infected=min(get_world_size()*get_world_size(),int((3921-num_items(Items.Weird_Substance)-1)/per_plant)+1)
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if infected>0:
                    while num_items(Items.Fertilizer)==0:
                        pass
                    use_item(Items.Fertilizer)
                    infected-=1
                move(East)
            move(North)
        harvest()
