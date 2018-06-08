using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Accord.Models;

namespace Accord.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : UserControl
    {
        private User _user;

        private User User
        {
            get => _user;
            set
            {
                _user = value;
                TbName.Text = value.FirstName;
                Image.Source = value.ProfileImage;
            }
        }

        public UserProfile()
        {
            InitializeComponent();
        }

        public UserProfile(User user) : this()
        {
            _user = user;
        }
    }
}
