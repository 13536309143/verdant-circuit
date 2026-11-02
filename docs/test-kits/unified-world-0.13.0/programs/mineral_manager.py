fleet_research('engineer')
# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'engineer' and drone['island'] == 7:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('engineer', 1, 1, 7)
if worker >= 0:
    program_bind(worker, 'stone_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')
