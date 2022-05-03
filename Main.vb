Imports System.Timers
Imports Simulator.Animals
Imports Simulator.Models

Public Class Main
    Public Property Clock As Timer
    Public Property Scene As Scene
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)

        Me.Scene = New Scene(Me.ClientRectangle)
        Me.Scene.Add(Of Omnivore)(100)

        Me.Clock = New Timer(50)
        AddHandler Me.Clock.Elapsed, AddressOf Me.Tick
        Me.Clock.Start()
    End Sub

    Private Sub Tick(sender As Object, e As ElapsedEventArgs)
        Using bm As New Bitmap(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
            Using g As Graphics = Graphics.FromImage(bm)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.Clear(Color.WhiteSmoke)
                Me.Scene.Update(g)
            End Using
            Me.Draw(CType(bm.Clone, Bitmap))
        End Using
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
