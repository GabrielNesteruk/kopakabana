
using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana
{

    class Sedzia
    {
        private string imie;
        private string nazwisko;

        public Sedzia() { }

        public Sedzia(string imie, string nazwisko)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
        }

        public void setImie(string imie)
        {
            this.imie = imie;
        }
        public string getImie()
        {
            return this.imie;
        }

        public void setNazwisko(string nazwisko)
        {
            this.nazwisko = nazwisko;
        }

        public string getNazwisko()
        {
            return this.nazwisko;
        }
    }
}
