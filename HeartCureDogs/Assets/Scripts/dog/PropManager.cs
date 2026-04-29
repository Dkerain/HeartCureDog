using UnityEngine;
using System.Collections.Generic;

public class PropManager : MonoBehaviour
{
    [System.Serializable]
    public class Prop
    {
        public string propName;
        public GameObject propObject;
    }

    public List<Prop> props;

    public void ShowProp(string propName)
    {
        Prop p = props.Find(x => x.propName == propName);
        if (p.propObject != null)
            p.propObject.SetActive(true);
    }

    public void HideProp(string propName)
    {
        Prop p = props.Find(x => x.propName == propName);
        if (p.propObject != null)
            p.propObject.SetActive(false);
    }
}