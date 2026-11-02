# Bind to Network. Start once; managers and workers remain independent.
island_run(0, 'earth_manager')
for island in world_islands():
    if island['unlocked']:
        if island['id'] == 6:
            island_run(6, 'flower_manager')
        elif island['id'] == 7:
            island_run(7, 'mineral_manager')
        elif island['id'] == 8:
            island_run(8, 'fish_manager')
