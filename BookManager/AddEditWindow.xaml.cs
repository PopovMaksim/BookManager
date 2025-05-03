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

namespace BookManager
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
		bool isAdd;
		string purpose;
        public AddEditWindow(bool isAdd)
        {
			this.isAdd = isAdd;
			if (isAdd)
			{
				purpose = "Додати";
				Title = "Додати книгу";
			}
			else
			{
				purpose = "Зберегти";
				Title = "Редагувати книгу";
			}
			InitializeComponent();

			AddEditButton.Content = purpose;
		}
    }
}
