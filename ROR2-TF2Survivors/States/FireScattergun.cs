﻿using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;

namespace ROR2_Scout.States
{
    public class FireScattergun : FireShotgunBase
    {
        public FireScattergun()
        {
            bulletCount = 6;
            recoilAmplitude = 1f;
        }
    }
}
