class Box{
    private double width;
    private double height;
    private double depth;

    Box(){
        width = 0;
        height = 0;
        depth = 0;
    }
    Box(Box ob){
        this.width = ob.width;
        this.height = ob.height;
        this.depth = ob.depth;
    }
    Box(double len){
        width = height = depth = len;
    }
    Box(double width, double height, double depth){
        this.width = width;
        this.height = height;
        this.depth = depth;
    }
    double volume() {
        return width * height * depth;
    }
    void setDim(double w, double h, double d){
        width = w;
        height = h;
        depth = d;
    }
    void setDim(double len){
        width = height = depth = len;
    }
} 

class BoxWeight extends Box {
    double weight;

    BoxWeight(){
        super();
        weight = 0;
    }

    BoxWeight(BoxWeight ob){
        super(ob);
        weight = ob.weight;
    }

    BoxWeight(double w, double d, double h, double m){
        super(w, h, d);
        weight = m;
    }
    BoxWeight(double len, double m){
        super(len);
        weight = m;
    }
}

class BoxDemoWeight {
    public static void main(String[] args) {

        BoxWeight weightBox1 = new BoxWeight(10, 20, 30, 5.5);

        double vol = weightBox1.volume();
        System.out.println("WeightBox vol: " + vol);
        System.out.println("WeightBox weight: " + weightBox1.weight);

        BoxWeight weightBox2 = new BoxWeight();

        vol = weightBox2.volume();
        System.out.println("WeightBox2 vol: " + vol);
        System.out.println("WeightBox2 weight: " + weightBox2.weight);

        BoxWeight weightBox3 = new BoxWeight(10, 3.5);

        vol = weightBox3.volume();
        System.out.println("WeightBox3 vol: " + vol);
        System.out.println("WeightBox3 weight: " + weightBox3.weight);

        BoxWeight weightBox4 = new BoxWeight(weightBox1);

        vol = weightBox4.volume();
        System.out.println("WeightBox4 vol: " + vol);
        System.out.println("WeightBox4 weight: " + weightBox4.weight);



    }
}