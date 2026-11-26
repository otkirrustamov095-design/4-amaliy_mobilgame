# 4-amaliy topshiriq — "Mashina va dushman" o'yini

## Qanday o'ynaladi
- **Play** tugmasini bosing (sahna: `Assets/Scenes/SampleScene`).
- **W/S** yoki ↑/↓ — oldinga/orqaga yurish, **A/D** yoki ←/→ — burilish.
- Sariq tangalarni yig'ing (har biri +10 ball).
- Ko'k dushman mashinasidan qoching. Jon 0 bo'lsa o'yin tugaydi.
- **"REKLAMA KO'RISH"** tugmasi — 3 soniyalik "reklama"dan keyin +50 ball beriladi va jon to'ladi
  (o'yin tugagan bo'lsa, davom ettirish imkonini beradi).

## Topshiriq bandlari qayerda bajarilgan

| Band | Skript |
|---|---|
| Ball va rekord uchun o'zgaruvchilar | `PlayerData` klassi — `SaveSystem.cs` (`bestScore`, `totalCoins`, `gamesPlayed`) |
| JSON formatida saqlash | `SaveSystem.Save()` — `JsonUtility.ToJson` + `File.WriteAllText` |
| Qayta ishga tushirilganda yuklash | `GameManager.Start()` ichida `SaveSystem.Load()` |
| Dushman obyekti | `EnemyCar` (sahnadagi ko'k mashina) |
| 3 ta patrul nuqtasi | `PatrolPoints/Point1..3` (moviy doirachalar) |
| Nuqtalar orasida harakat | `EnemyCar.Patrol()` |
| Yaqinlashganda ta'qib | `EnemyCar.Chase()` — 15 m masofada |
| Juda yaqin kelganda hujum | `EnemyCar.Attack()` — 4 m masofada, har 1 soniyada −1 jon |
| Reklama tugmasi (UI) | `Canvas/AdButton` |
| Reklamadan keyin bonus | `RewardedAd.cs` → `GameManager.GiveAdReward()` |

Dushman rangi holatini ko'rsatadi: **ko'k** — patrul, **to'q sariq** — ta'qib, **qizil** — hujum.
Holat chap yuqori burchakda ham yozilib turadi.

## Ko'rsatish (himoya) uchun
1. O'yinni boshlang, bir nechta tanga yig'ing → **Console** oynasida `Saqlandi: ...` va JSON matni chiqadi.
2. Play ni to'xtatib, qayta boshlang → pastdagi **Rekord / Jami tanga** saqlanib qolganini ko'rsating
   (Console da `Yuklandi: ...`).
3. Dushman yoniga boring → ko'k → to'q sariq → qizil bo'lishini, jon kamayishini ko'rsating.
4. Reklama tugmasini bosing → 3 soniya kutish → +50 ball va jon to'ladi.

JSON fayl manzili (Windows):
`C:\Users\<foydalanuvchi>\AppData\LocalLow\DefaultCompany\4-chi topshiriq mobil game\player_data.json`
Rekordni nolga qaytarish uchun shu faylni o'chirib yuboring.

Dushman ko'rish/hujum radiuslari va patrul yo'li **Scene** oynasida (Gizmos) chizilib turadi.
