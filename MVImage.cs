using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.CA.Scripting;

namespace PDExper
{
    public class MVImage
    {
        public string ImageID { get; set; }
        public double[,] Array { get => GetCUMatrix(Frame);  }
        public Tuple<int,int> XY { get; set; }
        public Frame Frame { get; set; }
        public string Log { get; set; }
        public string PlanId { get; set; }

        public string CourseId { get; set; }
        public MVImage(Frame frame, string courseId, string planId)
        {
            ImageID = planId + frame.Image.Id;
            XY = new Tuple<int, int>(frame.XSize, frame.YSize);
            Log = $"Image for {ImageID}";
            Frame = frame;
            PlanId = planId;
            CourseId = courseId;
        }

        public double[,] GetCUMatrix(Frame frame)
        {
            var prealoc = new ushort[frame.XSize, frame.YSize];
            frame.GetVoxels(0, prealoc);
            double[,] dArray = new double[prealoc.GetUpperBound(0) + 1,
                           prealoc.GetUpperBound(1) + 1];
            for (int x = 0; x <= prealoc.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= prealoc.GetUpperBound(1); y++)
                {
                    dArray[x, y] = frame.VoxelToDisplayValue(prealoc[x, y]);
                }
            }
            return dArray;
        }
        public double[,] ConvertArrayObjtoDouble<T>(T[,] array)
        {
            //Create an array of doubles the same size.
            double[,] dArray = new double[((T[,])array).GetUpperBound(0) + 1,
                                          ((T[,])array).GetUpperBound(1) + 1];
            //Cast and fill each item
            for (int x = 0; x <= ((T[,])array).GetUpperBound(0); x++)
            {
                for (int y = 0; y <= ((T[,])array).GetUpperBound(1); y++)
                {
                    dArray[x, y] = Convert.ToDouble(((T[,])array)[x, y]);
                }
            }
            return dArray;
        }
        public void ExportCUImage(string path)
        {
            File.WriteAllText(Path.Combine(path, "identifier" + ".txt"), "Exporting PDs");
            string exportpath = Path.Combine(path, "PD" + ImageID + ".csv");
            string line = "";
            var array = Array;
            File.WriteAllText(exportpath, "");
            for (int y = 0; y <= array.GetUpperBound(1); y++)
            {

                for (int x = 0; x <= array.GetUpperBound(0); x++)
                {
                    line += array[x, y].ToString("F3",System.Globalization.CultureInfo.InvariantCulture) + ",";
                }
                File.AppendAllText(exportpath, "\n" + line);
                line = "";
            }


        }
    }

}
