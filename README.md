# Uczelniana Wypożyczalnia Sprzętu

Aplikacja konsolowa w języku C# realizująca system zarządzania uczelnianą wypożyczalnią sprzętu. 

## Instrukcja uruchomienia
1. Sklonuj repozytorium na swój dysk.
2. Otwórz projekt w IDE (np. JetBrains Rider lub Visual Studio).
3. Uruchom projekt (plik `Program.cs`).
4. Aplikacja automatycznie załaduje dane testowe (sprzęt i użytkowników) za pomocą metody `SeedData()`, więc od razu możesz testować opcje w menu.

## Krótkie uzasadnienie decyzji projektowych

Zgodnie z założeniami projektu, skupiłem się na przemyślanym podziale kodu, aby uniknąć tworzenia tzw. "Boskich Obiektów" i chaosu w jednym pliku. Zdecydowałem się na podział na dwie główne warstwy: pliki domenowe (folder `Models`) oraz interfejs użytkownika (folder `UI`). Taki podział uznałem za sensowny, ponieważ pozwala łatwo zmieniać sposób wyświetlania menu bez ingerencji w główną logikę biznesową.

Oto jak w moim kodzie widać dbałość o dobre praktyki:

### 1. Odpowiedzialność klas i podział plików
Początkowo całe menu i interakcje znajdowały się w jednej klasie, co było trudne w utrzymaniu. Rozbiłem więc ten kod na mniejsze, wyspecjalizowane pliki w folderze `UI`. Na przykład `UserUI.cs` zajmuje się tylko wprowadzaniem danych studenta/pracownika, a `RentalUI.cs` tylko procesem wydawania i zwracania. Dzięki temu każda klasa ma jeden konkretny powód do ewentualnej zmiany.

### 2. Kohezja (Spójność logiki)
Aby zachować wysoką kohezję, cała "matematyka" i zasady biznesowe siedzą wyłącznie w klasie `RentalService`. Klasy z folderu `UI` nie dodają sprzętu bezpośrednio do list i nie liczą same kar za opóźnienia – po prostu przekazują dane do `RentalService` i odbierają gotowe wyniki. Dzięki temu logika wypożyczeń jest silnie skupiona w jednym miejscu, a nie rozrzucona po całym interfejsie.

### 3. Coupling (Sprzężenie) i chronienie stanu
Zadbałem o to, aby klasy nie ingerowały w swój stan wewnętrzny w sposób niekontrolowany (niskie sprzężenie). Widać to dobrze w klasie abstrakcyjnej `Equipment`. Właściwość `IsAvailable` posiada `protected set`. Oznacza to, że z zewnątrz (np. z warstwy UI) nie da się "na sztywno" wpisać `sprzet.IsAvailable = false`. Trzeba w tym celu wywołać konkretną metodę `MarkAsUnavailable()`. Zabezpiecza to obiekt przed przypadkowym zepsuciem jego stanu.

### 4. Dziedziczenie wynikające z domeny
Zamiast tworzyć jedną klasę `User` i sprawdzać instrukcjami `if/else`, czy to student (limit 2 sztuk) czy pracownik (limit 5 sztuk), użyłem klas pochodnych `Student` i `Employee`. Każda z nich sama definiuje (nadpisuje) swoją właściwość `MaxRentals`. Dzięki temu system jest elastyczny – jeśli w przyszłości dojdzie np. typ `Wykładowca`, po prostu dodam nową klasę bez ruszania istniejącej logiki walidacji.
