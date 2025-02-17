### ZeUnit - take the reins of your testing framework.

How Google Tests Software (link to the book) talks about a system of test clasification that doesn't neatly fit into the bucket of unit or integration testing and instead classifies them as small, medium or large.  But the duality of testing frameworks seem to push you into a binary choice. 

* **Unit Testing** - in dotnet, this would be XUnit or NUnit, both of which focus on small scale testing and don't come with tools that enable contextual state.  The dogma that comes with using these framework chastices the use of classes that set up state, preaching a pure test instanciating all of it's scope.
* **Integration Testing** - these would be your SpecFlow, Fitnesse and StoryTeller.  At the abstrcat layer, these frameworks enable some kind of system state abstracted behind a fixture.  But with the tendancy of these languages to use runtime test enterperters, writing low level integration tests is not really practical.

ZeUnit aims to fit as a bridge between these two worlds, allowing developer centric unit and intgration testing to be written with one framework and with out the nessary ceremony of creaing wiki file bindings.  

## What gives ZeUnit its power?

ZeUnit was cocieved as am idea for preloading state into XUnit because when colege asked for a Integration Testing framework recomendation, I couldn't give him one.  This led me down a rabbit hole of looking at XUnit code, and after some thinking I had an answer to my integration testing framework.  What if we give Test classes depdency injection?

Several itteration later, this has turned into a framework that support testing based set of custom class and method activators, that allow context to be created for the execution of a tests.  One example of this would be the Lamar container class that allows a user to re-user their *ServiceRegistry* classes to populate a container creates the test class instance.

```
[LamarContainer(typeof(SimpleServiceInjectionRegistry))]
public class InjectionZeUnitClass
{
    private readonly ISimpleInjectedService service;

    public InjectionZeTestClass(ISimpleInjectedService service)
    {
        this.service = service;
    }

    public ZeResult ConstructorInjectionTestMethodThatPasses()
    {
        return Ze.Assert()  
            .IsNotNull(this.service);

    }
}
```

Behind the scense, the *SimpleServiceInjectorRegistraty* that is passed into the *LamarContainerAttribute* creates the container registrion for *ISimpleInjectedService* with some implementation. When the testing framework calls the Activator defined in the *LamarContainerAttribute*, it creates the instance of the class.

At the same time, there is nothing that prevents the test to be as simple as an XUnit/NUnit test is today, that requires no additional context to exist before a test is run.

```
public class SampleZeUnitClass
{
    public ZeResult SimpleTestMethodThatPasses() 
    {            
        var result = 2 + 2;
        return Ze.Assert()
            .IsType<int>(result)
            .IsEqual(4, result);
    }

    public ZeResult SimpleTestMethodThatFailes()
    {
        var result = 2 + 2;
        return Ze.Assert()            
            .IsEqual(5, result);
    }
}
```

Because the power of the test and method activation is yours, you get to define the scale of each test class with existing or custom *ZeActivationAttributes* and the *ZeActivator* classes. 

### Some basics

* The way you should get data out of a function is by getting it's return value. To follow this functional practice ZeUnit does away with static Assert and instead expects that test methods will return their *ZeResult* value(s) instead.
* Because we know the return type of any test will allway be ZeResult, we don't need Attributes to mark methods for excetion.  The discovery simply looks at the method return *ZeResult*, *IEnumerable<ZeResult>*, *Task<ZeResult> or IObservable<ZeResult> registers the methods for testing.