using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeIA : MonoBehaviour {

    Perceptron net;
    public double ratioAprendizaje = 0.6;
    public bool debug;

    public GameObject prefab;
    public List<Casilla> lista;
    // Use this for initialization
    void Start () {
        if(FindObjectsOfType<SnakeIA>().Length >= 2) {
            Destroy(this);
            return;
        }

        lista = new List<Casilla>();
        CrearRed();
        InstanciarGrafico();
        DontDestroyOnLoad(this);
	}

    void CrearRed() {
        int[] capas = new int[] { 12, 16, 4 };

        net = new Perceptron(capas, ratioAprendizaje, new Sigmoide());

        /* Aprendiendo */
        /*for(int i = 0; i < iteracciones; i++) {
            double[] _inputs = new double[] { Mathf.Round(Random.value), Mathf.Round(Random.value) };
            double[] _output = new double[1];
            double error;

            // Si no cumple con la tabla devuelve 0, si cumple devolverá 1
            if((_inputs[0] == _inputs[1]) && (_inputs[0] == 1))
                _output[0] = 1f;
            else
                _output[0] = 0f;

            if(debug)
                Debug.Log(_inputs[0] + " and " + _inputs[1] + " = " + _output[0]);

            error = net.backPropagation(_inputs, _output);
            if(debug)
                Debug.Log("Error en el paso " + i + " es " + error);
        }*/
        
    }

    public double[] Pedir(double[] inputs) {
        double[] output = net.Ejecutar(inputs);
        MostrarDatos();
        return output;
    }

    public double Aprender(double[] inputs, double[] outputEsperado) {
        double error = net.backPropagation(inputs, outputEsperado);
        MostrarDatos();
        return error;
    }

    void Test(double[] inputs) {
        double[] output = net.Ejecutar(inputs);

        Debug.Log(inputs[0] + " y " + inputs[1] + " = " + System.Math.Round(output[0]) + " (" + output[0] + ")");
    }

    void InstanciarGrafico() {
        if(prefab == null)
            return;

        GameObject parent = new GameObject();
        parent.transform.position = Vector3.zero;
        parent.transform.parent = this.transform;

        GameObject child;

        for(int x = 0; x < net.getLayerCount(); x++) {
            for(int y = 0; y < net.getLayerSize(x); y++) {
                child=CrearInstancia(net.getNeuronaFromLayer(x, y), x, y);
                child.transform.parent = parent.transform;
            }
        }
    }

    GameObject CrearInstancia(Neurona neurona, int x, int y) {
        GameObject _obj = Instantiate(prefab);
        Casilla script = _obj.GetComponent<Casilla>();
        _obj.name = "Neurona " + x + " - " + y;

        _obj.transform.position = new Vector3(-4 + 3 * x, 2 - 1.5f * y, 0);

        if(script == null)
            return _obj;

        lista.Add(script);
        script.Configurar(neurona);

        return _obj;
    }

    void MostrarDatos() {
        for(int i = 0; i < lista.Count; i++) {
            lista[i].Actualizar();
        }
    }
}
