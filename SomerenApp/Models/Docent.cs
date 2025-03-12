namespace SomerenApp.Models
{
    public class Docent
    {
        int Docentnummer { get; set; }
        string Voornaam { get; set; }
        string Achternaam { get; set; }
        int Leeftijd { get; set; }
        int Telefoonnummer { get; set; }
        int Kamernummer { get; set; }

        public Docent(int docentnummer, string voornaam, string achternaam, int leeftijd, int telefoonnummer, int kamernummer)
        {
            Docentnummer = docentnummer;
            Voornaam = voornaam;
            Achternaam = achternaam;
            Leeftijd = leeftijd;
            Telefoonnummer = telefoonnummer;
            Kamernummer = kamernummer;
        }

        public Docent()
        {
            
        }
    }
}
