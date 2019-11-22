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

    private AudioSource sourceWave = null;

    private bool beatVisualize = false;
    private bool copy = false;
    private float[] spectrum = null;

    private List<int> controllerMarkers;

    private static List<int> beatListCopy;

    void Start()
    {
        sourceWave = GetComponent<AudioSource>();

        if(GameSettings.Instance.SavedData.DisableSound)
        {
            sourceWave.volume = 0;
        }

        if (!beatAnalysis.Analysed)
        {
            Debug.LogWarning("Beat Analysis was not done. Analysing now...");
            beatAnalysis.Analyze();
        }
        
        beatListCopy = new List<int>(beatAnalysis.ResultList);
        spectrum = new float[sourceWave.clip.samples];
        sourceWave.clip.GetData(spectrum, 0);
        controllerMarkers = new List<int>();
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
        timeSample = Instance.sourceWave.timeSamples - reactionTime;
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
        timeSample = Instance.sourceWave.timeSamples - onBeatRangeDelay;
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
            int sampleJump = 600;
            float viewWidth = Screen.width - visualOffsetX - 10;

            for (int i = -visualOffsetX; i < viewWidth; i++)
            {
                if(spectrum.Length < i * sampleJump + sourceWave.timeSamples)
                {
                    break;
                }

                if(i * sampleJump +sourceWave.timeSamples < 0)
                {
                    continue;
                }

                int currSample = Mathf.FloorToInt(sourceWave.timeSamples/sampleJump)*sampleJump;

                GUI.DrawTexture(new Rect(visualOffsetX + i * widthMulti, 5, widthMulti, heightMulti * Mathf.Abs(spectrum[(i * sampleJump) + currSample])), spectrumTexture[0]);
            }

            for (int j = 0; j < beatAnalysis.ResultList.Count; j++)
            {
                GUI.DrawTexture(new Rect(visualOffsetX + (beatAnalysis.ResultList[j] - sourceWave.timeSamples) / sampleJump, 5, widthMulti, heightMulti), spectrumTexture[1]);
            }

            for (int k = 0; k < controllerMarkers.Count; k++)
            {
                int pos = Mathf.FloorToInt((controllerMarkers[k]- sourceWave.timeSamples)/ sampleJump);

                if(visualOffsetX + pos > 0)
                {
                    GUI.DrawTexture(new Rect(visualOffsetX + pos, heightMulti*0.8f, widthMulti, heightMulti * 0.3f), spectrumTexture[2]);
                }
                else
                {
                    controllerMarkers.RemoveAt(k);
                    k--;
                }

            }

            GUI.DrawTexture(new Rect(visualOffsetX, 5, widthMulti, heightMulti * 1.1f), spectrumTexture[2]);
        }
    }

    public static void AddControllerMarker()
    {
        if(Instance.controllerMarkers.Count == 0 || Instance.sourceWave.timeSamples - Instance.controllerMarkers[Instance.controllerMarkers.Count-1] > 5000)
        Instance.controllerMarkers.Add(Instance.sourceWave.timeSamples);
    }

    public static void BeatVisualize()
    {
        Instance.beatVisualize = !Instance.beatVisualize;
    }
}