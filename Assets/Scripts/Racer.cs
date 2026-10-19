using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Racer : MonoBehaviour
{
    RaceManager raceManager;

    [SerializeField] GameObject[] bodies = new GameObject[2];

    float time;
    public int ID;
    public int nextCheckpoint = 0;
    [SerializeField] List<int> lapProgress = new List<int>() { -1, 0, 0, 0 };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lapProgress = new List<int>() { -1, 0, 0, 0 };
        raceManager = FindAnyObjectByType<RaceManager>();
        raceManager.RegisterRacer(this);
    }

    [SerializeField] TMPro.TextMeshProUGUI label;

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCar(int i)
    {
        bodies[i].SetActive(true);
    }

    public void SetID(int i)
    {
        label.text = $"P{i}";
        ID = i;
    }

    public void MarkProgress(int checkpointIndex)
    {
        int prevCheckpointIndex = (checkpointIndex + 3) % 4;
        //Debug.Log($"Entering Checkpoint {checkpointIndex} from {prevCheckpointIndex}");
        //Debug.Log(lapProgress.Count);
        //Debug.Log($"LapProgress: {lapProgress[0]}, {lapProgress[1]}, {lapProgress[2]}, {lapProgress[3]}");

        
        if (checkpointIndex != nextCheckpoint) return;
        if (lapProgress[prevCheckpointIndex] >= lapProgress[checkpointIndex]) lapProgress[checkpointIndex] += 1;

        if (lapProgress[0] >= 3 && lapProgress[1] >= 3 && lapProgress[2] >= 3 && lapProgress[3] >= 3) raceManager.ClaimWin(this);
        nextCheckpoint = (nextCheckpoint+1)%4;
    }
}


public class RacerProfile
{
    public int car;
    public string controlScheme;
    public InputDevice device;

    public RacerProfile(int car, string controlScheme=null, InputDevice device=null) { this.car = car; this.controlScheme = controlScheme; this.device = device ?? Keyboard.current; }
}