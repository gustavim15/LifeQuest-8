using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FilhoController : MonoBehaviour
{

    private ControllerGeral controllerGeral;

    [SerializeField] private InputField novaAcao = null;
    [SerializeField] private InputField qntdPontos = null;
    [SerializeField] private Dropdown tipoAcao = null;


    // Use this for initialization
    void Start()
    {
        controllerGeral = FindObjectOfType<ControllerGeral>();
    }

    public void AdicionarNovaAcao()
    {
        controllerGeral.ListaFilhos[controllerGeral.FilhoIndex].ListaAtividades.Add(novaAcao.text);
        if (tipoAcao.value == 0)
        {
            int pontos = int.Parse(qntdPontos.text);
            if (pontos < 0)
            {
                pontos = pontos * -1;
            }
            controllerGeral.ListaFilhos[controllerGeral.FilhoIndex].ListaPontos.Add(pontos);
            controllerGeral.ListaFilhos[controllerGeral.FilhoIndex].TotalPontos += pontos;
        }
        else
        {
            int pontos = int.Parse(qntdPontos.text);
            if (pontos < 0)
            {
                pontos = pontos * -1;
            }
            controllerGeral.ListaFilhos[controllerGeral.FilhoIndex].ListaPontos.Add(pontos * -1);
            controllerGeral.ListaFilhos[controllerGeral.FilhoIndex].TotalPontos -= pontos;
        }

    }

    public void CheckPainel1QntdColor()
    {
        if (tipoAcao.value == 0)
        {
            GameObject.FindGameObjectWithTag("TextQntdPontos").GetComponent<Text>().color = Color.green;
        }
        else
        {
            GameObject.FindGameObjectWithTag("TextQntdPontos").GetComponent<Text>().color = Color.red;
        }
    }
}
