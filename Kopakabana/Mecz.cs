using System;

namespace Kopakabana
{
	abstract class Mecz
	{
		private string faza                 = "";
		protected Druzyna druzyna1			;
		protected Druzyna druzyna2			;
		private int punktyDruzyny1			= 0;
		private int punktyDruzyny2			= 0;
		private const int punktyZwyciestwo	= 3;
		private const int punktyRemis		= 1;
		private const int punktyPrzegrana	= 0;
		protected Sedzia sedziaGlowny		    ;


		public Mecz(Druzyna druzyna1, Druzyna druzyna2, Sedzia sedziaGlowny, string faza)
		{
			this.druzyna1 = druzyna1;
			this.druzyna2 = druzyna2;
			this.sedziaGlowny = sedziaGlowny;
			this.faza = faza;
		}
		public Druzyna Druzyna1 { get; private set; }
		public Druzyna Druzyna2 { get; private set; }
		public Sedzia SedziaGlowny { get; private set; }


		public virtual void wypiszStatystykiMeczu() { }

        public int PunktyZwyciestwo
        {
            get
            {
                return punktyZwyciestwo;
            }
        }

        public int PunktyPrzegrana
        {
            get
            {
                return punktyPrzegrana;
            }
        }

        public int PunktyRemis
        {
            get
            {
                return punktyRemis;
            }
        }


        public int PunktyDruzyny_1 
		{
			get => punktyDruzyny1;
			private set {
				if (value > 0)
					punktyDruzyny1 = value;
			}
		}


		public int PunktyDruzyny_2
		{
			get => punktyDruzyny2;
			private set
			{
				if (value > 0)
					punktyDruzyny2 = value;
			}
		}

	}
}
