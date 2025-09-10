
using System.Windows.Controls;
using ProgPart17312.DataStructures;

namespace ProgPart17312
{
    public partial class ReportViewerPage : Page
    {
        public ReportViewerPage(StackLogHistory logHistory)
        {
            InitializeComponent();
            LoadReports(logHistory);
        }

        private void LoadReports(StackLogHistory logHistory)
        {
            foreach (var report in logHistory.GetAllLogs())
            {
                lstReports.Items.Add(report);
            }
        }
    }
}
