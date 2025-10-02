public class OgunYemegi
{
    public int Id { get; set; }

    public int MenukalemiId { get; set; }
    public Menukalemi Menukalemi { get; set; }

    public int YemekId { get; set; }
    public Yemek Yemek { get; set; }
}