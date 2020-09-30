using PDExper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMS.CA.Scripting;

namespace VMS.DV.PD.Scripting
{
    public class Script
    {
        public Script()
        {
                
        }

        public void Execute(ScriptContext scriptContext, Window window)
        {
            var planPortals = scriptContext.Patient.PDPlanSetups.SelectMany(e => e.Beams);
            var courses = planPortals.Select(e => e.PDPlanSetup.PlanSetup.Course.Id).Distinct();
            var models = planPortals.Select(e => new ShowImageModel(e));
            var vm = new ShowImageViewModel(models, courses);
            var showImg = new ShowImage(vm);
            showImg.Closed += new EventHandler((sender, e) => window.Close());

            window.Content = showImg;

        }
    }
}
