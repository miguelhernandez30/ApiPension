namespace ApiUniRoom.Models
{
    public class Documento
    {
        public string TipoDocumento { get; set; }  // VARCHAR(45)
        public string CaraDelantera { get; set; }  // TEXT
        public string CaraTrasera { get; set; }    // TEXT
        public DateTime FechaSubida { get; set; }  // DATETIME
        public string NumeroDocumento { get; set; } // VARCHAR(15)
        public int IdUsuario { get; set; }         // INT (foreign key)
    }
}
