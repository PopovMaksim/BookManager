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
using MaterialDesignThemes.Wpf;

namespace BookManager
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            TogglePasswordVisibility.Checked += TogglePasswordVisibility_Checked;
            TogglePasswordVisibility.Unchecked += TogglePasswordVisibility_Unchecked;
            PasswordBoxHidden.PasswordChanged += SyncPasswordBoxes;
            PasswordBoxVisible.TextChanged += SyncPasswordBoxes;
        }

        private void TogglePasswordVisibility_Checked(object sender, RoutedEventArgs e)
        {
            PasswordBoxVisible.Text = PasswordBoxHidden.Password;
            PasswordBoxHidden.Visibility = Visibility.Collapsed;
            PasswordBoxVisible.Visibility = Visibility.Visible;
            EyeIcon.Kind = PackIconKind.EyeOff;
        }

        private void TogglePasswordVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBoxHidden.Password = PasswordBoxVisible.Text;
            PasswordBoxVisible.Visibility = Visibility.Collapsed;
            PasswordBoxHidden.Visibility = Visibility.Visible;
            EyeIcon.Kind = PackIconKind.Eye;
        }

        private void SyncPasswordBoxes(object sender, RoutedEventArgs e)
        {
            if (sender == PasswordBoxHidden && TogglePasswordVisibility.IsChecked == true)
            {
                PasswordBoxVisible.Text = PasswordBoxHidden.Password;
            }
            else if (sender == PasswordBoxVisible && TogglePasswordVisibility.IsChecked == false)
            {
                PasswordBoxHidden.Password = PasswordBoxVisible.Text;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text;
            string password = TogglePasswordVisibility.IsChecked == true ? PasswordBoxVisible.Text : PasswordBoxHidden.Password;

            if (username == "admin" && password == "password")
            {
                MessageBox.Show("Вхід успішний!");
                this.Close();
            }
            else
            {
                Brush redBrush = new SolidColorBrush(Color.FromRgb(255, 102, 102));
                MessageBox.Show("Невірний логін або пароль.");
                PasswordBoxHidden.Background = redBrush;
                PasswordBoxVisible.Background = redBrush;
            }
        }
    }
}
