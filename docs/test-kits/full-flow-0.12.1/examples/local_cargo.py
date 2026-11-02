from local_common import own
routes = [(1,0,"water",64),(0,2,"grain",32),(1,2,"water",64)]
cursor = 0
while True:
    unit = own()
    if unit != None and unit["jobs"] == 0:
        for attempt in range(len(routes)):
            source,target,item,reserve = routes[cursor]
            cursor = (cursor+1)%len(routes)
            needed = max(0,reserve-fleet_stock(target,item))
            amount = int(min(needed,fleet_stock(source,item),unit["capacity"]))
            if amount > 0:
                if fleet_ship(unit["id"],source,target,item,amount):
                    break
    fleet_wait(1)
