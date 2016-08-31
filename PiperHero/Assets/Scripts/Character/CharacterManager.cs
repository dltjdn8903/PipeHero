using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SimpleJSON;

public class CharacterManager : BaseManager< CharacterManager >
{
    Dictionary<string, CharacterTemplateData> m_dicTemplateData = new Dictionary<string, CharacterTemplateData>();
    List<GameCharacter> m_listCharacter = new List<GameCharacter>();

    void Awake()
    {
        TextAsset characterText = Resources.Load<TextAsset>("CHARACTER_TEMPLATE_SUB");
        if (characterText != null)
        {
            JSONClass rootNodeData = JSON.Parse(characterText.text) as JSONClass;
            if (rootNodeData != null)
            {
                JSONClass characterTemplateNode = rootNodeData["CHARACTER_TEMPLATE"] as JSONClass;
                foreach( KeyValuePair<string,JSONNode> templateNode in characterTemplateNode )
                {
                    m_dicTemplateData.Add(templateNode.Key, new CharacterTemplateData( templateNode.Key, templateNode.Value));
                }
            }
        }
    }

    public GameCharacter AddCharacter( string strTemplateKey )
    {
        CharacterTemplateData templateData = GetTemplate(strTemplateKey);
        if (templateData == null)
            return null;

        GameCharacter gameCharacter = new GameCharacter();
        gameCharacter.SetTemplate(templateData);
        return gameCharacter;
    }

    public CharacterTemplateData GetTemplate( string strTemplateKey )
    {
        CharacterTemplateData templateData = null;
        m_dicTemplateData.TryGetValue(strTemplateKey, out templateData);
        return templateData;
    }
}
