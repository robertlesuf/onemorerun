public class Character 
{
    public string name { get; set;}
    public string description { get; set;}

    public int max_health { get; set;}
    public int health { get; set;}
    public int base_physical_damage { get; set;}
    public int base_magic_damage { get; set;}
    public int physical_damage_aplification { get; set;}
    public int magic_damage_aplification { get; set;}

    public int magic_resistance { get; set;}

    public int physical_resistance { get; set;}

    public Weapon weapon { get; set; }

    public void takeDamage(string damageType, int damageTotal)
	{
		if (damageType == "physical") {
			health = health - (damageTotal - ((damageTotal * (physical_resistance / 100))));
		} else if (damageType == "magic") {
			health = health - (damageTotal - ((damageTotal * (magic_resistance / 100))));
		}
	}

    public void dealDamage(Character target, string damageType, int damageTotal) {
        if (target == null) {
            return;
        }

        if (damageType == "") {
            return;
        }

        int calculatedDamage = damageType == "physical" ? (damageTotal + (damageTotal * (physical_damage_aplification / 100))) :
                                     (damageTotal + (damageTotal * (magic_damage_aplification / 100)));

        target.takeDamage(damageType, calculatedDamage);
    }
    
}