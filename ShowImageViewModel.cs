using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.CA.Scripting;
using VMS.DV.PD.Scripting;

namespace PDExper
{
    public class ShowImageViewModel
    {
        public double[,] Array { get; set; }
        public PlotModel ImageModel { get; set; }
        public string SelectedModel { get; set; }

        public IEnumerable<string> Courses { get; set; }
        public string SelectedCourse { get; set; }
        public string Log { get => string.Join("\n", Models.Select(e => e.Log)); }

        public IEnumerable<string> ModelsSource;
        public IEnumerable<MVImage> Models { get; set; }
        public ShowImageViewModel(IEnumerable<ShowImageModel> models, IEnumerable<string> courses)
        {
            Models = models.SelectMany(e=>e.MVs);
            Courses = courses;
            SelectedCourse = Courses.First();
            RefreshImages();
            CreatePlotModel();
        }
        
        public void RefreshImages()
        {
            ModelsSource = Models.Where(s=>s.CourseId == SelectedCourse).Select(e => e.ImageID);
            SelectedModel = Models.First(e=>e.ImageID == ModelsSource.First()).ImageID;
        }
        public void CreatePlotModel()
        {
            var mod = Models.First(e => e.ImageID == SelectedModel);
            Array = mod.Array;

            var model = new PlotModel { Title = "Portal Dosimetry Exporter" };

            // Color axis (the X and Y axes are generated automatically)
            model.Axes.Add(new OxyPlot.Axes.LinearColorAxis
            {
                Palette = OxyPalettes.Jet(100)
            });

            var heatMapSeries = new OxyPlot.Series.HeatMapSeries
            {
                Y0 = 0,
                Y1 = mod.XY.Item2,
                X0= 0,
                X1= mod.XY.Item1,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = Array
            };

            model.Series.Add(heatMapSeries);
            
            ImageModel= model;
        }



    }
}
