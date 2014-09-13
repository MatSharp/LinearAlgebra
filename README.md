LinearAlgebra
=============

Library that allows you to perform simple Linear Algebra calculations in your .NET Framework application. The matrices are <b>generic type</b>, so you can use the types you want. For example, the following piece of code:

```csharp
Matrix<double> matrix = new Matrix<double>(3, 3, new double[,] 
{{1,3,3},
 {1,4,3},
 {1,3,4}});
   
Matrix<double> result = matrix * matrix.Transpose();

Console.WriteLine("M*M'\n" + result.ToString() + "\n");
Console.WriteLine("M^(-1)\n" + matrix.Inverse().ToString());
```

Results in the following output:

```
M*M'
Rows=3, Cols=3
19 22 22 
22 26 25 
22 25 26 


M^(-1)
Rows=3, Cols=3
7 -3 -3 
-1 1 0 
-1 0 1 
```
