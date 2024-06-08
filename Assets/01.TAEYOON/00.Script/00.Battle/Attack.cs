namespace PokeRPG.Battle.Unit
{
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public enum type
    {
        fire,
        water,
        grass,
        dark,
        light,
    }

    public static class Attack
    {
        public static float DamageCalc(UnitProfile attacker, UnitProfile defender)
        {
            return (attacker.damage -= defender.defence) * GetTypeEffective(attacker.monsterType, defender.monsterType);
        }

        static float GetTypeEffective(type attak, type defending)
        {
            Dictionary<type, Dictionary<type, float>> multiplier = new Dictionary<type, Dictionary<type, float>>()
            {
                {type.fire, new Dictionary<type, float >() },
                {type.water, new Dictionary<type, float >() },
                {type.grass, new Dictionary<type, float >() },
                {type.dark, new Dictionary<type, float >() },
                {type.light, new Dictionary<type, float >() }
            };

            // 효과 굉장
            multiplier[type.fire][type.grass] = 2;
            multiplier[type.grass][type.water] = 2;
            multiplier[type.water][type.fire] = 2;
            multiplier[type.light][type.dark] = 2;
            multiplier[type.dark][type.light] = 2;

            // 효과 별로
            multiplier[type.grass][type.fire] = 0.5f;
            multiplier[type.water][type.grass] = 0.5f;
            multiplier[type.fire][type.water] = 0.5f;

            foreach (type atk in multiplier.Keys)
            {
                foreach(type def in multiplier.Keys )
                {
                    if (!multiplier[atk].ContainsKey(def))
                    {
                        multiplier[atk][def] = 1;
                    }
                }
            }

            return multiplier[attak][defending];
        }
    }
}