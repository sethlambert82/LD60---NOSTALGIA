using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkPointIndex;
    private void OnTriggerEnter(Collider other)
    {
        Racer racer;
        if(other.TryGetComponent<Racer>(out racer))
        {
            Debug.Log("Racer Detected at " + checkPointIndex);
            racer.MarkProgress(checkPointIndex);
        }
    }
}
