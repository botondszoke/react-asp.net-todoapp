# React and ASP.NET Core Web Application / React és ASP.NET Core alapú webalkalmazás
BME AUT Topic Laboratory - 2021/22/1 / BME AUT Témalaboratórium - 2021/22/1  
Szőke-Milinte Botond - JQ162H   
Advisor: Dudás Ákos / Konzulens: Dudás Ákos

## About the Application / Az alkalmazásról
**EN**: The application is essentially a clean, easy-to-use todo web app. Each column represents a todo group, containing todos for that specific state. Todos have a title, description, and due date. Since **any number of columns can be created** with custom names, virtually any data can be stored and organized into groups.

**HU**: Az alkalmazás alapvetően egy letisztult, könnyen használható, teendőkezelő weboldal. Egy oszlop felel meg egy teendőcsoportnak, benne az ehhez az állapothoz tartozó teendők találhatók. A teendő rendelkezik címmel, leírással és határidővel. Mivel **tetszőleges számú oszlop felvehető** tetszőleges névvel, ezért lényegében bármilyen adat tárolható, azok csoportokba rendezhetőek.

## Frontend
**EN**: The frontend is a React-based, JavaScript single-page web application. It uses **React Bootstrap** for aesthetic design. Data is fetched from the backend via REST API, organized into columns, and changes are sent back through the same API. ESLint handles code linting. New columns or todos can be added via header buttons (`Add new task`, `Add new column`) or column-specific `New task`. Edit/delete via `Edit` buttons; todo priority via **drag and drop**.

**HU**: A frontend egy React technológiára épülő, JavaScript nyelven íródott single-page webalkalmazás. Az esztétikus megjelenéshez **React Bootstrap-et használ**. A backendtől az adatokat egy REST API-n keresztül kérdezi le, azokat oszlopokba rendezi, módosítás esetén szintén ezen az API-n keresztül közli a backenddel a szükséges adatokat. A kódellenőrzésért az ESLint felel. Új oszlop, teendő felvételére a fejlécen levő `Add new task` és `Add new column` gombokkal, vagy az oszlophoz tartozó `New task` gombbal. Szerkesztés/törlés az `Edit` gombbal, sorrend **drag and drop**-pal.

## Backend
**EN**: The backend is built on ASP.NET Core with Entity Framework for the data layer. Built-in NetAnalyzer handles code analysis. The data access layer *(DAL project)* contains `Todo` and `Column` classes. Business logic models use repository pattern. Data stored in Code First local SQL database. Business logic layer *(BL project)* ensures priority consistency.

**HU**: A backend ASP.NET Core-on alapul, melyben az adatréteg Entity Framework-re épül. A kódellenőrzésért a beépített NetAnalyzer felel. Az adatrétegben *(DAL projekt)* találhatóak a `Todo` és `Column` osztályok. A Models mappában az üzleti logika model osztályai repository mintával. Code First lokális SQL adatbázisban történő adattárolás. Az üzleti logikai réteg *(BL projekt)* a prioritások konzisztenciáját biztosítja.

## Test
**EN**: The *Test project* contains unit tests for business logic using Moq to mock database behavior without real connections. Currently tests new column creation (expandable).

**HU**: A *Test projektben* unit tesztek a Moq NuGet csomaggal, adatbázis mockolással. Jelenleg új oszlop létrehozását teszteli (bővíthető).

## API
**EN**: REST API at `http://localhost:5000/` handles frontend-backend communication and error reporting.

**HU**: REST API a `http://localhost:5000/` címen kezeli a frontend-backend kommunikációt és hibajelzéseket.

**Endpoints / Végpontok:**
- **Delete**: `/api/column/{columnID}/`, `/api/todo/{todoID}/`
- **Get**: `/api/column/`, `/api/column/{columnID}/`, `/api/todo/`, `/api/todo/{todoId}/`
- **Post**: `/api/column/`, `/api/todo/`
- **Put**: `/api/column/{columnId}/`, `/api/column/{columnId}/priority`, `/api/todo/{todoId}/`, `/api/todo/{todoId}/priority/`

## Installation / Telepítés
**EN**: Clone repo → Install Node.js + `npm install react-scripts` → Run `TodoManagerApp.sln` → `npm start` in frontend folder. App at `https://localhost:3000`.

**HU**: Repository klónozás → Node.js telepítés + `npm install react-scripts` → `TodoManagerApp.sln` indítás → frontend mappában `npm start`. Alkalmazás: `https://localhost:3000`.

*(Tested under VS2022 / VS2022 alatt tesztelve)*

## Sources / Források
**EN**: BMEVIAUAC01 course materials  
**HU**: BMEVIAUAC01 (Adatvezérelt rendszerek) tananyagok
