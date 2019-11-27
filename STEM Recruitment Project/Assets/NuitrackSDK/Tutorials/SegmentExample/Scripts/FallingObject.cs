using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    int scoreValue = 5;

    bool active = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!active)
            return;

        active = false;

        if (gameObject.name == "button_score_1(Clone)")
        {
            MathGameProgress.instance.AddNumber("1", 1);
        }
        else if (gameObject.name == "button_score_2(Clone)")
        {
            MathGameProgress.instance.AddNumber("2", 2);
        }
        else if (gameObject.name == "button_score_3(Clone)")
        {
            MathGameProgress.instance.AddNumber("3", 3);
        }
        else if (gameObject.name == "button_score_4(Clone)")
        {
            MathGameProgress.instance.AddNumber("4", 4);
        }

        // by taking this out, you can move and drag around the balls
        // could be used in a different game
        Destroy(gameObject);

    }
}


