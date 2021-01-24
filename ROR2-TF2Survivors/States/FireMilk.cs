using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using RoR2;

namespace ROR2_Scout.States
{
    public class FireMilk : FireCleaver
    {
		public FireMilk()
        {
			damageCoefficient = 0f;
			projectilePrefab = Modules.Projectiles.milkjarProjectile;
		}
	}
}
