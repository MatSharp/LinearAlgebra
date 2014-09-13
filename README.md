LinearAlgebra
=============

Library that allows you to perform simple Linear Algebra calculations in your .NET Framework application. The matrices are <b>generic type</b>, so you can use the types you want.

```csharp
Matrix<double> matrix = new Matrix<double>(3, 3, new double[,] 
{{1,3,3},
 {1,4,3},
 {1,3,4}});
   
Matrix<double> result = matrix * matrix.Transpose();
```
