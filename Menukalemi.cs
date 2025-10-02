using System;
using System.Collections.Generic;

public class Menukalemi
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; } 
    public string Ogun { get; set; } 

    public ICollection<OgunYemegi> SecilenYemekler { get; set; } 
}