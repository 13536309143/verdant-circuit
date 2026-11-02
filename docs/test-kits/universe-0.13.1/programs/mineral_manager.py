# One free initial native worker per unlocked planet.
worker = worker_starter(3)
if worker > 0:
    program_bind(worker, 'miner_worker')
    program_run(worker)
