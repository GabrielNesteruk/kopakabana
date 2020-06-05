using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Kopakabana
{

	class Turniej
	{
		//komentarz
		private string nazwa;
		private string typ;
		private List<Sedzia> listaSedziow;
		private List<Mecz> listaWszystkichMeczy;
		private List<Druzyna> listaDruzynFazaGrupowa;
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
			listaDruzynFazaGrupowa = new List<Druzyna>();
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
			listaDruzynFazaGrupowa.Add(d);
		}


		public void usunDruzyne(string nazwa)
		{
			if (listaDruzynFazaGrupowa.Count == 0)
				Console.WriteLine("Nie dodano jeszcze zadnej druzyny.\nWybierz opcje nr 1 aby dodac druzyne...\n\n");
			else
				foreach (var druzyna in listaDruzynFazaGrupowa)
				{
					if (druzyna.getNazwa == nazwa)
					{
						listaDruzynFazaGrupowa.Remove(druzyna);
						break;
					}
				}
		}


		public void wyswietlDruzyny()
		{
			if (listaDruzynFazaGrupowa.Count == 0)
				Console.WriteLine("Nie dodano jeszcze zadnej druzyny.\nWybierz opcje nr 1 aby dodac druzyne...\n\n");
			else
				foreach (var druzyna in listaDruzynFazaGrupowa)
				{
					druzyna.pokazDruzyne();
				}
		}


		public bool wystartujTurniej()
		{
			if (listaDruzynFazaGrupowa.Count < 4)
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
			int pom = 0;
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
					break;
				}
				else
				{
					pom++;
				}
				if (pom == listaSedziow.Count)
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

		public void rozegrajMecze()
		{
			int punkty_1 = 0;
			int punkty_2 = 0;
			int randomowa = 0;
			int k = 1;
			int pomocnicza = 1;

			Random generator = new Random();

			if (this.typ == "Siatkowka")
			{
				for (int i = 0; i < listaDruzynFazaGrupowa.Count - 1; i++)
				{

					while (k < listaDruzynFazaGrupowa.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaGrupowa[i].dodajPunkty(punkty_1);
						listaDruzynFazaGrupowa[k].dodajPunkty(punkty_2);
						Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[1]);
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[2]);
						listaWszystkichMeczy.Add(siatkowka_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
				for (int i=0; i<listaWszystkichMeczy.Count; i++)
				{
					listaWszystkichMeczy[i].wypiszStatystykiMeczu();
					
					
						Console.WriteLine();
						Console.WriteLine();
						Console.WriteLine();
					
				}
				Console.ReadKey();
				klasyfikujDruzynyPolfinal();
				 k = 1;
				 pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaPolfinalowo.Count - 1; i++)
				{

					while (k < listaDruzynFazaPolfinalowo.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaPolfinalowo[i].dodajPunkty(punkty_1);
						listaDruzynFazaPolfinalowo[k].dodajPunkty(punkty_2);
						Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaPolfinalowo[i], listaDruzynFazaPolfinalowo[k], listaSedziow[0], "Faza grupowa");
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[1]);
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[2]);
						listaWszystkichMeczy.Add(siatkowka_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
				klasyfikujDruzynyFinal();
				k = 1;
				pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaFinalowa.Count - 1; i++)
				{

					while (k < listaDruzynFazaFinalowa.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaFinalowa[i].dodajPunkty(punkty_1);
						listaDruzynFazaFinalowa[k].dodajPunkty(punkty_2);
						Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaFinalowa[i], listaDruzynFazaFinalowa[k], listaSedziow[0], "Faza grupowa");
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[1]);
						siatkowka_mecz.sedziowieDodatkowi.Add(listaSedziow[2]);
						listaWszystkichMeczy.Add(siatkowka_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
			}
			

			else if (this.typ == "Dwa_ognie")
			{
				k = 1;
				pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaGrupowa.Count - 1; i++)
				{

					while (k < listaDruzynFazaGrupowa.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaGrupowa[i].dodajPunkty(punkty_1);
						listaDruzynFazaGrupowa[k].dodajPunkty(punkty_2);
						DwaOgnie dwaognie_mecz = new DwaOgnie(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
						listaWszystkichMeczy.Add(dwaognie_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
				for (int i = 0; i < listaWszystkichMeczy.Count; i++)
				{
					listaWszystkichMeczy[i].wypiszStatystykiMeczu();


					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine();

				}
				Console.ReadKey();
				klasyfikujDruzynyPolfinal();
				k = 1;
				pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaPolfinalowo.Count - 1; i++)
				{

					while (k < listaDruzynFazaPolfinalowo.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaPolfinalowo[i].dodajPunkty(punkty_1);
						listaDruzynFazaPolfinalowo[k].dodajPunkty(punkty_2);
						DwaOgnie dwaognie_mecz = new DwaOgnie(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
						listaWszystkichMeczy.Add(dwaognie_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
				klasyfikujDruzynyFinal();
				k = 1;
				pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaFinalowa.Count - 1; i++)
				{

					while (k < listaDruzynFazaFinalowa.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaFinalowa[i].dodajPunkty(punkty_1);
						listaDruzynFazaFinalowa[k].dodajPunkty(punkty_2);
						DwaOgnie dwaognie_mecz = new DwaOgnie(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
						listaWszystkichMeczy.Add(dwaognie_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
			}
			else if (this.typ == "Przeciaganie_Liny")
			{
				k = 1;
				pomocnicza = 1;
				for (int i = 0; i < listaDruzynFazaGrupowa.Count - 1; i++)
				{

					while (k < listaDruzynFazaGrupowa.Count)
					{
						randomowa = generator.Next(4);
						while (randomowa == 2)
						{
							randomowa = generator.Next(4);
						}
						punkty_1 = randomowa;
						if (punkty_1 == 0)
						{
							punkty_2 = 3;
						}
						else if (punkty_1 == 1)
						{
							punkty_2 = 1;
						}
						else if (punkty_1 == 3)
						{
							punkty_2 = 0;
						}
						listaDruzynFazaGrupowa[i].dodajPunkty(punkty_1);
						listaDruzynFazaGrupowa[k].dodajPunkty(punkty_2);
						PrzeciaganieLiny przeciaganieLiny_mecz = new PrzeciaganieLiny(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
						listaWszystkichMeczy.Add(przeciaganieLiny_mecz);
						k++;
					}
					pomocnicza++;
					k = pomocnicza;
				}
			}
			klasyfikujDruzynyPolfinal();
			k = 1;
			pomocnicza = 1;
			for (int i = 0; i < listaDruzynFazaPolfinalowo.Count - 1; i++)
			{

				while (k < listaDruzynFazaPolfinalowo.Count)
				{
					randomowa = generator.Next(4);
					while (randomowa == 2)
					{
						randomowa = generator.Next(4);
					}
					punkty_1 = randomowa;
					if (punkty_1 == 0)
					{
						punkty_2 = 3;
					}
					else if (punkty_1 == 1)
					{
						punkty_2 = 1;
					}
					else if (punkty_1 == 3)
					{
						punkty_2 = 0;
					}
					listaDruzynFazaPolfinalowo[i].dodajPunkty(punkty_1);
					listaDruzynFazaPolfinalowo[k].dodajPunkty(punkty_2);
					PrzeciaganieLiny przeciaganieLiny_mecz = new PrzeciaganieLiny(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
					listaWszystkichMeczy.Add(przeciaganieLiny_mecz);
					k++;
				}
				pomocnicza++;
				k = pomocnicza;
			}
			klasyfikujDruzynyFinal();
			k = 1;
			pomocnicza = 1;
			for (int i = 0; i < listaDruzynFazaFinalowa.Count - 1; i++)
			{

				while (k < listaDruzynFazaFinalowa.Count)
				{
					randomowa = generator.Next(4);
					while (randomowa == 2)
					{
						randomowa = generator.Next(4);
					}
					punkty_1 = randomowa;
					if (punkty_1 == 0)
					{
						punkty_2 = 3;
					}
					else if (punkty_1 == 1)
					{
						punkty_2 = 1;
					}
					else if (punkty_1 == 3)
					{
						punkty_2 = 0;
					}
					listaDruzynFazaFinalowa[i].dodajPunkty(punkty_1);
					listaDruzynFazaFinalowa[k].dodajPunkty(punkty_2);
					PrzeciaganieLiny przeciaganieLiny_mecz = new PrzeciaganieLiny(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[k], listaSedziow[0], "Faza grupowa");
					listaWszystkichMeczy.Add(przeciaganieLiny_mecz);
					k++;
				}
				pomocnicza++;
				k = pomocnicza;
			}

		}
		public void wczytajTurniejPlik() { }
		public void klasyfikujDruzynyPolfinal() { }
		public void klasyfikujDruzynyFinal() { }
		public void wypiszTabeleWynikow() { }

	}
}
