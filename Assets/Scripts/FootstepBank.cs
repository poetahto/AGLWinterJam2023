using System;
using System.Collections.Generic;
using poetools;
using UnityEngine;

[CreateAssetMenu]
public class FootstepBank : ScriptableObject
{
    [Serializable]
    public struct TagData
    {
        public Tag tag;
        public AudioClip audioClip;
        public float volume;
    }

    public List<TagData> data = new List<TagData>();
    public TagData defaultData;

    public TagData Lookup(params Tag[] tags)
    {
        foreach (var tagData in data)
        {
            foreach (var tag in tags)
            {
                if (tag == tagData.tag)
                    return tagData;
            }
        }

        return defaultData;
    }
}