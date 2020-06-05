using System;

namespace Kopakabana
{
    class Siatkowka : Mecz
	{
		private Sedzia sedziapomocniczy1;
		private Sedzia sedziapomocniczy2;

		public Sedzia Sedziapomocniczy1 { get; private set; }
		public Sedzia Sedziapomocniczy2 { get; private set; }


		public Siatkowka(Druzyna druzyna1, Druzyna druzyna2, Sedzia sedziaGlowny, string faza) : base(druzyna1, druzyna2, sedziaGlowny, faza)
		{

		}

		public override void wypiszStatystykiMeczu()
		{
			druzyna1.pokazDruzyne();
			druzyna2.pokazDruzyne();

			Console.WriteLine(
				$"Sedzia Glowny: {sedziaGlowny}" +
				$"Sedzia pomocniczy: {sedziapomocniczy1}" +
				$"Sedzia pomocniczy: {sedziapomocniczy2}"
			);
		}
	}
}
