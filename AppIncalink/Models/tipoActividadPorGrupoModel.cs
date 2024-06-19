namespace AppIncalink.Models
{
    public class tipoActividadPorGrupoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreGrupo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Recursos { get; set; }
        public string Responsables { get; set; }
        public string LugarDesde { get; set; }
        public string LugarHacia { get; set; }
        public string Observaciones { get; set; }
        public string tipoActividad { get; set; }
    }
}
