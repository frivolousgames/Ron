using UnityEngine;

public class JunkyardPlayerController : MonoBehaviour
{
    bool playerActivated;
    bool activated;
    Animator anim;

    [SerializeField]
    GameObject glassBreak;

    ///SWITCH///
    [SerializeField]
    GameObject beerBottle;
    [SerializeField]
    GameObject player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        anim.SetBool("playerActivated", playerActivated);
        playerActivated = JunkyardSceneController.playerActivated;
        PlayerActiveRoutine();
    }

    void PlayerActiveRoutine()
    {
        if (!activated)
        {
            if (playerActivated)
            {
                activated = true;
                beerBottle.SetActive(false);
            }
        }
    }

    public void SwitchPlayerProperties()
    {
        player.SetActive(true);
        gameObject.SetActive(false);
        JunkyardSceneController.playerReady = true;
    }

    public void GlassBreak()
    {
        glassBreak.SetActive(true);
    }
}
