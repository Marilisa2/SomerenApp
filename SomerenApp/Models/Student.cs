namespace SomerenApp.Models
{
    public class Student
    {
        int Studentnummer {  get; set; }
        string Voornaam { get; set; }
        string Achternaam { get; set; }
        string Klas {  get; set; }
        int Telefoonnummer { get; set; }
        int Kamernummer { get; set; }
        public Student(int studentnummer, string voornaam, string achternaam, string klas, int telefoonnummer, int kamernummer)
        {
            Studentnummer = studentnummer;
            Voornaam = voornaam;
            Achternaam = achternaam;
            Klas = klas;
            Telefoonnummer = telefoonnummer;
            Kamernummer = kamernummer;
        }
        public Student()
        {

        }
    }
}
