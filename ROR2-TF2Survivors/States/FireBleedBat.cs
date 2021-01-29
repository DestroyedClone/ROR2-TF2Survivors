using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2_Scout.States
{
    public class FireBleedBat : BaseFireBat
    {
        public FireBleedBat()
        {
            DamageTypeValue = RoR2.DamageType.BleedOnHit;
        }
    }
}
