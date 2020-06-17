using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

namespace Kopakabana
{

	class Turniej
	{
		private string nazwa;
		private string typ;
		private List<Sedzia> listaSedziow = new List<Sedzia>();
        private List<Mecz> listaMeczyFazaGrupowa = new List<Mecz>();
        private List<Mecz> listaMeczyFazaPolfinalowa = new List<Mecz>();
        private List<Mecz> listaMeczyFazaFinalowa = new List<Mecz>();

        private List<Druzyna> listaDruzynFazaGrupowa = new List<Druzyna>();
        private List<Druzyna> listaDruzynFazaPolfinalowo = new List<Druzyna>();
        private List<Druzyna> listaDruzynFazaFinalowa = new List<Druzyna>();
        private Druzyna finalista;

		public Turniej() { }

		public Turniej(string nazwa, string typ)
		{
			this.nazwa = nazwa;
            this.typ = typ;
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


        private int generujRezultatMeczu()
        {
            // 0 - wygrala druzyna 1
            // 1 - wygrala druzyna 2
            // 2 - remis
            Random generator = new Random();
            int randomowa = generator.Next(3);
            return randomowa;
        }
        private void pokazMecz(Mecz m)
        {
            Console.WriteLine($"***********************************************\nMecz pomiedzy:\n");
            m.wypiszStatystykiMeczu();
            Console.WriteLine($"***********************************************\n\n\n");
        }
        private Mecz utworzMecz(int i, int j, string faza)
        {
            if (this.typ == "Dwa_ognie")
                return new DwaOgnie(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[j], listaSedziow[0], faza);
            else if (this.typ == "Przeciaganie_Liny")
                return new PrzeciaganieLiny(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[j], listaSedziow[0], faza);
            else
                throw new WrongMatchTypeException("Blad programu! [ Obiekt meczu zostal stworzony nieprawidlowo ]");
        }

        private void dodajSedziowPomocnicznychSiatka(Siatkowka s)
        {
            s.sedziowieDodatkowi.Add(listaSedziow[1]);
            s.sedziowieDodatkowi.Add(listaSedziow[2]);
        }
        private void rozegrajMeczeSiatkowkiDanejFazy(string faza)
        {
            if (faza == "Faza grupowa")
            {
                for (int i = 0; i < listaDruzynFazaGrupowa.Count; i++)
                {
                    for (int j = i + 1; j < listaDruzynFazaGrupowa.Count; j++)
                    {
                        // 0 - wygrala druzyna 1
                        // 1 - wygrala druzyna 2
                        // 2 - remis

                        int rezultatMeczu = generujRezultatMeczu();
                        if (rezultatMeczu == 0)
                        {
                            // druzyna i win
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[j], listaSedziow[0], faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                            listaDruzynFazaGrupowa[j].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[i].getNazwa}");
                            listaMeczyFazaGrupowa.Add(siatkowka_mecz);
                            pokazMecz(siatkowka_mecz);
                        }
                        else if (rezultatMeczu == 1)
                        {
                            // druzyna j win
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[j], listaSedziow[0], faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                            listaDruzynFazaGrupowa[j].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            listaMeczyFazaGrupowa.Add(siatkowka_mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[j].getNazwa}");
                            pokazMecz(siatkowka_mecz);
                        }
                        else
                        {
                            // remis
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaGrupowa[i], listaDruzynFazaGrupowa[j], listaSedziow[0], faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(siatkowka_mecz.PunktyRemis);
                            listaDruzynFazaGrupowa[j].dodajPunkty(siatkowka_mecz.PunktyRemis);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            listaMeczyFazaGrupowa.Add(siatkowka_mecz);
                            Console.WriteLine($"Remis");
                            pokazMecz(siatkowka_mecz);
                        }
                        
                    }
                }
            }
            else if (faza == "Faza polfinalowa")
            {
                for (int i = 0; i < listaDruzynFazaPolfinalowo.Count; i++)
                {
                    for (int j = i + 1; j < listaDruzynFazaPolfinalowo.Count; j++)
                    {
                        // 0 - wygrala druzyna 1
                        // 1 - wygrala druzyna 2
                        // 2 - remis
                        int rezultatMeczu = generujRezultatMeczu();
                        if (rezultatMeczu == 0)
                        {
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaPolfinalowo[i], listaDruzynFazaPolfinalowo[j], listaSedziow[0], faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaPolfinalowo[i].getNazwa}");
                            listaMeczyFazaPolfinalowa.Add(siatkowka_mecz);
                            pokazMecz(siatkowka_mecz);
                        }                         
                        else if (rezultatMeczu == 1)
                        {                         
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaPolfinalowo[i], listaDruzynFazaPolfinalowo[j], listaSedziow[0], faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaPolfinalowo[j].getNazwa}");
                            listaMeczyFazaPolfinalowa.Add(siatkowka_mecz);
                            pokazMecz(siatkowka_mecz);
                        }
                        else
                        {
                            Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaPolfinalowo[i], listaDruzynFazaPolfinalowo[j], listaSedziow[0], faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(siatkowka_mecz.PunktyRemis);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(siatkowka_mecz.PunktyRemis);
                            dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                            Console.WriteLine($"Remis");
                            listaMeczyFazaPolfinalowa.Add(siatkowka_mecz);
                            pokazMecz(siatkowka_mecz);
                        }
                    }
                }
            }
            else if (faza == "Faza finalowa")
            {
                {
                    for (int i = 0; i < listaDruzynFazaFinalowa.Count; i++)
                    {
                        for (int j = i + 1; j < listaDruzynFazaFinalowa.Count; j++)
                        {
                            int rezultatMeczu = generujRezultatMeczu();
                            if (rezultatMeczu == 0)
                            {
                                Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaFinalowa[i], listaDruzynFazaFinalowa[j], listaSedziow[0], faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                                listaDruzynFazaFinalowa[j].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                                dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                                listaMeczyFazaFinalowa.Add(siatkowka_mecz);
                                Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[i].getNazwa}");
                                pokazMecz(siatkowka_mecz);
                            }
                            else if (rezultatMeczu == 1)
                            {
                                Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaFinalowa[i], listaDruzynFazaFinalowa[j], listaSedziow[0], faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(siatkowka_mecz.PunktyPrzegrana);
                                listaDruzynFazaFinalowa[j].dodajPunkty(siatkowka_mecz.PunktyZwyciestwo);
                                dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                                listaMeczyFazaFinalowa.Add(siatkowka_mecz);
                                Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[j].getNazwa}");
                                pokazMecz(siatkowka_mecz);
                            }
                            else
                            {
                                Siatkowka siatkowka_mecz = new Siatkowka(listaDruzynFazaFinalowa[i], listaDruzynFazaFinalowa[j], listaSedziow[0], faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(siatkowka_mecz.PunktyRemis);
                                listaDruzynFazaFinalowa[j].dodajPunkty(siatkowka_mecz.PunktyRemis);
                                dodajSedziowPomocnicznychSiatka(siatkowka_mecz);
                                listaMeczyFazaFinalowa.Add(siatkowka_mecz);
                                Console.WriteLine($"Remis");
                                pokazMecz(siatkowka_mecz);
                            }
                        }
                    }
                }
            }
            else
                throw new WrongMatchPhaseException("Blad programu! [ Nieprawidlowa faza meczu ]");
        }
        private void rozegrajMeczeDanejFazy(string faza)
        {
            if (faza == "Faza grupowa")
            {
                for (int i = 0; i < listaDruzynFazaGrupowa.Count; i++)
                {
                    for (int j = i + 1; j < listaDruzynFazaGrupowa.Count; j++)
                    {
                        int rezultatMeczu = generujRezultatMeczu();
                        if (rezultatMeczu == 0)
                        {
                            // druzyna i win
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(mecz.PunktyZwyciestwo);
                            listaDruzynFazaGrupowa[j].dodajPunkty(mecz.PunktyPrzegrana);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[i].getNazwa}");
                            listaMeczyFazaGrupowa.Add(mecz);
                            pokazMecz(mecz);
                        }
                        else if (rezultatMeczu == 1)
                        {
                            // druzyna j win
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(mecz.PunktyPrzegrana);
                            listaDruzynFazaGrupowa[j].dodajPunkty(mecz.PunktyZwyciestwo);
                            listaMeczyFazaGrupowa.Add(mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaGrupowa[j].getNazwa}");
                            pokazMecz(mecz);
                        }
                        else
                        {
                            // remis
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaGrupowa[i].dodajPunkty(mecz.PunktyRemis);
                            listaDruzynFazaGrupowa[j].dodajPunkty(mecz.PunktyRemis);
                            listaMeczyFazaGrupowa.Add(mecz);
                            Console.WriteLine($"Remis");
                            pokazMecz(mecz);
                        }
                    }
                }
            }
            else if (faza == "Faza polfinalowa")
            {
                for (int i = 0; i < listaDruzynFazaPolfinalowo.Count; i++)
                {
                    for (int j = i + 1; j < listaDruzynFazaPolfinalowo.Count; j++)
                    {
                        int rezultatMeczu = generujRezultatMeczu();
                        if (rezultatMeczu == 0)
                        {
                            // druzyna i win
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(mecz.PunktyZwyciestwo);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(mecz.PunktyPrzegrana);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaPolfinalowo[i].getNazwa}");
                            listaMeczyFazaPolfinalowa.Add(mecz);
                            pokazMecz(mecz);
                        }
                        else if (rezultatMeczu == 1)
                        {
                            // druzyna j win
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(mecz.PunktyPrzegrana);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(mecz.PunktyZwyciestwo);
                            listaMeczyFazaPolfinalowa.Add(mecz);
                            Console.WriteLine($"Wygrala druzyna {listaDruzynFazaPolfinalowo[j].getNazwa}");
                            pokazMecz(mecz);
                        }
                        else
                        {
                            // remis
                            Mecz mecz = utworzMecz(i, j, faza);
                            listaDruzynFazaPolfinalowo[i].dodajPunkty(mecz.PunktyRemis);
                            listaDruzynFazaPolfinalowo[j].dodajPunkty(mecz.PunktyRemis);
                            listaMeczyFazaPolfinalowa.Add(mecz);
                            Console.WriteLine($"Remis");
                            pokazMecz(mecz);
                        }
                    }
                }
            }
            else if (faza == "Faza finalowa")
            {
                {
                    for (int i = 0; i < listaDruzynFazaFinalowa.Count; i++)
                    {
                        for (int j = i + 1; j < listaDruzynFazaFinalowa.Count; j++)
                        {
                            int rezultatMeczu = generujRezultatMeczu();
                            if (rezultatMeczu == 0)
                            {
                                // druzyna i win
                                Mecz mecz = utworzMecz(i, j, faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(mecz.PunktyZwyciestwo);
                                listaDruzynFazaFinalowa[j].dodajPunkty(mecz.PunktyPrzegrana);
                                Console.WriteLine($"Wygrala druzyna {listaDruzynFazaFinalowa[i].getNazwa}");
                                listaMeczyFazaFinalowa.Add(mecz);
                                pokazMecz(mecz);
                            }
                            else if (rezultatMeczu == 1)
                            {
                                // druzyna j win
                                Mecz mecz = utworzMecz(i, j, faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(mecz.PunktyPrzegrana);
                                listaDruzynFazaFinalowa[j].dodajPunkty(mecz.PunktyZwyciestwo);
                                listaMeczyFazaFinalowa.Add(mecz);
                                Console.WriteLine($"Wygrala druzyna {listaDruzynFazaFinalowa[j].getNazwa}");
                                pokazMecz(mecz);
                            }
                            else
                            {
                                // remis
                                Mecz mecz = utworzMecz(i, j, faza);
                                listaDruzynFazaFinalowa[i].dodajPunkty(mecz.PunktyRemis);
                                listaDruzynFazaFinalowa[j].dodajPunkty(mecz.PunktyRemis);
                                listaMeczyFazaFinalowa.Add(mecz);
                                Console.WriteLine($"Remis");
                                pokazMecz(mecz);
                            }
                        }
                    }
                }
            }
            else
                throw new WrongMatchPhaseException("Blad programu! [ Nieprawidlowa faza meczu ]");
        }
		public void rozegrajMecze()
		{
			Random generator = new Random();

            //
            //          SIATKOWKA
            //

            if (this.typ == "Siatkowka")
            {
                /*
                             
                 MECZE FAZA GRUPOWA + DODAWANIE MECZY DO LIST FAZY GRUPOWEJ
                 
                 */
                Console.WriteLine("Lista meczy w fazie grupowej\n====================================================\n");
                rozegrajMeczeSiatkowkiDanejFazy("Faza grupowa");
                Console.WriteLine("Koniec fazy grupowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyPolfinal();
                /*

                MECZE FAZA POLFINALOWA + DODAWANIE MECZY DO LIST FAZY POLFINALOWA

                */
                Console.WriteLine("Lista meczy w fazie polfinalowej\n====================================================\n");
                rozegrajMeczeSiatkowkiDanejFazy("Faza polfinalowa");
                Console.WriteLine("Koniec fazy polfinalowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyFinal();
                /*

                MECZE FAZA FNALOWA + DODAWANIE MECZY DO LIST FAZY FINALOWA

                */
                Console.WriteLine("Lista meczy w fazie fianlowej\n====================================================\n");
                rozegrajMeczeSiatkowkiDanejFazy("Faza finalowa");
                Console.WriteLine("Koniec fazy finalowej\n=============================");
                Console.ReadKey();
            }

            //
            //          DWA OGNIE
            //

            else if (this.typ == "Dwa_ognie")
            {
                /*
                             
                 MECZE FAZA GRUPOWA + DODAWANIE MECZY DO LIST FAZY GRUPOWEJ
                 
                 */
                Console.WriteLine("Lista meczy w fazie grupowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza grupowa");
                Console.WriteLine("Koniec fazy grupowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyPolfinal();
                /*

                MECZE FAZA POLFINALOWA + DODAWANIE MECZY DO LIST FAZY POLFINALOWA

                */
                Console.WriteLine("Lista meczy w fazie polfinalowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza polfinalowa");
                Console.WriteLine("Koniec fazy polfinalowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyFinal();

                /*

                MECZE FAZA FNALOWA + DODAWANIE MECZY DO LIST FAZY FINALOWA

                */

                Console.WriteLine("Lista meczy w fazie finalowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza finalowa");
                Console.WriteLine("Koniec fazy finalowej\n=============================");
                Console.ReadKey();
                Console.Clear();

            }
            //
            //          PRZECIAGANIE LINY
            //
            else if (this.typ == "Przeciaganie_Liny")
            {
                /*

                    MECZE FAZA GRUPOWA + DODAWANIE MECZY DO LIST FAZY GRUPOWEJ

                    */
                Console.WriteLine("Lista meczy w fazie grupowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza grupowa");
                Console.WriteLine("Koniec fazy grupowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyPolfinal();
                /*

                MECZE FAZA POLFINALOWA + DODAWANIE MECZY DO LIST FAZY POLFINALOWA

                */
                Console.WriteLine("Lista meczy w fazie polfinalowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza polfinalowa");
                Console.WriteLine("Koniec fazy polfinalowej\n=============================");
                Console.ReadKey();
                Console.Clear();
                klasyfikujDruzynyFinal();

                /*

                MECZE FAZA FNALOWA + DODAWANIE MECZY DO LIST FAZY FINALOWA

                */

                Console.WriteLine("Lista meczy w fazie finalowej\n====================================================\n");
                rozegrajMeczeDanejFazy("Faza finalowa");
                Console.WriteLine("Koniec fazy finalowej\n=============================");
                Console.ReadKey();
                Console.Clear();
            }
            okreslLidera();

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
		public void wczytajTurniejPlik(string fname)
        {
            StreamReader file_read;
            try
            {
                file_read = new StreamReader(fname + ".txt");
                string tmp = String.Empty;
                int i = 0;
                while((tmp = file_read.ReadLine()) != null)
                {
                    switch (i)
                    {
                        case 0: // nazwa
                            this.nazwa = tmp;
                            i++;
                            break;
                        case 1: // typ
                            this.typ = tmp;
                            i++;
                            break;
                        case 2: // druzyny
                            string[] words = tmp.Split(' ');
                            foreach (var word in words)
                            {
                                this.listaDruzynFazaGrupowa.Add(new Druzyna(word, 0, this.typ));
                            }
                            i++;
                            break;
                        case 3: // sedzia glowny
                            string[] words_SedziaG = tmp.Split(' ');
                            listaSedziow.Add(new Sedzia(words_SedziaG[0], words_SedziaG[1]));
                            i++;
                            break;
                        case 4: // pomocniczy1 w siatce
                            if (typ == "Siatkowka")
                            {
                                string[] words_SedziaP1 = tmp.Split(' ');
                                this.listaSedziow.Add(new Sedzia(words_SedziaP1[0], words_SedziaP1[1]));
                                i++;
                            }
                            break;
                        case 5: // pomocniczy2 w siatce
                            if (typ == "Siatkowka")
                            {
                                string[] words_SedziaP2 = tmp.Split(' ');
                                this.listaSedziow.Add(new Sedzia(words_SedziaP2[0], words_SedziaP2[1]));
                            }
                            break;
                    }
                }
                file_read.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }
        public void sprawdzTurniejPlik()
        {
            if((typ != "Siatkowka" && typ != "Dwa_ognie" && typ != "Przeciaganie_Liny") || nazwa == "" || listaDruzynFazaGrupowa.Count < 4 || listaSedziow.Count < 0)
            {
                Console.Clear();
                Console.WriteLine("Blad wczytywania z pliku, prawdopodobnie jego zawartosc jest nieprawidlowa!");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

		public void klasyfikujDruzynyPolfinal()
        {
            listaDruzynFazaGrupowa.Sort((x, y) => x.getPunkty.CompareTo(y.getPunkty));
            for (int i = 0; i < 4; i++)
                listaDruzynFazaPolfinalowo.Add(listaDruzynFazaGrupowa[i]);
        }
		public void klasyfikujDruzynyFinal()
        {
            listaDruzynFazaPolfinalowo.Sort((x, y) => x.getPunkty.CompareTo(y.getPunkty));
            for (int i = 0; i < 2; i++)
                listaDruzynFazaFinalowa.Add(listaDruzynFazaPolfinalowo[i]);
        }

        public void okreslLidera()
        {
            if (listaDruzynFazaFinalowa[0].getPunkty > listaDruzynFazaFinalowa[1].getPunkty)
                finalista = listaDruzynFazaFinalowa[0];
            else
                finalista = listaDruzynFazaFinalowa[1];
        }

		public void wypiszTabeleWynikow() {
            Console.Clear();
            Console.WriteLine($"PODSUMOWANIE TURNIEJU\n ** {this.nazwa} **\n\n" +
                $"W turnieju wzielo udzial {listaDruzynFazaGrupowa.Count } druzyn\n" +
                $"Rozegrano {listaMeczyFazaPolfinalowa.Count + listaMeczyFazaGrupowa.Count + listaDruzynFazaFinalowa.Count} meczy\n\n" +
                $"**** LIDER ****\n\n");
            finalista.pokazDruzyne();
        }

	}
}
