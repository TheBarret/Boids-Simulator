Imports Simulator.Models

Public Class World
    Public Const Max As Integer = 2
    Public Const Levels As Integer = 16
    Public Property Depth As Integer
    Public Property Bounds As RectangleF
    Public Property Regions As List(Of World)
    Public Property Cached As List(Of Entity)

    Sub New(level As Integer, bound As RectangleF)
        Me.Depth = level
        Me.Bounds = bound
        Me.Regions = New List(Of World)
        Me.Cached = New List(Of Entity)
    End Sub

    Public Sub Draw(g As Graphics)
        For Each n As World In Me.Regions.ToArray
            n.Draw(g)
        Next
        If (Me.Cached.Count > 0) Then
            If (Me.Regions.Count = 0) Then
                g.DrawRectangle(Pens.DarkGray, Me.Bounds.X, Me.Bounds.Y, Me.Bounds.Width, Me.Bounds.Height)
            End If
        End If
    End Sub

    Public Function Locate(a As Entity) As List(Of Entity)
        Return Me.Locate(a, New List(Of Entity))
    End Function

    Public Function Locate(a As Entity, ByRef result As List(Of Entity)) As List(Of Entity)
        Dim index As Integer = Me.IndexOf(a)
        If (result Is Nothing) Then result = New List(Of Entity)
        If (index <> -1 AndAlso index < Me.Regions.Count) Then
            Me.Regions(index).Locate(a, result)
        End If
        If (Me.Cached.Count > 0) Then
            result.AddRange(Me.Cached)
        End If
        Return result
    End Function

    Public Sub Cache(entity As Entity)

        If Me.Regions.Count > 0 Then
            Dim index As Integer = Me.IndexOf(entity)
            If index <> -1 Then
                Me.Regions(index).Cache(entity)
                Return
            End If
        End If

        Me.Cached.Add(entity)

        If Me.Cached.Count > World.Max AndAlso Me.Depth < World.Levels Then
            If Me.Regions.Count = 0 Then
                Me.Divide()
            End If
            Dim i As Integer = 0
            While i < Me.Cached.Count
                Dim index As Integer = Me.IndexOf(Me.Cached(i))
                If index <> -1 Then
                    Dim ri As Entity = Me.Cached(i)
                    If (Me.Cached.Remove(ri)) Then
                        Me.Regions(index).Cache(ri)
                    End If
                Else
                    i += 1
                End If
            End While
        End If
    End Sub

    Public Function IndexOf(entity As Entity) As Integer
        Dim index As Integer = -1
        Dim vMpoint As Double = Me.Bounds.X + (Me.Bounds.Width / 2)
        Dim hMpoint As Double = Me.Bounds.Y + (Me.Bounds.Height / 2)
        Dim tQuadrant As Boolean = (entity.RectangleF.Y < hMpoint And entity.RectangleF.Y + entity.RectangleF.Height < hMpoint)
        Dim bQuadrant As Boolean = (entity.RectangleF.Y > hMpoint)

        If (entity.RectangleF.X < vMpoint And entity.RectangleF.X + entity.RectangleF.Width < vMpoint) Then
            If (tQuadrant) Then
                index = 1
            ElseIf (bQuadrant) Then
                index = 2
            End If
        ElseIf (entity.RectangleF.X > vMpoint) Then
            If (tQuadrant) Then
                index = 0
            ElseIf (bQuadrant) Then
                index = 3
            End If
        End If
        Return index
    End Function

    Public Sub Divide()
        Dim width As Single = Me.Bounds.Width / 2
        Dim height As Single = Me.Bounds.Height / 2
        Dim x As Single = Me.Bounds.X
        Dim y As Single = Me.Bounds.Y
        Me.Regions.Insert(0, New World(Me.Depth + 1, New RectangleF(x + width, y, width, height)))
        Me.Regions.Insert(1, New World(Me.Depth + 1, New RectangleF(x, y, width, height)))
        Me.Regions.Insert(2, New World(Me.Depth + 1, New RectangleF(x, y + height, width, height)))
        Me.Regions.Insert(3, New World(Me.Depth + 1, New RectangleF(x + width, y + height, width, height)))
    End Sub

    Public Sub Clear()
        SyncLock Me.Cached
            Me.Cached.Clear()
        End SyncLock
        SyncLock Me.Regions
            For i As Integer = 0 To Me.Regions.Count - 1
                Me.Regions(i).Clear()
            Next
            Me.Regions.Clear()
        End SyncLock
    End Sub

    Public ReadOnly Property TotalDepth As Integer
        Get
            Dim max As Integer = 0
            For Each n As World In Me.Regions
                max += n.Depth
            Next
            Return max + 1
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0} Cached | {1} Regions | Depth {2}", Me.Cached.Count, Me.Regions.Count, Me.TotalDepth)
    End Function
End Class
