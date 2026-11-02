clear()
while not can_harvest():
    pass
def column(seconds):
    start=get_time()
    while get_time()-start<seconds:
        if can_harvest():
            harvest()
        move(North)
    return True
workers=[]
for col in range(get_world_size()):
    handle=spawn_drone(column,8)
    if handle==None:
        column(8)
    else:
        workers.append(handle)
    move(East)
for worker in workers:
    wait_for(worker)

print("parallel_complete", num_drones()==1, "capacity", max_drones())

