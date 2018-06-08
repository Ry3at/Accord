using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private BitmapImage _profileImage;

        public int UserID { get => userID; set => userID = value; }
        public string UserName { get => userName; set => userName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public DateTime RegistrationDate { get => registrationDate; set => registrationDate = value; }

        public BitmapImage ProfileImage
        {
            get => _profileImage;
            set => _profileImage = value;
        }
    }
}
