# Id�beoszt�s kezel�

## El�k�sz�letek

A `config.json` f�jlban meg kell adni egy intervallumot, 
ami k�z�tt beoszthat egyszeri esem�nyeket a rendszer.

| Param�ter    | T�pus      | Tulajdons�gok    | Megjegyz�s              |
|--------------|------------|------------------|-------------------------|
| outDir       | string     | -                | Gener�lt beoszt�sok mapp�ja |
| threshold    | objektum   | minimum, maximum | Intervallum objektuma   |
| minimum      | eg�sz sz�m | -                | Intervallum kezdete     |
| maximum      | eg�sz sz�m | -                | Intervallum v�ge        |

A projekt `root` mapp�j�ban tal�lhat� egy `activities.csv` f�jl,
amiben teszt adatok vannak.
	
- Egy adott esem�nynek 6 param�tere van minden esetben.
Ezeknek a sorrendje:
  - egyedi azonos�t� (eg�sz sz�m),
  - d�tum (yyyy.mm.dd hh:mm),
  - n�v (sz�veg),
  - esem�ny hossza percben (eg�sz sz�m),
  - priorit�s (High, Medium, Low),
  - ism�tl�d�s (0, 1, 7, 365)
- Egyszeri esem�nyn�l tilos megadni a d�tumot, mert a 
  rendszer dinamikusan gener�lja le az oszt�lyhierarchi�t,
  ez�rt ha nem megfelel� a param�ter sz�m, akkor nem fog 
  lefutni (kezelve van, de nem fut tov�bb).
- Az ism�tl�d�s napokban �rtend�, teh�t ahol 0 van, 
  az egyszeri esem�nynek min�s�l.

## Amit tudni kell..

A rendszer ny�lv�ntart minden olyan napot amit gener�lt. 
Ez azt jelenti, hogy a fentebb eml�tett `outDir` mapp�ba t�rolja
el a napi beoszt�sokat, �gy minden alkalommal amikor fut a 
program, el�sz�r beolvassa a m�r elk�sz�tett beoszt�sokat �s 
azok alapj�n k�sz�ti el az �jabbakat.

Fel�l�r�sra van lehet�s�g, viszont az egyszeri alkalmakat amiket
beosztott aznapra, m�r nem tudja �jraosztani, vagyis fix esem�nyek.

## M�k�d�se

Sz�ks�ge van egy f�jlra, amiben a beosztani k�v�nt esem�nyek
vannak `;` elv�lasztva! **FONTOS**

Ez a f�jl b�rhol lehet �s b�rmilyen kiterjeszt�se lehet, amennyiben
�tkonvert�lhat� egyszer� sz�vegg� �s `;`-al van tagolva.

### Beoszt�s

Legfontosabb m�k�d�si elv a rendszerben, hogy a beosztani k�v�nt
egyszeri esem�nyeket priorit�s szerinti cs�kken� sorrendben osztja be.

Ez azt jelenti, hogy a legmagasabb priorit�ssal rendelkez� esem�nyt
fogja beosztani legel�sz�r, amennyiben annak id�tartama engedi. Ez
a megk�zel�t�s az�rt optim�lis, mert �gy a legfontosabb esem�nyek
lesznek legels�nek beosztva az adott id�ablak
maxim�lis kihaszn�lts�ga mellett.

### Sorrend

A program csak h�romf�le priorit�si szintet k�l�nb�ztet meg,
de van lehet�s�g ezek k�z�tt is sorrend kialak�t�s�ra.

Annyit kell csak tenni, hogy az beosztani k�v�nt esem�nyt el�bb
veszi fel, mint az azonos priorit�s�akat.

#### P�lda

##### Megk�l�nb�ztet� sorrend n�lk�l

| Id | D�tum | N�v | Id�tartam | Priorit�s | Ism�tl�d�s |
|----|-------|-----|-----------|-----------|------------|
| 1  |       | A1  | 90        | High      | 0          |
| 2  |       | A2  | 30        | High      | 0          |
| 3  |       | A3  | 60        | High      | 0          |

##### Megk�l�nb�ztet� sorrendben

| Id | D�tum | N�v | Id�tartam | Priorit�s | Ism�tl�d�s |
|----|-------|-----|-----------|-----------|------------|
| 1  |       | A1  | 90        | High      | 0          |
| 3  |       | A3  | 60        | High      | 0          |
| 2  |       | A2  | 30        | High      | 0          |

_Az azonos�t�kat csak a demonstr�ci� v�gett cser�lt�k fel._

Ut�bbi t�bl�zat szerint az `A3` n�vvel el�ttott esem�ny el�bb
fog beker�lni a beoszt�sba, mint az `A2` esem�ny (felt�ve, ha
el�g nagy az id�ablak, hogy bef�rjen).