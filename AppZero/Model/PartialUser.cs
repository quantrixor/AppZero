using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZero.Model
{
    public partial class User
    {
        public string RoleTitle
        {
            get
            {
                return this.SignIn.Rule.Title;
            }
        }
    }
}
