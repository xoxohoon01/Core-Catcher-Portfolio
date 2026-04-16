using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenEventListener : MonoBehaviour
{
    public bool isUse = true;
    public GameObject LeftPistol;
    public GameObject RightPistol;
    public GameObject Shotgun;
    public GameObject SniperRifle;

    public void HideAll()
    {
        if (!isUse) return;

        LeftPistol.SetActive(false);
        RightPistol.SetActive(false);
        Shotgun.SetActive(false);
        SniperRifle.SetActive(false);
    }
    public void HideLeft()
    {
        if (!isUse) return;

        LeftPistol.SetActive(false);
        Shotgun.SetActive(false);
    }
    public void HideRight()
    {
        if (!isUse) return;

        RightPistol.SetActive(false);
        SniperRifle.SetActive(false);
    }

    public void ShowLeftPistol()
    {
        if (!isUse) return;

        LeftPistol.SetActive(true);
    }
    public void ShowRightPistol()
    {
        if (!isUse) return;

        RightPistol.SetActive(true);
    }
    public void ShowShotgun()
    {
        if (!isUse) return;

        Shotgun.SetActive(true);
    }
    public void ShowSniperRifle()
    {
        if (!isUse) return;

        SniperRifle .SetActive(true);
    }

}
