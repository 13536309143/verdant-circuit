clear()
while not can_harvest():
    pass
def column():
    for row in range(get_world_size()):
        if can_harvest():
            harvest()
        move(North)
    return True
workers=[]
for col in range(get_world_size()):
    handle=spawn_drone(column)
    if handle == None:
        column()
    else:
        workers.append(handle)
    move(East)
for worker in workers:
    wait_for(worker)
