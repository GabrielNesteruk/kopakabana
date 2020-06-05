using System;
using System.Collections.Generic;

namespace Kopakabana
{
    class Siatkowka : Mecz
	{

		public List<Sedzia> sedziowieDodatkowi = new List<Sedzia>();

        public Siatkowka(Druzyna druzyna1, Druzyna druzyna2, Sedzia sedziaGlowny, string faza) : base(druzyna1, druzyna2, sedziaGlowny, faza)
        {

        }

        public override void wypiszStatystykiMeczu()
		{
			druzyna1.pokazDruzyne();
			druzyna2.pokazDruzyne();

			Console.WriteLine("Sedzia główny: " + sedziaGlowny.getImie() + sedziaGlowny.getNazwisko() + " Sedzia dodatkowy: " + sedziowieDodatkowi[0].getImie() + " " + sedziowieDodatkowi[0].getNazwisko() + ", "+ sedziowieDodatkowi[1].getImie() + " " + sedziowieDodatkowi[1].getNazwisko());
			Console.WriteLine();
		}
	}
}
