using System;

namespace Kopakabana
{
	class PrzeciaganieLiny : Mecz
	{
		public PrzeciaganieLiny(Druzyna druzyna1, Druzyna druzyna2, Sedzia sedziaGlowny, string faza) : base(druzyna1, druzyna2, sedziaGlowny, faza)
		{
		}

		public override void wypiszStatystykiMeczu()
		{
			druzyna1.pokazDruzyne();
			druzyna2.pokazDruzyne();

			Console.WriteLine($"Sedzia Glowny: {sedziaGlowny.getImie()} {sedziaGlowny.getNazwisko()}");
		}
	}
}
