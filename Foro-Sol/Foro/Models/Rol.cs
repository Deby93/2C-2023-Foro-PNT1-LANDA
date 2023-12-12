using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Foro
{
    public class Rol : IdentityRole<int>
    {
        #region Constructores
        public Rol() : base(){ }

        public Rol(string name): base(name){ }
        #endregion
        #region Propiedades
        [Display(Name = Alias.RolName)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        public override string NormalizedName { get => NormalizedName; set => NormalizedName = value; }
    }
    #endregion
}
