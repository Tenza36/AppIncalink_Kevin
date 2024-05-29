using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppIncalink.Models; // Asegúrate de importar el espacio de nombres correcto
namespace AppIncalink.Models
{
    public class nhabitacionModel
    {

        [Required(ErrorMessage = "El numero de habitación es obligatorio")]
        public int id { get; set; }
        [Required(ErrorMessage = "El numero de camas es obligatorio")]
        public int numCamas { get; set; }
        public ICollection<personaModel> Personas { get; set; }
    }
}
