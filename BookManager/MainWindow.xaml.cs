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

			books.Add(new Book { code = "2", author="dsd", title="lkfj", year="jkf", rack="skjdf", shelf="klsj"});
			BooksGataGrid.ItemsSource = books;
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



}
