import java.util.*;

class Szamologep{
    char let = '#';
    char quit = 'q';
    char print = ';';
    char numbver = '8';
    char name = 'a';
    char square_r = '@';
    string quitk = "exit";
    string sqrtk = "sqrt";
    string prompt = "> ";
    string result = "= ";

    


}


class Calculator {

    public static void main(String[] args) {
        System.out.println("Ez egy számológép!");
        Scanner sc = new Scanner(System.in);
        Szamologep Sza = new Szamologep();

        System.out.println("Kérem adjon meg egy számot!");
        int a = sc.nextInt();
        System.out.println("Kérem adjon meg még egy számot!");
        System.out.println("A számok összege: ");
    }
}
