using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2_Scout.States
{
    public class FireMarkFan : BaseFireBat
    {
        public FireMarkFan()
        {
            DamageTypeValue = RoR2.DamageType.BleedOnHit;
        }
    }
}
