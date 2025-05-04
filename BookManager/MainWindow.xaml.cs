using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

		public MainWindow()
        {
			InitializeComponent();
			
			ObservableCollection<Book> books = new ObservableCollection<Book>();

			books.Add(new Book { code = "test", author="test", title="test", year= "test", rack= "test", shelf= "test" });
			BooksGataGrid.ItemsSource = books;



		}
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.ShowDialog();
        }

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			AddEditWindow editWindow = new AddEditWindow(false);
			editWindow.ShowDialog();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddEditWindow addWindow = new AddEditWindow(true);
			addWindow.ShowDialog();
		}
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow();
            deleteWindow.ShowDialog();
        }
    }



	public class Book {
		public required string code { get; set; }
		public required string author { get; set; }
		public required string title { get; set; }
		public required string year { get; set; }
		public required string rack { get; set; }
		public required string shelf { get; set; }
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
		
	}




}
