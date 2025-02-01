using System.Dynamic;

public class Wizard : Character
{
   public Wizard() {
        this.name = "Wizard";
        this.description = "A wizard is never late.\n" + "He might or might not have a wand.";

        this.max_health = 80;
        this.health = 80;

        this.base_physical_damage = 0;
        this.base_magic_damage = 20;

        this.magic_damage_aplification = 20;
        this.physical_damage_aplification = 0;

        this.physical_resistance = 5;
        this.magic_resistance = 20;
   }
}