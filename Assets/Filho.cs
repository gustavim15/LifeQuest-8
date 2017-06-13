using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Filho
{
    [SerializeField] private string nome;
    private int idade;
    private bool sexo;

    [SerializeField] private List<string> listaAtividades = new List<string>();
    [SerializeField] private List<int> listaPontos = new List<int>();
    private int totalPontos;


    public Filho(string nome, int idade, bool sexo)
    {
        this.nome = nome;
        this.idade = idade;
        this.sexo = sexo;
    }

    public string Nome
    {
        get{ return nome; }
        set{ nome = value; }
    }

    public int Idade
    {
        get{ return idade; }
        set{ idade = value; }
    }

    public bool Sexo
    {
        get{ return sexo; }
        set{ sexo = value; }
    }

    public List<string> ListaAtividades
    {
        get{ return listaAtividades; }
        set{ listaAtividades = value; }
    }

    public List<int> ListaPontos
    {
        get{ return listaPontos; }
        set{ listaPontos = value; }
    }

    public int TotalPontos
    {
        get{ return totalPontos; }
        set{ totalPontos = value; }
    }
}
