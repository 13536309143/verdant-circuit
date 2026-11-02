#!/usr/bin/env python3
"""Regenerate bilingual runtime text and Markdown manuals from authored C# content."""
from pathlib import Path
import re,json,hashlib
root=Path(__file__).resolve().parents[1];assets=root/'Assets/VerdantCircuit'
version=re.search(r'Version="([^"]+)"',(assets/'Scripts/VerdantContent.cs').read_text())[1]
quoted=r'"(?:[^"\\]|\\.)*"'
strings=assets/'Localization/strings.json';data=json.loads(strings.read_text());entries={e['key']:e for e in data['entries']}
def register(en,zh):
    key='t_'+hashlib.sha1(zh.encode()).hexdigest()[:16];entries[key]=dict(key=key,source=zh,zh=zh,en=en)
for path in (assets/'Scripts/Automation').glob('*.cs'):
    for en,zh in re.findall(r'Tr\(('+quoted+r')\s*,\s*('+quoted+r')\)',path.read_text()):register(json.loads(en),json.loads(zh))
# Island definitions and contract names/descriptions are authored as bilingual pairs.
archipelago=(assets/'Scripts/Automation/ArchipelagoData.cs').read_text()
for m in re.finditer(r'new IslandDefinition\(('+quoted+r'),('+quoted+r'),('+quoted+r'),',archipelago):
    _,en,zh=map(json.loads,m.groups());register(en,zh)
for m in re.finditer(r'new FleetContract\(('+quoted+r'),('+quoted+r'),('+quoted+r'),('+quoted+r'),',archipelago):
    en,zh,lesson_en,lesson_zh=map(json.loads,m.groups());register(en,zh);register(lesson_en,lesson_zh)
manual=(assets/'Scripts/Automation/ClassicManual.cs').read_text()
chapters=[]
for m in re.finditer(r'new ManualChapter\(\s*('+quoted+r')\s*,\s*('+quoted+r')\s*,\s*('+quoted+r')\s*,\s*('+quoted+r')\s*,\s*('+quoted+r')(?:\s*,\s*('+quoted+r'))?\s*\)',manual):
    id,en_title,zh_title,en,zh,code=[json.loads(v) if v else '' for v in m.groups()];register(en_title,zh_title);register(en,zh);chapters.append(dict(id=id,en_title=en_title,zh_title=zh_title,en=en,zh=zh,code=code))
apis=[]
for m in re.finditer(r'new Api\(\s*('+quoted+r')\s*,\s*('+quoted+r')\s*,\s*('+quoted+r')\s*\)',manual):
    signature,en,zh=map(json.loads,m.groups());register(en,zh);apis.append(dict(signature=signature,en=en,zh=zh))
assert len(chapters)==19 and len(apis)>=30
progress=(assets/'Scripts/Automation/ClassicProgression.cs').read_text();tiers={k:json.loads('['+v+']') for k,v in re.findall(r'\{"(\w+)",new\[\]\{([^}]+)\}\}',progress)}
goals=[(id,item,int(amount)) for id,item,amount in re.findall(r'\{"(\w+)",new Stock\("(\w+)",(\d+)\)\}',progress)]
for locale,name in [('en','MANUAL.md'),('zh','MANUAL.zh-CN.md')]:
    lines=['# '+(f'Verdant Circuit {version} — Field Manual' if locale=='en' else f'禾序 {version} — 田野手册'),'\n[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)\n']
    for c in chapters:
        lines+=['## '+c[locale+'_title']+'\n',c[locale]+'\n']
        if c['id']=='progression':
            lines+=['| Research | Discoveries required for successive levels |','|---|---|']
            for tech,values in tiers.items():lines.append('| '+tech+' | '+' / '.join(f'{i+1}: {v or "base"}' for i,v in enumerate(values))+' |')
            lines+=['\n| First discovery | Cumulative production |','|---|---:|']
            for id,item,amount in goals:lines.append(f'| {id} | {item} {amount:,} |')
        if c['code']:lines+=['```python',c['code'],'```\n']
        if c['id']=='api':
            for a in apis:lines+=['### `'+a['signature']+'`\n',a[locale]+'\n']
    (root/'docs'/name).write_text('\n'.join(lines))
data['entries']=list(entries.values());strings.write_text(json.dumps(data,ensure_ascii=False,indent=2)+'\n')
print(f'{len(chapters)} bilingual chapters; {len(apis)} API groups; {len(entries)} localized entries authored.')
