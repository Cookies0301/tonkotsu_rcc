using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class AnalysisSO : ScriptableObject
{
    [Header("Clip to Analyse")]
    [Tooltip("Clip that should be analysed.")]
    public AudioClip Clip;

    [ReadOnly]
    public List<int> resultList;

    [ReadOnly]
    public bool analysed;

    protected string lastClipName = null;

    protected float[] spectrum = null;

    public float[] Spectrum { get => spectrum; }
    public List<int> ResultList { get => resultList;}
    public bool Analysed { get => analysed; }

    [Button]
    public virtual void Analyze()
    {
        #if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Analysis");
        #endif
        
        resultList = AnalyseClip();

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        #endif
    }

    protected abstract List<int> AnalyseClip();
    
    protected virtual void OnValidate()
    {
        if(Clip == null)
        {
            analysed = false;
        }
        else if(lastClipName != Clip.name)
        {
            analysed = false;
            lastClipName = Clip.name;
        }
    }
}