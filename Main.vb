Imports System.Timers
Imports Simulator.Entities

Public Class Main
    Public Property Clock As Timer
    Public Property Engine As Engine

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)

        Me.Engine = New Engine(Me.ClientRectangle)
        Me.Engine.Add(Of Boid)(200)
        Me.Engine.Add(Of Predator)(2)

        Me.Clock = New Timer(50)
        AddHandler Me.Clock.Elapsed, AddressOf Me.Tick
        Me.Clock.Start()
    End Sub

    Private Sub Tick(sender As Object, e As ElapsedEventArgs)
        Me.Draw(Me.Engine.Frame)
    End Sub

    Private Sub Draw(bm As Bitmap)
        Try
            If (Not Me.IsDisposed AndAlso Me.InvokeRequired) Then
                Me.Invoke(Sub() Me.Draw(bm))
            Else
                Me.BackgroundImage = bm
            End If
        Catch
        End Try
    End Sub
End Class
