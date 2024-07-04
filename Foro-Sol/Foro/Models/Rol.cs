using Foro.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
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
        public override string NormalizedName {
            get => base.NormalizedName;
            set => base.NormalizedName = value; }
    }
    #endregion
}
