using UnityEngine;
using System.Collections;

public class StageTemplateData
{
    public int EPISODE_ID { get; private set; }
    public int STAGE_ID { get; private set; }

    public string SCENE_NAME { get; private set; }
    public string SPAWN_ID { get; private set; }

    public StageTemplateData( SimpleJSON.JSONNode nodeData )
    {
        EPISODE_ID = nodeData["EPISODE_ID"].AsInt;
        STAGE_ID = nodeData["STAGE_ID"].AsInt;
        SCENE_NAME = nodeData["SCENE_NAME"];
        SPAWN_ID = nodeData["SPAWN_ID"];
    }
}
