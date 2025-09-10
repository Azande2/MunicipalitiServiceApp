
using System.Collections.Generic;
using System.Windows;
using ProgPart17312.DataStructures;

namespace ProgPart17312
{
    public partial class ViewReportsWindow : Window
    {
        public ViewReportsWindow(List<Report> reports)
        {
            InitializeComponent();
            ReportsGrid.ItemsSource = reports;
        }
    }
}
