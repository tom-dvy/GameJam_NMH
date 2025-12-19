using UnityEngine;

/// <summary>
/// Classe helper avec des presets de stats pour différents types d'items
/// </summary>
public static class ItemStatPresets
{
    // ========== ARMES ==========
    
    public static ItemStat[] GetWeaponStats(float damage, float critChance, float accuracy, float durability)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.Damage, value = damage },
            new ItemStat { statType = StatType.CriticalChance, value = critChance },
            new ItemStat { statType = StatType.Accuracy, value = accuracy },
            new ItemStat { statType = StatType.Durability, value = durability }
        };
    }

    // Presets d'armes communes
    public static ItemStat[] CommonSword => GetWeaponStats(10, 5, 85, 100);
    public static ItemStat[] CommonAxe => GetWeaponStats(15, 3, 70, 80);
    public static ItemStat[] CommonDagger => GetWeaponStats(7, 15, 90, 60);
    
    public static ItemStat[] RareSword => GetWeaponStats(25, 10, 90, 150);
    public static ItemStat[] EpicSword => GetWeaponStats(45, 15, 95, 200);
    public static ItemStat[] LegendarySword => GetWeaponStats(75, 25, 98, 300);

    // ========== ARMURES ==========
    
    public static ItemStat[] GetArmorStats(float armor, float thorns, float durability)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.Armor, value = armor },
            new ItemStat { statType = StatType.Thorns, value = thorns },
            new ItemStat { statType = StatType.Durability, value = durability }
        };
    }

    // Presets d'armures par partie
    public static ItemStat[] CommonHelmet => GetArmorStats(5, 1, 100);
    public static ItemStat[] CommonChestplate => GetArmorStats(15, 3, 120);
    public static ItemStat[] CommonLeggings => GetArmorStats(10, 2, 110);
    public static ItemStat[] CommonBoots => GetArmorStats(5, 1, 100);
    
    public static ItemStat[] RareHelmet => GetArmorStats(12, 3, 150);
    public static ItemStat[] RareChestplate => GetArmorStats(30, 8, 180);

    // ========== BOUCLIERS ==========
    
    public static ItemStat[] GetShieldStats(float armor, float thorns, float durability)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.Armor, value = armor },
            new ItemStat { statType = StatType.Thorns, value = thorns },
            new ItemStat { statType = StatType.Durability, value = durability }
        };
    }

    public static ItemStat[] CommonShield => GetShieldStats(20, 5, 150);
    public static ItemStat[] RareShield => GetShieldStats(40, 12, 200);

    // ========== CONSOMMABLES ==========
    
    public static ItemStat[] GetHealingPotion(float healAmount)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.Heal, value = healAmount }
        };
    }

    public static ItemStat[] GetDamageBoostPotion(float boostPercent, float duration)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.DamageBoost, value = boostPercent }
        };
    }

    public static ItemStat[] GetAccuracyBoostPotion(float boostPercent)
    {
        return new ItemStat[]
        {
            new ItemStat { statType = StatType.AccuracyBoost, value = boostPercent }
        };
    }

    // Presets de consommables
    public static ItemStat[] SmallHealthPotion => GetHealingPotion(25);
    public static ItemStat[] MediumHealthPotion => GetHealingPotion(50);
    public static ItemStat[] LargeHealthPotion => GetHealingPotion(100);
    
    public static ItemStat[] StrengthPotion => GetDamageBoostPotion(20, 30);
    public static ItemStat[] FocusPotion => GetAccuracyBoostPotion(15);
}