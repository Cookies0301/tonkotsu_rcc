using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatHandler : Singleton<BeatHandler>
{
    [SerializeField]
    private int timeWindow = 0;
    [SerializeField]
    private int reactionTime = 0;

    [SerializeField]
    private AnalysisSO beatAnalysis;
    [SerializeField]
    private List<Texture2D> spectrumTexture;
    [SerializeField]
    [Range(0, 300)]
    private int visualOffsetX = 0;

    private static float timeSample = 0;

    private static AudioSource sourceWave = null;

    private static bool beatVisualize = false;
    private bool copy = false;


    private static List<int> beatListCopy;

    void Start()
    {
        sourceWave = GetComponent<AudioSource>();

        if(GameSettings.DisableSound)
        {
            sourceWave.volume = 0;
        }

        if (!beatAnalysis.Analysed)
        {
            Debug.LogWarning("Beat Analysis was not done. Analysing now...");
            beatAnalysis.Analyze();
        }
        
        beatListCopy = new List<int>(beatAnalysis.ResultList);
    }

    private void Update()
    {
        if ((sourceWave.timeSamples >= beatAnalysis.ResultList[beatAnalysis.ResultList.Count - 1] && copy) || beatListCopy.Count == 0)
        {
            copy = false;
            beatListCopy = new List<int>(beatAnalysis.ResultList);
        }
        if (sourceWave.timeSamples <= beatAnalysis.ResultList[0] && !copy)
        {
            copy = true;
        }
    }

    private static bool IsOnBeat(int reactionTime, int timeWindow)
    {
        timeSample = sourceWave.timeSamples - reactionTime;
        for (int i = 0; i < beatListCopy.Count; i++)
        {
            if (timeSample >= (beatListCopy[i] - timeWindow) &&
                timeSample <= (beatListCopy[i] + timeWindow))
            {
                return true;

            }
            else if (timeSample > beatListCopy[i] + timeWindow)
            {
                beatListCopy.RemoveAt(i);
                i -= 1;
            }
        }
        return false;
    }

    public static float BeatRangePercent(int onBeatRangeDelay, int onBeatRangeWindow)
    {
        var beats = Instance.beatAnalysis.ResultList;
        timeSample = sourceWave.timeSamples - onBeatRangeDelay;
        for (int i = 0; i < beats.Count; i++)
        {
            float total = onBeatRangeWindow * 2;
            float value = timeSample - (beats[i] - onBeatRangeWindow);

            //if outside of +- window, return 0
            if(value < 0 || value > total)
            {
                continue;
            }
            //else return a value with 1 when close to the exact beat and 0 when further away
            else
            {
              
                var normalizedValue = value / total;
                var minus1to1 = (normalizedValue - 0.5f) * 2;
                //go from -1 to 1 -> 0 to 1 with 1 being closer to the previous 0. Practically: Absolute values only and inversion of graph.
                var final = 1 - (Mathf.Abs(minus1to1));

                return final;
            }
        }

        return 0;

    }

    private void OnGUI()
    {
        if (beatVisualize)
        {
            float heightMulti = 100;
            float widthMulti = 1;
            int sampleJump = 1800;

            for (int i = 0; i * sampleJump < beatAnalysis.Spectrum.Length; i++)
            {
                if (i > Screen.width - 1)
                {
                    break;
                }
                GUI.DrawTexture(new Rect(visualOffsetX + i * widthMulti, 5, widthMulti, heightMulti * beatAnalysis.Spectrum[i * sampleJump]), spectrumTexture[0]);
            }
            for (int j = 0; j < beatAnalysis.ResultList.Count; j++)
            {
                GUI.DrawTexture(new Rect(visualOffsetX + beatAnalysis.ResultList[j] / sampleJump, 5, widthMulti, heightMulti), spectrumTexture[1]);
            }
            GUI.DrawTexture(new Rect(visualOffsetX + sourceWave.timeSamples / sampleJump, 5, widthMulti, heightMulti * 1.1f), spectrumTexture[2]);
        }
    }

    public static void BeatVisualize()
    {
        beatVisualize = !beatVisualize;
    }
}