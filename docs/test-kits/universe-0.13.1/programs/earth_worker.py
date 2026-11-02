# Bounded latitude rows: sweep east, then return west at each row.
while True:
    for y in range(get_world_height()):
        for x in range(get_world_width()):
            if can_harvest():
                harvest()
            if x + 1 < get_world_width():
                move(East)
        for x in range(get_world_width() - 1):
            move(West)
        if y + 1 < get_world_height():
            move(North)
    for y in range(get_world_height() - 1):
        move(South)
