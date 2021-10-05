using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CountryEnum
{
    France,
    Germany,
    England,
    Netherlands,
    Belgium,
    Switzerland
}

[System.Serializable]
public class CountryCard
{
    public Sprite blackMap;
    public Sprite cardBg;

    public string countryName;
}
