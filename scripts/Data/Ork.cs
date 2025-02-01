using System.Dynamic;

public class Ork : Character
{
    public string name { get; set;} = "Ork";

    public string description { get; set;} = "Ork is ork and his name is Ork.";

    public int health { get; set;} = 120;
    public int max_health { get; set;} = 120;
    public int base_physical_damage { get; set;} = 30;
    public int base_magic_damage { get; set;} = 0;
    public int physical_damage_aplification { get; set;} = 0;
    public int magic_damage_aplification { get; set;} = 0;

    public int magic_resistance { get; set;} = 0;

    public int physical_resistance { get; set;} = 5;
}