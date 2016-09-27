using UnityEngine;
using System.Collections;

public class HookPart : MonoBehaviour {

    private bool inverse = false;

    // Use this for initialization
    void Start ()
    {
        HookWeapon.onRotate += Inverse;
	}

    void OnDestroy()
    {
        HookWeapon.onRotate -= Inverse;
    }

    public void Inverse()
    {
        Vector3 euler = gameObject.transform.rotation.eulerAngles;
        
        if (!inverse)
        {
            //set rotation to 180 in Y
            euler.y = 180.0f;
        }
        else
        {
            //set rotation to 0 in Y
            euler.y = 0.0f;
        }
        gameObject.transform.rotation = Quaternion.Euler(euler);
        inverse = !inverse;
    }
}
