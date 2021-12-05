# React és ASP.NET Core alapú webalkalmazás
BME AUT Témalaboratórium - 2021/22/1  
Szőke-Milinte Botond - JQ162H   
Konzulens: Dudás Ákos

## Az alkalmazásról
Az alkalmazás alapvetően egy letisztult, könnyen használható, teendőkezelő weboldal. Egy oszlop felel meg egy teendőcsoportnak, benne az ehhez az állapothoz tartozó teendők találhatók. A teendő rendelkezik címmel, leírással és határidővel. Mivel **tetszőleges számú oszlop felvehető** tetszőleges névvel, ezért lényegében bármilyen adat tárolható, azok csoportokba rendezhetőek.


## Frontend
A frontend egy React technológiára épülő, JavaScript nyelven íródott single-page webalkalmazás. Az esztétikus megjelenéshez **React Bootstrap-et használ**. A backendtől az adatokat egy REST API-n keresztül kérdezi le, azokat oszlopokba rendezi, módosítás esetén szintén ezen az API-n keresztül közli a backenddel a szükséges adatokat. A kódellenőrzésért az ESLint felel.

Új oszlop, illetve teendő felvételére a fejlécen levő `Add new task` és `Add new column` gombokkal, vagy az oszlophoz tartozó `New task` gombbal van lehetőség. Egy oszlop, illetve teendő adatai a hozzájuk tartozó `Edit` gombra kattintva változtathatóak, továbbá a teendő állapotának / oszlopának változtatása is itt lehetséges. A törlésre szintén itt van lehetőség, fontos azonban, hogy egy oszlop törlése az összes benne található teendő törlésével is együtt jár. A teendők egymáshoz képesti sorrendje (prioritása) **drag and drop** módon változtatható.

![A webalkalmazás frontendje] (frontend.jpg)

## Backend
A backend ASP.NET Core-on alapul, melyben az adatréteg pedig Entity Framework-re épül. A kódellenőrzésért a beépített NetAnalyzer felel. Az adatrétegben *(DAL projekt)* találhatóak a `Todo` és `Column` osztályok, melyek az adatok leképzéséért felelősek. Itt találhatók továbbá a Models mappában az üzleti logika számára használható úgynevezett model osztályok, melyek könnyű használhatóságát a repository tervezési minta biztosítja. A tényleges adattárolás egy Code First technológiával létrehozott, lokális SQL adatbázisban történik.

Az adatelérési rétegre épül az üzleti logikai réteg *(BL projekt)*, melynek fő feladata, hogy a teendőkhez (és oszlopokhoz) tartozó prioritások konzisztensek maradjanak, az adatbázisban ne állhasson elő az alkalmazás működése szempontjából inkonzisztens állapot. Az itt található `TodoManager` és `ColumnManager` osztályokat használja fel a WebApi a működéséhez.

## Test

Az üzleti logika teszteléséhez a *Test projektben* található egy teszt osztály, mely unit tesztek végrehajtására képes. Egyelőre csupán egy egyszerű, az új oszlop felvételét tesztelő metódus kapott itt helyet, ez a későbbiekben bővíthető. A metódus a Moq NuGet csomagot használja, így az adatbázis viselkedését csak mockolja, ahhoz valójában nem kapcsolódik.

## API

A frontend és backend közötti kommunikáció egy REST API-n keresztül zajlik *(Web projekt)*, az adatok a http kérések belsejében vannak. Az API felelős azért, hogy tudassa a klienssel az egyes kérések végrehajtása során esetlegesen fellépő hibákat is. A `http://localhost:5000/` címen érhető el, a megfelelő végpontokon keresztül.

Az egyes végpontok a következők:
- Delete
  - /api/column/{columnID}/ - adott ID-hoz tartozó oszlop törlése
  - /api/todo/{todoID}/ - adott ID-hoz tartozó teendő törlése 

- Get
  - /api/column/ - az összes oszlop adatainak lekérdezése
  - /api/column/{columnID}/ - adott ID-hoz tartozó oszlop adatainak lekérdezése
  - /api/todo/ - az összes teendő adatainak lekérdezése
  - /api/todo/{todoId}/ - adott ID-hoz tartozó teendő adatainak lekérdezése

- Post
  - /api/column/ - új oszlop létrehozása
  - /api/todo/ - új teendő létrehozása

- Put
  - /api/column/{columnId}/ - adott ID-hoz tartozó oszlop módosítása, a priority attribútum kivételével
  - /api/column/{columnId}/priority – adott ID-hoz tartozó oszlop priority attribútumának módosítása
  - /api/todo/{todoId}/ - az adott ID-hoz tartozó teendő módosítása, a priority attribútum kivételével
  - /api/todo/{todoId}/priority/ - az adott ID-hoz tartozó teendő priority attribútumának módosítása

## Telepítés
A telepítéshez először le kell klónozni a repositoryt. Ezt követően a frontend első futtatásához a következő előzetes műveleteket kell elvégezni:
- Amennyiben a számítógépen még nincs telepítve a Node.js, akkor azt telepíteni kell a nodejs.org weboldalról
- Amennyiben a számítógépen még nincs telepítve a 'react-scripts', akkor azt telepíteni kell a `npm install react-scripts` paranccsal

A Visual Studio solution indításához nem szükséges további csomagok telepítése (tesztelve VS2022 alatt).

Az alkalmazás indításához a backend/Backend/TodoManagerApp.sln-t kell először elindítani, majd futtatni. A sikeres futtatás eredményeként egy console ablak jelenik meg néhány info üzenettel, illetve a korábban említett `http://localhost:5000` címen már tudunk kommunikálni az API-val.
Ezt követően a frontend/Frontend mappában meg kell nyitni egy parancssort, melyben az `npm start` parancsot kiadva megnyílik a webalkalmazás a `https://localhost:3000` címen.

## Források
- A BMEVIAUAC01 (Adatvezérelt rendszerek) tárgyhoz kapcsolódó tanayagok, példaalkalmazások
- A feladatkiírásban megjelölt vonatkozó források
