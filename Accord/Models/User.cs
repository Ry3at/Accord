using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Accord.Models
{
    public class User
    {
        private int userID;
        private string userName;
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private DateTime registrationDate;
        private BitmapImage profileImage;

        public int UserID { get => userID; set => userID = value; }
        public string UserName { get => userName; set => userName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public DateTime RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public BitmapImage ProfileImage
        {
            get
            {   
                return profileImage ?? new BitmapImage(new Uri(@"\Accord;component\Asset\Image\avatar615_0_NEW.png", UriKind.Relative));
            }
            set => profileImage = value;
        }

        public async void updateUserProfile()
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=Accord; Integrated Security=True; ");
            try
            {
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                string storedProcedure = "updateUserProfile";

                SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlCon);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@userID", App._user.userID);
                sqlCmd.Parameters.AddWithValue("@firstName", App._user.firstName);
                sqlCmd.Parameters.AddWithValue("@lastName", App._user.lastName);
                sqlCmd.Parameters.AddWithValue("@email", App._user.email);
                sqlCmd.Parameters.AddWithValue("@phoneNumber", App._user.phoneNumber);

                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
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
    }
}
