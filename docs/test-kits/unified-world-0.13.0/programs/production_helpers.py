# Import this library from managers or workers.
def idle(drone_id):
    for drone in fleet_units():
        if drone['id'] == drone_id:
            return drone['jobs'] == 0 and not drone['flying']
    return False

def shipment(hauler, source, target, item, amount):
    return fleet_send(hauler, {'from': source, 'to': target, 'item': item, 'amount': amount})
