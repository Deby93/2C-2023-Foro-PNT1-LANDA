using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foro
{
    public class Rol : IdentityRole<int>
    { 
  public Rol() : base()
        {
             
        }

        public Rol(String rolName) : base(rolName)
        {

        }

        [Display(Name=Alias.RolName)]
       public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
