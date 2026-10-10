## Requisiti per l'uso

- Windows 10/11 a 64 bit
- SQL Server LocalDB (incluso in Visual Studio, oppure installabile come SQL Server Express LocalDB)
- Un account Windows con password (il tool usa l'accesso di Windows)

## Primo avvio

Al primo avvio il database `LCToolDb` viene creato automaticamente, con le tabelle e le voci delle liste (Stato e Rilevata da). Non serve eseguire script.


## Per sviluppatori

1. Clona il repository:

```bash
   git clone https://github.com/Lux2030/LCTool-Osservazione.git
   cd LCTool-Osservazione
```

2. Ripristina le dipendenze e compila:

```bash
   dotnet restore
   dotnet build
```

3. Avvia l'applicazione:

```bash
   dotnet run --project LCTool.Osservazione
```
