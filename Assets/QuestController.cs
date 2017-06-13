using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class QuestController : MonoBehaviour
{
    //Variaveis TAB Criar Quest
    [Header("Variaveis TAB Criar Quests")]
    [SerializeField] private InputField questName = null;
    [SerializeField] private InputField questDescription = null;
    [SerializeField] private InputField questReward = null;
    [SerializeField] private InputField questScore = null;
    [SerializeField] private InputField questDias = null;
    [SerializeField] private Dropdown dropdownFilhos = null;
    private List<Quest> listOfQuests = new List<Quest>();
    private List<string> questFilhos = new List<string>();
    [SerializeField] private Button btnAddFilho = null;
    [SerializeField] private Button btnCriarQuest = null;

    private bool removerFilho = false;

    public List<Quest> ListOfQuests
    {
        get{ return listOfQuests; }
        set{ listOfQuests = value; }
    }

    //--------------------------------------------------------------------

    //Variaveis TAB Quests
    [Header("Variaveis TAB Quests")]
    [SerializeField] private Dropdown dropdownFilhos2 = null;
    [SerializeField] private Dropdown dropdownQuests = null;
    private List<string> questsFilhoSelec = new List<string>();
    [SerializeField] private Text description = null;
    [SerializeField] private Text scoreNeeded = null;
    [SerializeField] private Text reward = null;
    [SerializeField] private Text filhoScore = null;
    [SerializeField] private Button btnResgatar = null;
    [SerializeField] private Slider sliderProgresso = null;
    [SerializeField] private Text qntdDias = null;
    [SerializeField] private Toggle toggleIlimitado = null;
    [SerializeField] private GameObject panelSemQuest = null;

    private bool attTempo = false;
    private DateTime tempoQuest;

    public bool AttTempo
    {
        get{ return attTempo; }
        set{ attTempo = value; }
    }

    //--------------------------------------------------------------------

    //Variaveis TAB QuestsCompleted
    [Header("Variaveis TAB QuestsCompleted")]
    [SerializeField] private Text frasePrincipal = null;
    [SerializeField] private Text fraseRecompensa = null;

    public static QuestController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (SaveAndLoad.instance.Load(ControllerGeral.instance.UserID) != null)
        {
            listOfQuests = SaveAndLoad.instance.Load(ControllerGeral.instance.UserID).listQuests;
        }
    }

    void Update()
    {
        if (attTempo)
        {
            TimeSpan tempoRestante = tempoQuest.Subtract(DateTime.Now);
            if (tempoRestante.Hours <= 0 && tempoRestante.Minutes <= 0 && tempoRestante.Seconds <= 0)
            {
                qntdDias.text = "Desafio Expirado";
            }
            else
            {
                qntdDias.text = tempoRestante.Hours + ":" + tempoRestante.Minutes + ":" + tempoRestante.Seconds;
            }
        }
    }

    public void CriarQuest()
    {
        DateTime dataLimite;
        if (toggleIlimitado.isOn)
        {
            dataLimite = DateTime.Now.AddDays(9999);
        }
        else
        {
            dataLimite = DateTime.Now.AddDays(double.Parse(questDias.text));
        }
        Quest newQuest = new Quest(questName.text, questDescription.text, questReward.text, int.Parse(questScore.text), dataLimite);
        for (int i = 0; i < questFilhos.Count; i++)
        {
            newQuest.Filhos.Add(questFilhos[i]);
        }
        listOfQuests.Add(newQuest);
        dropdownFilhos.value = 0;
        questName.text = "";
        questDescription.text = "";
        questReward.text = "";
        questScore.text = "";
        questDias.text = "";
        toggleIlimitado.isOn = false;
        VerificarToggle();
        SaveAndLoad.instance.Save(ControllerGeral.instance.ListaFilhos, listOfQuests, ControllerGeral.instance.UserID);
        ResetAll();
    }

    public void ResetAll()
    {
        dropdownFilhos.value = 0;
        questName.text = "";
        questDescription.text = "";
        questReward.text = "";
        questScore.text = "";
        questDias.text = "";
        toggleIlimitado.isOn = false;
        VerificarToggle();
        questFilhos.Clear();
        AttBtnAddFilho();
    }

    public void AddFilhoQuest()
    {
        if (removerFilho)
        {
            questFilhos.Remove(dropdownFilhos.options[dropdownFilhos.value].text);
        }
        else if (!questFilhos.Contains(dropdownFilhos.options[dropdownFilhos.value].text))
        {
            questFilhos.Add(dropdownFilhos.options[dropdownFilhos.value].text);
        }
        AttBtnAddFilho();
    }

    public void AttQuestTab()
    {
        if (ControllerGeral.instance.ListaFilhos.Count != 0)
        {
            string filhoSelecionado = dropdownFilhos2.options[dropdownFilhos2.value].text;
            questsFilhoSelec.Clear();
            for (int i = 0; i < listOfQuests.Count; i++)
            {
                if (listOfQuests[i].Filhos.Contains(filhoSelecionado))
                {
                    questsFilhoSelec.Add(listOfQuests[i].Name);
                }
            }
            dropdownQuests.options.Clear();
            dropdownQuests.AddOptions(questsFilhoSelec);
            if (questsFilhoSelec.Count == 0)
            {
                panelSemQuest.SetActive(true);
            }
            else
            {
                panelSemQuest.SetActive(false);
                for (int i = 0; i < listOfQuests.Count; i++)
                {
                    if (listOfQuests[i].Filhos.Contains(filhoSelecionado) && listOfQuests[i].Name == dropdownQuests.options[dropdownQuests.value].text)
                    {
                        description.text = listOfQuests[i].Description;
                        scoreNeeded.text = listOfQuests[i].Score.ToString();
                        reward.text = listOfQuests[i].Reward;

                        int mesAtual = DateTime.Now.Month;
                        int diaAtual = DateTime.Now.Day;

                        TimeSpan tempoRestante = listOfQuests[i].DataLimite.Subtract(DateTime.Now);

                        if (tempoRestante.Days > 99)
                        {
                            attTempo = false;
                            qntdDias.text = "Tempo ilimitado";
                        }
                        else
                        {

                            if (listOfQuests[i].DataLimite.Month == mesAtual)
                            {
                                if (listOfQuests[i].DataLimite.Day == diaAtual || listOfQuests[i].DataLimite.Day == (diaAtual + 1))
                                {
                                    tempoQuest = listOfQuests[i].DataLimite;
                                    attTempo = true;
                                }
                                else
                                {
                                    attTempo = false;
                                    if (int.Parse(tempoRestante.Days.ToString()) != 1)
                                    {
                                        qntdDias.text = tempoRestante.Days + " dias";
                                    }
                                    else
                                    {
                                        qntdDias.text = tempoRestante.Days + " dia";
                                    }
                                }
                            }
                            else
                            {
                                qntdDias.text = tempoRestante.Days + " dias";
                            }
                        }

                        for (int y = 0; y < ControllerGeral.instance.ListaFilhos.Count; y++)
                        {
                            if (ControllerGeral.instance.ListaFilhos[y].Nome == filhoSelecionado)
                            {
                                filhoScore.text = ControllerGeral.instance.ListaFilhos[y].TotalPontos.ToString();
                                if (int.Parse(filhoScore.text) < 0)
                                {
                                    sliderProgresso.value = 0;
                                }
                                else
                                {
                                    sliderProgresso.value = float.Parse(filhoScore.text) / float.Parse(scoreNeeded.text);
                                }

                                if (int.Parse(filhoScore.text) >= int.Parse(scoreNeeded.text) && qntdDias.text != "Desafio Expirado")
                                {
                                    btnResgatar.interactable = true;
                                }
                                else
                                {
                                    btnResgatar.interactable = false;
                                }
                            }   
                        }
                    }
                }
            }
        }
        else
        {
            panelSemQuest.SetActive(true);
        }
    }

    public void AttBtnAddFilho()
    {
        if (questFilhos.Contains(dropdownFilhos.options[dropdownFilhos.value].text))
        {
            ColorBlock x = btnAddFilho.colors;
            x.normalColor = Color.red;
            x.highlightedColor = Color.red;
            btnAddFilho.colors = x;
            btnAddFilho.GetComponentInChildren<Text>().text = "Remover filho do desafio";
            removerFilho = true;
        }
        else
        {
            ColorBlock x = btnAddFilho.colors;
            x.normalColor = Color.green;
            x.highlightedColor = Color.green;
            btnAddFilho.colors = x;
            btnAddFilho.GetComponentInChildren<Text>().text = "Adicionar filho ao desafio";
            removerFilho = false;
        }
    }

    public void ExcluirQuest()
    {
        for (int i = 0; i < ListOfQuests.Count; i++)
        {
            if (ListOfQuests[i].Name == dropdownQuests.options[dropdownQuests.value].text)
            {
                ListOfQuests[i].Filhos.Remove(dropdownFilhos2.options[dropdownFilhos2.value].text);
                if (listOfQuests[i].Filhos.Count == 0)
                {
                    ListOfQuests.RemoveAt(i);
                }
            }
        }
        dropdownQuests.value = dropdownQuests.value - 1;
        ControllerGeral.instance.SaveListaFilhos();
        AttQuestTab();
    }

    public void AttQuestCompleted()
    {
        frasePrincipal.text = "<color=red>" + dropdownFilhos2.options[dropdownFilhos2.value].text + "</color> completou o desafio  <color=red>" + dropdownQuests.options[dropdownQuests.value].text + "</color> e ganhou:";
        for (int i = 0; i < ListOfQuests.Count; i++)
        {
            if (ListOfQuests[i].Name == dropdownQuests.options[dropdownQuests.value].text && ListOfQuests[i].Filhos.Contains(dropdownFilhos2.options[dropdownFilhos2.value].text))
            {
                fraseRecompensa.text = ListOfQuests[i].Reward;
                for (int y = 0; y < ControllerGeral.instance.ListaFilhos.Count; y++)
                {
                    if (ControllerGeral.instance.ListaFilhos[y].Nome == dropdownFilhos2.options[dropdownFilhos2.value].text)
                    {
                        ControllerGeral.instance.ListaFilhos[y].TotalPontos = ControllerGeral.instance.ListaFilhos[y].TotalPontos - ListOfQuests[i].Score;
                        ControllerGeral.instance.ListaFilhos[y].ListaAtividades.Add(dropdownQuests.options[dropdownQuests.value].text);
                        ControllerGeral.instance.ListaFilhos[y].ListaPontos.Add(ListOfQuests[i].Score * -1);
                        ControllerGeral.instance.SaveListaFilhos();
                    }   
                }
            }
        }
    }

    public void VerificarTodosOsCampos()
    {
        if (questName.text != "" && questDescription.text != "" && questReward.text != "" && questFilhos.Count != 0 && questScore.text != "")
        {
            if (toggleIlimitado.isOn || (questDias.text != "" && int.Parse(questDias.text) > 0))
            {
                btnCriarQuest.interactable = true;
            }
            else
            {
                btnCriarQuest.interactable = false;
            }
        }
        else
        {
            btnCriarQuest.interactable = false;
        }
    }

    public void VerificarToggle()
    {
        if (toggleIlimitado.isOn)
        {
            questDias.interactable = false;
        }
        else
        {
            questDias.interactable = true;
        }
    }

}

[System.Serializable]
public class Quest
{
    [SerializeField] private string name = null;
    [SerializeField] private string description = null;
    [SerializeField] private string reward = null;
    [SerializeField] private int score = 0;
    [SerializeField] private DateTime dataLimite;
    [SerializeField] private List<string> filhos = new List<string>();

    public Quest(string _name, string _description, string _reward, int _scoreNeeded, DateTime _dataLimite)
    {
        name = _name;
        description = _description;
        reward = _reward;
        score = _scoreNeeded;
        dataLimite = _dataLimite;
    }

    public string Name
    {
        get{ return name; }
    }

    public string Description
    {
        get{ return description; }
    }

    public string Reward
    {
        get{ return reward; }
    }

    public List<string> Filhos
    {
        get{ return filhos; }
        set{ filhos = value; }
    }

    public int Score
    {
        get{ return score; }
    }

    public DateTime DataLimite
    {
        get{ return dataLimite; }
    }

}
