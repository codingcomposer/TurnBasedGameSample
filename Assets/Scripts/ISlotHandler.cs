using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG {
    public interface ISlotHandler
    {
        public void SlotClicked(BaseSlot slot);
    }
}