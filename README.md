# Lemmikkikannan_API

Edellisen lemmikkitehtävän pohjalta teet API toteutus jolla voit :

-POST pyynnöt lemmikin ja omistajan lisäykseen

-GET pyyntö jolla voi hakea omistajan puhelinnumeron lemmikin nimen perusteella



API testaus

Tein testauksen HTTPie, tein myös molemmille POST ja GET pyynnöille omat tabit.
Post pyynnöille tein omat bodyt HTTpiessä.

Kun käytin POST http://localhost:5148/omistajat
bodynä toimi:
{
  "Nimi": "Matti",
  "Puhelin": "0401234567"
}

ja kun käytin POST http://localhost:5148/lemmikit
bodynä toimi:
{
  "Nimi": "Musti",
  "Laji": "Koira",
  "Omistaja": "Matti"
}

Molemmat bodyt ovat JSON.

Nyt GET http://localhost:5148/lemmikit/Musti toimii ja se tulostaa seuraavasti:
Lemmikki: Musti, Omistaja: Matti, Puhelin: 0401234567
