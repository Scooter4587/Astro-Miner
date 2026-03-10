# AstroMiner Roadmap — 0.0.11 → 0.1.0

## Základný prístup

Tento roadmap slúži ako prvý stabilný plán pre nový štart projektu **AstroMiner**.  
Cieľ je ísť **krok za krokom**, dokončiť vždy jeden malý milestone, otestovať ho a až potom pokračovať ďalej.

### Princípy
- jeden nový väčší risk naraz
- každý krok musí mať jasný **done stav**
- najprv placeholdery a funkčnosť, až potom krajší obsah
- vždy testovať pred ďalším krokom
- používať iba moderný workflow a neísť cez deprecated riešenia

### Technické guardrails
- používať moderný `TileMapLayer` workflow
- nepoužívať deprecated tilemap prístup
- drill brať ako samostatný mode/state, nie len „delete tile“
- mining držať **cell-based**, nie pixel-based
- striktne odlišovať:
  - global/world pozíciu
  - local pozíciu
  - map cell koordináty
- po každom kroku: test, commit, changelog

---

# Roadmap

## 0.0.11 — project foundation

**Cieľ:**  
Pripraviť čistý základ projektu.

**Obsah:**
- priečinky projektu
- hlavná scéna
- základná node štruktúra
- input map
- placeholder assets
- changelog baseline

**Done stav:**
- projekt sa otvorí bez chaosu
- všetko má svoje miesto
- scéna je pripravená na ďalší krok

---

## 0.0.12 — visual movement testbed

**Cieľ:**  
Aby bolo vôbec vidno pohyb v priestore.

**Obsah:**
- space background
- hviezdy
- jednoduchý parallax
- prípadne debug referenčný bod alebo jemný grid

**Done stav:**
- keď sa loď alebo kamera pohne, pohyb je okamžite čitateľný

---

## 0.0.13 — ship scene placeholder

**Cieľ:**  
Pripraviť loď ako samostatný objekt.

**Obsah:**
- `ship.tscn`
- placeholder sprite
- collision shape
- základný script skeleton
- export premenné pre tuning

**Done stav:**
- loď je vložená do scény a pripravená na movement

---

## 0.0.14 — core movement v0

**Cieľ:**  
Spraviť prvý funkčný let.

**Obsah:**
- rotácia
- thrust dopredu
- inertia
- brzdenie
- max speed ako premenná

**Done stav:**
- loď sa dá ovládať
- pohyb je stabilný
- nič sa nespráva divoko

---

## 0.0.15 — camera + debug HUD

**Cieľ:**  
Spraviť movement čitateľný a ľahko testovateľný.

**Obsah:**
- follow camera
- smoothing
- debug text pre speed
- prípadne movement mode a velocity readout

**Done stav:**
- vieme presne testovať feeling pohybu
- hneď vidíme, čo loď robí

---

## 0.0.16 — asteroid test slice

**Cieľ:**  
Prvý kontakt so svetom.

**Obsah:**
- test asteroid scéna
- `TileSet`
- `TileMapLayer` pre asteroid
- collision tiles
- prvé umiestnenie testovacieho telesa

**Done stav:**
- asteroid je v scéne
- asteroid má kolíziu
- loď doň vie naraziť

---

## 0.0.17 — collision tuning

**Cieľ:**  
Aby kontakt ship vs asteroid nebol pinball.

**Obsah:**
- ladenie collision shape lode
- slide / scrape pocit
- tlmenie nárazu
- základ crash behavior bez chaosu

**Done stav:**
- náraz pôsobí kontrolovane
- loď sa neodráža nezmyselne

---

## 0.0.18 — drill detection v0

**Cieľ:**  
Vedieť presne, čo loď pred sebou vŕta.

**Obsah:**
- drill point / forward detection
- iba vpredu pred loďou
- safety speed limit
- prevod world pozície na lokálnu map cell

**Pravidlo:**  
`global hit -> to_local() -> local_to_map()`

**Done stav:**
- vieme spoľahlivo určiť, ktorú bunku sa snažíme vŕtať

---

## 0.0.19 — drill action placeholder

**Cieľ:**  
Prvý reálny mining pocit.

**Obsah:**
- stlačíš drill
- trafená bunka sa odstráni alebo prepne do mined stavu
- bez resource economy, len funkčný proof of concept

**Done stav:**
- vieš prísť k asteroidu a vyvŕtať prvú bunku

---

## 0.0.20 — integration pass / cleanup

**Cieľ:**  
Spojiť všetko do stabilného prototype flow.

**Obsah:**
- bugfixy
- cleanup skriptov
- kontrola inputov
- kontrola naming conventions
- malý refactor len ak bude nutný

**Done stav:**
- prototype je stabilný
- po opätovnom spustení sa správa predvídateľne

---

## 0.1.0 — first playable prototype

### Definícia verzie 0.1.0
- viditeľný priestor
- placeholder ship
- funkčný let
- kamera
- asteroid cez `TileMapLayer`
- kolízia
- jednoduchý drill
- prvý vyvŕtaný tile

**Význam verzie 0.1.0:**  
Toto bude prvý build, pri ktorom sa dá povedať:  
**„hra už existuje v prototype forme“**

---

# Po 0.1.0

Od tejto chvíle sa môžeme presunúť z prototype slice na samostatné systémy.

## 0.1.1 — ship system
- flight states
- drill mode vs flight mode
- jemnejšie ovládanie
- lepšie tunable variables
- lepšia crash logika

## 0.1.2 — drill system
- samostatný drill controller
- cooldown / power / range
- upgrades
- bezpečná mining speed logika

## 0.1.3 — resource collection system
- resource definitions
- tile metadata pre resource typy
- pickup / add to inventory
- základný UI counter

## 0.1.4 — cargo + resource UI
- cargo capacity
- weight / limits
- HUD

## 0.1.5 — energy pass
- napojenie ship + drill + systems

---

# Pracovná metodika pre každý milestone

Pri každom ďalšom kroku budeme držať rovnaký formát:

1. cieľ kroku
2. čo spraviť v Godote
3. čo spraviť vo VS Code
4. čo presne otestovať
5. čo je success / fail
6. commit + changelog

---

# Zhrnutie

Najdôležitejšie je držať poradie:

**foundation -> visible motion -> ship -> movement -> camera/debug -> asteroid -> drill**

Až po stabilnom prototype 0.1.0 pôjdeme viac do systémov:

**ship system -> drill system -> resource collection -> cargo -> energy**

Tento roadmap je zámerne praktický, jednoduchý a vhodný na iteratívny vývoj bez chaosu.