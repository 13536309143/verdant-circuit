from local_common import own
while True:
    unit = own()
    if unit != None and unit["jobs"] == 0:
        area = unit["width"]*unit["height"]
        if fleet_stock(0,"grain") < 200 and fleet_stock(0,"water") >= area:
            fleet_work(unit["id"],0,"grain",1)
    fleet_wait(1)
