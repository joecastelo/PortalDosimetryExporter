using Microsoft.WindowsAPICodePack.Dialogs;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace PDExper
{
    /// <summary>
    /// Interação lógica para ShowImage.xam
    /// </summary>
    public partial class ShowImage : UserControl
    {
        public ShowImageViewModel ShowImageViewModel { get; set; }
        public event EventHandler Closed;
        public string Log { get; set; }
        public ShowImage(ShowImageViewModel show)
        {
            InitializeComponent();
            ShowImageViewModel = show;
            DataContext = ShowImageViewModel;
            PortalImages.ItemsSource = ShowImageViewModel.ModelsSource.ToList();
            PortalImages.SelectedItem = ShowImageViewModel.SelectedModel;
        }

        private void PortalImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowImageViewModel.SelectedModel = PortalImages.SelectedItem.ToString();
            ShowImageViewModel.CreatePlotModel();
            OxyView.Model = ShowImageViewModel.ImageModel;
        }

        private void ExportPD_Click(object sender, RoutedEventArgs e)
        {
            string dirToSave = "";
            using (var fbd = new CommonOpenFileDialog())
            {
                fbd.IsFolderPicker = true;
                var result = fbd.ShowDialog();

                if (result == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    
                    dirToSave = fbd.FileName;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Files were not exported");
                }
            }
            try
            {
                Log += "Saving Images";
                var exported = ShowImageViewModel.Models.Where(s => s.CourseId == ShowImageViewModel.SelectedCourse);
                foreach (var model in exported)
                {
                    model.ExportCUImage(dirToSave);
                    Log += $"\n Exporting {model.ImageID}";
                }
                //exported.First().ExportCUImage(dirToSave);
                //System.Windows.Forms.MessageBox.Show($"Images were succesfully saved in {dirToSave}");
            }
            catch (Exception)
            {

                Log+= "Did not export";
            }
        }

        private void Courses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowImageViewModel.SelectedCourse = Courses.SelectedItem.ToString();
            ShowImageViewModel.RefreshImages();
            PortalImages.SelectedItem = ShowImageViewModel.SelectedModel;
            PortalImages.ItemsSource = ShowImageViewModel.ModelsSource;
        }
    }
}
