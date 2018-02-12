using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron2 {

    protected double fLearningRate = 0.6;
    protected Capa[] fLayers;
    protected Funciones fTransferFunction;



    /**
	 * Crea una rete neuronale mlp
	 * 
	 * @param layers Numero di neuroni per ogni layer
	 * @param learningRate Costante di apprendimento
	 * @param fun Funzione di trasferimento
	 */
    public Perceptron2(int[] layers, double learningRate, Funciones fun) {
        fLearningRate = learningRate;
        fTransferFunction = fun;

        fLayers = new Capa[layers.Length];

        for(int i = 0; i < layers.Length; i++) {
            if(i != 0) {
                fLayers[i] = new Capa(layers[i], layers[i - 1]);
            } else {
                fLayers[i] = new Capa(layers[i], 0);
            }
        }
    }



    /**
	 * Esegui la rete
	 * 
	 * @param input Valori di input
	 * @return Valori di output restituiti dalla rete
	 */
    public double[] Ejecutar(double[] input) {
        int i;
        int j;
        int k;
        double new_value;

        double[] output = new double[fLayers[fLayers.Length - 1].Length];

        // Put input
        for(i = 0; i < fLayers[0].Length; i++) {
            fLayers[0].Neurons[i].Value = input[i];
        }

        // Execute - hiddens + output
        for(k = 1; k < fLayers.Length; k++) {
            for(i = 0; i < fLayers[k].Length; i++) {
                new_value = 0.0f;
                for(j = 0; j < fLayers[k - 1].Length; j++)
                    new_value += fLayers[k].Neurons[i].Weights[j] * fLayers[k - 1].Neurons[j].Value;

                new_value += fLayers[k].Neurons[i].Bias;

                fLayers[k].Neurons[i].Value = fTransferFunction.evaluar(new_value);
            }
        }


        // Get output
        for(i = 0; i < fLayers[fLayers.Length - 1].Length; i++) {
            output[i] = fLayers[fLayers.Length - 1].Neurons[i].Value;
        }

        return output;
    }

    /**
	 * Algoritmo di backpropagation per il learning assistito
	 * (Versione single thread)
	 * 
	 * Convergenza non garantita e molto lenta; utilizzare come criteri
	 * di stop una norma tra gli errori precedente e corrente, ed un
	 * numero massimo di iterazioni.
	 * 
	 * @param input Valori in input (scalati tra 0 ed 1)
	 * @param output Valori di output atteso (scalati tra 0 ed 1)
	 * @return Errore delta tra output generato ed output atteso
	 */
    public double backPropagation(double[] input, double[] output) {
        double[] new_output = Ejecutar(input);
        double error;
        int i;
        int j;
        int k;

        /* doutput = correct output (output) */

        // Calcoliamo l'errore dell'output
        for(i = 0; i < fLayers[fLayers.Length - 1].Length; i++) {
            error = output[i] - new_output[i];
            fLayers[fLayers.Length - 1].Neurons[i].Delta = error * fTransferFunction.evaluarDerivada(new_output[i]);
        }


        for(k = fLayers.Length - 2; k >= 0; k--) {
            // Calcolo l'errore dello strato corrente e ricalcolo i delta
            for(i = 0; i < fLayers[k].Length; i++) {
                error = 0.0f;
                for(j = 0; j < fLayers[k + 1].Length; j++)
                    error += fLayers[k + 1].Neurons[j].Delta * fLayers[k + 1].Neurons[j].Weights[i];

                fLayers[k].Neurons[i].Delta = error * fTransferFunction.evaluarDerivada(fLayers[k].Neurons[i].Value);
            }

            // Aggiorno i pesi dello strato successivo
            for(i = 0; i < fLayers[k + 1].Length; i++) {
                for(j = 0; j < fLayers[k].Length; j++)
                    fLayers[k + 1].Neurons[i].Weights[j] += fLearningRate * fLayers[k + 1].Neurons[i].Delta * fLayers[k].Neurons[j].Value;
                fLayers[k + 1].Neurons[i].Bias += fLearningRate * fLayers[k + 1].Neurons[i].Delta;
            }
        }

        // Calcoliamo l'errore 
        error = 0.0;

        for(i = 0; i < output.Length; i++) {
            error += System.Math.Abs(new_output[i] - output[i]);

            //System.out.println(output[i]+" "+new_output[i]);
        }

        error = error / output.Length;
        return error;
    }
    

    /**
	 * @return Costante di apprendimento
	 */
    public double getLearningRate() {
        return fLearningRate;
    }


    /**
	 * 
	 * @param rate
	 */
    public void setLearningRate(double rate) {
        fLearningRate = rate;
    }


    /**
	 * Imposta una nuova funzione di trasferimento
	 * 
	 * @param fun Funzione di trasferimento
	 */
    public void setTransferFunction(Funciones fun) {
        fTransferFunction = fun;
    }



    /**
	 * @return Dimensione layer di input
	 */
    public int getInputLayerSize() {
        return fLayers[0].Length;
    }


    /**
	 * @return Dimensione layer di output
	 */
    public int getOutputLayerSize() {
        return fLayers[fLayers.Length - 1].Length;
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
