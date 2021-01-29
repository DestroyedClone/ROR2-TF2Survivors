using RoR2;
using R2API;
using UnityEngine;

namespace ROR2_SaxtonHale.Misc
{
    public class HaleController : MonoBehaviour
    {
        public SkillLocator skillLocator;
        public float airborneStopwatch = 0f;
        readonly float minTimeForWeighdown = 6f;
        private bool addStock = false;


        public void Start()
        {
            if (!skillLocator)
            {
                skillLocator = gameObject.GetComponent<CharacterBody>()?.skillLocator;
            }
            if (!skillLocator) Destroy(this);
        }

        public void FixedUpdate()
        {
            var body = gameObject.GetComponent<CharacterBody>();
            if (body && body.characterMotor)
            {
                if (body.characterMotor.isGrounded)
                {
                    airborneStopwatch = 0f;
                    if (addStock)
                    {
                        skillLocator.utility.SetSkillOverride(body, Modules.Skills.crouchSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                        skillLocator.utility.stock = 1;
                        addStock = false;
                    }
                }
                else
                {
                    airborneStopwatch += Time.deltaTime;
                    if (airborneStopwatch >= minTimeForWeighdown)
                    {
                        if (!addStock)
                        {
                            skillLocator.utility.SetSkillOverride(body, Modules.Skills.weighdownSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                            skillLocator.utility.stock = 1;
                            addStock = true;
                        }
                    }
                }
            }
        }
    }
}
