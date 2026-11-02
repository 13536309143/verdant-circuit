# One free initial native worker per unlocked planet.
worker = worker_starter(2)
if worker > 0:
    program_bind(worker, 'boat_worker')
    program_run(worker)
