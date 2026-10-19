using UnityEngine;
using UnityEngine.InputSystem;


public class VehicleSpawner : MonoBehaviour
{
    //[SerializeField] int playerCount = 1;
    Collider spawnZone;
    PlayerInputManager playerManager;
    [SerializeField] GameObject[] prefab = new GameObject[1];

    float positioningWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spawnZone = GetComponent<Collider>();
        playerManager = FindAnyObjectByType<PlayerInputManager>();
        positioningWidth = spawnZone.bounds.max.magnitude / 2 / RaceManager.numPlayers;

        //for (int i=0; i < RaceManager.numPlayers; i++)
        //{
        //    //PlayerInput player = GameObject.Instantiate(prefab[RaceManager.racerProfiles[i].car]).GetComponent<PlayerInput>();
        //    PlayerInput player = GameObject.Instantiate(prefab[0]).GetComponent<PlayerInput>();
        //    player.gameObject.name = player.currentControlScheme;



        //    player.transform.position = spawnZone.transform.position + spawnZone.transform.right * (player.playerIndex % 2 == 0 ? -1 : 1) * player.playerIndex * positioningWidth;
        //    player.transform.forward = this.transform.forward;


        //}
        foreach (var i in InputSystem.actions.controlSchemes)
        {
            Debug.Log(i.name);
        }



        for (int i = 0;i<RaceManager.racerProfiles.Count;i++)
        {
            PlayerInput player = playerManager.JoinPlayer(playerIndex:i, controlScheme:RaceManager.racerProfiles[i].controlScheme, pairWithDevice:RaceManager.racerProfiles[i].controlScheme.StartsWith("KB")?Keyboard.current:null); 
            player.gameObject.name = player.currentControlScheme;
            player.transform.position = spawnZone.transform.position + spawnZone.transform.right * (player.playerIndex % 2 == 0 ? -1 : 1) * player.playerIndex * positioningWidth;
            player.transform.forward = this.transform.forward;
            player.GetComponent<Racer>().SetCar(RaceManager.racerProfiles[i].car);
            player.GetComponent<Racer>().SetID(i);
        }
    }


    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log("PlayerAdded");
        Debug.Log(player.playerIndex);

        //positioningWidth = spawnZone.bounds.max.magnitude * 2f / playerManager.playerCount;
        //
        //player.transform.position = spawnZone.transform.position + spawnZone.transform.right * (player.playerIndex % 2 == 0 ? -1 : 1) * player.playerIndex * positioningWidth;
    }

    private void Update()
    {
    }
}
