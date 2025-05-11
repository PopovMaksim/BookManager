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

			Globals.connection.Open();
			Brush whiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
			AuthorTextBox.Background = whiteBrush;
			TitleTextBox.Background = whiteBrush;
			YearTextBox.Background = whiteBrush;
			RackTextBox.Background = whiteBrush;
			ShelfTextBox.Background = whiteBrush;


			Brush redBrush = new SolidColorBrush(Color.FromRgb(255, 102, 102));
			bool haveError = false;
			string errors = "";
			if (AuthorTextBox.Text == "")
			{
				errors = errors + "Поле 'Автор' не може бути пустим.\n";
				AuthorTextBox.Background = redBrush;

				haveError = true;
			}
			if (TitleTextBox.Text == "")
			{
				TitleTextBox.Background = redBrush;
				errors = errors + "Поле 'Назва' не може бути пустим.\n";
				haveError = true;
			}
			if (!int.TryParse(YearTextBox.Text, out int year))
			{
				YearTextBox.Background = redBrush;
				errors = errors + "Поле 'Рік' повинно містити число.\n";

				haveError = true;
			}
			if (!int.TryParse(RackTextBox.Text, out int rack))
			{
				RackTextBox.Background = redBrush;
				errors = errors + "Поле 'Стелаж' повинно містити число.\n";

				haveError = true;
			}
			if (!int.TryParse(ShelfTextBox.Text, out int shelf))
			{
				ShelfTextBox.Background = redBrush;
				errors = errors + "Поле 'Полиця' повинно містити число.\n";

				haveError = true;
			}
			if (haveError == true)
			{
				MessageBox.Show(errors, "Помилка введення");
				Globals.connection.Close();
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
			if (isEdit)
			{
				using var cmd = new MySqlCommand(updateQuery, Globals.connection);
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

				using var cmd = new MySqlCommand(insertQuery, Globals.connection);
				cmd.Parameters.AddWithValue("@Author", AuthorTextBox.Text);
				cmd.Parameters.AddWithValue("@Title", TitleTextBox.Text);
				cmd.Parameters.AddWithValue("@Year", int.Parse(YearTextBox.Text));
				cmd.Parameters.AddWithValue("@Rack", int.Parse(RackTextBox.Text));
				cmd.Parameters.AddWithValue("@Shelf", int.Parse(ShelfTextBox.Text));
				cmd.ExecuteNonQuery();
			}
			Globals.connection.Close();
			this.DialogResult = true;
			this.Close();
		}
    }
}
