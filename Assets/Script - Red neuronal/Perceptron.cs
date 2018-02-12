using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;

[System.Serializable]
public class Perceptron {

    public double rateAprender = 0.6;
    public Capa[] fLayers;
    public Funciones fTransferFunction;


    /// <summary>
    /// Crea una red neuronal perceptron multicapa
    /// </summary>
    /// <param name="capas"> Numero de capas que tendrá el perceptron (Ejemplo {2, 3, 1} para una red con 2 inputs, 1 capa oculta con 3 neuronas y 1 output</param>
    /// <param name="learningRate">Ratio de aprendizaje</param>
    /// <param name="funcion">Funcion de derivada</param>
    public Perceptron(int[] capas, double learningRate, Funciones funcion) {
        rateAprender = learningRate;
        fTransferFunction = funcion;

        fLayers = new Capa[capas.Length];

        for(int i = 0; i < capas.Length; i++) {
            if(i != 0) {
                fLayers[i] = new Capa(capas[i], capas[i - 1]);
            } else {
                fLayers[i] = new Capa(capas[i], 0);
            }
        }
    }

    /// <summary>
    /// Ejecuta la red neuronal
    /// </summary>
    /// <param name="input">input, debe coincidir con el valor inicial de entradas</param>
    /// <returns></returns>
    public double[] Ejecutar(double[] input) {
        int i;
        int j;
        int k;
        double nuevoValor;

        double[] output = new double[fLayers[fLayers.Length - 1].getLength()];

        // Introduce los inputs
        for(i = 0; i < fLayers[0].getLength(); i++) {
            fLayers[0].getNeurona(i).setValor(input[i]);
        }

        // Ejecuta: Capas ocultas y la output
        for(k = 1; k < fLayers.Length; k++) {
            for(i = 0; i < fLayers[k].getLength(); i++) {
                nuevoValor = 0f;
                for(j = 0; j < fLayers[k - 1].getLength(); j++)
                    nuevoValor += fLayers[k].getNeurona(i).getPesos(j) * fLayers[k - 1].getNeurona(j).getValor();

                nuevoValor += fLayers[k].getNeurona(i).getBias();

                fLayers[k].getNeurona(i).setValor(fTransferFunction.evaluar(nuevoValor));
            }
        }


        // Devuelve el output
        for(i = 0; i < fLayers[fLayers.Length - 1].getLength(); i++) {
            output[i] = fLayers[fLayers.Length - 1].getNeurona(i).getValor();
        }

        return output;
    }

    /// <summary>
    /// Algoritmo de "back propagation" por el perceptrón.
    /// Es el usado para que la red neuronal "aprenda" a base de valores correctos
    /// 
    /// La etapa de aprendizaje es muy importante y tiene que dar valores correctos si no
    /// la maquina dará valores no deseados posteriormente.
    /// 
    /// TO DO:
    /// La convergencia no está garantizada y es muy lenta, hay que usar como detención
    /// una regla entre los errores previos y actuales, y un número máximo de iteraciones.
    /// 
    /// </summary>
    /// <param name="input">Valores de los inputs (Del 0 al 1)</param>
    /// <param name="output">Valores deseado de salida (Del 0 al 1)</param>
    /// <returns>Error generado tras el output</returns>
    public double backPropagation(double[] input, double[] output) {
        double[] nOutput = Ejecutar(input);
        double error;
        int i;
        int j;
        int k;
        
        // Calculamos el error del output
        for(i = 0; i < fLayers[fLayers.Length - 1].getLength(); i++) {
            error = output[i] - nOutput[i];
            fLayers[fLayers.Length - 1].getNeurona(i).setDelta(error * fTransferFunction.evaluarDerivada(nOutput[i]));
        }


        for(k = fLayers.Length - 2; k >= 0; k--) {
            // Calcula el error de la capa actual y recalcula las deltas (Tambien llamados umbrales)
            for(i = 0; i < fLayers[k].getLength(); i++) {
                error = 0f;
                for(j = 0; j < fLayers[k + 1].getLength(); j++)
                    error += fLayers[k + 1].getNeurona(j).getDelta() * fLayers[k + 1].getNeurona(j).getPesos(i);

                fLayers[k].getNeurona(i).setDelta(error * fTransferFunction.evaluarDerivada(fLayers[k].getNeurona(i).getValor()));
            }
            
            // Actualiza los pesos de la siguiente capa
            for(i = 0; i < fLayers[k + 1].getLength(); i++) {
                for(j = 0; j < fLayers[k].getLength(); j++)
                    fLayers[k + 1].getNeurona(i).aumentarPesos(j, rateAprender * fLayers[k + 1].getNeurona(i).getDelta() * fLayers[k].getNeurona(j).getValor());
                fLayers[k + 1].getNeurona(i).aumentarBias(rateAprender * fLayers[k + 1].getNeurona(i).getDelta());
            }
        }

        // Calculamos el error final (El que será devuelto)
        error = 0.0;

        for(i = 0; i < output.Length; i++) {
            error += Math.Abs(nOutput[i] - output[i]);

            //Debug.Log(output[i]+" "+new_output[i]);
        }

        error = error / output.Length;
        return error;
    }

    /// <summary>
    /// 
    /// Guarda una red en un archivo para poder ser
    /// posteriormente usado.
    /// 
    /// </summary>
    /// <param name="path"> Ruta del archivo a guardar</param>
    /// <returns>Devuelve si ha habido algun error</returns>
    public bool Guardar(string path) {
        try {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, this);
            file.Close();
        } catch(Exception e) {
            Debug.LogWarning("ERROR: " + e);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// Carga la red neuronal en un archivo para poder ser 
    /// usado.
    /// 
    /// </summary>
    /// <param name="path"> Ruta del archivo </param>
    /// <returns> El perceptron cargado</returns>
    public static Perceptron load(String path) {
        //Si no existe ningun archivo en la ruta ignora la petición de carga.
        if(File.Exists(path))
            return null;

        try {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Perceptron net = (Perceptron) bf.Deserialize(file);
            file.Close();

            return net;
        } catch(Exception e) {
            Debug.LogWarning("ERROR: " + e);
            return null;
        }
    }



    /// <summary>
    /// Constante de entendimiento
    /// </summary>
    /// <returns> rate</returns>
    public double getLearningRate() {
        return rateAprender;
    }


    /// <summary>
    /// Cambia el rate
    /// </summary>
    /// <param name="rate"></param>
    public void setLearningRate(double rate) {
        rateAprender = rate;
    }


    /// <summary>
    /// Pone una nueva función para evaluar la función.
    /// </summary>
    /// <param name="fun"></param>
    public void setTransferFunction(Funciones fun) {
        fTransferFunction = fun;
    }



    /// <summary>
    /// Devuelve el tamaño del input
    /// </summary>
    /// <returns></returns>
    public int getInputLayerSize() {
        return fLayers[0].getLength();
    }
    
    /// <summary>
    /// Devuelve el tamaño del output
    /// </summary>
    /// <returns></returns>
    public int getOutputLayerSize() {
        return fLayers[fLayers.Length - 1].getLength();
    }
    
    
    public int getLayerCount() {
        return fLayers.Length;
    }

    public int getLayerSize(int index) {
        return fLayers[index].getLength();
    }

    public Neurona getNeuronaFromLayer(int layer, int index) {
        return fLayers[layer].getNeurona(index);
    }
}
