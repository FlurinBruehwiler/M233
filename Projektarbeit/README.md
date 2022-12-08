# Projektarbeit

## Angepasste Anforderungen

Die nicht Erweiterten nicht funktionalen Anforderungen haben sich im Vergleich zur Planung verändert, da mir erst später klar wurde, das zwei dieser Anforderungen obligatorisch sind.

Nicht-funktionale Anforderungen **vorher**:
- Alle Endpunkte sind mit OpenAPI dokumentiert
- Die API ist in einem Docker container ausführbar

Nicht-funktionale Anforderungen **nachher**:
- Das Passwort muss mindestens 8 Zeichen lang sein und ein Sonderzeichen beinhalten.
- Die Email Adresse wird min einem Regex überprüft


## Aufsetzen

1. Stellen Sie sicher, dass Docker installiert ist und läuft.
2. Stellen Sie sicher, dass Visual Studio Code und die Erweiterung Remote Container installiert ist.
3. Öffnen Sie das Projekt mit Visual Studio Code.
4. Öffnen Sie das Projekt im Entwicklungscontainer.
5. Führen sie folgende befehle aus

````
cd ./Projektarbeit
dotnet dev-certs https --trust
````

## Datenbank
Als Datenbank wird SQLite verwendet. Eine leere Datenbank mit den ensprechenden Tabellen ist bereits im Projekt vorhanden. Es muss also nichts aufgesetzt werden. :)

## Starten
```
dotnet run
```

Das Projekt wird nun unter localhost:5000 gehostet

Swagger ist auf localhost:5000/swagger aufrufbar

## Tests durchführen

Habe ich leider auf Linux (devcontainer, docker, Github Actions) nicht zum laufen gebracht. :(

Ansonsten müsste man in den Root den Projekts navigieren und folgendes ausführen:

```
dotnet test
```

## Testdaten

Die Testdaten werden geladen, wenn man eine Umgebungsvariable auf development setzt.

```
export ASPNETCORE_ENVIRONMENT=Development
```

Wenn man danach das Program startet, werden die Testdaten eingelesen.

Wenn man die einlesung wieder austellen möchte, kann man die Umgebungsvariable wieder auf production setzen

```
export ASPNETCORE_ENVIRONMENT=Production
```

Es gibt 4 Benutzer mit denen man sich anmelden kann
example1@example.com
example2@example.com
example3@example.com
example4@example.com (Admin)

Alle haben das Passwort "password"
