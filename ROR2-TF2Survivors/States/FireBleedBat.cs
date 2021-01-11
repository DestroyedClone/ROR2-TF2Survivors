using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2_Scout.States
{
    public class FireBleedBat : FireMarkFan
    {
        public FireBleedBat()
        {
            DamageTypeValue = RoR2.DamageType.BleedOnHit;
        }
    }
}
