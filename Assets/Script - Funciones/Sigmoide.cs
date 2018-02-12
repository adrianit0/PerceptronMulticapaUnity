using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sigmoide : Funciones {
    
    public double evaluar(double value) {
        return 1 / (1 + System.Math.Exp(-value));
    }
    
    public double evaluarDerivada(double value) {
        return (value - System.Math.Pow(value, 2));
    }
}