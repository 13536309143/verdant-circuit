from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        mine_stone(fleet_self(), 1)
    fleet_wait(1)
