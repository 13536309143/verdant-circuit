# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'grower' and drone['island'] == 6:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('grower', 1, 1, 6)
if worker >= 0:
    program_bind(worker, 'rose_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')
