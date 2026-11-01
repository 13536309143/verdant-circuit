tiers={Unlocks.Expand:[Unlocks.Plant,Unlocks.Trees,Unlocks.Pumpkins,Unlocks.Cactus,Unlocks.Dinosaurs],Unlocks.Speed:[Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm],Unlocks.Grass:[Unlocks.Plant,Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Trees:[None,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm],Unlocks.Carrots:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation],Unlocks.Pumpkins:[None,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard],Unlocks.Sunflowers:[None,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains],Unlocks.Cactus:[None,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat],Unlocks.Mazes:[None,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat,Unlocks.Top_Hat],Unlocks.Dinosaurs:[None,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat,Unlocks.Top_Hat,Unlocks.Top_Hat],Unlocks.Watering:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Fertilizer:[None,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation],Unlocks.Polyculture:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Megafarm:[Unlocks.Dinosaurs,Unlocks.Dinosaurs,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains]}
dependencies={Unlocks.Plant:[],Unlocks.Expand:[Unlocks.Plant],Unlocks.Speed:[Unlocks.Expand,Unlocks.Trees],Unlocks.Grass:[Unlocks.Plant],Unlocks.Trees:[Unlocks.Plant],Unlocks.Carrots:[Unlocks.Expand],Unlocks.Watering:[Unlocks.Carrots],Unlocks.Pumpkins:[Unlocks.Carrots],Unlocks.Sunflowers:[Unlocks.Watering,Unlocks.Pumpkins],Unlocks.Fertilizer:[Unlocks.Pumpkins],Unlocks.Polyculture:[Unlocks.Trees],Unlocks.Cactus:[Unlocks.Pumpkins,Unlocks.Sunflowers],Unlocks.Mazes:[Unlocks.Fertilizer,Unlocks.Cactus],Unlocks.Dinosaurs:[Unlocks.Cactus,Unlocks.Mazes],Unlocks.Megafarm:[Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Simulation:[Unlocks.Dinosaurs],Unlocks.Leaderboard:[Unlocks.Simulation],Unlocks.Loops:[],Unlocks.Operators:[Unlocks.Loops],Unlocks.Variables:[Unlocks.Operators],Unlocks.Functions:[Unlocks.Variables],Unlocks.Lists:[Unlocks.Functions],Unlocks.Dicts:[Unlocks.Lists],Unlocks.Import:[Unlocks.Functions],Unlocks.Senses:[],Unlocks.Costs:[],Unlocks.Auto_Unlock:[],Unlocks.Timing:[],Unlocks.Debug:[],Unlocks.Hats:[],Unlocks.Debug_2:[Unlocks.Debug],Unlocks.Utilities:[Unlocks.Operators],Unlocks.The_Farmers_Remains:[Unlocks.Leaderboard],Unlocks.Top_Hat:[Unlocks.The_Farmers_Remains],}
goals={Unlocks.Trees:(Items.Hay,160),Unlocks.Carrots:(Items.Wood,600),Unlocks.Pumpkins:(Items.Carrot,1500),Unlocks.Sunflowers:(Items.Pumpkin,8000),Unlocks.Cactus:(Items.Power,6000),Unlocks.Mazes:(Items.Cactus,30000),Unlocks.Dinosaurs:(Items.Gold,30000),}
def buy(tech):
    if num_unlocked(tech)==0:
        for parent in dependencies[tech]:
            if num_unlocked(parent)==0:
                buy(parent)
        if tech in goals:
            item,target=goals[tech]
            missing=target-num_produced(item)
            if missing>0:
                ensure(item,num_items(item)+missing)
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
                        if get_water()<0.3 and num_items(Items.Water)>0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
                    infected=min(get_world_size()*get_world_size(),int((target-num_items(Items.Weird_Substance)-1)/per_plant)+1)
                    for row in range(get_world_size()):
                        for col in range(get_world_size()):
                            if infected>0:
                                while num_items(Items.Fertilizer)==0:
                                    pass
                                use_item(Items.Fertilizer)
                                infected-=1
                            move(East)
                        move(North)
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
                        if get_water()<0.3 and num_items(Items.Water)>0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    harvest()
    elif item==Items.Power:
        if num_unlocked(Unlocks.Sunflowers)==0:
            buy(Unlocks.Sunflowers)
        amount=(int((target-num_items(item))/(n*n*2**(num_unlocked(Unlocks.Sunflowers)-1)))+3)*n*n
        fund(Entities.Sunflower,amount)
        clear()
        while num_items(Items.Power)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type()!=Grounds.Soil:
                        till()
                    if get_entity_type()==None:
                        plant(Entities.Sunflower)
                    move(East)
                move(North)
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    while not can_harvest():
                        pass
                    move(East)
                move(North)
            for petals in range(15,6,-1):
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_entity_type()==Entities.Sunflower and measure()==petals:
                            harvest()
                        move(East)
                    move(North)
    elif item==Items.Cactus:
        if num_unlocked(Unlocks.Cactus)==0:
            buy(Unlocks.Cactus)
        amount=(int((target-num_items(item))/(n*n*min(n*n,16)*2**(num_unlocked(Unlocks.Cactus)-1)))+3)*n*n
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
        scale=2**(num_unlocked(Unlocks.Mazes)-1)
        count=int((target-num_items(Items.Gold)-1)/(n*n*scale))+1
        ensure(Items.Weird_Substance,count*n*scale)
        while num_items(Items.Gold)<target:
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

def upgrade():
    for i in range(8):
        for tech in [Unlocks.Expand,Unlocks.Grass,Unlocks.Speed,Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Fertilizer,Unlocks.Watering,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs]:
            level=num_unlocked(tech)
            if (level>0 or tech==Unlocks.Grass or tech==Unlocks.Speed) and level<len(tiers[tech]):
                parent=tiers[tech][level]
                if parent==None or num_unlocked(parent)>0:
                    buy(tech)

buy(Unlocks.Plant)
buy(Unlocks.Expand)
upgrade()
buy(Unlocks.Trees)
upgrade()
buy(Unlocks.Carrots)
upgrade()
buy(Unlocks.Watering)
buy(Unlocks.Pumpkins)
upgrade()
buy(Unlocks.Fertilizer)
buy(Unlocks.Sunflowers)
upgrade()
buy(Unlocks.Cactus)
upgrade()
buy(Unlocks.Mazes)
upgrade()
buy(Unlocks.Dinosaurs)
buy(Unlocks.Simulation)
buy(Unlocks.Leaderboard)
