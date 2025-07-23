namespace ApiUniRoom.Models
{
    public class CuartoRequest
    {
        public string CodCuarto { get; set; }
        public int Capacidad { get; set; }
        public string Descripcion { get; set; }
        public int idPension_fk { get; set; }
    }

    public class CuartoUpdateRequest
    {
        public int idCuarto { get; set; }
        public string CodCuarto { get; set; }
        public int Capacidad { get; set; }
        public string Descripcion { get; set; }
    }


}
