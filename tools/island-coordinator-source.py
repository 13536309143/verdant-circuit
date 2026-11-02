# Requires Functions, Dictionaries, Lists, Operators, Variables, Timing and Debug.
# Signs the charter with real farm resources, then automates all 18 island orders.
# Every queued job uses physical machines, local depot inputs and ordinary credit costs.
# View Archipelago / Operations to watch the network and the current order.

def provider(item, opened):
    if item == "water":
        if opened > 1:
            return 1
        return 0
    locations = {"grain":0, "ore":2, "alloy":2, "crystal":3, "medicine":4, "core":5}
    return locations[item]

outputs = {"water":6, "grain":2, "ore":2, "alloy":2, "crystal":2, "medicine":2, "core":1}
inputs = {"water":{}, "grain":{"water":1}, "ore":{}, "alloy":{"ore":2,"grain":1,"water":1}, "crystal":{"alloy":1,"water":2}, "medicine":{"grain":2,"crystal":1,"water":1}, "core":{"alloy":2,"crystal":2,"medicine":1}}
order_items = ["core","medicine","crystal","alloy","ore","grain","water"]
roles = ["irrigator","grower","engineer","scout","grower","engineer"]
sizes = [(1,1),(2,1),(2,2),(3,2),(3,3)]

if not fleet_start():
    print("Charter needs Carrots research, 100 Wood and 100 Carrots.")

while fleet_state()["started"] and not fleet_state()["complete"]:
    # Slot count, hull size and specialisms are independent paid research tracks.
    while fleet_research("slots"):
        pass
    while fleet_research("size"):
        pass
    for kind in ["engineer","scout","harvester"]:
        fleet_research(kind)
    state = fleet_state()
    opened = state["islands"]
    tier = state["size_tier"]
    width,height = sizes[tier]
    units = fleet_units()
    hauler_count = 0
    for unit in units:
        if unit["role"] == "hauler":
            hauler_count += 1
    if "harvester" in state["roles"]:
        for unit in units:
            if unit["role"] == "grower" and unit["island"] == 0 and unit["jobs"] == 0 and not unit["flying"]:
                fleet_remove(unit["id"])
        units = fleet_units()
    grain_role = "grower"
    if "harvester" in state["roles"]:
        grain_role = "harvester"
    used = set()
    workers = {}
    # One machine runs both ore and alloy recipes on Ember.
    for location in range(opened):
        if location == 1:
            continue
        role = roles[location]
        destination = location
        if location == 0:
            for water_role in ["irrigator",grain_role]:
                destination = provider("water",opened)
                if water_role != "irrigator":
                    destination = 0
                candidate = None
                for unit in units:
                    if unit["role"] == water_role and unit["id"] not in used and (water_role == "irrigator" or unit["island"] == destination):
                        candidate = unit
                        break
                if candidate == None and len(units) < state["slot_limit"]-max(0,1-hauler_count):
                    identifier = fleet_build(water_role,width,height,destination)
                    units = fleet_units()
                    for unit in units:
                        if unit["id"] == identifier:
                            candidate = unit
                if candidate != None:
                    used.add(candidate["id"])
                    workers[(destination,water_role)] = candidate
        else:
            candidate = None
            for unit in units:
                if unit["role"] == role and unit["island"] == destination and unit["id"] not in used:
                    candidate = unit
                    break
            if candidate == None and len(units) < state["slot_limit"]-max(0,1-hauler_count):
                identifier = fleet_build(role,width,height,destination)
                units = fleet_units()
                for unit in units:
                    if unit["id"] == identifier:
                        candidate = unit
            if candidate != None:
                used.add(candidate["id"])
                workers[(destination,role)] = candidate
    # Upgrade only idle machines. Dismantling refunds the paid hull cost.
    for key in workers:
        worker = workers[key]
        if worker["jobs"] == 0 and not worker["flying"] and worker["width"]*worker["height"] < width*height:
            if fleet_remove(worker["id"]):
                identifier = fleet_build(worker["role"],width,height,worker["island"])
                for unit in fleet_units():
                    if unit["id"] == identifier:
                        workers[key] = unit
    units = fleet_units()
    while len(units) < state["slot_limit"] and hauler_count < 2:
        if fleet_build("hauler",width,height,0) < 0:
            break
        units = fleet_units()
        hauler_count += 1
    for unit in units:
        if unit["role"] == "hauler" and unit["jobs"] == 0 and not unit["flying"] and unit["width"]*unit["height"] < width*height:
            if fleet_remove(unit["id"]):
                fleet_build("hauler",width,height,unit["island"])
    units = fleet_units()
    demand = {}
    for goal in fleet_order()["goals"]:
        demand[(goal["island"],goal["item"])] = goal["amount"]
    expected = {}
    for unit in units:
        if unit["shipment_remaining"] > 0:
            key = (unit["shipment_target"],unit["shipment_item"])
            if key not in expected:
                expected[key] = 0
            expected[key] += unit["shipment_remaining"]
        if unit["production_amount"] > 0:
            key = (unit["island"],unit["production_item"])
            if key not in expected:
                expected[key] = 0
            expected[key] += unit["production_amount"]
    shipments = []
    production = {}
    # Gather every remote requirement before expanding a source recipe once.
    for item in order_items:
        source = provider(item,opened)
        if source >= opened:
            continue
        source_key = (source,item)
        for island in range(opened):
            key = (island,item)
            if island == source or key not in demand:
                continue
            incoming = 0
            if key in expected:
                incoming = expected[key]
            deficit = max(0,demand[key]-fleet_stock(island,item)-incoming)
            if deficit > 0:
                shipments.append([source,island,item,deficit])
                if source_key not in demand:
                    demand[source_key] = 0
                demand[source_key] += deficit
        if source_key in demand:
            incoming = 0
            if source_key in expected:
                incoming = expected[source_key]
            deficit = max(0,demand[source_key]-fleet_stock(source,item)-incoming)
            production[source_key] = deficit
            production_role = "engineer"
            if item == "water":
                production_role = "irrigator"
            elif item == "grain" or item == "medicine":
                production_role = "grower"
                if item == "grain" and grain_role == "harvester":
                    production_role = "harvester"
            elif item == "crystal":
                production_role = "scout"
            worker_key = (source,production_role)
            area = 1
            if worker_key in workers:
                area = workers[worker_key]["width"]*workers[worker_key]["height"]
            batches = int((deficit+outputs[item]*area-1)/(outputs[item]*area))
            for raw in inputs[item]:
                parent = (source,raw)
                if parent not in demand:
                    demand[parent] = 0
                demand[parent] += batches*area*inputs[item][raw]
    for key in workers:
        worker = workers[key]
        if worker["jobs"] != 0:
            continue
        destination,role = key
        for item in ["water","grain","ore","alloy","crystal","medicine","core"]:
            task = (destination,item)
            allowed = role == "irrigator" and item == "water" or role == "grower" and item in ["grain","medicine"] or role == "harvester" and item == "grain" or role == "engineer" and item in ["ore","alloy","core"] or role == "scout" and item == "crystal"
            if task in production and production[task] > 0 and allowed:
                ready = True
                for raw in inputs[item]:
                    if fleet_stock(destination,raw) < inputs[item][raw]*worker["width"]*worker["height"]:
                        ready = False
                if ready:
                    fleet_work(worker["id"],destination,item,1)
                    break
    reserved = {}
    for unit in units:
        if unit["role"] != "hauler" or unit["jobs"] != 0:
            continue
        for request in shipments:
            source,destination,item,needed = request
            key = (source,item)
            if key not in reserved:
                reserved[key] = 0
            # Keep the source's own order reserve; other inputs are recomputed next pass.
            reserve = 0
            for goal in fleet_order()["goals"]:
                if goal["island"] == source and goal["item"] == item:
                    reserve += goal["amount"]
            available = max(0,fleet_stock(source,item)-reserve)
            amount = int(min(needed,available,unit["capacity"]))
            if amount > 0:
                if fleet_ship(unit["id"],source,destination,item,amount):
                    reserved[key] += amount
                    request[3] -= amount
                    break
    if fleet_submit():
        print("island_order_complete",fleet_state()["order"],"time",fleet_state()["time"])
    fleet_wait(2)
print("archipelago_complete",fleet_state()["complete"])
