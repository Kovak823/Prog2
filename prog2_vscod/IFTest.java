interface IntStack {
    void push(int item);
    int pop();
    int size();
}

class DynStack implements IntStack {
    private int stck[];
    private int tos;

    DynStack(int size) {
        stck = new int[size];
        tos = -1;
    }

    public void push(int item) {
        if(tos == stck.length-1){
            int temp[] = new int[stck.length * 2];
            for (int i = 0; i < stck.length; i++) 
               temp[i] = stck[i]; 
            stck = temp;
            stck[++tos] = item;
            System.out.println("Stck length: " + stck.length);
        }
        else
            stck[++tos] = item;
    }

    public int size()
    {
        return stck.length;
    }

    public int pop(){
        if(tos < 0){
            System.out.println("Stack is empty");
            return 0;
        }
        else
            return stck[tos--];
    }
}

class IFTest {
    public static void main(String[] args)
    {
        DynStack dyn = new DynStack(5);

        for (int i = 0; i < 15; i++) dyn.push(i);

        for (int i = 0; i < 15; i++) {
            System.out.println(dyn.pop());
        }

        System.out.println("Length: " + dyn.size());

    }
}
