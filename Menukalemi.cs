/ Models/Menukalemi.cs
public class Menukalemi
{
    public int Id { get; set; } // Primary Key
    public DateTime Tarih { get; set; } // Menünün oluşturulduğu gün
    
    // Öğün Tipi (Sabah, Oglen, Aksam, Gece)
    public string Ogun { get; set; } 

    // İlişki (Foreign Key)
    public int YemekId { get; set; } 
    
    // Navigasyon Özelliği
    public Yemek Yemek { get; set; } // Hangi yemeğin menüde olduğunu gösterir
}