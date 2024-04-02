# CPU Assessment

This is a project for me to explore various serial, threading, parallel, async, and simd execution methods in dotnet/C#.

### What does this actually do?

Nothing useful.  At best, it's a cpu and memory stress test.  At worst, it is a shiny toy to play with.  At it's core, it builds a list/array of ip addresses in byte variables, and adds them together by various means.  Currently it uses x86's AVX2, arm64's NEON, parallel.foreach, threading, and async to demonstrate the difference in the performance characteristics as a demostration of when and by how much these runtime strategies can be helpful for cpu bound tasks. 

### How this has been organized

The source code is found in the src directory, and is subdivided across multiple projects.

The cpuAssessment directory is the main cli project.  If you wanted to run this project on any computer without setting up a dev enviroment, this is the project to build for the target machine.

The cpuAssessment.Class has all the main logic for the runtime.  This doesn't build to a fully executable artifact.

The cpuAssessment.Benchmark has the code to run dotnet's built in code benchmark runtime.

The cpuAssessment.Test has all integration and unit tests.  There isn't much and I've typically used these projects as playgrounds to test out implementation ideas without having to modify the main project's codebase.

### Building/Running this project

This project is managed by [Cake](https://cakebuild.net).  That means that everything is handled for you in the build.cake file at the root of this repo with C# code.  To get started, once you've cloned this repo, run the following command to install the cake tool through dotnet tool:

```bash
dotnet tool restore
```

Now that it's successfully installed, you can use it just like you would a Makefile.  By default, it will run the "dotnet run" command, but with some extra steps, such as running a directory clean and the unit tests from the test project everytime.

```bash
dotnet cake
```

If you'd like to run the Benchmark project, you'll need to pass in an argument, which can be done like this:

```bash
dotnet cake --target=benchmark
```

Target is an argument defined in the cake file on line 1, and is used on line 251 in the RunTarget function to determine which task to run.  Without any overwriting parameters, it will run the Default task which is dependent on the Run task, but the Default task doesn't "do" anything as it doesn't have a .Does() lambda function defined.



