def own():
    identifier = fleet_self()
    for unit in fleet_units():
        if unit["id"] == identifier:
            return unit
    return None
