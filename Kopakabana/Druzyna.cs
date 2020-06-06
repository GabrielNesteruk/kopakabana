using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class Druzyna
    {
        private string nazwa;
        private int iloscPunktow;
        private string typ;

        public Druzyna()
        {
            
        }

        public Druzyna(string _nazwa, int _iloscPunktow, string _typ)
        {
            nazwa = _nazwa;
            iloscPunktow = _iloscPunktow;
            typ = _typ;
        }

        public string getNazwa => nazwa;

        public int getPunkty => iloscPunktow;

        public void pokazDruzyne()
        {
            Console.WriteLine($"Nazwa druzyny: {nazwa}\nIlosc punktow: {iloscPunktow}\nDyscyplina: {typ}\n");
        }
        public bool dodajPunkty(int x)
        {
            if (x > 0)
            {
                iloscPunktow += x;
                return true;
            }
            else
                return false;
        }
    }
}
