using UnityEngine;
using System.Collections;
using System;

public class SkillFireNova : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        for (int i = 0;i<8;++i)
        {
            Vector3 novaVecter = Vector3.zero;
            switch (i)
            {
                case 0:
                    novaVecter = new Vector3(0, 0, -2);
                    break;

                case 1:
                    novaVecter = new Vector3(2, 0, -2);
                    break;

                case 2:
                    novaVecter = new Vector3(2, 0, 0);
                    break;

                case 3:
                    novaVecter = new Vector3(2, 0, 2);
                    break;

                case 4:
                    novaVecter = new Vector3(0, 0, 2);
                    break;

                case 5:
                    novaVecter = new Vector3(-2, 0, 2);
                    break;

                case 6:
                    novaVecter = new Vector3(-2, 0, 0);
                    break;

                case 7:
                    novaVecter = new Vector3(-2, 0, -2);
                    break;
            }
            SkillManager.Instance.RunSkill(OWNER, "FIRENOVA_BULLET_1", OWNER.SelfTransform.position + novaVecter);
        }
        END = true;
    }

    public override void UpdateSkill()
    {
    }
}
