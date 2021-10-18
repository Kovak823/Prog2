class A {
    synchronized void foo(B b){
        String name = Thread.currentThread().getName();

        System.out.println(name + " entered A foo......");

        try{
            Thread.sleep(1000);
        }catch(InterruptedException e){
            System.out.println("InterruptException");
        }
        System.out.println(name + " calling B.last()");
        b.last();
    }

    synchronized void last(){
        System.out.println("We are in A.last()");
    }
}

class B {
    synchronized void bar(A a){
        String name = Thread.currentThread().getName();

        System.out.println(name + " entered B bar......");

        try{
            Thread.sleep(1000);
        }catch(InterruptedException e){
            System.out.println("InterruptException");
        }
        System.out.println(name + " calling A.last()");
        a.last();
    }

    synchronized void last(){
        System.out.println("We are in B.last()");
    }
}

class Deadlock implements Runnable {
    A a = new A();
    B b = new B();
    Thread t;

    Deadlock(){
        Thread.currentThread().setName("Mainthread");
        t = new Thread(this, "RacingThread");
    }
    void deadlockStart(){
        t.start();
        a.foo(b);
        System.out.println("Mainthread");
    }
    public void run(){
        b.bar(a);
        System.out.println("Otherthread");
    }
    public static void main(String[] args){
        Deadlock d1 = new Deadlock();
        d1.deadlockStart();
    }
}