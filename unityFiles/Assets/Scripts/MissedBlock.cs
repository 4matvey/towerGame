using UnityEngine;

public class MissedBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f); //Destroy after 10 seconds
    }
}