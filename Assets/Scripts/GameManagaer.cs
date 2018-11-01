using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class GameManagaer : MonoBehaviour
{
    public static GameManagaer Instant;

    [SerializeField]
    private GameObject _playerPrefab = null;

    [SerializeField]
    private GameObject _arrowPrefab = null;

    [SerializeField]
    private GameObject _spawnerPrefab = null;

    [SerializeField]
    private GameObject _shieldPrefab = null;

    [SerializeField]
    private Button _newGameButton = null;

    private EntityManager _entityManager;

    public GameObject ArrowPrefab => _arrowPrefab;

    private void Awake()
    {
        if (Instant == null)
        {
            Instant = this;
        }

        _newGameButton.onClick.AddListener(NewGame);
    }

    private void Start()
    {
        Input.simulateMouseWithTouches = true;
        _entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }

    private void NewGame()
    {
        _entityManager.Instantiate(_playerPrefab);
        _entityManager.Instantiate(_spawnerPrefab);
        _entityManager.Instantiate(_shieldPrefab);
    }

    public void UpdateUI(bool gameStarted)
    {
        _newGameButton.gameObject.SetActive(!gameStarted);
    }
}
