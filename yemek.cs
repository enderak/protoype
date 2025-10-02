// Models/Yemek.cs
public class Yemek
{
    public int Id { get; set; } // Primary Key
    public string Ad { get; set; } // Yemek Adı
    public int Kalori { get; set; } // Kalori Değeri
    public string Icerikler { get; set; } // Yemeğin İçerikleri (geniş metin)

    // Navigasyon Özelliği (isteğe bağlı ama faydalı)
    public ICollection<Menukalemi> Menuler { get; set; } 
}