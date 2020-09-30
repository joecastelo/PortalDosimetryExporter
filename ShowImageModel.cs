using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using VMS.CA.Scripting;
using VMS.DV.PD.Scripting;
using Frame = VMS.CA.Scripting.Frame;

namespace PDExper
{
    public class ShowImageModel
    {

        public IEnumerable<MVImage> MVs { get; set; }
        public ShowImageModel(PDBeam pDBeam)
        {
            MVs = pDBeam.PortalDoseImages.SelectMany(e=>e.Image.Frames).Select(x=>new MVImage(x, pDBeam.PDPlanSetup.PlanSetup.Course.Id,
                pDBeam.PDPlanSetup.PlanSetup.Id + pDBeam.Id));

        }

    }
}
