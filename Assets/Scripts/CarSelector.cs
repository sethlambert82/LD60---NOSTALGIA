using UnityEngine;
using System.Collections;

public class CarSelector : MonoBehaviour
{
    public static int maxCars = 2;
    public string[] names = new string[2] { "Green Machine", "Boogie Mobile" };

    public int racerIndex;
    public int car;
    public Transform platter;
    [SerializeField] TMPro.TextMeshProUGUI label;
    Coroutine animation;

    public void NextCar()
    {
        if (animation!=null) return;
        car = (car + 1) % maxCars;
        animation = StartCoroutine(PlatterAnimation());
        label.text = names[car];
        MainMenu.SetCar(racerIndex, car);
    }

    public void PrevCar()
    {
        if(animation!=null)return;
        car = (car + maxCars - 1) % maxCars;
        animation = StartCoroutine(PlatterAnimation());
        label.text = names[car];
        MainMenu.SetCar(racerIndex, car);
    }


    IEnumerator PlatterAnimation()
    {
        float duration = 0.75f;
        float t = 0;
        float startAngle = car*180+180;
        while(t<duration)
        {
            platter.localRotation = Quaternion.Euler(0, 0,Mathf.Lerp(startAngle, startAngle+540, AnimationCurve.EaseInOut(0,0,1,1).Evaluate(t/duration)));


            t += Time.deltaTime;
            yield return null;
        }
        animation = null;
    }
}
