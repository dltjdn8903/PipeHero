using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class StageManager : BaseManager< StageManager >
{
    Dictionary<int, List<StageTemplateData>> m_dicStageTemplate = new Dictionary<int, List<StageTemplateData>>();

    public GameObject Marker
    {
        get;
        set;
    }

    public GameObject PlayerPlane { get; set; }

    void Awake()
    {
        Touch_EventManager.Instance.OnStart();
        ObjectPoolManager.Instance.OnStart();
        EffectManager.Instance.OnStart();

        Marker = Resources.Load<GameObject>("point");
        PlayerPlane = Resources.Load<GameObject>("pPlane1");
        PlayerPlane = GameObject.Instantiate(PlayerPlane);
        Marker = GameObject.Instantiate(Marker);
        DontDestroyOnLoad(PlayerPlane);
        DontDestroyOnLoad(Marker);

        TextAsset asset = Resources.Load<TextAsset>("STAGE_TEMPLATE");
        JSONClass rootNode = JSON.Parse(asset.text) as JSONClass;
        if (rootNode == null)
            return;

        JSONArray arrTemplate = rootNode["STAGE_TEMPLATE"] as JSONArray;
        for( int i = 0; i < arrTemplate.Count; ++i )
        {
            int nEpisode = (arrTemplate[i])["EPISODE_ID"].AsInt;

            StageTemplateData stageTemplate = new StageTemplateData(arrTemplate[i]);
            List<StageTemplateData> listTemplateData = null;

            if( m_dicStageTemplate.ContainsKey( nEpisode ) == false )
            {
                listTemplateData = new List<StageTemplateData>();
                m_dicStageTemplate.Add(nEpisode, listTemplateData);
            }
            else
            {
                listTemplateData = m_dicStageTemplate[nEpisode];
            }

            listTemplateData.Add(stageTemplate);
        }
    }

    public bool IsValidEpisode( int nEpisode )
    {
        return m_dicStageTemplate.ContainsKey(nEpisode);
    }

    public List<StageTemplateData> GetStageList( int nEpisode )
    {
        List<StageTemplateData> listTemplateData = null;
        m_dicStageTemplate.TryGetValue(nEpisode, out listTemplateData);
        return listTemplateData;
    }

    public StageTemplateData GetStage( int nEpisode, int nStage )
    {
        List<StageTemplateData> listTemplateData = GetStageList(nEpisode);
        for( int i = 0; i < listTemplateData.Count; ++i )
        {
            if (listTemplateData[i].STAGE_ID == nStage)
                return listTemplateData[i];
        }
        return null;
    }
}
