Namespace Models
    Public MustInherit Class Entity
        Public Property Color As Color
        Public Property Scene As Scene
        Public Property Index As Integer
        Public Property Enabled As Boolean
        Public Property Position As Vector
        Public Property Velocity As Vector
        Public Property Type As EntityType

        Sub New(scene As Scene, type As EntityType, x As Double, y As Double)
            Me.Enabled = False
            Me.Type = type
            Me.Scene = scene
            Me.Index = 0
            Me.Velocity = Vector.Zero
            Me.Position = New Vector(x, y)
        End Sub

        Public Overridable Sub Draw(g As Graphics)
            g.FillEllipse(New SolidBrush(Me.Color), CSng(Me.Position.X), CSng(Me.Position.Y), Me.Size, Me.Size)
            g.DrawEllipse(Pens.Black, CSng(Me.Position.X), CSng(Me.Position.Y), Me.Size, Me.Size)
        End Sub

        Public Overridable Sub Update(g As Graphics)
            Me.Interact(g)
            Me.KeepSpeed(0.1, 5)
            Me.UpdateBounds(1.5, 30)
        End Sub

        Public Overridable Sub Interact(g As Graphics)
            Me.Velocity += Me.Group(0.01)
            Me.Velocity += Me.Align(0.7)
            Me.Velocity += Me.Avoid(0.005)
        End Sub

        Public MustOverride Sub Initialize(index As Integer)
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Size As Integer
        Public MustOverride ReadOnly Property Sensor As Integer

        Public Sub Randomize()
            Me.Velocity.X = Randomizer.Double(-2, 2)
            Me.Velocity.Y = Randomizer.Double(-2, 2)
            Me.Position.X = Randomizer.Integer(0, CInt(Me.Scene.Bounds.Width))
            Me.Position.Y = Randomizer.Integer(0, CInt(Me.Scene.Bounds.Height))
        End Sub

        Public Function Resist(power As Double, Optional offset As Double = 0) As Vector
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor - offset)
            If (others.Count > 0) Then
                Dim cx, cy, dist, m As Double
                For Each other As Entity In others
                    dist = Me.Distance(other)
                    If (dist < Me.Sensor) Then
                        m = Me.Sensor - dist
                        cx += (Me.Position.X - other.Position.X) * m
                        cy += (Me.Position.Y - other.Position.Y) * m
                    End If
                Next
                Return New Vector(cx * power, cy * power)
            End If
            Return Vector.Zero
        End Function

        Public Function Group(power As Double) As Vector
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor)
            If (others.Count > 0) Then
                Dim mx, my, dx, dy As Double
                For Each other As Entity In others
                    mx = others.Sum(Function(x) x.Position.X) / others.Count
                    my = others.Sum(Function(y) y.Position.Y) / others.Count
                    dx = mx - Me.Position.X
                    dy = my - Me.Position.Y
                Next
                Return New Vector(dx * power, dy * power)
            End If
            Return Vector.Zero
        End Function

        Public Function Collision(ByRef p As Entity) As Boolean
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor)
            If (others.Count > 0) Then
                For Each other As Entity In others
                    If (Me.Distance(other) < Me.Size + other.Size) Then
                        p = other
                        Return True
                    End If
                Next
            End If
            Return False
        End Function

        Public Function Avoid(power As Double) As Vector
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor)
            If (others.Count > 0) Then
                Dim sx, sy, dist As Double
                For Each other As Entity In others
                    dist = Me.Sensor - Me.Distance(other)
                    sx += (Me.Position.X - other.Position.X) * dist
                    sy += (Me.Position.Y - other.Position.Y) * dist
                Next
                Return New Vector(sx * power, sy * power)
            End If
            Return Vector.Zero
        End Function

        Public Function Align(power As Double) As Vector
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor)
            If (others.Count > 0) Then
                Dim mx, my, vx, vy As Double
                For Each other As Entity In others
                    mx = others.Sum(Function(x) x.Velocity.X) / others.Count
                    my = others.Sum(Function(y) y.Velocity.Y) / others.Count
                    vx = mx - Me.Velocity.X
                    vy = my - Me.Velocity.Y
                Next
                Return New Vector(vx * power, vy * power)
            End If
            Return Vector.Zero
        End Function

        Public Function KeepBounds(p As Entity, turn As Double, padding As Integer) As Vector
            Dim force As Vector = Vector.Zero
            If (p.Position.X < padding) Then
                force.X += Me.Velocity.X + turn
            ElseIf (Me.Position.X > Me.Scene.Bounds.Width - padding) Then
                force.X += Me.Velocity.X - turn
            End If
            If (Me.Position.Y < padding) Then
                force.Y += Me.Velocity.Y + turn
            ElseIf (Me.Position.Y > Me.Scene.Bounds.Height - padding) Then
                force.Y += Me.Velocity.Y - turn
            End If
            Return force
        End Function

        Public Sub KeepSpeed(min As Double, max As Double)
            Dim speed As Double = Me.Speed
            Me.Position.X += Me.Velocity.X
            Me.Position.Y += Me.Velocity.Y
            If (speed > max) Then
                Me.Velocity.X = (Me.Velocity.X / speed) * max
                Me.Velocity.Y = (Me.Velocity.Y / speed) * max
            ElseIf (speed < min) Then
                Me.Velocity.X = (Me.Velocity.X / speed) * min
                Me.Velocity.Y = (Me.Velocity.Y / speed) * min
            End If
            If (Double.IsNaN(Me.Velocity.X)) Then Me.Velocity.X = 0
            If (Double.IsNaN(Me.Velocity.Y)) Then Me.Velocity.Y = 0
        End Sub

        Public Sub UpdateBounds(turn As Double, padding As Integer)
            If (Me.Position.X < padding) Then
                Me.Velocity.X += turn
            ElseIf (Me.Position.X > Me.Scene.Bounds.Width - padding) Then
                Me.Velocity.X -= turn
            End If
            If (Me.Position.Y < padding) Then
                Me.Velocity.Y += turn
            ElseIf (Me.Position.Y > Me.Scene.Bounds.Height - padding) Then
                Me.Velocity.Y -= turn
            End If
        End Sub

        Public Sub Wrap()
            If (Me.Position.X < 0) Then
                Me.Position.X += Me.Scene.Bounds.Width
            ElseIf (Me.Position.X > Me.Scene.Bounds.Width) Then
                Me.Position.X -= Me.Scene.Bounds.Width
            End If
            If (Me.Position.Y < 0) Then
                Me.Position.Y += Me.Scene.Bounds.Height
            ElseIf (Me.Position.Y > Me.Scene.Bounds.Height) Then
                Me.Position.Y -= Me.Scene.Bounds.Height
            End If
        End Sub

        Public Sub DrawNeighborhood(g As Graphics)
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(Me.Sensor)
            If (others.Count >= 3) Then
                Dim path As New List(Of PointF)
                For Each other As Entity In others
                    path.Add(other.Center)
                Next
                If (path.Count >= 2) Then
                    g.DrawLines(New Pen(Color.DeepPink, 1), path.ToArray)
                End If
            End If
        End Sub

        Public ReadOnly Property Center As PointF
            Get
                Return New PointF(CSng(Me.Position.X + (Me.Size / 2)), CSng(Me.Position.Y + (Me.Size / 2)))
            End Get
        End Property

        Public ReadOnly Property RectangleF As RectangleF
            Get
                Return New RectangleF(CSng(Me.Position.X), CSng(Me.Position.Y), Me.Size, Me.Size)
            End Get
        End Property

        Public ReadOnly Property Speed() As Double
            Get
                Return Me.Velocity.SqrtMagnitude
            End Get
        End Property

        Public ReadOnly Property Distance(other As Entity) As Double
            Get
                Return Me.Position.Distance(other.Position)
            End Get
        End Property

        Public ReadOnly Property GetNeighbors(distance As Double) As List(Of Entity)
            Get
                Dim result As New List(Of Entity)
                Dim quadrant As List(Of Entity) = Me.Scene.Locate(Me)
                If (quadrant.Count > 0) Then
                    For Each e As Entity In quadrant
                        If (e.Type <> Me.Type) Then Continue For
                        If (e IsNot Nothing AndAlso e Is Me) Then Continue For
                        If (Me.Distance(e) <= distance) Then
                            result.Add(e)
                        End If
                    Next
                End If
                Return result
            End Get
        End Property

        Public ReadOnly Property GetNeighbors(distance As Double, condition As Func(Of Entity, Boolean)) As List(Of Entity)
            Get
                Dim result As New List(Of Entity)
                Dim quadrant As List(Of Entity) = Me.Scene.Locate(Me)
                If (quadrant.Count > 0) Then
                    For Each e As Entity In quadrant
                        If (e IsNot Nothing AndAlso e Is Me) Then Continue For
                        If (Me.Distance(e) <= Me.Sensor) Then
                            If (condition(e)) Then
                                result.Add(e)
                            End If
                        End If
                    Next
                End If
                Return result
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} [{1}]", Me.Name, Me.Position.Round)
        End Function
    End Class
End Namespace