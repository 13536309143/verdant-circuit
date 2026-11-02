fleet_research('harvester')
# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'harvester' and drone['island'] == 8:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('harvester', 1, 1, 8)
if worker >= 0:
    program_bind(worker, 'carp_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')
