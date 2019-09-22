using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.Models
{
    public class Controllers
    {
        public List<ControllerInfo> SavedControllers { get; set; }
    }

    public class ControllerInfo
    {
        public string ConfigurationClass { get; set; }
        public string ControllerClass { get; set; }
        public ControllerType Type { get; set; }
        public string FriendlyName { get; set; }
    }

    public enum ControllerType
    {
        SlimDX,
        USB,
        Other
    }
}
