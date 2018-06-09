using Accord.Models;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Accord
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User _user = new User();
        App()
        {
            //    byte[] bytes = File.ReadAllBytes(@"D:\Personal\Personal\Music\Sirvan Khosravi - Road Of Dreams\02 Sirvan Khosravi - Inam Migzare.mp3");
            //    SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=Accord; Integrated Security=True; ");
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandText = "insert into tblSong(SongName, SongFile, AddDate) values (@SongName, @SongFile, @AddDate)";
            //    cmd.Parameters.AddWithValue("@SongName", "اینم میگذره");
            //    cmd.Parameters.AddWithValue("@SongFile", bytes);
            //    cmd.Parameters.AddWithValue("@AddDate", DateTime.Now);
            //    cmd.Connection = sqlCon;
            //    sqlCon.Open();
            //    cmd.ExecuteNonQuery();
            //    sqlCon.Close();
        }
    }
}
