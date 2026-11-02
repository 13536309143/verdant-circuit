a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
b=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
c=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)
print(a==b)

simulation_ok = a==b and a!=c
print("simulation_ok", simulation_ok)
