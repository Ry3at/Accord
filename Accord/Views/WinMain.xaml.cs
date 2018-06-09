using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Accord.Views
{
    /// <summary>
    /// Interaction logic for WinMain.xaml
    /// </summary>
    public partial class WinMain
    {
        string tempFilePath;

        public WinMain()
        {
            InitializeComponent();
            userNameField.Text += App._user.UserName;
            registrationDateField.Text += App._user.RegistrationDate.ToString();

            UserProfileViewer.User = App._user;
            Models.AccordEntities DB = new Models.AccordEntities();
            var x = DB.tblTempSongs.FirstOrDefault();
            byte[] tempsong = x.SongFile.ToArray();

            var file = File.OpenWrite("temp.wav");
            file.Write(tempsong, 0, tempsong.Length);
            file.Close();

            me.Source = new Uri("temp.wav", UriKind.Relative);
            me.Play();
            //me.MediaEnded += Me_MediaEnded;
        }

        //private void Me_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    var cu = me.Position;
        //    Models.AccordEntities DB = new Models.AccordEntities();
        //    var x = DB.tblTempSongs.FirstOrDefault();
        //    byte[] tempsong = x.SongFile.ToArray();
        //    me.Close();
        //    var file = File.Open("temp.mp3", FileMode.Append);
        //    if (file.Length != tempsong.Length)
        //        file.Write(tempsong, (int)file.Length, tempsong.Length / 16);
        //    file.Close();

        //    me.Source = new Uri("temp.mp3", UriKind.Relative);

        //    me.Position = cu;
        //    me.Play();
        //}

        ~WinMain()
        {
            File.Delete("temp.wav");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            mainTabControl.SelectedItem = mainTabControl.Items[1];
        }

        private void SettingTabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            fNameField.Text = App._user.FirstName;
            lNameField.Text = App._user.LastName;
            emailField.Text = App._user.Email;
            phoneNumberField.Text = App._user.PhoneNumber;
        }

        private void phoneNumberField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Only Allow numbers in phoneNumberField
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            App._user.FirstName = fNameField.Text;
            App._user.LastName = lNameField.Text;
            App._user.Email = emailField.Text;
            App._user.PhoneNumber = phoneNumberField.Text;

            App._user.updateUserProfile();

            MetroDialogSettings mds = new MetroDialogSettings();
            mds.AffirmativeButtonText = "خب";
            mds.ColorScheme = MetroDialogColorScheme.Inverted;
            await this.ShowMessageAsync("اعمال تغییرات", "جهت مشاهده تغییرات اعمال شده، صفحه تنظیمات را مجددا باز کنید.", MessageDialogStyle.Affirmative, mds);


        }
    }
}
