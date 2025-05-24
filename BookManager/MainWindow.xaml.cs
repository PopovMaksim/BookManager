using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
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
using MySql.Data.MySqlClient;

namespace BookManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Book> books = new ObservableCollection<Book>();
        public MainWindow()
        {
			InitializeComponent();
            loadData();
			BooksGataGrid.ItemsSource = books;
		}
		public void loadData()
		{
            books.Clear();
            string connectionString = "server=localhost;port=3306;user=root;password=Hkle4X!5di_3k;database=bookmanager;";
            Globals.connection = new MySqlConnection(connectionString);

            try
            {
				Globals.connection.Open();
                string query = "SELECT * FROM books;";
                using var cmd = new MySqlCommand(query, Globals.connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
					books.Add(new Book()
					{
						code = reader.GetInt32("code"),
						 author=reader.GetString("author"),
						 title=reader.GetString("title"),
						 year=reader.GetInt32("year"),
						 rack=reader.GetInt32("rack"),
						 shelf=reader.GetInt32("shelf")
					}
						);

                }
				Globals.connection.Close();
            }
            catch (MySqlException sqlEx)
            {
                MessageBox.Show("Не вдалось отрмати доступ до бази даних з інформаціє. Перевірте підключення до мережі та перезавантажте додаток");
                Application.Current.Shutdown();
            }
            if (books == null)
            {
                MessageBox.Show("В базі даних відсутня інформація про книги");
            }
        }
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.ShowDialog();
        }

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
            Button button = sender as Button;

            Book selectedBook = button.DataContext as Book;
            AddEditWindow editWindow = new AddEditWindow(selectedBook);
            if (editWindow.ShowDialog() == true)
            {
                loadData();
            }
        }

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{

			AddEditWindow addWindow = new AddEditWindow();
			if (addWindow.ShowDialog() == true)
            {
               loadData();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow();
            if (deleteWindow.ShowDialog() == true)
            {
                Button button = sender as Button;
                Book selectedBook = button.DataContext as Book;
                try
                {
                    Globals.connection.Open();
                    string deleteQuery = "DELETE FROM books WHERE code = @Code";
                    using var command = new MySqlCommand(deleteQuery, Globals.connection);
                    command.Parameters.AddWithValue("@Code", selectedBook.code);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Книгу успішно видалено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("Книгу не знайдено або вже видалена.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
					Globals.connection.Close();
				}
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при видаленні: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
        // Це метод створює новий фільтр
        private void SearchAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Створює контейнер
            StackPanel searchFilterPanel = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 10)
            };

            // Панель з ComboBox та кнопкою ❌
            StackPanel headerPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5)
            };

            // Випадаючий список
            ComboBox comboBox = new()
            {
                Width = 120,
                Margin = new Thickness(0, 0, 5, 0)
            };
            comboBox.Items.Add(new ComboBoxItem { Content = "Автор і назва" });
            comboBox.Items.Add(new ComboBoxItem { Content = "Рік видання" });
            comboBox.SelectedIndex = 0;

			// Кнопка видалення
			Button removeButton = new()
			{
				Background = Brushes.IndianRed,
				Content = "❌",
				Width = 23,
				Height = 23,
				VerticalAlignment = VerticalAlignment.Center,
				ToolTip = "Видалити",
				Padding = new Thickness(0)
			};
            removeButton.Click += (s, args) => SearchFieldsPanel.Children.Remove(searchFilterPanel);

			headerPanel.Children.Add(comboBox);
            headerPanel.Children.Add(removeButton);

            // Панель для введення (TextBox'и)
            StackPanel inputFieldsPanel = new()
            {
                Orientation = Orientation.Vertical
            };

            // Метод оновлення полів під ComboBox
            void UpdateInputFields()
            {
                inputFieldsPanel.Children.Clear();
                string selected = (comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

                if (selected == "Автор і назва")
                {
                    inputFieldsPanel.Children.Add(CreateTextBox("Автор"));
                    inputFieldsPanel.Children.Add(CreateTextBox("Назва"));
                }
                else if (selected == "Рік видання")
                {
                    inputFieldsPanel.Children.Add(CreateTextBox("Рік"));
                }
            }

            comboBox.SelectionChanged += (s, args) => UpdateInputFields();
            UpdateInputFields();

            searchFilterPanel.Children.Add(headerPanel);
            searchFilterPanel.Children.Add(inputFieldsPanel);

            SearchFieldsPanel.Children.Add(searchFilterPanel);
		}

        // Створення текстбокса
        private TextBox CreateTextBox(string placeholder)
        {
            return new TextBox
            {
                Width = 160,
                Height = 23,
                Margin = new Thickness(0, 0, 0, 5),
                ToolTip = placeholder,
                Tag = placeholder,
                VerticalContentAlignment = VerticalAlignment.Center
            };
        }

        private string author = "";
        private string title = "";
        private int? year = null;
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchFieldsPanel.Children.Count == 0)
            {
                //MessageBox.Show($"Не було вибрано ніякого фільтра.");
				BooksGataGrid.ItemsSource = books;

				return;
            }
            author = "";
            title = "";
            year = null;

			ObservableCollection<Book> filtredB = new ObservableCollection<Book>();
			for (int i = 0; i < books.Count(); i++)
			{
				// Зчитування інформації з текстбоксів
				foreach (var child in SearchFieldsPanel.Children)
				{
					if (child is StackPanel filterPanel && filterPanel.Children.Count == 2)
					{
						// 1. headerPanel (ComboBox + кнопка)
						StackPanel headerPanel = filterPanel.Children[0] as StackPanel;
						// 2. inputFieldsPanel (TextBox-и)
						StackPanel inputFieldsPanel = filterPanel.Children[1] as StackPanel;

						if (headerPanel == null || inputFieldsPanel == null)
							continue;

						ComboBox comboBox = headerPanel.Children.OfType<ComboBox>().FirstOrDefault();
						if (comboBox == null)
							continue;

						string selected = (comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

						foreach (var field in inputFieldsPanel.Children)
						{
							if (field is TextBox tb && tb.Tag is string tag)
							{
								string value = tb.Text.Trim();

								if (tag == "Автор")
									author = value;
								else if (tag == "Назва")
								{
									if (value == "")
									{
										MessageBox.Show($"Не було вибрано назву для пошуку.");
										return;
									}
									title = value;
								}

								else if (tag == "Рік")
								{
									if (!(int.TryParse(value, out int parsedYear)))
									{
										MessageBox.Show($"Рік для пошуку введено некоректно.");
										return;
									}
									year = parsedYear;
								}

							}
						}
					}



					// Відбір книг за критеріями


					if (title != null)
					{
						if (books[i].title == title && books[i].author == author)
						{
							filtredB.Add(books[i]);
							continue;
						}
					}
					if (year != null)
					{
						if (books[i].year == year)
						{
							filtredB.Add(books[i]);
							continue;
						}
					}
				}
			}
			BooksGataGrid.ItemsSource = filtredB;
			if (filtredB.Count==0)
				MessageBox.Show($"Не було знайдено книг, що відповідають вибраним критеріям");

		}

    }



	public class Book
	{

		public Book() {}

		public Book(int code) { this.code = code; }
		public  int code { get; set; }
		public string author { get; set; } = "";
		public string title { get; set; } = "";
		public int year { get; set; } = -1;
		public  int rack { get; set; } = -1;
		public  int shelf { get; set; } = -1;
	}

	public class Authorization
	{
		public Authorization()
		{
			authorized = false;
		}
		private bool authorized;
		public bool authorize(string username, string password)
		{
			if (username == "admin" && password == "password")
			{
				authorized = true;
			}
			return authorized;
		}

		public void unauthorize() {
			authorized = false;
		}


	}

	public static class Globals
	{
		public static Authorization mainAuthorization = new Authorization();
		public static ObservableCollection<Book> book_template = new ObservableCollection<Book>();
		public static MySqlConnection connection;
	}




}
