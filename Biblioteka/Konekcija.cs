using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Biblioteka
{
    internal class Konekcija
    {
        public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-3KD3EVK\SQLEXPRESS02",
                InitialCatalog = "Biblioteka", 
                IntegratedSecurity = true 
            };
            string con = ccnSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}







//naziv lokalnog servera Vašeg računara
//Baza na lokalnom serveru
//koristice se trenutni windows kredencijali za autentifikaciju,
//u slucaju da je false potrebno bi bilo u okviru konekcionog stringa navesti User ID i password