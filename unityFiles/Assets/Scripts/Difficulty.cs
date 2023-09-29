using UnityEngine;

public class Difficulty : MonoBehaviour
{
    float difficultySpeed; //will be used to control speed of moving block
    public static Difficulty instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void setDifficultySpeed(float speed)
    {
        difficultySpeed = speed;
    }
    public float getDifficultySpeed()
    {
        return difficultySpeed;
    }
}
