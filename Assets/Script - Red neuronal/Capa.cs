public class Capa {

    public Neurona[] Neurons;
    public int Length;

    /**
	 * Capa de la neurona
     * 
	 */
    public Capa(int l, int prev) {
        Length = l;
        Neurons = new Neurona[l];

        for(int j = 0; j < l; j++)
            Neurons[j] = new Neurona(prev);
    }

    public int getLength() {
        return Neurons.Length;
    }

    public Neurona getNeurona(int index) {
        return Neurons[index];
    }
}