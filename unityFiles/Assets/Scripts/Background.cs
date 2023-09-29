using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject mountains, space;
    public static Background instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //slides mountains down smoothly
    //partial movements are spread out over time to prevent sharp jumps
    public IEnumerator moveMountains()
    {
        float totalMovement = 2f;      //totalMovment is the full distance the mountain will move
        float partialMovement = 0.06f; //totalMovement will be split up into smaller partialMovements
        while (totalMovement > 0)
        {
            totalMovement -= partialMovement;
            mountains.transform.Translate(0, -partialMovement, 0, Space.World);
            yield return new WaitForSeconds(0.03f);
        }
    }

    //transitions background from sky to space through use of image transparency
    //transparency adjustments are spread out over time to prevent sharp changes
    public IEnumerator transitionToSpace()
    {
        float totalMovement = .05f;    //totalMovment is the full amount the object will move
        float partialMovement = 0.01f; //totalMovement will be split up into smaller partialMovements
        while (totalMovement > 0)
        {
            totalMovement -= partialMovement;
            Color newColor = space.GetComponent<SpriteRenderer>().color;
            space.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, newColor.a + partialMovement);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
