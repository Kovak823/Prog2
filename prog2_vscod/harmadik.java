class Box {
    double width;
    double height;
    double length;

    Box(){
        width = 0;
        height = 0;
        length = 0;
    }

    Box(Box ob){
        this.width = ob.width;
        this.height = ob.height;
        this.length = ob.length;
    }

    Box(double len){
        length = width = height = len;
    }

    Box(double width, double length, double height){
        this.width = width;
        this.height = height;
        this.length = length;
    }

    double volume() {
        return width * height * length;
    }
    void setDim(double w, double h, double l){
        width = w;
        height = h;
        length = l;
    }
    void setDim(double len){
        width = length = height = len;
    }
}
public class harmadik {

    public static void main(String[] args){

        Box myBox1;
        myBox1 = new Box();

        myBox1.width = 10;
        myBox1.height = 20;
        myBox1.length = 30;

        //double vol = myBox1.width * myBox1.height * myBox1.length;
        double vol = myBox1.volume();

        System.out.println("Vol: " + vol);

        Box myBox2 = new Box(myBox1);
        System.out.println("Vol2: " + myBox2.volume());

        Box myBox3 = new Box(10.0);
        System.out.println("Vol3: " + myBox3.volume());

        Box myBox4 = new Box(10.0, 50.0, 30.0);
        System.out.println("Vol4: " + myBox4.volume());
    }
    
}
