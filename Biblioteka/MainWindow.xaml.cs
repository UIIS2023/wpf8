using Azure.Core;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    
    public partial class MainWindow : Window
    {
        private string ucitanaTabela; 
        Konekcija kon = new Konekcija(); 
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj = false;
        private DataRowView red;

        #region Select upiti 

        private static string autoriSelect = @"select AutorID as ID, ImeAutora as 'Ime autora', PrezimeAutora as 'Prezime autora' from Autor";
        private static string bibliotekariSelect = @"select BibliotekarID as ID, Ime as 'Ime bibliotekara', Prezime as 'Prezime bibliotekara' , JMBG, 
                                                            AdresaBibliotekara as 'Adresa bibliotkara', BrojTelefona as 'Broj telefona', 
                                                            KorisnickoIme as 'Korisnicko ime', Lozinka as Lozinka from Bibliotekar";
        private static string clanoviSelect = @"select ClanID as ID, ImeClana as ime, PrezimeClana as 'Prezime clana', AdresaClana as Adresa, 
                                                        BrojTelefonaClana as 'Broj telefona' from Clan";
        private static string izdavaciSelect = @"select IzdavacID as ID, NazivIzdavaca as 'Naziv izdavaca' from Izdavac";
        private static string zanroviSelect = @"select ZanrID as ID, NazivZanra as 'Naziv zanra' from Zanr";
        private static string knjigeSelect = @"select KnjigaID as ID, Naziv as 'Naziv knjige', Dostupnost as 'Dostupnost knjige', NazivZanra as Zanr,
                                                         NazivIzdavaca as Izdavac, ImeAutora +' ' + PrezimeAutora as Autor
                                              from Knjiga join Zanr on Knjiga.ZanrID= Zanr.ZanrID
                                                          join Izdavac on Knjiga.IzdavacID= Izdavac.IzdavacID
                                                          join Autor on Knjiga.AutorID = Autor.AutorID";
        private static string izdavanjeKnjigaSelect = @"select IzdavanjeKnjigeID as ID, Ime + ' ' + Prezime as Bibliotekar,
                                                                ImeClana + ' ' + PrezimeClana as Clan,
                                                                    Naziv as Knjiga,DatumIzdavanja as 'Datum izdavanja'
                                                        from IzdavanjeKnjige join Bibliotekar on IzdavanjeKnjige.BibliotekarID = Bibliotekar.BibliotekarID
                                                                             join Clan on IzdavanjeKnjige.ClanID = Clan.ClanID
                                                                             join Knjiga on IzdavanjeKnjige.KnjigaID = Knjiga.KnjigaID";
        private static string povratKnjigaSelect = @"select PovratKnjigeID as ID, Ime + ' ' + Prezime as Bibliotekar,
                                                                                   ImeClana + ' ' + PrezimeClana as Clan, 
                                                                                    Naziv as Knjiga, DatumVracanja as 'Datum vracanja'
                                                     from PovratKnjiga join Bibliotekar on PovratKnjiga.BibliotekarID = Bibliotekar.BibliotekarID
                                                                       join Knjiga on PovratKnjiga.KnjigaID = Knjiga.KnjigaID
                                                                       join Clan on PovratKnjiga.ClanID = Clan.ClanID";
        #endregion

        #region Select sa uslovom
        private static string selectUsloviAutori = @"select * from Autor where AutorID=";
        private static string selectUsloviBibliotekari = @"select * from Bibliotekar where BibliotekarID=";
        private static string selectUsloviClanovi = @"select * from Clan where ClanID=";
        private static string selectUsloviIzdavaci = @"select * from Izdavac where IzdavacID=";
        private static string selectUsloviZanrovi = @"select * from Zanr where ZanrID=";
        private static string selectUsloviKnjige = @"select * from Knjiga where KnjigaID=";
        private static string selectUsloviIzdavanjeKnjiga = @"select * from IzdavanjeKnjige where IzdavanjeKnjigeID= ";
        private static string selectUsloviPovratKnjiga = @"select * from PovratKnjiga where PovratKnjigeID=";
        #endregion

        #region Delete naredbe
        private static string autoriDelete = @"delete from Autor where AutorID=";
        private static string bibliotekariDelete = @"delete  from Bibliotekar where BibliotekarID=";
        private static string clanoviDelete = @"delete from Clan where ClanID=";
        private static string izdavaciDelete = @"delete  from Izdavac where IzdavacID=";
        private static string zanroviDelete = @"delete from Zanr where ZanrID=";
        private static string knjigeDelete = @"delete from Knjiga where KnjigaID=";
        private static string izdavanjeKnjigaDelete = @"delete from IzdavanjeKnjige where IzdavanjeKnjigeID= ";
        private static string povratKnjigaDelete = @"delete from PovratKnjiga where PovratKnjigeID=";

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(bibliotekariSelect);
        }

        private void UcitajPodatke(string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija); 
                DataTable dataTable = new DataTable(); 
                dataAdapter.Fill(dataTable); 
                if (dataGridCentralni != null)
                {
                    dataGridCentralni.ItemsSource = dataTable.DefaultView;
                }
                ucitanaTabela = selectUpit; 
                dataAdapter.Dispose();
                dataTable.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Neuspesno ucitani podaci:{ex.Message}", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnBibliotekari_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(bibliotekariSelect);
        }

        private void btnClanovi_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(clanoviSelect);
        }

        private void btnKnjige_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(knjigeSelect);
        }

        private void btnAutori_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(autoriSelect);
        }
        private void btnZanrovi_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(zanroviSelect);
        }

        private void btnIzdavanjeKnjiga_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(izdavanjeKnjigaSelect);
        }

        private void btnVracanjeKnjiga_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(povratKnjigaSelect);
        }
        private void btnIzdavac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(izdavaciSelect);
        }
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(bibliotekariSelect))
            {
                prozor = new Bibliotekar();
                prozor.ShowDialog();
                UcitajPodatke(bibliotekariSelect);
            }
            else if (ucitanaTabela.Equals(clanoviSelect))
            {
                prozor = new Clan();
                prozor.ShowDialog();
                UcitajPodatke(clanoviSelect);
            }
            else if (ucitanaTabela.Equals(autoriSelect))
            {
                prozor = new Autor();
                prozor.ShowDialog();
                UcitajPodatke(autoriSelect);
            }
            else if (ucitanaTabela.Equals(izdavaciSelect))
            {
                prozor = new Izdavac();
                prozor.ShowDialog();
                UcitajPodatke(izdavaciSelect);
            }
            else if (ucitanaTabela.Equals(zanroviSelect))
            {
                prozor = new Zanr();
                prozor.ShowDialog();
                UcitajPodatke(zanroviSelect);
            }
            else if (ucitanaTabela.Equals(knjigeSelect))
            {
                prozor = new Knjiga();
                prozor.ShowDialog();
                UcitajPodatke(knjigeSelect);
            }
            else if (ucitanaTabela.Equals(izdavanjeKnjigaSelect))
            {
                prozor = new IzdavanjeKnjiga();
                prozor.ShowDialog();
                UcitajPodatke(izdavanjeKnjigaSelect);
            }
            else if (ucitanaTabela.Equals(povratKnjigaSelect))
            {
                prozor = new PovratKnjiga();
                prozor.ShowDialog();
                UcitajPodatke(povratKnjigaSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCentralni.SelectedItems.Count == 1)
            {
                if (ucitanaTabela.Equals(bibliotekariSelect))
                {
                    PopuniFormu(selectUsloviBibliotekari);
                    UcitajPodatke(bibliotekariSelect);
                }
                else if (ucitanaTabela.Equals(clanoviSelect))
                {
                    PopuniFormu(selectUsloviClanovi);
                    UcitajPodatke(clanoviSelect);
                }
                else if (ucitanaTabela.Equals(autoriSelect))
                {
                    PopuniFormu(selectUsloviAutori);
                    UcitajPodatke(autoriSelect);
                }
                else if (ucitanaTabela.Equals(izdavaciSelect))
                {
                    PopuniFormu(selectUsloviIzdavaci);
                    UcitajPodatke(izdavaciSelect);
                }
                else if (ucitanaTabela.Equals(zanroviSelect))
                {
                    PopuniFormu(selectUsloviZanrovi);
                    UcitajPodatke(zanroviSelect);
                }
                else if (ucitanaTabela.Equals(knjigeSelect))
                {
                    PopuniFormu(selectUsloviKnjige);
                    UcitajPodatke(knjigeSelect);
                }
                else if (ucitanaTabela.Equals(izdavanjeKnjigaSelect))
                {
                    PopuniFormu(selectUsloviIzdavanjeKnjiga);
                    UcitajPodatke(izdavanjeKnjigaSelect);
                }
                else if (ucitanaTabela.Equals(povratKnjigaSelect))
                {
                    PopuniFormu(selectUsloviPovratKnjiga);
                    UcitajPodatke(povratKnjigaSelect);
                }
            }
            else
            {
                MessageBox.Show("Morate selektovati red koji želite da izmenite!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopuniFormu(string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                var selectedRow = (DataRowView)dataGridCentralni.SelectedItem;
                object a = selectedRow.Row.ItemArray[0];
                int? id = (int?)a;
                SqlCommand cmd = new SqlCommand { Connection = konekcija };

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id ;

                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();

                if(citac.Read())
                {
                    if (ucitanaTabela.Equals(bibliotekariSelect))
                    {
                        Bibliotekar prozorBibliotekar = new Bibliotekar(azuriraj, id);
                        prozorBibliotekar.txtImeBibliotekar.Text = citac["Ime"].ToString();
                        prozorBibliotekar.txtPrezimeBibliotekar.Text = citac["Prezime"].ToString();
                        prozorBibliotekar.txtJmbg.Text = citac["JMBG"].ToString();
                        prozorBibliotekar.txtAdresaBibliotekara.Text = citac["AdresaBibliotekara"].ToString();
                        prozorBibliotekar.txtBrojTelefona.Text = citac["BrojTelefona"].ToString();
                        prozorBibliotekar.txtKorisnickoIme.Text = citac["KorisnickoIme"].ToString();
                        prozorBibliotekar.txtLozinka.Text = citac["Lozinka"].ToString();
                        prozorBibliotekar.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(clanoviSelect))
                    {
                        Clan prozorClan = new Clan(azuriraj, id);
                        prozorClan.txtImeClana.Text = citac["ImeClana"].ToString();
                        prozorClan.txtPrezimeClana.Text = citac["PrezimeClana"].ToString();
                        prozorClan.txtAdresaClana.Text = citac["AdresaClana"].ToString();
                        prozorClan.txtBrojTelefona.Text = citac["BrojTelefonaClana"].ToString();
                        prozorClan.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(knjigeSelect))
                    {
                        Knjiga prozorKnjiga = new Knjiga(azuriraj, id);
                        prozorKnjiga.txtNazivKnjige.Text = citac["Naziv"].ToString();
                        prozorKnjiga.chbDostupnost.IsChecked = (bool)citac["Dostupnost"];
                        prozorKnjiga.cbZanr.SelectedValue = citac["ZanrID"];
                        prozorKnjiga.cbIzdavac.SelectedValue = citac["IzdavacID"];
                        prozorKnjiga.cbAutor.SelectedValue = citac["AutorID"];
                        prozorKnjiga.ShowDialog();
                    }
                    else if(ucitanaTabela.Equals(autoriSelect))
                    {
                        Autor prozorAutor = new Autor(azuriraj, id);
                        prozorAutor.txtImeAutora.Text = citac["ImeAutora"].ToString();
                        prozorAutor.txtPrezimeAutora.Text = citac["PrezimeAutora"].ToString();
                        prozorAutor.ShowDialog();
                    }
                    else if(ucitanaTabela.Equals(zanroviSelect))
                    {
                        Zanr prozorZanr = new Zanr(azuriraj, id);
                        prozorZanr.txtNazivZanra.Text = citac["NazivZanra"].ToString();
                        prozorZanr.ShowDialog();
                    }
                    else if(ucitanaTabela.Equals(izdavaciSelect))
                    {
                        Izdavac prozorIzdavac = new Izdavac(azuriraj, id);
                        prozorIzdavac.txtNazivIzdavaca.Text = citac["NazivIzdavaca"].ToString();
                        prozorIzdavac.ShowDialog();
                    }
                    else if(ucitanaTabela.Equals(izdavanjeKnjigaSelect))
                    {
                        IzdavanjeKnjiga prozorIzdavanjeKnjiga = new IzdavanjeKnjiga(azuriraj, id);
                        prozorIzdavanjeKnjiga.dpDatumIzdavanja.SelectedDate = (DateTime)citac["DatumIzdavanja"];
                        prozorIzdavanjeKnjiga.cbBibliotekar.SelectedValue = citac["BibliotekarID"];
                        prozorIzdavanjeKnjiga.cbClan.SelectedValue = citac["ClanID"];
                        prozorIzdavanjeKnjiga.cbKnjiga.SelectedValue = citac["KnjigaID"];
                        prozorIzdavanjeKnjiga.ShowDialog();
                    }
                    else if(ucitanaTabela.Equals(povratKnjigaSelect))
                    {
                        PovratKnjiga prozorPovratKnjiga = new PovratKnjiga(azuriraj, id);
                        prozorPovratKnjiga.dpDatumVracanja.SelectedDate = (DateTime)citac["DatumVracanja"];
                        prozorPovratKnjiga.cbBibliotekar.SelectedValue = citac["BibliotekarID"];
                        prozorPovratKnjiga.cbKnjiga.SelectedValue = citac["KnjigaID"];
                        prozorPovratKnjiga.cbClan.SelectedValue = citac["ClanID"];
                        prozorPovratKnjiga.ShowDialog();
                    }
                    citac.Close();
                    cmd.Dispose();
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Greška prilikom popunjavanja forme.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void Obrisi(string uslov)
        {
            try
            {
                konekcija.Open();
                var selectedRow = (DataRowView)dataGridCentralni.SelectedItem;
                object a = selectedRow.Row.ItemArray[0]; 
                int? id = (int?)a;
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.CommandText = uslov + "@id";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {

                MessageBox.Show("Ne možete obrisati element koji se koristi u drugoj tabeli kao strani ključ! ", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)

                    konekcija.Close();
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {

            if (dataGridCentralni.SelectedItems.Count == 1)
            {
                MessageBoxResult pitanje = MessageBox.Show("Da li ste sigurni da želite da obrišete?", "Provera", MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (pitanje == MessageBoxResult.Yes)
                {
                    if (ucitanaTabela.Equals(bibliotekariSelect))
                    {
                        Obrisi(bibliotekariDelete);
                        UcitajPodatke(bibliotekariSelect);
                    }
                    else if (ucitanaTabela.Equals(clanoviSelect))
                    {
                        Obrisi(clanoviDelete);
                        UcitajPodatke(clanoviSelect);
                    }
                    else if (ucitanaTabela.Equals(knjigeSelect))
                    {
                        Obrisi(knjigeDelete);
                        UcitajPodatke(knjigeSelect);
                    }
                    else if (ucitanaTabela.Equals(autoriSelect))
                    {
                        Obrisi(autoriDelete);
                        UcitajPodatke(autoriSelect);
                    }
                    else if (ucitanaTabela.Equals(zanroviSelect))
                    {
                        Obrisi(zanroviDelete);
                        UcitajPodatke(zanroviSelect);
                    }
                    else if (ucitanaTabela.Equals(izdavaciSelect))
                    {
                        Obrisi(izdavaciDelete);
                        UcitajPodatke(izdavaciSelect);
                    }
                    else if (ucitanaTabela.Equals(izdavanjeKnjigaSelect))
                    {
                        Obrisi(izdavanjeKnjigaDelete);
                        UcitajPodatke(izdavanjeKnjigaSelect);
                    }
                    else if (ucitanaTabela.Equals(povratKnjigaSelect))
                    {
                        Obrisi(povratKnjigaDelete);
                        UcitajPodatke(povratKnjigaSelect);
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate selektovati red koji želite da izmenite", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
            
    }
}
