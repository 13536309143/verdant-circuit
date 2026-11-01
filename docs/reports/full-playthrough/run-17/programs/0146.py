clear()
entity,position=get_companion()
x,y=position
while get_pos_x()!=x:
    move(East)
while get_pos_y()!=y:
    move(North)
till()
plant(entity)
while get_pos_x()!=0:
    move(East)
while get_pos_y()!=0:
    move(North)
while not can_harvest():
    do_a_flip()
harvest()
