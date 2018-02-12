using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01 : MonoBehaviour {

    Perceptron net;
    public int iteracciones=10000;
    public bool debug;

    public GameObject prefab;

    /// <summary>
    /// 
    /// TEST 01:
    /// A partir de 10000 iteracciones, aprender como funciona la puerta lógica AND
    /// 2 inputs, 1 capa oculta con 2 neuronas y un input
    /// 
    /// Valores deseados:
    /// x1 x2 | y
    /// 0  0  | 0
    /// 0  1  | 0
    /// 1  0  | 0
    /// 1  1  | 1
    /// 
    /// </summary>
    public void Start() {
        int[] capas = new int[] { 2, 2, 1 };

        net = new Perceptron(capas, 0.6f, new Sigmoide());

        /* Aprendiendo */
        for(int i = 0; i < iteracciones; i++) {
            double[] _inputs = new double[] { Mathf.Round(Random.value), Mathf.Round(Random.value) };
            double[] _output = new double[1];
            double error;

            // Si no cumple con la tabla devuelve 0, si cumple devolverá 1
            if((_inputs[0] == _inputs[1]) && (_inputs[0] == 1))
                _output[0] = 1f;
            else
                _output[0] = 0f;

            if (debug)
                Debug.Log(_inputs[0] + " and " + _inputs[1] + " = " + _output[0]);

            error = net.backPropagation(_inputs, _output);
            if (debug)
                Debug.Log("Error en el paso " + i + " es " + error);
        }

        Debug.Log("APRENDIZAJE COMPLETADO!");

        /* Test */
        Test(new double[] { 0, 0 });
        Test(new double[] { 0, 1});
        Test(new double[] { 1, 0 });
        Test(new double[] { 1, 1 });

        InstanciarGrafico();
    }

    void Test(double[] inputs) {
        double[] output = net.Ejecutar(inputs);

        Debug.Log(inputs[0] + " y " + inputs[1] + " = " + System.Math.Round(output[0]) + " (" + output[0] + ")");
    }

    void InstanciarGrafico() {
        if(prefab == null)
            return;

        for(int x = 0; x < net.getLayerCount(); x++) {
            for(int y = 0; y < net.getLayerSize(x); y++) {
                CrearInstancia(net.getNeuronaFromLayer(x, y), x, y);
            }
        }
    }

    void CrearInstancia(Neurona neurona, int x, int y) {
        GameObject _obj = Instantiate(prefab);
        Casilla script = _obj.GetComponent<Casilla>();
        _obj.name = "Neurona "+x+ " - "+y;

        _obj.transform.position = new Vector3(-4 + 3 * x, 2 - 2 *y, 0);

        if(script == null)
            return;

        script.Configurar(neurona);
    }
}
