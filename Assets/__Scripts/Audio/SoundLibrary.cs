using UnityEngine;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour
{
    public SoundGroup[] soundGroups;

    private Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            groupDictionary.Add(soundGroup.groupID, soundGroup.group);
        }
    }

    public AudioClip GetClipFromName(string name)
    {
        if (groupDictionary.ContainsKey(name))
        {
            AudioClip[] sounds = groupDictionary[name];
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }
        return null;
    }
}

