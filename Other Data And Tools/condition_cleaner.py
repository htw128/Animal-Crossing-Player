import json

with open('/Users/oliver/Documents/Work/UnityProjects/Animal Crossing Player/conditions.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

out = []
for item in data:
    zh = next(l for l in item['languages'] if l["lang_iso"] == 'zh_cmn')
    out.append({
        'code': item['code'],
        'day': item['day'],
        'night': item['night'],
        'icon': item['icon'],
        'day_text': zh['day_text'],
        'night_text': zh['night_text']
    })

    with open('conditions_zh_cmn.json', 'w', encoding='utf-8') as f:
        json.dump(out, f, ensure_ascii=False, indent=4)