using UnityEngine;
using Unity.Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject hero;
    public GameObject knight;
    public CinemachineCamera cinemachineCam;

    void Start()
    {
        string player = PlayerPrefs.GetString("Player");

        hero.SetActive(false);
        knight.SetActive(false);

        GameObject activePlayer = null;

        if (player == "Hero")
        {
            hero.SetActive(true);
            activePlayer = hero;
        }
        else if (player == "Knight")
        {
            knight.SetActive(true);
            activePlayer = knight;
        }

        if (activePlayer != null)
        {
            cinemachineCam.Target.TrackingTarget = activePlayer.transform;
        }
    }
}
