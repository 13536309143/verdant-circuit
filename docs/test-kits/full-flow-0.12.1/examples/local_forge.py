from local_common import own
while True:
    unit = own()
    if unit != None and unit["jobs"] == 0:
        area = unit["width"]*unit["height"]
        if fleet_stock(2,"alloy") < 200:
            if fleet_stock(2,"ore") < 2*area:
                fleet_work(unit["id"],2,"ore",1)
            elif fleet_stock(2,"grain") >= area and fleet_stock(2,"water") >= area:
                fleet_work(unit["id"],2,"alloy",1)
    fleet_wait(1)
