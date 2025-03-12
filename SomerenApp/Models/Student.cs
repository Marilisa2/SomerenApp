namespace SomerenApp.Models
{
    public class Student
    {
        public int Studentnummer {  get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Klas {  get; set; }
        public string Telefoonnummer { get; set; }
        public int Kamernummer { get; set; }
        public Student(int studentnummer, string voornaam, string achternaam, string klas, string telefoonnummer, int kamernummer)
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
