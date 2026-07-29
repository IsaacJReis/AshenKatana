using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject panelRoot;   // o objeto raiz do painel (pode deixar vazio pra usar este mesmo GameObject)

    [Header("Cena de destino")]
    [SerializeField] private string sceneToLoad;      // nome exato da cena (tem que estar no Build Settings)

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (panelRoot == null) panelRoot = gameObject;
        panelRoot.SetActive(false);
    }

    public void Open()
    {
        panelRoot.SetActive(true);
        IsOpen = true;
        Time.timeScale = 0f; // opcional: pausa o jogo com o painel aberto. Remove se não quiser isso
    }

    public void Close()
    {
        panelRoot.SetActive(false);
        IsOpen = false;
        Time.timeScale = 1f;
    }

    // Chame esse método no OnClick() do botão dentro do painel (ex: "Aceitar" / "Ir")
    public void OnAcceptButton()
    {
        Time.timeScale = 1f; // garante que o tempo normaliza antes de trocar de cena
        SceneFadeManager.Instance.FadeToScene(sceneToLoad);
        panelRoot.SetActive(false);
    }
}