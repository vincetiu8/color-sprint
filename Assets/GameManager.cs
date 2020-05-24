using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static float difficulty;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject UI;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject tile;

    [SerializeField] private Text dialogueText;

    private PlayerController pc;
    private List<GameObject[]> tileCollection;
    private int row;
    private Coroutine enableUI, writeText;

    // Start is called before the first frame update
    void Start()
    {
        if (gm != null)
        {
            Destroy(this);
        }
        else
        {
            gm = this;
        }

        pc = player.GetComponent<PlayerController>();
        UI.SetActive(false);
        difficulty = 1;
        row = 0;
        tileCollection = new List<GameObject[]>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.dead && enableUI == null)
        {
            enableUI = StartCoroutine(EnableUI());
        }
    }

    public IEnumerator WriteText(string _message, float _speed)
    {
        ShowText(true);
        dialogueText.text = "";

        foreach (char c in _message)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                dialogueText.text = _message;
                yield break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(_speed);
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        yield return 0;
    }

    public void ShowText(bool _show)
    {
        dialogueText.transform.parent.gameObject.SetActive(_show);
    }

    private bool isIntInArray(int[] _array, int _item)
    {
        foreach (int i in _array)
        {
            if (i == _item)
            {
                return true;
            }
        }

        return false;
    }

    private int getNewRandomNumber(int[] _blacklist)
    {
        while (true)
        {
            int _rand = Random.Range(0, 9);
            if (!isIntInArray(_blacklist, _rand))
            {
                return _rand;
            }
        }
    }

    private void Populate(int[] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }

    public IEnumerator CreateRow(int _rowNumber, float _fallSpeed)
    {
        int[] _nums = new int[9];
        Populate(_nums, -1);
        for (int i = 0; i < 9; i++)
        {
            int _rand = getNewRandomNumber(_nums);
            yield return StartCoroutine(CreateTile(_rand, _rowNumber, Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1), _fallSpeed));
            _nums[i] = _rand;
        }
    }

    public IEnumerator CreateTile(int x, int z, Color _color, float _fallSpeed)
    {
        if (z >= tileCollection.Count)
        {
            tileCollection.Add(new GameObject[9]);
        }
        GameObject _tile = Instantiate(tile, new Vector3(x, 10, z), Quaternion.identity);
        _tile.GetComponent<Renderer>().material.color = _color;
        tileCollection[z][x] = _tile;

        while (_tile.transform.position.y > 0)
        {
            _tile.transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);
            yield return null;
        }
        _tile.transform.position = new Vector3(x, 0, z);
    }

    private IEnumerator EnableUI()
    {
        UI.SetActive(true);
        RectTransform rt = UI.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -Screen.height);
        yield return null;

        float _rate = Screen.height / 1f;
        while (rt.anchoredPosition.y < 0)
        {
            rt.anchoredPosition += new Vector2(0, _rate);
            yield return null;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
