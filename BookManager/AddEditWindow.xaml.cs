using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookManager
{
    public partial class AddEditWindow : Window
    {
        private Book selectedBook;
        private bool isEdit;

        public AddEditWindow(Book selectedBook)
        {
            InitializeComponent();
            this.selectedBook = selectedBook;
            isEdit = true;
            Title = "Редагувати книгу";

            CodeTextBlock.Text = selectedBook.code.ToString();
            AuthorTextBox.Text = selectedBook.author;
            TitleTextBox.Text = selectedBook.title;
            YearTextBox.Text = selectedBook.year.ToString();
            RackTextBox.Text = selectedBook.rack.ToString();
            ShelfTextBox.Text = selectedBook.shelf.ToString();
        }

        public AddEditWindow()
        {
            InitializeComponent();
            isEdit = false;
            Title = "Додати книгу";
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "server=localhost;port=3306;user=root;password=Hkle4X!5di_3k;database=bookmanager;";
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            if (isEdit)
            {
                Brush redBrush = new SolidColorBrush(Color.FromRgb(255, 102, 102));
                bool haveError = false;
                if (AuthorTextBox.Text == "")
                {
                    MessageBox.Show("Поле 'Автор' не може бути пустим.", "Помилка введення");
                    AuthorTextBox.Background = redBrush;
                    haveError = true;
                }
                if (TitleTextBox.Text == "")
                {
                    MessageBox.Show("Поле 'Назва' не може бути пустим.", "Помилка введення");
                    TitleTextBox.Background = redBrush;
                    haveError = true;
                }
                if (!int.TryParse(YearTextBox.Text, out int year))
                {
                    MessageBox.Show("Поле 'Рік' повинно містити число.", "Помилка введення");
                    YearTextBox.Background = redBrush;
                    haveError = true;
                }
                if (!int.TryParse(RackTextBox.Text, out int rack))
                {
                    MessageBox.Show("Поле 'Стелаж' повинно містити число.", "Помилка введення");
                    RackTextBox.Background = redBrush;
                    haveError = true;
                }

                if (!int.TryParse(ShelfTextBox.Text, out int shelf))
                {
                    MessageBox.Show("Поле 'Полиця' повинно містити число.", "Помилка введення");
                    ShelfTextBox.Background = redBrush;
                    haveError = true;
                }
                if(haveError == true)
                {
                    return;
                }
                string updateQuery = @"
                    UPDATE books 
                    SET author = @Author,
                        title = @Title,
                        year = @Year,
                        rack = @Rack,
                        shelf = @Shelf
                    WHERE code = @Code";

                using var cmd = new MySqlCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("@Author", AuthorTextBox.Text);
                cmd.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                cmd.Parameters.AddWithValue("@Year", int.Parse(YearTextBox.Text));
                cmd.Parameters.AddWithValue("@Rack", int.Parse(RackTextBox.Text));
                cmd.Parameters.AddWithValue("@Shelf", int.Parse(ShelfTextBox.Text));
                cmd.Parameters.AddWithValue("@Code", selectedBook.code);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string insertQuery = @"
                    INSERT INTO books (author, title, year, rack, shelf)
                    VALUES (@Author, @Title, @Year, @Rack, @Shelf)";

                using var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@Author", AuthorTextBox.Text);
                cmd.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                cmd.Parameters.AddWithValue("@Year", int.Parse(YearTextBox.Text));
                cmd.Parameters.AddWithValue("@Rack", int.Parse(RackTextBox.Text));
                cmd.Parameters.AddWithValue("@Shelf", int.Parse(ShelfTextBox.Text));
                cmd.ExecuteNonQuery();
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
