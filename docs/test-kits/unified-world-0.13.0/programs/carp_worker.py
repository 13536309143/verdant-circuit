from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        catch_carp(fleet_self(), 1)
    fleet_wait(1)
