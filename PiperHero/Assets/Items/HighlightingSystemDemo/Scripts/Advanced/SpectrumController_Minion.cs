using UnityEngine;
using System.Collections;

public class SpectrumController_Minion : HighlighterController
{

	new void Update()
	{
        base.Update();
		
		Color col = new Color(0, 1, 0, 1f);
		
		h.ConstantOnImmediate(col);
	}

}
