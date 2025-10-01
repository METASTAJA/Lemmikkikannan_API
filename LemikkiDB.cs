using System.Data;
using Microsoft.Data.Sqlite;

public class LemmikkiDB
{
    private string _connectionstring = "Data Source = lemmikki.db";

    //Rakentaja
    public LemmikkiDB()
    {
        //Luodaan yhteys tietokantaan
        using var connection = new SqliteConnection(_connectionstring);
        //avaa yhteyden tietokantaan
        connection.Open();
        //luodaan taulut, jos niitä ei vielä ole
        //Omistajat taulu
        var commandForOmistjat = connection.CreateCommand();
        commandForOmistjat.CommandText = "CREATE TABLE IF NOT EXISTS Omistajat (id INTEGER PRIMARY KEY,nimi TEXT,puhelin TEXT)";
        commandForOmistjat.ExecuteNonQuery(); //emmen odota vastausta tältä komennolta

        //Lemmitikit taulu
        var commandForLemmikit = connection.CreateCommand();
        commandForLemmikit.CommandText = "CREATE TABLE IF NOT EXISTS Lemmikit (id INTEGER PRIMARY KEY,nimi TEXT,laji TEXT,omistajan_id INTEGER)";
        commandForLemmikit.ExecuteNonQuery(); //emmen odota vastausta tältä komennolta
    }

    //Lisää omistjan
    public void LisaaOmistaja(string nimi, string puhelin)
    {
        //Luodaan yhteys tietokantaan
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            //Lisätään omistaja tietokantaan
            var commandForInsert = connection.CreateCommand();
            commandForInsert.CommandText = "INSERT INTO Omistajat (nimi, puhelin) VALUES (@Nimi, @Puhelin)";
            //Parametrien lisäys
            commandForInsert.Parameters.AddWithValue("Nimi", nimi);
            commandForInsert.Parameters.AddWithValue("Puhelin", puhelin);
            commandForInsert.ExecuteNonQuery();

            Console.WriteLine("Omistaja lisätty");
        }

    }

    //Lisää lemmikin
    public void LisaaLemmikki(string nimi, string laji, string omistajannimi)
    {
        //Luodaan yhteys tietokantaan
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            //Haetaan omistajan id
            var command1 = connection.CreateCommand();
            command1.CommandText = "SELECT id FROM Omistajat WHERE nimi = @Nimi";
            //Parametrien lisäys
            command1.Parameters.AddWithValue("Nimi", omistajannimi);
            object? idObj = command1.ExecuteScalar();

            if (idObj == null)
            {
                Console.WriteLine("Omistajaa ei löytynyt!");
                return;
            }

            int omistajanid = Convert.ToInt32(idObj);

            //Lisätään lemmikki tietokantaan
            var command2 = connection.CreateCommand();
            command2.CommandText = "INSERT INTO Lemmikit (nimi, laji, omistajan_id) VALUES (@Nimi, @Laji, @Omistajanid)";
            //Parametrien lisäys
            command2.Parameters.AddWithValue("Nimi", nimi);
            command2.Parameters.AddWithValue("Laji", laji);
            command2.Parameters.AddWithValue("Omistajanid", omistajanid);
            command2.ExecuteNonQuery();

            Console.WriteLine("Lemmikki lisätty");
        }
    }

    public void puhelinnumeronpaivitys(string nimi, string uusinumero)
    {
        //Luodaan yhteys tietokantaan
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            //luodaan SQL komento, joka päivittää puhelinnumeron nimen mukaan
            //SET määrittää mikä sarake on kyseessä
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Omistajat SET puhelin = @Puhelin WHERE nimi = @Nimi";
            //Parametrien lisäys
            command.Parameters.AddWithValue("Puhelin", uusinumero);
            command.Parameters.AddWithValue("Nimi", nimi);
            //SQL komento joka katsoo montako riviä päivittyi
            if (command.ExecuteNonQuery() > 0)
            {
                //jos rivi päivittyi näytetään tämä
                Console.WriteLine("Puhelinnumero päivitetty");
            }
            else
            {
                //jos rivi ei päivittynyt näytetään tämä
                Console.WriteLine("Omistajaa ei löytynyt");
            }
        }
    }

    public  string HaeLemmikki(string haettavanimi)
    {
        //luodaan yhteys tietokantaan
        using (var connection = new SqliteConnection(_connectionstring))
        {
            connection.Open();
            //Luodaan SQL komento joka hakee tarvittavat tiedot
            var commandForSelect = connection.CreateCommand();
            commandForSelect.CommandText = @"SELECT Lemmikit.nimi, Omistajat.nimi, Omistajat.puhelin 
            FROM Lemmikit 
            JOIN Omistajat ON Lemmikit.omistajan_id = Omistajat.id WHERE Lemmikit.nimi LIKE @Nimi";
            //lisää parametrit
            commandForSelect.Parameters.AddWithValue("Nimi", haettavanimi);

            var reader = commandForSelect.ExecuteReader();

            string lemmikit = "";
            //käydään tulokset läpi riveittäin
            while (reader.Read())
            {
                //Haetaan lemmikin nimi,omistjan nimi ja puhelin numero
                lemmikit += $"Lemmikki: {reader.GetString(0)}, Omistaja: {reader.GetString(1)}, Puhelin: {reader.GetString(2)}";
            }
            reader.Close(); //sulkee readerin
            //Jos yhtään osumaa ei tullut
            if (lemmikit == "")
            {
                return "Lemmikkiä ei löytynyt";
            }
            //Palauttaa osumat
            return lemmikit;
        }
    }

}