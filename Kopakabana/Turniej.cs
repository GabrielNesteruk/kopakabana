using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Kopakabana
{

	class Turniej
	{

		private string nazwa;
		private string typ;
		private List<Sedzia> listaSedziow;
		private List<Mecz> listaWszystkichMeczy;
		private List<Druzyna> listaDruznFazaGrupowa;
		private List<Druzyna> listaDruzynFazaPolfinalowo;
		private List<Druzyna> listaDruzynFazaFinalowa;
		private Druzyna finalista;

		public Turniej() { }

		public Turniej(string nazwa, string typ)
		{
			this.nazwa = nazwa;
			this.typ = typ;
			listaSedziow = new List<Sedzia>();
			listaWszystkichMeczy = new List<Mecz>();
			listaDruznFazaGrupowa = new List<Druzyna>();
			listaDruzynFazaPolfinalowo = new List<Druzyna>();
			listaDruzynFazaFinalowa = new List<Druzyna>();
		}

		public string getTyp => typ;

		private bool regexName(string imie, string nazwisko)
		{
			Regex regex = new Regex("^[a-zA-Z]+$");
			if (regex.IsMatch(imie) && regex.IsMatch(nazwisko))
				return true;
			else
				return false;
		}

		public void dodajDruzyne(Druzyna d)
		{
			listaDruznFazaGrupowa.Add(d);
		}


		public void usunDruzyne(string nazwa)
		{
			if (listaDruznFazaGrupowa.Count == 0)
				Console.WriteLine("Nie dodano jeszcze zadnej druzyny.\nWybierz opcje nr 1 aby dodac druzyne...\n\n");
			else
				foreach (var druzyna in listaDruznFazaGrupowa)
				{
					if (druzyna.getNazwa == nazwa)
					{
						listaDruznFazaGrupowa.Remove(druzyna);
						break;
					}
				}
		}


		public void wyswietlDruzyny()
		{
			if (listaDruznFazaGrupowa.Count == 0)
				Console.WriteLine("Nie dodano jeszcze zadnej druzyny.\nWybierz opcje nr 1 aby dodac druzyne...\n\n");
			else
				foreach (var druzyna in listaDruznFazaGrupowa)
				{
					druzyna.pokazDruzyne();
				}
		}


		public bool wystartujTurniej()
		{
			if (listaDruznFazaGrupowa.Count < 4)
			{
				Console.WriteLine("Turniej nie wystartowal! [ za mala ilosc druzyn ]");
				return false;
			}
			else if (listaSedziow.Count < 1)
			{
				Console.WriteLine("Turniej nie wystartowal! [ musisz dodac przynajmniej 1 sedziego ]");
				return false;
			}
			else if (listaSedziow.Count < 3 && typ == "Siatkowka")
			{
				Console.WriteLine("Turniej nie wystartowal! [ w przypadku meczy siatkowki musisz dodac przynajmniej 3 sedziow! ]");
				return false;
			}
			else
			{
				Console.WriteLine("Warunki startu turnieju zostaly spelnione");
				return true;
			}
		}

		public void dodajSedzieTurniejowego()
		{
			Console.Clear();
			string imie = String.Empty, nazwisko = String.Empty;
			int pomocnicza = 0;
			while (regexName(imie, nazwisko) == false)
			{
				Console.WriteLine("Podaj imie i nazwisko sedziego:\n");
				Console.WriteLine("Imie:");
				imie = Console.ReadLine();
				Console.WriteLine("Nazwisko:");
				nazwisko = Console.ReadLine();
				for (int i = 0; i < listaSedziow.Count; i++)
				{
					if (listaSedziow[i].getNazwisko().Equals(nazwisko, StringComparison.OrdinalIgnoreCase) && listaSedziow[i].getImie().Equals(imie, StringComparison.OrdinalIgnoreCase))
					{
						pomocnicza++;
					}
				}
				if (pomocnicza == 1)
				{
					Console.Clear();
					Console.WriteLine("Dany sedzia widnieje w bazie danych, aby przejsc dalej nacisnij cokolwiek");
					Console.ReadKey();
				}
				else
				{
					if (regexName(imie, nazwisko) == true)
					{
						Sedzia sedzia = new Sedzia(imie, nazwisko);
						listaSedziow.Add(sedzia);
					}
				}
				pomocnicza = 0;
			}
		}
		public void usunSedziegoTurniejowego()
		{
			string imie = String.Empty, nazwisko = String.Empty;
			Console.WriteLine("Podaj imie: ");
			imie = Console.ReadLine();
			Console.WriteLine("Podaj nazwisko: ");
			nazwisko = Console.ReadLine();
			for (int i = 0; i < listaSedziow.Count; i++)
			{
				if (listaSedziow[i].getNazwisko().Equals(nazwisko, StringComparison.OrdinalIgnoreCase) && listaSedziow[i].getImie().Equals(imie, StringComparison.OrdinalIgnoreCase))
				{
					listaSedziow.RemoveAt(i);
				}
				else
				{
					Console.WriteLine("Podany sedzia nie istnieje w bazie, aby przejsc dalej nacisnij cokolwiek");
					Console.ReadKey();
				}
			}
		}
		public void wyswietlSedziowTurniejowych()
		{
			int zmienna = 1;
			foreach (var sedzia in listaSedziow)
			{
				Console.WriteLine("Sedzia numer: " + zmienna);
				Console.WriteLine("Imie: " + sedzia.getImie());
				Console.WriteLine("Nazwisko: " + sedzia.getNazwisko());
				zmienna++;
				Console.WriteLine();
			}
		}

		public void rozegrajMecze() { }
		public void wczytajTurniejPlik() { }
		public void klasyfikujDruzynyPolfinal() { }


		public void klasyfikujDruzynyFinal()
		{
			int zwyciezca = -1;
			foreach (var druzyna in listaDruzynFazaPolfinalowo)
			{
				if (druzyna.getIloscPunktow > zwyciezca)
					finalista = druzyna;
			}
		}


		public void wypiszTabeleWynikow() { }

	}
}
