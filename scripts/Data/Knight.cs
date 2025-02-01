using System.Dynamic;

public class Knight : Character
{
    public Knight() 
    {
        this.name = "Knight";
        this.description = "A knight is never bored.\n" + "He is always ready to cook chicken!";

        this.max_health = 100;
        this.health = 100;

        this.base_physical_damage = 20;
        this.base_magic_damage = 0;

        this.magic_damage_aplification = 10;
        this.physical_damage_aplification = 0;

        this.physical_resistance = 25;
        this.magic_resistance = 10;
   }
}