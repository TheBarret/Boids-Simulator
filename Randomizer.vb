Imports System.Security.Cryptography

Public Class Randomizer

    Public Const Characters As String = "ABCDEF1234567890"

    Public Shared Function [String](length As Integer) As String
        Return New String(Enumerable.Repeat(Randomizer.Characters, length).Select(Function(s) s(Randomizer.Provider.Next(s.Length))).ToArray)
    End Function

    Public Shared Function [String](length As Integer, table As String) As String
        Return New String(Enumerable.Repeat(table, length).Select(Function(s) s(Randomizer.Provider.Next(s.Length))).ToArray)
    End Function

    Public Shared Function [Integer](min As Integer, max As Integer) As Integer
        Return Randomizer.Provider.Next(min, max)
    End Function

    Public Shared Function [Byte](min As Integer, max As Integer) As Byte
        If (min > 0 And max < 256) Then
            Return Convert.ToByte(Randomizer.Provider.Next(min, max))
        End If
        Return CByte(Randomizer.Provider.Next(0, 255))
    End Function

    Public Shared Function [Double]() As Double
        Return Randomizer.Provider.NextDouble
    End Function

    Public Shared Function [Double](min As Double, max As Double) As Double
        Return Randomizer.Provider.NextDouble * (max - min) + min
    End Function

    Public Shared Function [Single](min As Single, max As Single) As Single
        Return CSng(Randomizer.Provider.NextDouble * (max - min) + min)
    End Function

    Public Shared Function Provider() As Random
        Static rnd As New Random(Randomizer.Seed)
        Return rnd
    End Function

    Public Shared Function Seed() As Integer
        Static r As RandomNumberGenerator = RandomNumberGenerator.Create
        Dim bytes As Byte() = New Byte(3) {}
        r.GetBytes(bytes)
        Return BitConverter.ToInt32(bytes, 0)
    End Function
End Class

