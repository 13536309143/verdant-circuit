import farm_lib
farm_lib.ensure(Items.Hay,1000)
farm_lib.ensure(Items.Wood,1000)
before = num_items(Items.Hay)
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

companion_ok = num_items(Items.Hay)-before > 2**num_unlocked(Unlocks.Grass)
print("companion_ok", companion_ok)
