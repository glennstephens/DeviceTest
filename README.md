# DeviceTest v0.05

So Unit Testing is great, I like it a lot. I unit test my servers, the code in my mobile applications and use test runners like NUnit to test how my applications with services like Xamarin Test Cloud.

But when you are testing classes such as classes or ViewModels that integrate with device capabilities its not so easy to test them using traditional unit testing mechanisms. There are several situations that come to mind:

* You have a class/method that accesses a capability that requires a security permission. Think of something like accessing the location of the device or gaining access to iOS HealthKit. You also have to run this test on several versions of the same operating system to ensure that it works. For iOS, you might test iOS 7/8/9/10 and the dot versions in between
* You have a data layer that doesn't lend itself to mocking very well. It might be that the creation of the mock object is significantly more work than you need. For example I have an app that performs data synchronisation between a local Sqlite database and a remote REST service. Rathan than mock the Sqlite access, I would prefer to use it and create a component test that uses the service as it was intended.
* When you create another project for the unit tests, you are duplicating the effort in configuring the project. Provisioning profiles, app ids, resources and assets used for a project all have to be managed and duplicated

These are the core reasons for working on this and certainly I could run these using NUnitLite if I really needed to, but I wanted a mechanism where I could also easily see the effects of these operations through Test Cloud. 

