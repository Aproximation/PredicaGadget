using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKYPE4COMLib;

namespace TriangleOfPower.Code
{
    public class SkypeManager
    {
        public Skype Skype => _skype;

        private Skype _skype = new Skype();

        public SkypeManager()
        {
            _skype.Attach();

        }
    }
}
