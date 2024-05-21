using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppIncalink.Models
{
    public class personaModel
    {
        
        public int id { get; set; }
        [Required(ErrorMessage = "El nombre completo de la persona es obligatorio")]
        public string nombreCompleto { get; set; }
        public int idSexo { get; set; }
        [Required(ErrorMessage = "El documento id de la persona es obligatorio")]
        public string documentoId { get; set; }
        [Required(ErrorMessage = "El correo de la persona es obligatorio")]
        public string correo { get; set; }
        [Required(ErrorMessage = "El telefono completo de la persona es obligatorio")]
        public string telefono { get; set; }
        [Required(ErrorMessage = "La Alergia de la persona es obligatorio")]
        public string alergiaAlimentacion { get; set; }
        [Required(ErrorMessage = "La  Alergia de la persona es obligatorio")]
        public string alegiaVarias { get; set; }
        [Required(ErrorMessage = "Si no tiene alguna observación poner (N/A)")]
        public string observaciones { get; set; }
        public int? idGrupo { get; set; }
        public int? idHabitacion { get; set; } 
        public int? idRol { get; set; } 
   
    }
}
