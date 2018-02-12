using UnityEngine; //PARA LOS RANDOMS

public class Neurona {

    public double Value;
    public double[] Weights;
    public double Bias;
    public double Delta;

    public Neurona(int prevLayerSize) {
        Weights = new double[prevLayerSize];
        Bias = Random.value / 10000000000000.0;
        Delta = Random.value / 10000000000000.0;
        Value = Random.value / 10000000000000.0;

        for(int i = 0; i < Weights.Length; i++)
            Weights[i] = Random.value/ 10000000000000.0;
    }

    public double getValor() {
        return Value;
    }

    public double getPesos(int index) {
        return Weights[index];
    }

    public int getPesosLength() {
        return Weights.Length;
    }

    public double getBias () {
        return Bias;
    }

    public double getDelta() {
        return Delta;
    }

    public void setValor (double nValor) {
        Value =nValor;
    }

    public void setPesos(int index, double valor) {
        Weights[index] = valor;
    }

    public void aumentarPesos(int index, double aumento) {
        Weights[index] += aumento;
    }

    public void setBias(double valor) {
        Bias = valor;
    }

    public void aumentarBias(double valor) {
        Bias += valor;
    }

    public void setDelta(double valor) {
        Delta = valor;
    }
}