using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for IzdavanjeKnjiga.xaml
    /// </summary>
    public partial class IzdavanjeKnjiga : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id;

        private static string bibliotekar = @"select Ime + ' ' + Prezime as Bibliotekar, BibliotekarID from Bibliotekar";
        private static string clan = @"select ImeClana + ' ' + PrezimeClana as Clan, ClanID from Clan";
        private static string knjiga = @"select * from Knjiga";
        

        public IzdavanjeKnjiga(bool azuriraj, int? id)
        {
            InitializeComponent();
            PopunjavanjePolja.Popuni(cbBibliotekar, bibliotekar);
            PopunjavanjePolja.Popuni(cbClan, clan);
            PopunjavanjePolja.Popuni(cbKnjiga, knjiga);
            dpDatumIzdavanja.Focus(); 
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
        }
        public IzdavanjeKnjiga()
        {
            InitializeComponent();
            dpDatumIzdavanja.Focus(); 
            PopunjavanjePolja.Popuni(cbBibliotekar, bibliotekar);
            PopunjavanjePolja.Popuni(cbClan, clan);
            PopunjavanjePolja.Popuni(cbKnjiga, knjiga);
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@DatumIzdavanja", SqlDbType.DateTime).Value = DateTime.Parse(dpDatumIzdavanja.Text); 
                cmd.Parameters.Add("@BibliotekarID", SqlDbType.Int).Value = cbBibliotekar.SelectedValue;
                cmd.Parameters.Add("@ClanID", SqlDbType.Int).Value =  cbClan.SelectedValue;
                cmd.Parameters.Add("@KnjigaID", SqlDbType.Int).Value =  cbKnjiga.SelectedValue;

                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value =id; 
                    cmd.CommandText = @"update IzdavanjeKnjige
                                        set DatumIzdavanja = @DatumIzdavanja,
                                            BibliotekarID = @BibliotekarID,
                                            ClanID = @ClanID,
                                            KnjigaID = @KnjigaID
                                       where IzdavanjeKnjigeID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into IzdavanjeKnjige
                                            values(@DatumIzdavanja, @BibliotekarID, @ClanID, @KnjigaID)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Unos odredjenih vrednosti nije validan: {ex.Message} ", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }

            }
        }
        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbBibliotekar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbClan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbKnjiga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
