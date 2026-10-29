"""0.10 扩展规则原型；独立模拟器，尚未接入游戏内农场语言。python3 本文件"""
from dataclasses import dataclass
from collections import deque
from pathlib import Path
import json, random

@dataclass(frozen=True)
class Body:
    x:int; y:int; w:int=1; h:int=1
    def cells(self):return {(x,y) for x in range(self.x,self.x+self.w) for y in range(self.y,self.y+self.h)}

class Island:
    def __init__(self,w,h,holes=()):self.w=w;self.h=h;self.holes=set(holes)
    def valid(self,body):return all(0<=x<self.w and 0<=y<self.h and (x,y) not in self.holes for x,y in body.cells())
    def path(self,start,goal,occupied=()):
        occupied=set(occupied)
        if not self.valid(start) or start.cells()&occupied:return None
        queue=deque([start]);previous={start:None}
        while queue:
            node=queue.popleft()
            if (node.x,node.y)==goal:
                result=[]
                while previous[node] is not None:result.append(node);node=previous[node]
                return result[::-1]
            for dx,dy in [(1,0),(-1,0),(0,1),(0,-1)]:
                candidate=Body(node.x+dx,node.y+dy,node.w,node.h)
                if candidate not in previous and self.valid(candidate) and not candidate.cells()&occupied:
                    previous[candidate]=node;queue.append(candidate)
        return None

class Fleet:
    def __init__(self,island):self.island=island;self.units={};self.money=1000;self.penalized=set()
    def add(self,name,body):
        if not self.island.valid(body) or any(body.cells()&other.cells() for other in self.units.values()):return False
        self.units[name]=body;return True
    def step(self,requests,unsafe=False):
        # One scheduler tick uses a snapshot. Occupied origins remain reserved:
        # swaps, following through a vacated cell, and large-body overlaps are blocked.
        snapshot=dict(self.units);reserved=set().union(*(b.cells() for b in snapshot.values())) if snapshot else set();approved={};results={}
        for ticket,name,dx,dy in requests:
            origin=snapshot[name];candidate=Body(origin.x+dx,origin.y+dy,origin.w,origin.h)
            if abs(dx)+abs(dy)!=1:results[ticket]='invalid_step';continue
            others=reserved-origin.cells()
            if not self.island.valid(candidate):results[ticket]='invalid_terrain'
            elif candidate.cells()&others:
                results[ticket]='collision' if unsafe else 'waiting'
                if unsafe and ticket not in self.penalized:
                    self.money=max(0,self.money-20);self.penalized.add(ticket)
            elif name in approved:results[ticket]='duplicate_unit_command'
            else:
                approved[name]=candidate;reserved|=candidate.cells();results[ticket]='moved'
        self.units.update(approved);return results

class DockQueue:
    def __init__(self):self.queue=deque();self.sequence=0
    def submit(self,drone):
        self.sequence+=1;self.queue.append((self.sequence,drone));return self.sequence
    def tick(self,landing_free):return self.queue.popleft() if landing_free and self.queue else None

class QuantityTask:
    def __init__(self,target,existing=0):self.target=target;self.baseline=existing
    def ready(self,total):return total-self.baseline>=self.target

def verify():
    checks=[]
    def check(condition,name):
        if not condition:raise AssertionError(name)
        checks.append(name)
    island=Island(12,12,{(6,y) for y in range(9)})
    for w,h in [(1,1),(2,2),(3,2),(3,3)]:
        start=Body(0,0,w,h);route=island.path(start,(8,9))
        check(route is not None and all(island.valid(b) for b in route),f'{w}x{h} footprint route avoids holes')
    corridor=Island(7,5,{(x,y) for x in range(7) for y in range(5) if y!=2})
    check(corridor.path(Body(0,2),(6,2)) is not None,'1x1 traverses narrow corridor')
    check(corridor.path(Body(0,1,2,2),(5,1)) is None,'2x2 rejects narrow corridor')
    f=Fleet(Island(8,8));check(f.add('A',Body(0,0)) and f.add('B',Body(1,0)),'distinct initial occupancy')
    result=f.step([(1,'A',1,0),(2,'B',-1,0)])
    check(list(result.values())==['waiting','waiting'] and f.money==1000,'safe edge swap waits without penalty')
    check(not f.add('wide',Body(0,0,3,2)),'spawn rejects full footprint overlap')
    result=f.step([(10,'A',1,0)],unsafe=True);f.step([(10,'A',1,0)],unsafe=True)
    check(result[10]=='collision' and f.money==980,'unsafe command charged once per ticket')
    check(f.step([(11,'A',-1,0)],unsafe=True)[11]=='invalid_terrain' and f.money==980,'off-island movement is rejected, no wrap or fee')
    g=Fleet(Island(6,6));g.add('A',Body(0,1));g.add('B',Body(2,1))
    result=g.step([(1,'A',1,0),(2,'B',-1,0)])
    check(result[1]=='moved' and result[2]=='waiting','FIFO same-target reservation has single winner')
    q=DockQueue();tickets=[q.submit(n) for n in ['A','B','C']]
    check(q.tick(False) is None,'blocked landing consumes no ticket')
    check([q.tick(True)[0] for _ in range(3)]==tickets,'portal FIFO arrival order')
    rng=random.Random(1029);g=Fleet(Island(12,12))
    for i,body in enumerate([Body(0,0,2,2),Body(4,0,3,2),Body(8,8,3,3),Body(0,8)]):g.add(str(i),body)
    for tick in range(2000):
        commands=[]
        for n in g.units:
            dx,dy=rng.choice([(1,0),(-1,0),(0,1),(0,-1)]);commands.append((tick*10+int(n),n,dx,dy))
        g.step(commands)
        bodies=list(g.units.values())
        assert all(g.island.valid(b) for b in bodies)
        assert all(not a.cells()&b.cells() for i,a in enumerate(bodies) for b in bodies[i+1:])
    check(g.money==1000,'2000 seeded multi-size ticks preserve occupancy and safe-mode money')
    goals={'grain':2000,'radish':1000,'flax':1400,'berry':2400,'sunflower':1300,'pumpkin':1200,'tea':1250,'starbloom':500}
    for crop,target in goals.items():
        task=QuantityTask(target,50000)
        check(not task.ready(50000+target-1) and task.ready(50000+target),crop+' quantity boundary without timer')
    check(len(set(goals.values()))==8,'all crop quantity goals differ')
    # This checks safety and finite reachability, not completeness of general multi-agent planning.
    report={'checksPassed':len(checks),'checks':checks,'limits':['not integrated into Unity VM or rendering','general multi-agent deadlock freedom not proven','turning swept footprint, energy and weather transport remain design items','novelty is a design assessment, not an originality guarantee']}
    return report

if __name__=='__main__':
    result=verify();p=Path(__file__).with_name('机群与浮空岛-规则验证.json');p.write_text(json.dumps(result,ensure_ascii=False,indent=2));print(json.dumps(result,ensure_ascii=False,indent=2))
