# Idõbeosztás kezelõ

## Elõkészületek

A `config.json` fájlban meg kell adni egy intervallumot, 
ami között beoszthat egyszeri eseményeket a rendszer.

| Paraméter    | Típus      | Tulajdonságok    | Megjegyzés              |
|--------------|------------|------------------|-------------------------|
| outDir       | string     | -                | Generált beosztások mappája |
| threshold    | objektum   | minimum, maximum | Intervallum objektuma   |
| minimum      | egész szám | -                | Intervallum kezdete     |
| maximum      | egész szám | -                | Intervallum vége        |

A projekt `root` mappájában található egy `activities.csv` fájl,
amiben teszt adatok vannak.
	
- Egy adott eseménynek 6 paramétere van minden esetben.
Ezeknek a sorrendje:
  - egyedi azonosító (egész szám),
  - dátum (yyyy.mm.dd hh:mm),
  - név (szöveg),
  - esemény hossza percben (egész szám),
  - prioritás (High, Medium, Low),
  - ismétlõdés (0, 1, 7, 365)
- Egyszeri eseménynél tilos megadni a dátumot, mert a 
  rendszer dinamikusan generálja le az osztályhierarchiát,
  ezért ha nem megfelelõ a paraméter szám, akkor nem fog 
  lefutni (kezelve van, de nem fut tovább).
- Az ismétlõdés napokban értendõ, tehát ahol 0 van, 
  az egyszeri eseménynek minõsül.

## Amit tudni kell..

A rendszer nyílvántart minden olyan napot amit generált. 
Ez azt jelenti, hogy a fentebb említett `outDir` mappába tárolja
el a napi beosztásokat, így minden alkalommal amikor fut a 
program, elõször beolvassa a már elkészített beosztásokat és 
azok alapján készíti el az újabbakat.

Felülírásra van lehetõség, viszont az egyszeri alkalmakat amiket
beosztott aznapra, már nem tudja újraosztani, vagyis fix események.

## Mûködése

Szüksége van egy fájlra, amiben a beosztani kívánt események
vannak `;` elválasztva! **FONTOS**

Ez a fájl bárhol lehet és bármilyen kiterjesztése lehet, amennyiben
átkonvertálható egyszerû szöveggé és `;`-al van tagolva.

### Beosztás

Legfontosabb mûködési elv a rendszerben, hogy a beosztani kívánt
egyszeri eseményeket prioritás szerinti csökkenõ sorrendben osztja be.

Ez azt jelenti, hogy a legmagasabb prioritással rendelkezõ eseményt
fogja beosztani legelõször, amennyiben annak idõtartama engedi. Ez
a megközelítés azért optimális, mert így a legfontosabb események
lesznek legelsõnek beosztva az adott idõablak
maximális kihasználtsága mellett.

### Sorrend

A program csak háromféle prioritási szintet különböztet meg,
de van lehetõség ezek között is sorrend kialakítására.

Annyit kell csak tenni, hogy az beosztani kívánt eseményt elõbb
veszi fel, mint az azonos prioritásúakat.

#### Példa

##### Megkülönböztetõ sorrend nélkül

| Id | Dátum | Név | Idõtartam | Prioritás | Ismétlõdés |
|----|-------|-----|-----------|-----------|------------|
| 1  |       | A1  | 90        | High      | 0          |
| 2  |       | A2  | 30        | High      | 0          |
| 3  |       | A3  | 60        | High      | 0          |

##### Megkülönböztetõ sorrendben

| Id | Dátum | Név | Idõtartam | Prioritás | Ismétlõdés |
|----|-------|-----|-----------|-----------|------------|
| 1  |       | A1  | 90        | High      | 0          |
| 3  |       | A3  | 60        | High      | 0          |
| 2  |       | A2  | 30        | High      | 0          |

_Az azonosítókat csak a demonstráció végett cseréltük fel._

Utóbbi táblázat szerint az `A3` névvel eláttott esemény elõbb
fog bekerülni a beosztásba, mint az `A2` esemény (feltéve, ha
elég nagy az idõablak, hogy beférjen).