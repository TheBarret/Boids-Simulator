Imports System.Runtime.CompilerServices
Imports Simulator.Models

Public Module Extensions

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

    <Extension> Public Function AverageVelocity(values As IEnumerable(Of Entity)) As Vector
        Dim v As New Vector
        For Each p In values
            v += p.Velocity
        Next
        v.X = v.X / values.Count
        v.Y = v.Y / values.Count
        Return v
    End Function

End Module
