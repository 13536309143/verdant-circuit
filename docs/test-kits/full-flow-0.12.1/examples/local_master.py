# Use a test farm with the first three islands discovered and four free fleet slots.
# This is an autonomous three-island production example, not the 18-order solver.
if fleet_start():
    while fleet_research("slots"):
        pass
    fleet_research("engineer")
    jobs = [("irrigator",1,"local_water"),("grower",0,"local_grain"),("engineer",2,"local_forge"),("hauler",0,"local_cargo")]
    for role,island,program in jobs:
        unit = fleet_build(role,1,1,island)
        if unit > 0:
            fleet_run(unit,program)
        else:
            print("Need free slot, role research, parking space or credits:",role)
print("The master can finish; independent island programs keep working.")
