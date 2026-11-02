#!/usr/bin/env python3
"""Keep typed parameter/return documentation explicit for the 0.13.1 public API."""
from pathlib import Path
import json,re
root=Path(__file__).resolve().parents[1];p=root/'Assets/VerdantCircuit/Resources/UnifiedReference.json';data=json.loads(p.read_text())
params={
'planet':('integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked','整数：0地球、1花卉、2海洋、3矿物，须已解锁'),
'id':('stable native-worker ID; ufo_* uses its separate UFO ID space','稳定本地载具编号；ufo_*使用另一套独立UFO编号'),
'source':('unlocked source planet ID0–3','已开放的源星球编号0～3'),
'destination':('unlocked destination planet ID0–3','已开放的目的星球编号0～3'),
'file':('existing program filename without .py','现有程序名，不含.py'),
'item':('canonical item string or Items member; coins/gold_ore/gold are distinct','规范物品字符串或Items成员；coins、gold_ore、gold不可混用'),
'amount':('positive integer; UFO shipment1–1000000, refuel1–200','正整数：UFO货运1～1000000，充电1～200'),
'width':('researched footprint width1–3','已研究的机体宽度1～3'),
'height':('researched footprint height1–3','已研究的机体高度1～3'),
'kind':('slots or size for workers; slots or cargo for UFO research','本地研究为slots或size；UFO研究为slots或cargo'),
'direction':('North,East,South,West; boundaries and whole footprints apply','North、East、South、West，受边界与完整占地限制'),
'x':('local unlocked longitude column, zero-based','从0起的已开放局部经度列'),
'y':('local unlocked latitude row, zero-based','从0起的已开放局部纬度行'),
'ax':('start x column','起点横坐标'),'ay':('start y row','起点纵坐标'),'bx':('destination x column','终点横坐标'),'by':('destination y row','终点纵坐标'),
'name':('nonempty local swarm name, at most80 characters','非空本地集群名称，最多80字符'),
'ids':('unique existing worker IDs, all on one planet','同一星球上存在且不重复的载具编号列表'),
'payload':('deep-copied script value; inbox holds16 values','深拷贝的脚本值，收件箱最多16条'),
'seconds':('game seconds0–3600; yields the caller only','0～3600游戏秒，只让出调用者'),
'entity':('original Earth Entities member','地球原有Entities作物成员'),
'batches':('positive integer1–10000; default1','1～10000正整数，默认1')}
returns={
'universe_planets':('list[dict] with four stable IDs and unlocked flags','含四个稳定编号和开放状态的字典列表'),
'universe_state':('dict with clock,credits,contract,worker totals','含时间、资金、订单、载具总量的字典'),
'planet_self':('number: worker/manager planet, -1 in universe context','数值：载具或管理程序所属星球，宇宙总控为-1'),
'planet_inventory':('number: available unreserved stock','数值：未预留的可用库存'),
'planet_products':('list[str] of native recipe item IDs','本地配方产物标识字符串列表'),
'planet_surface':('dict: width,height,bounds,wrap,worker_limit,hull_tier,worker_type','字典：宽高、边界、环绕、载具上限、尺寸等级、类型'),
'planet_scan':('dict for an unlocked cell; None for locked/invalid coordinates','已开放格的字典；锁定或无效坐标为None'),
'worker_build':('numeric worker ID; -1 if blocked,full,unfunded or unavailable','数值载具编号；空间、数量、资金或科技不足为-1'),
'worker_starter':('existing/free native worker ID; -1 if planet locked','已有或免费本地载具编号；星球未开放为-1'),
'worker_units':('list[dict] of worker snapshots','载具快照字典列表'),
'worker_self':('numeric native-worker ID; -1 in managers','本地载具编号；管理程序为-1'),
'worker_path':('list[(x,y)]; empty for no route or already there','坐标元组列表；不可达或已到终点为空'),
'worker_state':('worker snapshot dict or None','载具快照字典或None'),
'worker_wait':('None; other programs and physical tasks continue','None；其他程序和实体任务继续'),
'worker_receive':('oldest copied message, or None if empty/manager','最早的消息副本，空箱或管理程序为None'),
'worker_result':('copied result/return value, or None','结果／返回值副本，或None'),
'swarm_members':('list[number], empty when the swarm is unknown','编号列表，未知集群为空'),
'ufo_build':('numeric UFO ID; -1 on insufficient slots,credits or Earth supplies','UFO数值编号；数量、资金或地球原料不足为-1'),
'ufo_fleet':('list[dict] containing cargo,ETA,battery,capacity,planet,status,busy','字典列表，含货物、ETA、电量、载荷、停靠星球和忙碌状态'),
'ufo_capacity':('number:24,72,144 or288 units per payload','数值：每舱24、72、144或288份'),
'ufo_wait':('None; this caller yields while flights advance','None；当前程序让出，航行继续'),
'get_world_width':('number: current surface columns1–360','数值：当前表层列数1～360'),
'get_world_height':('number: current surface rows1–128','数值：当前表层行数1～128'),
'get_longitude':('number: worker anchor longitude in degrees','数值：载具锚点经度，单位度'),
'get_latitude':('number: worker anchor latitude in degrees','数值：载具锚点纬度，单位度'),
'harvest_area':('number: successful harvest actions, merged regions settle once','数值：成功收获动作数，合并区域只结算一次'),
'plant_area':('number: successful plantings; partial success is possible','数值：种植成功格数，允许部分成功'),
'surface_distance':('number: grid distance with full-circle longitude shortcut','数值：完整经度环线下取最短路的网格距离'),
'market_prices':('dict[str,number] of item export prices','物品标识到出口单价的字典'),
'market_sell':('number: credits earned to cents; zero on failure','数值：两位小数的所得资金；失败为0'),
'contract_current':('contract dict; None after all eight contracts','订单字典，八单全部完成后为None'),
'control_state':('list[dict]: scope,program,running,error','字典列表：作用域、绑定文件、运行状态、错误'),
'product_info':('recipe dict, or None when no recipe exists','配方字典；无配方时为None'),
'program_state':('binding/running/error/result dict, or None','绑定、运行、错误、结果字典，或None'),
'stop_all':('True after stopping code and initiating safe physical cancellation','停止代码并发起安全实体取消后为True')}
booleans={'universe_run','planet_run','planet_stop','planet_expand','worker_move','worker_route','worker_work','worker_cancel','worker_remove','worker_research','worker_rotate','worker_send','swarm_create','swarm_run','swarm_stop','ufo_transfer','ufo_cancel','ufo_research','ufo_refuel','ufo_reposition','contract_submit','program_bind','program_run','program_stop'}
for e in data['entries']:
 n=e['name']
 if n=='worker_units':e['signature']='worker_units() / worker_units(planet)'
 if e['category'] in ('control','product'):
  signature=e['signature'].split('(',1)[1].split(')',1)[0];en=[];zh=[]
  for token in signature.split(','):
   key=token.strip().split('=')[0]
   if key in params:en.append(key+': '+params[key][0]);zh.append(key+'：'+params[key][1])
  e['parametersEn']='; '.join(en) if en else 'No arguments.';e['parameters']='；'.join(zh) if zh else '无参数。'
  if n in returns:e['returnsEn'],e['returns']=returns[n]
  elif n in booleans:e['returnsEn']='bool: True on acceptance/success; False when state or resources do not permit it.';e['returns']='bool：接受／成功为True，状态或资源不满足为False。'
 if n.startswith('ufo_'):e['requirementsEn']='Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.';e['requirements']='星际航行科技、已开放的航线端点和有效UFO编号；充电必须有当前停靠地库存。'
 if n.startswith('get_world_'):e['requirementsEn']='Bound native worker or planet manager. Universe context must use planet_surface(id).';e['requirements']='本地载具或星球管理上下文；宇宙总控需用planet_surface指定星球。'
p.write_text(json.dumps(data,ensure_ascii=False,indent=2)+'\n');print('Enriched',len(data['entries']),'reference entries')
