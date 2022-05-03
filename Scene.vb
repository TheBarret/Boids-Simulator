Imports Simulator.Models

Public Class Scene
    Inherits World
    Public Property Entities As List(Of Entity)

    Sub New(bounds As Rectangle)
        MyBase.New(0, bounds)
        Me.Bounds = bounds
        Me.Entities = New List(Of Entity)
    End Sub

    Public Sub Update(g As Graphics)
        For Each e As Entity In Me.Entities.Where(Function(x) x.Enabled)
            Me.Cache(e)
        Next
        Me.Draw(g)
        For Each e As Entity In Me.Entities.Where(Function(x) x.Enabled)
            e.Draw(g)
        Next
        For Each e As Entity In Me.Entities.Where(Function(x) x.Enabled)
            e.Update(g)
        Next
        Me.Clear()
    End Sub

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
