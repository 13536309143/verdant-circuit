# One free initial native worker per unlocked planet.
worker = worker_starter(1)
if worker > 0:
    program_bind(worker, 'bee_worker')
    program_run(worker)
