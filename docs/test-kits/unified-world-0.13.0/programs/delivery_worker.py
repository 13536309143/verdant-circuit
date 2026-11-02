# Bind to a hauler. Commands come from an island or network manager.
while True:
    message = fleet_receive()
    if message != None:
        accepted = fleet_ship(fleet_self(), message['from'], message['to'], message['item'], message['amount'])
        print(accepted)
    fleet_wait(1)
