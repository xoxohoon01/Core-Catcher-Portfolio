using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenEventListener : MonoBehaviour
{
    public GameObject LeftPistol;
    public GameObject RightPistol;
    public GameObject Shotgun;
    public GameObject SniperRifle;

    public void HideAll()
    {
        LeftPistol.SetActive(false);
        RightPistol.SetActive(false);
        Shotgun.SetActive(false);
        SniperRifle.SetActive(false);
    }
    public void HideLeft()
    {
        LeftPistol.SetActive(false);
        Shotgun.SetActive(false);
    }
    public void HideRight()
    {
        RightPistol.SetActive(false);
        SniperRifle.SetActive(false);
    }

    public void ShowLeftPistol()
    {
        LeftPistol.SetActive(true);
    }
    public void ShowRightPistol()
    {
        RightPistol.SetActive(true);
    }
    public void ShowShotgun()
    {
        Shotgun.SetActive(true);
    }
    public void ShowSniperRifle()
    {
        SniperRifle .SetActive(true);
    }

}
