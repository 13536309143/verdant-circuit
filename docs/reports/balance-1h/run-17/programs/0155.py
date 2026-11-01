a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
b=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
c=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)
print(a==b)
