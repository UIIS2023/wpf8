using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
    /// Interaction logic for Autor.xaml
    /// </summary>
    public partial class Autor : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private int? id; 
        
        public Autor(bool azuriraj, int? id) 
        {
            InitializeComponent();
            txtImeAutora.Focus();
            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.id = id;
        }
        public Autor()
        {
            InitializeComponent();
            txtImeAutora.Focus(); 
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
                cmd.Parameters.Add("@ImeAutora", SqlDbType.NVarChar).Value = txtImeAutora.Text; 
                cmd.Parameters.Add("@PrezimeAutora", SqlDbType.NVarChar).Value = txtPrezimeAutora.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = id; 
                    cmd.CommandText = @"update Autor
                                       set ImeAutora = @ImeAutora,
                                           PrezimeAutora = @PrezimeAutora
                                       where AutorID = @id";
                    id = null; 
                }
                else
                {
                    cmd.CommandText = @"insert into Autor
                                            values(@ImeAutora, @PrezimeAutora)";
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
