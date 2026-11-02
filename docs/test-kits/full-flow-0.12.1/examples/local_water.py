from local_common import own
while True:
    unit = own()
    if unit != None and unit["jobs"] == 0:
        if fleet_stock(1,"water") < 400:
            fleet_work(unit["id"],1,"water",1)
    fleet_wait(1)
