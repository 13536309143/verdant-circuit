import farm_lib
amount = get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
farm_lib.ensure(Items.Weird_Substance, amount*2)
before = num_items(Items.Gold)
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))

use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()

maze_reuse_ok = num_items(Items.Gold)-before == get_world_size()*get_world_size()*2**num_unlocked(Unlocks.Mazes)
print("maze_reuse_ok", maze_reuse_ok)

