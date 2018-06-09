using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using NHotkey;
using NHotkey.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
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
        Timer Timer { get; set; }
        public WinMain()
        {
            InitializeComponent();

            //try
            //{
            //    HotkeyManager.Current.AddOrReplace("Pp", Key.Space, ModifierKeys.None, OnPp);
            //}
            //catch (Exception err)
            //{

            //}

            userNameField.Text += App._user.UserName;
            registrationDateField.Text += App._user.RegistrationDate.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("fa-IR"));
            FillDataGrid();

            Timer = new Timer(1)
            {
                AutoReset = true
            };
            Timer.Elapsed += Timer_Elapsed;


            UserProfileViewer.User = App._user;

            me.MediaEnded += Me_MediaEnded;
        }

        //private void OnPp(object sender, HotkeyEventArgs e)
        //{
        //    BtnPp_Click(null, null);
        //}


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AudioSlider.ValueChanged -= AudioSlider_ValueChanged;
            try
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        AudioSlider.Value = (1d * me.Position.Ticks / me.NaturalDuration.TimeSpan.Ticks) * 100;
                        CuTime.Text = me.Position.ToString(@"mm\:ss");
                        if (me.NaturalDuration.HasTimeSpan)
                            DuTime.Text = me.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                    }
                    catch { }
                });
            }
            catch
            {

            }
            AudioSlider.ValueChanged += AudioSlider_ValueChanged;
        }

        private void FillDataGrid()
        {
            string CmdString = string.Empty;
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=Accord; Integrated Security=True; ");
            sqlCon.Open();
            CmdString = @"SELECT TOP (10) [Id]
                        ,[SongName]
                        ,[ArtistName]
                        ,[AddDate]
                        FROM[Accord].[dbo].[tblSong]";
            SqlCommand cmd = new SqlCommand(CmdString, sqlCon);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("latestSongs");
            sda.Fill(dt);
            latestSongs.ItemsSource = dt.DefaultView;
            sqlCon.Close();
        }

        private void Me_MediaEnded(object sender, RoutedEventArgs e)
        {
            iconPp.Kind = PackIconMaterialKind.Play;
            AudioSlider.Value = 0;
            me.Stop();
        }

        ~WinMain()
        {
            File.Delete("temp.mp3");
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

            //entity

            if (!(String.IsNullOrEmpty(profPath)))
            {
                var db = new Models.AccordEntities();

                var user = db.tblUsers.FirstOrDefault(u => App._user.UserID == u.UserID);
                user.ProfileImage = File.ReadAllBytes(profPath);

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            await this.ShowMessageAsync("اعمال تغییرات", "جهت مشاهده تغییرات اعمال شده، صفحه تنظیمات را مجددا باز کنید.", MessageDialogStyle.Affirmative, mds);
            App._user.updateUserProfile();
            UserProfileViewer.User = App._user;
        }

        private void BtnPp_Click(object sender, RoutedEventArgs e)
        {
            if (iconPp.Kind == PackIconMaterialKind.Pause)
            {
                iconPp.Kind = PackIconMaterialKind.Play;
                me.Pause();
            }
            else if (iconPp.Kind == PackIconMaterialKind.Play)
            {
                iconPp.Kind = PackIconMaterialKind.Pause;
                me.Play();
            }
        }

        private void AudioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (me.NaturalDuration.HasTimeSpan)
            {
                var dur = me.NaturalDuration.TimeSpan;
                var place = (long)(dur.Ticks * (e.NewValue / 100));
                var ts = new TimeSpan(place);
                me.Position = ts;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var i = latestSongs.SelectedItem as DataRowView;
            var songId = (int)i.Row[0];
            PlaySong(songId);
        }

        private void PlaySong(int id)
        {
            me.Stop();
            me.Close();

            Models.AccordEntities DB = new Models.AccordEntities();
            var x = DB.tblSongs.FirstOrDefault(s => s.Id == id);
            byte[] tempsong = x.SongFile.ToArray();

            TbSongName.Text = x.SongName;
            TbArtistName.Text = x.ArtistName;

            if (File.Exists("temp.mp3"))
                File.Delete("temp.mp3");
            var file = File.OpenWrite("temp.mp3");
            file.Write(tempsong, 0, tempsong.Length);
            file.Close();

            me.Source = new Uri("temp.mp3", UriKind.Relative);
            me.Play();
            Timer.Start();
        }

        private string profPath = "";
        private void profilePicAddress_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image Files(*.BMP; *.JPG; *.JPEG; *.GIF; *.PNG)| *.BMP; *.JPG; *JPEG; *.GIF; *.PNG"
            };
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var tump = new BitmapImage();
                var mem = new MemoryStream();
                var f = File.OpenRead(dialog.FileName);
                f.CopyTo(mem);
                f.Close();
                mem.Position = 0;
                tump.BeginInit();
                tump.StreamSource = mem;
                tump.EndInit();
                ProfileThumb.Source = tump;
                profPath = dialog.FileName;
            }
        }
    }
}
