using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "ManualBeatAnalysis", menuName = "Analysis/ManualBeat")]
public class ManualBeatAnalysisSO : AnalysisSO
{
    [SerializeField]
    int samplePerBeat, initialSampleOffset;


    protected override List<int> AnalyseClip()
    {
        var results = new List<int>();

        spectrum = new float[Clip.samples];
        Clip.GetData(spectrum, 0);

        for (int i = initialSampleOffset; i < Clip.samples; i+= samplePerBeat)
        {
            results.Add(i);
        }

        analysed = true;
        lastClipName = Clip.name;

        Debug.Log("Result: " + results.Count);

        return results as List<int>;
    }
}
