using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Users;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int page = 0;
    [SerializeField] Transform pagePivot;
    PlayerInputManager manager;


    [SerializeField]public int selectedLevel=1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindAnyObjectByType<PlayerInputManager>();
        selectedLevel = 1;
        InputSystem.onAnyButtonPress.CallOnce((ctrl => { Debug.Log("GANK"); if (page == 0) NextPage(); }));
        RaceManager.racerProfiles.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        switch(page)
        {
            case 0:
                break;
            case 1:



                break;
            case 2:




                break;
            case 3:




                break;
        }
    }


    public void NextPage()
    {
        if (page == 3) { LoadLevel(); return; }
        page++;
        StartCoroutine(AnimatePageTransition());
    }

    IEnumerator AnimatePageTransition()
    {
        float duration = 0.5f;
        float time = 0;
        while(time<duration)
        {
            time += Time.deltaTime;

            float normTime = Mathf.Clamp(time, 0, duration);
            float lerpVal = normTime / duration;


            pagePivot.rotation = Quaternion.Euler(0, -Mathf.Lerp(90 * (page - 1), 90 * page, AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(lerpVal)),0);



            yield return null;
        }
        yield return null;
    }

    int numKeyboardUsers = 1;
    static List<RacerProfile> racerProfiles = new List<RacerProfile>() { new RacerProfile(0, "KB1") };
    [SerializeField] GameObject icon;
    [SerializeField] List<GameObject> icons = new List<GameObject>(4);
    [SerializeField] RectTransform parent;
    [SerializeField] List<GameObject> container = new List<GameObject>();
    public void AddPlayer()
    {
        if (numKeyboardUsers > 4) return;
        numKeyboardUsers++;
        racerProfiles.Add(new RacerProfile(0, $"KB{numKeyboardUsers}"));
        Debug.Log($"Added Player KB{numKeyboardUsers}");
        var newPlayer = manager.JoinPlayer(controlScheme: $"KB{numKeyboardUsers}", pairWithDevice: Keyboard.current);
        container.Add(newPlayer.gameObject);
        newPlayer.transform.rotation = Quaternion.Euler(0, 90 * (numKeyboardUsers-1), 0);
        newPlayer.GetComponentInChildren<CarSelector>().racerIndex = numKeyboardUsers-1;
        icons[numKeyboardUsers-1] = GameObject.Instantiate(icon, parent);
        icons[numKeyboardUsers - 1].GetComponent<playerIcon>().Set($"KB{ numKeyboardUsers}");


        //foreach (var device in InputUser.GetUnpairedInputDevices())
        //{
            //if (device is Mouse) continue;
            //if(device is Keyboard && numKeyboardUsers<4)
            //{
                //numKeyboardUsers++;
                //racerProfiles.Add(new RacerProfile(0, $"KB{numKeyboardUsers}"));
            //}
            ////else if(device is Gamepad || device is Joystick)
            ////{
            ////    racerProfiles.Add(new RacerProfile(0, device: device));
            ////}
            ////device.
            ////if(InputSystem.actions.scheme
        //}
    }


    public void RemovePlayer()
    {
        racerProfiles.RemoveAt(racerProfiles.Count - 1);
        GameObject.Destroy(icons[numKeyboardUsers - 1]);
        GameObject.Destroy(container[numKeyboardUsers - 1]);
        numKeyboardUsers--;
    }


    public static void SetCar(int player, int car)
    {
        Debug.Log($"Player {player} selected car {car}");
        racerProfiles[player].car = car;
    }





    public void SetLevel(int level)
    {
        selectedLevel = level + 1;
    }

    public void LoadLevel()
    {
        RaceManager.racerProfiles = racerProfiles;
        UnityEngine.SceneManagement.SceneManager.LoadScene(selectedLevel);
    }
}
