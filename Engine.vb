Imports Simulator.Models

Public Class Engine
    Inherits Quadrant
    Public Property Entities As List(Of Entity)

    Sub New(bounds As Rectangle)
        MyBase.New(0, bounds)
        Me.Bounds = bounds
        Me.Entities = New List(Of Entity)
    End Sub

    Public Sub Update(g As Graphics)
        Dim entities As IEnumerable(Of Entity) = Me.Entities.Where(Function(x) x.Enabled)
        Me.Prerender(entities)
        Me.DrawQuadrants(g)
        For Each e As Entity In Me.Entities.Where(Function(x) x.Enabled)
            e.Update(g)
            e.Draw(g)
        Next
        Me.Cleanup()
    End Sub

    Public Function Frame() As Bitmap
        Using bm As New Bitmap(Me.Bounds.Integral.Width, Me.Bounds.Integral.Height)
            Using g As Graphics = Graphics.FromImage(bm)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.Clear(Color.WhiteSmoke)
                Me.Update(g)
                Return CType(bm.Clone, Bitmap)
            End Using
        End Using
    End Function

    Public Sub Add(Of T As Entity)(amount As Integer)
        Dim instance As Object = Nothing
        For i = 0 To amount - 1
            instance = Activator.CreateInstance(GetType(T), Me)
            If (instance IsNot Nothing) Then
                Me.Entities.Add(CType(instance, Entity))
                Me.Entities.Last.Initialize(i)
            End If
        Next
    End Sub

End Class
