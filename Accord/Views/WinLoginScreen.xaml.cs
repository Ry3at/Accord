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
using System.IO;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Accord
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : MetroWindow
    {
        public LoginScreen()
        {
            InitializeComponent();

            //byte[] bytes = File.ReadAllBytes(@"C:\Users\Etern\source\repos\Accord\Accord\Test Songs\1-03 Ezio's Family.mp3");
            //SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=Accord; Integrated Security=True; ");
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "insert into tblTempSong(MID, SongName, SongFile) values (@MID, @SongName, @SongFile)";
            //cmd.Parameters.AddWithValue("@MID", 1);
            //cmd.Parameters.AddWithValue("@SongName", "testname");
            //cmd.Parameters.AddWithValue("@SongFile", bytes);
            //cmd.Connection = sqlCon;
            //sqlCon.Open();
            //cmd.ExecuteNonQuery();
            //sqlCon.Close();

        }

        private async void btnSubmit_ClickAsync(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=Accord; Integrated Security=True; ");
            try
            {
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                //string query = "SELECT COUNT(1) FROM tblUser WHERE Username = @Username AND Password = @Password";

                string storedProcedure = "userLogin";

                SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlCon);

                //sqlCmd.CommandType = System.Data.CommandType.Text;
                //sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                //sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                { 
                    //Get user info from database
                    storedProcedure = "getUserInfo";
                    sqlCmd = new SqlCommand(storedProcedure, sqlCon);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        App._user.UserID = (int)reader["UserID"];
                        App._user.UserName = (string)reader["Username"];
                        App._user.FirstName = (string)reader["FirstName"];
                        App._user.LastName = (string)reader["LastName"];
                        App._user.Email = (string)reader["Email"];
                        App._user.PhoneNumber = (string)reader["PhoneNumber"];
                        App._user.RegistrationDate = (DateTime)reader["RegisteraionDate"];
                        //Profile Image ...
                    }

                // MessageBox.Show($"{App._user.UserID}\n{App._user.UserName}\n{App._user.FirstName}\n{App._user.LastName}\n");
                Accord.Views.WinMain Dashboard = new Views.WinMain();
                Dashboard.Show();
                    this.Close();
                }

                else
                {
                    MetroDialogSettings mds = new MetroDialogSettings();
                    mds.AffirmativeButtonText = "خب";
                    mds.ColorScheme = MetroDialogColorScheme.Inverted;
                    await this.ShowMessageAsync("خطای ورود به برنامه", "نام کاربری یا کلمه عبور، نادرست می باشد. لطفا مجددا امتحان کنید.", MessageDialogStyle.Affirmative, mds);
                    //MessageBox.Show("نام کاربری یا کلمه عبور، نادرست می باشد.\nلطفا مجددا امتحان کنید.", "خطای ورود به برنامه",MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.None, MessageBoxOptions.RightAlign|MessageBoxOptions.RtlReading);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Gold);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.White);
        }
    }
}
