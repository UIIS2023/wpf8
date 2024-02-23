using System;
using System.Collections.Generic;
using System.Data;
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

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for Zanr.xaml
    /// </summary>
    public partial class Zanr : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id;
        public Zanr(bool azuriraj, int? id)
        {
            InitializeComponent();
            txtNazivZanra.Focus(); 
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
        }

        public Zanr()
        {
            InitializeComponent();
            txtNazivZanra.Focus(); 
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
                cmd.Parameters.Add("@NazivZanra", SqlDbType.NVarChar).Value = txtNazivZanra.Text; 
              
                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = id;
                    cmd.CommandText = @"update Zanr
                                       set NazivZanra = @NazivZanra
                                       where ZanrID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into Zanr
                                            values(@NazivZanra)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan:{ex.Message} ", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
