# Imported by specialist programs. Each machine has its own interpreter and inbox.
def run(allowed):
    identifier = fleet_self()
    while True:
        own = None
        for unit in fleet_units():
            if unit["id"] == identifier:
                own = unit
                break
        if own == None:
            break
        if own["jobs"] == 0:
            command = fleet_receive()
            if command != None:
                if command[0] == "work" and command[2] in allowed:
                    fleet_work(identifier,command[1],command[2],command[3])
                elif command[0] == "ship" and "cargo" in allowed:
                    fleet_ship(identifier,command[1],command[2],command[3],command[4])
        fleet_wait(0.5)
