# One free initial native worker per unlocked planet.
worker = worker_starter(0)
if worker > 0:
    program_bind(worker, 'earth_worker')
    program_run(worker)
