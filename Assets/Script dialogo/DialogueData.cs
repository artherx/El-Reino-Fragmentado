using UnityEngine;

[System.Serializable]
public class DialogoData
{
    public string nombre;
    [TextArea(2, 5)]
    public string texto;
}