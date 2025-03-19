namespace SomerenApp.Models
{
    public class Docent
    {
        public int Docentnummer { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public int Leeftijd { get; set; }
        public string Telefoonnummer { get; set; }
        public int Kamernummer { get; set; }

        public Docent(int docentnummer, string voornaam, string achternaam, int leeftijd, string telefoonnummer, int kamernummer)
        {
            Docentnummer = docentnummer;
            Voornaam = voornaam;
            Achternaam = achternaam;
            Leeftijd = leeftijd;
            Telefoonnummer = telefoonnummer;
            Kamernummer = kamernummer;
        }

        public Docent(string voornaam, string achternaam, int leeftijd, string telefoonnummer)
        {
            Achternaam = achternaam;
            Voornaam = voornaam;
            Leeftijd = leeftijd;
            Telefoonnummer = telefoonnummer;
        }

        public Docent()
        {
            
        }
    }
}
