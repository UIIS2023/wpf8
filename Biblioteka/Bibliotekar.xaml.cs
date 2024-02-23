using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for Bibliotekar.xaml
    /// </summary>
    public partial class Bibliotekar : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id;

        public Bibliotekar(bool azuriraj, int? id )
        {
            InitializeComponent();
            txtImeBibliotekar.Focus(); 
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
        }
        public Bibliotekar()
        {
            InitializeComponent();
            txtImeBibliotekar.Focus(); 
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
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtImeBibliotekar.Text; 
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezimeBibliotekar.Text;
                cmd.Parameters.Add("@Jmbg", SqlDbType.VarChar).Value = txtJmbg.Text;
                cmd.Parameters.Add("@AdresaBibliotekara", SqlDbType.NVarChar).Value = txtAdresaBibliotekara.Text;
                cmd.Parameters.Add("@BrojTelefona", SqlDbType.VarChar).Value = txtBrojTelefona.Text;
                cmd.Parameters.Add("@KorisnickoIme", SqlDbType.NVarChar).Value = txtKorisnickoIme.Text;
                cmd.Parameters.Add("@Lozinka", SqlDbType.NVarChar).Value = txtLozinka.Text;

                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = id; 
                    cmd.CommandText = @"update Bibliotekar
                                       set Ime = @Ime,
                                           Prezime = @Prezime,
                                           Jmbg = @Jmbg,
                                           AdresaBibliotekara = @AdresaBibliotekara,
                                           BrojTelefona = @BrojTelefona,
                                           KorisnickoIme = @KorisnickoIme,
                                          Lozinka = @Lozinka 
                                       where BibliotekarID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into Bibliotekar
                                            values(@Ime, @Prezime, @Jmbg, @AdresaBibliotekara, @BrojTelefona, @KorisnickoIme, @Lozinka)";
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
    }
}
