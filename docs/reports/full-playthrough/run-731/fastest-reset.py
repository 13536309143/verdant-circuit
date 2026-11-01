def buy(tech):
    cost=get_cost(tech)
    if cost==None:
        return
    for item in cost:
        ensure(item,cost[item])
    unlock(tech)

def fund(entity,amount):
    cost=get_cost(entity)
    for item in cost:
        ensure(item,cost[item]*amount)

def ensure(item,target):
    if num_items(item)>=target:
        return
    n=get_world_size()
    if item==Items.Hay:
        clear()
        while num_items(Items.Hay)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    move(East)
                move(North)
    elif item==Items.Wood:
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Wood)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type()==None:
                        if num_unlocked(Unlocks.Trees)>0 and (get_pos_x()+get_pos_y())%2==0:
                            plant(Entities.Tree)
                        else:
                            plant(Entities.Bush)
                    move(East)
                move(North)
    elif item==Items.Carrot:
        if num_unlocked(Unlocks.Carrots)==0:
            buy(Unlocks.Carrots)
        amount=(int((target-num_items(item))/(n*n*2**(num_unlocked(Unlocks.Carrots)-1)))+3)*n*n
        fund(Entities.Carrot,amount)
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Carrot)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type()==None:
                        plant(Entities.Carrot)
                    move(East)
                move(North)
    elif item==Items.Pumpkin or item==Items.Weird_Substance:
        if num_unlocked(Unlocks.Pumpkins)==0:
            buy(Unlocks.Pumpkins)
        infected=item==Items.Weird_Substance
        if infected and num_unlocked(Unlocks.Fertilizer)==0:
            buy(Unlocks.Fertilizer)
        yield_amount=n*n*min(n,6)*2**(num_unlocked(Unlocks.Pumpkins)-1)
        if infected:
            yield_amount=yield_amount/2
        amount=(int((target-num_items(item))/yield_amount)+3)*n*n*3
        fund(Entities.Pumpkin,amount)
        if infected:
            clear()
            while num_items(Items.Weird_Substance)<target:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type()!=Grounds.Soil:
                            till()
                        if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                            complete=False
                        move(East)
                    move(North)
                if complete and num_items(Items.Fertilizer)>0:
                    use_item(Items.Fertilizer)
                    harvest()
        else:
            clear()
            while num_items(Items.Pumpkin)<target:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type()!=Grounds.Soil:
                            till()
                        if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                            complete=False
                        move(East)
                    move(North)
                if complete:
                    harvest()
    elif item==Items.Cactus:
        if num_unlocked(Unlocks.Cactus)==0:
            buy(Unlocks.Cactus)
        amount=(int((target-num_items(item))/(n*n*n*n*2**(num_unlocked(Unlocks.Cactus)-1)))+3)*n*n
        fund(Entities.Cactus,amount)
        clear()
        while num_items(Items.Cactus)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type()!=Grounds.Soil:
                        till()
                    if get_entity_type()==None:
                        plant(Entities.Cactus)
                    move(East)
                move(North)
            complete=False
            while not complete:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if not can_harvest():
                            complete=False
                        move(East)
                    move(North)
            # Run on a field of mature cacti. Bubble each row, then each column.
            for axis in [East, North]:
                for sweep in range(get_world_size()):
                    for row in range(get_world_size()):
                        for col in range(get_world_size()-1):
                            if measure() > measure(axis):
                                swap(axis)
                            move(axis)
                        move(axis)
                        if axis == East:
                            move(North)
                        else:
                            move(East)
            harvest()
    elif item==Items.Bone:
        while num_items(Items.Bone)<target:
            ensure(Items.Cactus,n*n+1)
            clear()
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    till()
                    move(East)
                move(North)
            # Use an even-sized empty field and keep cactus in inventory.
            change_hat(Hats.Dinosaur_Hat)
            while True:
                direction=East
                if get_pos_x()==0:
                    if get_pos_y()>0:
                        direction=South
                elif get_pos_y()%2==0:
                    if get_pos_x()==get_world_size()-1:
                        direction=North
                else:
                    direction=West
                    if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
                        direction=North
                if not move(direction):
                    break
            change_hat(Hats.Straw_Hat)
    elif item==Items.Gold:
        while num_items(Items.Gold)<target:
            ensure(Items.Weird_Substance,n*2**(num_unlocked(Unlocks.Mazes)-1))
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
            if get_entity_type() == Entities.Treasure:
                harvest()

buy(Unlocks.Plant)
for i in range(3):
    buy(Unlocks.Expand)
for i in range(8):
    buy(Unlocks.Grass)
    buy(Unlocks.Speed)
for i in range(8):
    buy(Unlocks.Trees)
buy(Unlocks.Carrots)
buy(Unlocks.Watering)
buy(Unlocks.Pumpkins)
buy(Unlocks.Fertilizer)
buy(Unlocks.Cactus)
buy(Unlocks.Dinosaurs)
buy(Unlocks.Simulation)
buy(Unlocks.Mazes)
buy(Unlocks.Leaderboard)
