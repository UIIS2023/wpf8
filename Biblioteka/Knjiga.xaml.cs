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
using System.Runtime.CompilerServices;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for Knjiga.xaml
    /// </summary>
    public partial class Knjiga : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id;

        private static string zanr = @"select * from Zanr";
        private static string izdavac = @"select * from Izdavac";
        private static string autor = @"select ImeAutora + ' ' + PrezimeAutora as Autor, AutorID from Autor"; 


        public Knjiga(bool azuriraj, int? id)
        {
            InitializeComponent();
            PopunjavanjePolja.Popuni(cbZanr, zanr);
            PopunjavanjePolja.Popuni(cbIzdavac, izdavac);
            PopunjavanjePolja.Popuni(cbAutor, autor);
            txtNazivKnjige.Focus(); 
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
           

        }
        public Knjiga()
        {
            InitializeComponent();
            txtNazivKnjige.Focus();
            PopunjavanjePolja.Popuni(cbZanr, zanr);
            PopunjavanjePolja.Popuni(cbIzdavac, izdavac);
            PopunjavanjePolja.Popuni(cbAutor, autor);
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
                cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = txtNazivKnjige.Text; 
                cmd.Parameters.Add("@Dostupnost", SqlDbType.Bit).Value = chbDostupnost.IsChecked; 
                cmd.Parameters.Add("@ZanrID", SqlDbType.Int).Value =  cbZanr.SelectedValue;
                cmd.Parameters.Add("@IzdavacID", SqlDbType.Int).Value = cbIzdavac.SelectedValue;
                cmd.Parameters.Add("@AutorID", SqlDbType.Int).Value = cbAutor.SelectedValue;


                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = id; 
                    cmd.CommandText = @"update Knjiga
                                        set Naziv = @Naziv,
                                             Dostupnost  = @Dostupnost,
                                             ZanrID = @ZanrID,
                                             IzdavacID = @IzdavacID, 
                                             AutorID = @AutorID
                                       where KnjigaID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into Knjiga
                                            values(@Naziv, @Dostupnost, @ZanrID, @IzdavacID, @AutorID)";
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

        private void cbIzdavac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbAutor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbZanr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
