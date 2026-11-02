# Earth farmer uses the original field and inventory.
while True:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                plant(Entities.Grass)
            move(East)
        move(North)
