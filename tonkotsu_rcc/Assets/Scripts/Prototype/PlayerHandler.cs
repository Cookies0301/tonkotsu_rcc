using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PlayerHandler : Singleton<PlayerHandler>
{
    [SerializeField] GameObject autoBeatPlayer, multiBeatPlayer;

    [ReadOnly]
    [SerializeField] PlayerPrototypeType playerType;

    PlayerController player;

    public static Transform Player { get => Instance.GetPlayerTransform(); }

    public static void ChangePrototypePlayer()
    {
        if(Instance.playerType == PlayerPrototypeType.AutoBeat)
        {
            Instance.ChangeToAutoBeat();
        }
        else
        {
            Instance.ChangeToMultiBeat();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (GameSettings.Instance.SavedData.SpawnPlayer)
        {
            var go = Instantiate(autoBeatPlayer);

            player = go.GetComponent<PlayerController>();
        }
        else
        {
            player = GameObject.FindObjectOfType<PlayerController>();
        }

        if (player == null)
        {
            Debug.LogError("No PlayerController found");
        }
        else
        {
            var spawnPos = GameObject.FindObjectOfType<PlayerSpawnPos>();

            if (spawnPos != null)
                player.transform.position = spawnPos.transform.position;
        }
    }

    private Transform GetPlayerTransform()
    {
        if(Instance.player != null)
        {
            return Instance.player.transform;
        }
        else
        {
            Debug.LogWarning("Creating Empty Player to avoid NullReferences");
            var go = new GameObject("__ERROR_AVOIDING_TEMP_PLAYER");
            go.hideFlags = HideFlags.DontSave;
            Instance.player = go.AddComponent<PlayerController>();

            return go.transform;
        }
    }

    private void ChangeToAutoBeat()
    {
        playerType = PlayerPrototypeType.AutoBeat;
    }

    private void ChangeToMultiBeat()
    {
        playerType = PlayerPrototypeType.MultiBeat;
    }

    public enum PlayerPrototypeType
    {
        AutoBeat,
        MultiBeat
    }

}
