Imports System.Runtime.CompilerServices
Imports Simulator.Models

Public Module Extensions

    <Extension> Public Function ToDegrees(radians As Double) As Double
        Return (180 / Math.PI) * radians
    End Function

    <Extension> Public Function Integral(r As RectangleF) As Rectangle
        Return New Rectangle(CInt(r.X), CInt(r.Y), CInt(r.Width), CInt(r.Height))
    End Function

    <Extension> Public Function SetAlpha(base As Color, steps As Integer) As Color
        Dim a As Byte, f As Double, c As Color
        If (steps = 0) Then Return base
        For i As Integer = steps To 0 Step -1
            f = (i + 1.0F) / steps
            a = CByte(255 * f * 0.5)
            c = Color.FromArgb(a, base.R, base.G, base.B)
        Next
        Return c
    End Function

End Module
