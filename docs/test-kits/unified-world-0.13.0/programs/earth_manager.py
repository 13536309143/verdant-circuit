# Bind to Earth manager. Reuse the existing farmer instead of building duplicates.
for drone in fleet_units():
    if drone['role'] == 'farmer':
        if not drone['program_running']:
            program_bind(drone['id'], 'earth_worker')
            program_run(drone['id'])
        break
