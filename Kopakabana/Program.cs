using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Kopakabana
{
    class Program
    {
        static void menuWstepne()
        {
            Console.WriteLine("1. Wczytaj turniej z pliku");
            Console.WriteLine("2. Utworz turniej");
            Console.WriteLine("0. Wyjdz");
        }

        static void menuTypTurnieju()
        {
            Console.WriteLine("1. Siatkowka");
            Console.WriteLine("2. Dwa ognie");
            Console.WriteLine("3. Przeciaganie liny");
            Console.WriteLine("0. Wyjdz");
        }

        static void menuGlowne()
        {
            Console.WriteLine("1. Dodaj druzyne");
            Console.WriteLine("2. Usun druzyne");
            Console.WriteLine("3. Wyswietl druzyny");
            Console.WriteLine("4. Dodaj sedziego");
            Console.WriteLine("5. Usun sedziego");
            Console.WriteLine("6. Wyswietl sedziow");
            Console.WriteLine("7. Wystartuj turniej");
            Console.WriteLine("0. Wyjdz z menu");
        }

        static bool regexName(string imie, string nazwisko)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            if (regex.IsMatch(imie) && regex.IsMatch(nazwisko))
                return true;
            else
                return false;
        }

        static void Main()
        {
            /* INICJALIZACJA ZMIENNYCH STARTOWYCH*/

            Turniej turniej = new Turniej();

            string wybor, nazwaTurnieju;
            bool warunkiStartuTurnieju = false;

            menuWstepne();
            int etapWyboru = 0; // 0 - menuWstepne // 1 - menuTypTurnieju // 2 - menuGlowne   
            wybor = Console.ReadLine();

            ////////////////////////////////////////////////////

           
            while (wybor != "0")
            {
                if (etapWyboru == 0)
                {
                    Console.Clear();
                    menuWstepne();
                    try
                    {
                        switch (Int32.Parse(wybor))
                        {
                            case 1:
                                // wczytywanie turnieju z pliku
                                etapWyboru++;
                                break;
                            case 2:
                                // tworzenie turnieju
                                etapWyboru++;
                                break;
                            default:
                                wybor = Console.ReadLine();
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        wybor = Console.ReadLine();
                    }
                }
                else if (etapWyboru == 1)
                {
                    Console.Clear();
                    menuTypTurnieju();
                    wybor = Console.ReadLine();
                    try
                    {
                        switch (Int32.Parse(wybor))
                        {
                            case 1:
                                // typ siatkowka
                                Console.Clear();
                                Console.WriteLine("Podaj nazwe turnieju:");
                                nazwaTurnieju = Console.ReadLine();
                                turniej = new Turniej(nazwaTurnieju, "Siatkowka");
                                etapWyboru++;
                                break;
                            case 2:
                                // typ dwa ognie
                                Console.Clear();
                                Console.WriteLine("Podaj nazwe turnieju:");
                                nazwaTurnieju = Console.ReadLine();
                                turniej = new Turniej(nazwaTurnieju, "Dwa_ognie");
                                etapWyboru++;
                                break;
                            case 3:
                                // typ przeciaganie liny
                                Console.Clear();
                                Console.WriteLine("Podaj nazwe turnieju:");
                                nazwaTurnieju = Console.ReadLine();
                                turniej = new Turniej(nazwaTurnieju, "Przeciaganie_Liny");
                                etapWyboru++;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        menuTypTurnieju();
                    }
                }
                else if (etapWyboru == 2)
                {
                    Console.Clear();
                    menuGlowne();
                    wybor = Console.ReadLine();
                    try
                    {
                        switch (Int32.Parse(wybor))
                        {
                            case 1:
                                // dodaj druzyne
                                Console.Clear();
                                string naz_druzyny = String.Empty, typ = turniej.getTyp;
                                int iloscPunktow = 0;
                                Console.WriteLine("Podaj nazwe druzyny: ");
                                naz_druzyny = Console.ReadLine();
                                Druzyna druzyna = new Druzyna(naz_druzyny, iloscPunktow, typ);
                                turniej.dodajDruzyne(druzyna);
                                break;
                            case 2:
                                // usun druzyne
                                Console.Clear();
                                turniej.wyswietlDruzyny();
                                Console.WriteLine("Podaj nazwe druzyny ktora chcesz usunac: ");
                                naz_druzyny = Console.ReadLine();
                                turniej.usunDruzyne(naz_druzyny);
                                break;
                            case 3:
                                // wyswietl druzyne
                                Console.Clear();
                                turniej.wyswietlDruzyny();
                                Console.WriteLine("Kliknij cokolwiek aby kontynuowac :)");
                                Console.ReadLine();
                                break;
                            case 4:
                                // dodaj sedziego
                                /*przyklad dodawania sedziego z kodu Pawla z moja funkcja regexName*/
                                turniej.dodajSedzieTurniejowego();                          
                                break;
                            case 5:
                                Console.Clear();
                                turniej.usunSedziegoTurniejowego();
                                break;
                            case 6:
                                // wyswietl sedziow
                                //hehe
                                Console.Clear();
                                Console.WriteLine("Lista sedziow");                               
                                turniej.wyswietlSedziowTurniejowych();
                                Console.WriteLine("Wcisnij cokolwiek, aby wyjsc");
                                Console.ReadLine();
                                break;
                            case 7:
                                // wystartuj turniej
                                warunkiStartuTurnieju = turniej.wystartujTurniej();
                                if (warunkiStartuTurnieju == true)
                                    wybor = "0";
                                Console.WriteLine("Wcisninij dowolony przycisk...");
                                Console.ReadLine();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        menuGlowne();
                    }
                }
                else
                    Console.WriteLine("Oszukano zabezpieczenia");
            }
            // rozgrywanie meczy
            turniej.rozegrajMecze();
            turniej.wypiszTabeleWynikow();
        }
    }
}
