# Bind to Universe. Managers stay alive independently.
planet_run(0, 'earth_manager')
for planet in universe_planets():
    if planet['unlocked']:
        if planet['id'] == 1:
            planet_run(1, 'flower_manager')
        elif planet['id'] == 2:
            planet_run(2, 'ocean_manager')
        elif planet['id'] == 3:
            planet_run(3, 'mineral_manager')
