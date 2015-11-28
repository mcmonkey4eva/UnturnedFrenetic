using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnturnedFrenetic
{
    class UnturnedFreneticTicker: MonoBehaviour
    {
        public void FixedUpdate()
        {
            UnturnedFreneticMod.Instance.Tick(Time.deltaTime);
        }
    }
}
