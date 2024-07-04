
namespace Foro.Models
{
    public class ErrorViewModel
    {
        #region Propiedades
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
     #endregion
    }
}