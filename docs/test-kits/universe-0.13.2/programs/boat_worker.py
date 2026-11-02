from worker_helpers import idle
while True:
    if idle(worker_self()):
        catch_carp(worker_self(), 1)
    worker_wait(1)
