using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class RaceManager : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip countdown;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip music;
    float timer = 0;
    List<Racer> racers = new List<Racer>();
    Dictionary<Racer, float> winningContenders = new Dictionary<Racer, float>();

    public int playerCount;
    public static int numPlayers = 1;
    [SerializeField] UnityEngine.UI.Image winBanner;
    [SerializeField] TMPro.TextMeshProUGUI winLbl;

    public static List<RacerProfile> racerProfiles = new List<RacerProfile>() { new RacerProfile(0, "KB1")};

    public static bool raceBegun = false;

    public UnityEngine.Events.UnityEvent OnRaceBegin = new UnityEngine.Events.UnityEvent();
    public UnityEngine.Events.UnityEvent OnRaceEnd = new UnityEngine.Events.UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        audioSource.PlayOneShot(countdown);
        float fontSize = winLbl.fontSize;
        float countDown = 4;

        winBanner.gameObject.SetActive(true);
        winBanner.enabled = false;
        while (countDown>1f)
        {
            countDown -= Time.fixedDeltaTime;
            winLbl.text = $"{(int)countDown}";
            winLbl.fontSize = Mathf.Lerp(fontSize, fontSize + 50, (countDown -(int)countDown));
            yield return new WaitForFixedUpdate();
        }

        winLbl.text = "GO! GO! GO!";
        raceBegun = true;
        OnRaceBegin.Invoke();

        yield return new WaitForSeconds(1.5f);
        winBanner.gameObject.SetActive(false);
        winBanner.enabled = true;
        winLbl.fontSize = fontSize;


        yield return null;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if(raceBegun) timer += Time.fixedDeltaTime;

        if (Keyboard.current.escapeKey.wasPressedThisFrame) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    private void LateUpdate()
    {
        if(winningContenders.Count>0)
        {
            //TODO evaluate win condition and compare times
        }
    }


    public static void SubscribePlayer(RacerProfile p)
    {
        racerProfiles.Add(p);
    }

    public static void UnsubscribePlayer()
    {
        racerProfiles.RemoveAt(racerProfiles.Count-1);
    }

    public List<string> GetControlSchemes()
    {
        return null;
        List<string> schemes = new List<string>();
        foreach(var d in InputSystem.devices)
        {

        }
    }



    public void RegisterRacer(Racer racer)
    {
        if (racers.Contains(racer)) return;
        racers.Add(racer);
    }

    public void ClaimWin(Racer winner)
    {
        if (winningContenders.ContainsKey(winner)) return;
        winningContenders.Add(winner, timer);

        TriggerWin(winner);

        Debug.Log($"Winner Detected: {winner.name}, {timer.ToString("F2")}");
    }

    void TriggerWin(Racer winner)
    {
        audioSource.PlayOneShot(win);
        winBanner.gameObject.SetActive(true);
        winLbl.text = $"P{winner.ID} WINS! TIME: {timer.ToString("F2")}";
    }

    public void ReturnToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnBeforeSerialize()
    {
        playerCount = numPlayers;
    }

    public void OnAfterDeserialize()
    {
        numPlayers = playerCount;
    }
}
