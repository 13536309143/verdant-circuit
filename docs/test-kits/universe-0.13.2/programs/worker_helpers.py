def idle(id):
    state = worker_state(id)
    return state != None and state['jobs'] == 0
