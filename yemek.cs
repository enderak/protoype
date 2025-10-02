using System.Collections.Generic;

public class Yemek
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public int Kalori { get; set; }
    public string Icerikler { get; set; }

    public ICollection<OgunYemegi> OgunYemekleri { get; set; } 
}
