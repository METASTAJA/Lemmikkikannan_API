//Luo Web-sovelluksen.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Luodaan Lemmikki.DB olio.
var db = new LemmikkiDB();

//POST Pyyntö omistajan lisäämiseen.
app.MapPost("/omistajat", (Omistaja omistaja) =>
{
    db.LisaaOmistaja(omistaja.Nimi, omistaja.Puhelin);
    return "Omistaja lisätty!";
});

// POST Pyyntö lemmikin lisäämisen.
app.MapPost("/lemmikit", (Lemmikki lemmikki) =>
{
    db.LisaaLemmikki(lemmikki.Nimi, lemmikki.Laji, lemmikki.Omistaja);
    return "Lemmikki lisätty!";
});

// GET Pyyntö hakemiseen omistajan puhelinnumeron lemmikin nimen perusteella.
app.MapGet("/lemmikit/{nimi}", (string nimi) =>
{
    string tulos = db.HaeLemmikki(nimi);
    return tulos;
});

//käynistää web-palvelimen
app.Run();

//Luokka datan siirtämiseen sovelluksien osien välillä.
public record Omistaja(string Nimi, string Puhelin);
public record Lemmikki(string Nimi, string Laji, string Omistaja);
