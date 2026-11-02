# Run from Universe, after Spaceflight and Flower Planet are researched.
ships = ufo_fleet()
if len(ships) == 0:
    ship = ufo_build()
else:
    ship = ships[0]['id']
if ship > 0:
    if planet_inventory(0, 'water') >= 12:
        ufo_transfer(ship, 0, 1, 'water', 12)
