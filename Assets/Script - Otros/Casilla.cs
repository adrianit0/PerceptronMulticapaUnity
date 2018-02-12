using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {

    public TextMesh valor;
    public TextMesh bias;
    public TextMesh delta;

    public TextMesh[] pesos;

    Neurona neurona;

    public void Configurar(Neurona neurona) {
        this.neurona = neurona;

        valor.text = Round(neurona.getValor()).ToString();
        bias.text = Round(neurona.getBias()).ToString();
        delta.text = Round(neurona.getDelta()).ToString();

        for(int i = 0; i < Mathf.Min(pesos.Length, neurona.getPesosLength()); i++) {
            pesos[i].text = Round(neurona.getPesos(i)).ToString();
        }
    }

    public void Actualizar() {
        valor.text = Round(neurona.getValor()).ToString();
        bias.text = Round(neurona.getBias()).ToString();
        delta.text = Round(neurona.getDelta()).ToString();

        for(int i = 0; i < Mathf.Min(pesos.Length, neurona.getPesosLength()); i++) {
            pesos[i].text = Round(neurona.getPesos(i)).ToString();
        }
    }

    double Round(double valor) {
        return System.Math.Round(valor, 4);
    }
}
