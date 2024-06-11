using UnityEngine;
using UnityEngine.UI;

public class FloorInfo : MonoBehaviour
{
    [SerializeField] private Text floorText;
    [SerializeField] private Text enemiesText;

    public void SetFloor(int floor)
    {
        floorText.text = "Floor " + floor;
    }

    public void SetEnemiesLeft(int enemiesLeft)
    {
        enemiesText.text = enemiesLeft + " enemies left";
    }
}
