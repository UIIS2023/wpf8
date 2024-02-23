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
    /// Interaction logic for PovratKnjiga.xaml
    /// </summary>
    public partial class PovratKnjiga : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id;

        private static string bibliotekar = @"select Ime + ' ' + Prezime as Bibliotekar, BibliotekarID from Bibliotekar";
        private static string knjiga = @"select * from Knjiga";
        private static string clan = @"select ImeClana + ' ' + PrezimeClana as Clan, ClanID from Clan";
        public PovratKnjiga(bool azuriraj, int? id)
        {
            InitializeComponent();
            dpDatumVracanja.Focus();
            PopunjavanjePolja.Popuni(cbBibliotekar, bibliotekar);
            PopunjavanjePolja.Popuni(cbKnjiga, knjiga);
            PopunjavanjePolja.Popuni(cbClan, clan);
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
        }
        public PovratKnjiga()
        {
            InitializeComponent();
            dpDatumVracanja.Focus(); 
            PopunjavanjePolja.Popuni(cbBibliotekar, bibliotekar);
            PopunjavanjePolja.Popuni(cbKnjiga, knjiga);
            PopunjavanjePolja.Popuni(cbClan, clan);
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
                cmd.Parameters.Add("@DatumVracanja", SqlDbType.DateTime).Value = DateTime.Parse(dpDatumVracanja.Text); 
                cmd.Parameters.Add("@BibliotekarID", SqlDbType.Int).Value = (int) cbBibliotekar.SelectedValue;
                cmd.Parameters.Add("@KnjigaID", SqlDbType.Int).Value = (int) cbKnjiga.SelectedValue;
                cmd.Parameters.Add("@ClanID", SqlDbType.Int).Value = (int) cbClan.SelectedValue;


                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = id; 
                    cmd.CommandText = @"update PovratKnjiga
                                        set DatumVracanja = @DatumVracanja,
                                         BibliotekarID = @BibliotekarID,
                                         KnjigaID = @KnjigaID,
                                         ClanID = @ClanID
                                       where PovratKnjigeID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into PovratKnjiga
                                            values(@DatumVracanja, @BibliotekarID, @KnjigaID, @ClanID )";
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

        private void cbKnjiga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbClan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
