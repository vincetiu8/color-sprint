using System.Collections;
using UnityEngine;

public class Level1Script : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private PlayerController pc;
    private GameManager gm;
    private Rigidbody rb;

    private Coroutine game;

    private void Start()
    {
        gm = GetComponent<GameManager>();
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();

        if (game == null)
        {
            game = StartCoroutine(Game());
        }
    }

    private IEnumerator Game()
    {
        player.SetActive(false);
        yield return StartCoroutine(gm.WriteText("Hello there...", 0.05f));
        yield return StartCoroutine(gm.WriteText("... Welcome to Color Balance...", 0.05f));
        gm.ShowText(false);
        yield return StartCoroutine(gm.CreateTile(4, 0, Color.gray, 5));

        player.SetActive(true);
        pc.enabled = false;
        player.transform.position = new Vector3(4, 10, 0);
        yield return 0;

        float startPosition, endPosition;
        do
        {
            startPosition = player.transform.position.y;
            yield return new WaitForSeconds(0.1f);
            endPosition = player.transform.position.y;
        } while (startPosition != endPosition);

        yield return StartCoroutine(gm.WriteText("This is you.", 0.05f));
        yield return StartCoroutine(gm.WriteText("A little sphere.", 0.05f));
        gm.ShowText(false);
        for (int i = 1; i < 5; i++)
        {
            StartCoroutine(gm.CreateTile(4, i, Color.gray, 5));
            yield return new WaitForSeconds(0.25f);
        }
        yield return StartCoroutine(gm.CreateTile(4, 5, Color.gray, 5));
        yield return StartCoroutine(gm.WriteText("How bout you try walking forward?", 0.05f));

        pc.enabled = true;
        pc.movable = true;
    }
}
