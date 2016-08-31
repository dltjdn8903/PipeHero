using UnityEngine;
using System.Collections;

public class SpectrumController_Enemy : HighlighterController
{

	new void Update()
	{
        base.Update();
		
		Color col = new Color(1, 0, 0, 1f);
		
		h.ConstantOnImmediate(col);
	}

}
