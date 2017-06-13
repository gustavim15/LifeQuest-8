using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControllerGeral : MonoBehaviour
{
    [SerializeField] private InputField ifNomeNovoFilho = null;
    [SerializeField] private InputField ifIdadeNovoFilho = null;
    [SerializeField] List<Dropdown> listDropdownFilhos = null;
    [SerializeField] private List<Filho> listaFilhos = new List<Filho>();
    [SerializeField] private int filhoIndex = 0;
    private bool sexoFilho = true;
    private bool sexoEscolhido = false;
    [SerializeField] private Button btnAddFilho = null;
    [SerializeField] private GameObject panelNenhumFilho = null;

    public List<Filho> ListaFilhos
    {
        get{ return listaFilhos; }
        set{ listaFilhos = value; }
    }

    public int FilhoIndex
    {
        get{ return filhoIndex; }
        set{ filhoIndex = value; }
    }

    public bool SexoFilho
    {
        get{ return sexoFilho; }
        set{ sexoFilho = value; }
    }

    public bool SexoEscolhido
    {
        get{ return sexoEscolhido; }
        set{ sexoEscolhido = value; }
    }

    //-----------------------------------------------------------------
    [Header("Painel Filho\n\n")]
    [SerializeField] private Text nomeFilhoSelecionado = null;

    //-----------------------------------------------------------------
    [Header("Painel2\n\n")]
    [SerializeField] private Text nomeFilhoPainel2 = null;
    [SerializeField] private Text pontuacaoTotalPainel2 = null;
    [SerializeField] private Dropdown dropdownPainel2 = null;
    [SerializeField] private Text acaoPainel2 = null;
    [SerializeField] private Text pontoPainel2 = null;
    [SerializeField] private GameObject painelNenhumaAcao = null;



    public static ControllerGeral instance;
    [SerializeField] private string userID = null;

    public string UserID
    {
        get{ return userID; }
        set{ userID = value; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (SaveAndLoad.instance.Load(UserID) != null)
        {
            listaFilhos = SaveAndLoad.instance.Load(UserID).listFilhos;
            AtualizarListaFilhos();
        }
    }


    public void AtualizarPainelFilho()
    {
        if (listaFilhos.Count != 0)
        {
            FilhoIndex = listDropdownFilhos[0].value;
            nomeFilhoSelecionado.text = listaFilhos[listDropdownFilhos[0].value].Nome;
        }
    }


    public void CriarNovoFilho()
    {
        Filho novoFilho = new Filho(ifNomeNovoFilho.text, int.Parse(ifIdadeNovoFilho.text), SexoFilho);
        listaFilhos.Add(novoFilho);
        SaveAndLoad.instance.Save(listaFilhos, QuestController.instance.ListOfQuests, UserID);
        AtualizarListaFilhos();
    }

    public void SaveListaFilhos()
    {
        SaveAndLoad.instance.Save(listaFilhos, QuestController.instance.ListOfQuests, UserID);
    }

    public void AtualizarListaFilhos()
    {
        List<string> nomeFilhos = new List<string>();
        foreach (Filho x in listaFilhos)
        {
            nomeFilhos.Add(x.Nome);
        }

        for (int i = 0; i < listDropdownFilhos.Count; i++)
        {
            listDropdownFilhos[i].options.Clear();
            listDropdownFilhos[i].AddOptions(nomeFilhos);   
        }
    }

    public void AtualizarPainel2()
    {
        pontuacaoTotalPainel2.text = "Pontuação: " + listaFilhos[filhoIndex].TotalPontos;
        nomeFilhoPainel2.text = nomeFilhoSelecionado.text;
        if (ListaFilhos[filhoIndex].ListaAtividades.Count > 0)
        {
            painelNenhumaAcao.SetActive(false);
            dropdownPainel2.AddOptions(ListaFilhos[filhoIndex].ListaAtividades);
            acaoPainel2.text = listaFilhos[filhoIndex].ListaAtividades[dropdownPainel2.value];
            if (listaFilhos[filhoIndex].ListaPontos[dropdownPainel2.value] > 0)
            {
                pontoPainel2.color = Color.green;
            }
            else
            {
                pontoPainel2.color = Color.red;
            }
            pontoPainel2.text = listaFilhos[filhoIndex].ListaPontos[dropdownPainel2.value].ToString();
        }
        else
        {
            painelNenhumaAcao.SetActive(true);
        }
    }

    public void TrocarAcaoPainel2()
    {
        if (listaFilhos[filhoIndex].ListaPontos[dropdownPainel2.value] > 0)
        {
            pontoPainel2.color = Color.green;
        }
        else
        {
            pontoPainel2.color = Color.red;
        }
        acaoPainel2.text = listaFilhos[filhoIndex].ListaAtividades[dropdownPainel2.value];
        pontoPainel2.text = listaFilhos[filhoIndex].ListaPontos[dropdownPainel2.value].ToString();
    }

    public void AtualizarDropdDownFilhos()
    {
        for (int i = 0; i < listDropdownFilhos.Count; i++)
        {
            listDropdownFilhos[i].value = 0;   
        }
        AtualizarPainelFilho();
    }

    public void VerificarBtnAddFilho()
    {
        if (sexoEscolhido && ifNomeNovoFilho.text.Length > 0 && ifIdadeNovoFilho.text.Length > 0)
        {
            btnAddFilho.interactable = true;
        }
        else
        {
            btnAddFilho.interactable = false;
        }
    }

    public void VerificarQntdFilhos()
    {
        if (listaFilhos.Count != 0)
        {
            panelNenhumFilho.SetActive(false);
        }
        else
        {
            panelNenhumFilho.SetActive(true);
        }
    }

}
