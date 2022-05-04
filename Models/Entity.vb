
Imports Simulator.Entities

Namespace Models
    Public MustInherit Class Entity
        Public Property Color As Color
        Public Property Engine As Engine
        Public Property Index As Integer
        Public Property Enabled As Boolean
        Public Property Position As Vector
        Public Property Velocity As Vector
        Public Property Type As EntityType
        Public MustOverride Sub Initialize(index As Integer)
        Public MustOverride ReadOnly Property Name As String

        Sub New(engine As Engine, type As EntityType, x As Double, y As Double)
            Me.Enabled = False
            Me.Type = type
            Me.Engine = engine
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
            Me.KeepSpeed(Me.MinSpeed, Me.MaxSpeed)
            Me.UpdateBounds(Me.TurnRate, Me.TurnZone)
        End Sub

        Public Overridable Sub Interact(g As Graphics)
            Select Case Me.Type
                Case EntityType.Boid
                    Dim enemies As List(Of Entity) = Me.GetEntity(Of Predator)(Me.Sensor)
                    If (enemies.Count > 0) Then
                        Me.Velocity += Me.Depart(Me.Reflect, enemies)
                    End If
                Case EntityType.Pedator
                    Dim prey As List(Of Entity) = Me.GetEntity(Of Boid)(Me.Sensor)
                    If (prey.Count > 0) Then
                        Me.Velocity += Me.Follow(Me.Attract, prey)
                    End If
            End Select
            Me.Velocity += Me.Group(Me.Grouping)
            Me.Velocity += Me.Align(Me.Alignment)
            Me.Velocity += Me.Avoid(Me.Avoidance)
        End Sub

        Public Sub Randomize()
            Me.Velocity.X = Randomizer.Double(-2, 2)
            Me.Velocity.Y = Randomizer.Double(-2, 2)
            Me.Position.X = Randomizer.Integer(0, Me.Engine.Bounds.Integral.Width)
            Me.Position.Y = Randomizer.Integer(0, Me.Engine.Bounds.Integral.Height)
        End Sub

#Region "Control"
        Public Sub Break(power As Double)
            Me.Velocity *= power
        End Sub
#End Region
#Region "Agents"

        Public Function Depart(power As Double, group As List(Of Entity)) As Vector
            Dim range As Double = Me.Sensor * 3
            If (group.Count > 0) Then
                Dim cx, cy, dist, m As Double
                For Each other As Entity In group
                    dist = Me.Distance(other)
                    If (dist <= range) Then
                        m = range - dist
                        cx += (Me.Position.X - other.Position.X) * m
                        cy += (Me.Position.Y - other.Position.Y) * m
                    End If
                Next
                Return New Vector(cx * power, cy * power)
            End If
            Return Vector.Zero
        End Function

        Public Function Follow(power As Double, group As List(Of Entity)) As Vector
            If (group.Count > 0) Then
                Dim mx, my, dx, dy As Double
                For Each other As Entity In group
                    mx = group.Sum(Function(x) x.Position.X) / group.Count
                    my = group.Sum(Function(y) y.Position.Y) / group.Count
                    dx = mx - Me.Position.X
                    dy = my - Me.Position.Y
                Next
                Return New Vector(dx * power, dy * power)
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
            Dim half As Double = Me.Sensor / 2
            Dim others As IEnumerable(Of Entity) = Me.GetNeighbors(half)
            If (others.Count > 0) Then
                Dim sx, sy, dist As Double
                For Each other As Entity In others
                    dist = half - Me.Distance(other)
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
            ElseIf (Me.Position.X > Me.Engine.Bounds.Width - padding) Then
                force.X += Me.Velocity.X - turn
            End If
            If (Me.Position.Y < padding) Then
                force.Y += Me.Velocity.Y + turn
            ElseIf (Me.Position.Y > Me.Engine.Bounds.Height - padding) Then
                force.Y += Me.Velocity.Y - turn
            End If
            Return force
        End Function

        Public Sub KeepSpeed(min As Double, max As Double)
            Dim speed As Double = Me.GetSpeed
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
            ElseIf (Me.Position.X > Me.Engine.Bounds.Width - padding) Then
                Me.Velocity.X -= turn
            End If
            If (Me.Position.Y < padding) Then
                Me.Velocity.Y += turn
            ElseIf (Me.Position.Y > Me.Engine.Bounds.Height - padding) Then
                Me.Velocity.Y -= turn
            End If
        End Sub

        Public Sub Wrap()
            If (Me.Position.X < 0) Then
                Me.Position.X += Me.Engine.Bounds.Width
            ElseIf (Me.Position.X > Me.Engine.Bounds.Width) Then
                Me.Position.X -= Me.Engine.Bounds.Width
            End If
            If (Me.Position.Y < 0) Then
                Me.Position.Y += Me.Engine.Bounds.Height
            ElseIf (Me.Position.Y > Me.Engine.Bounds.Height) Then
                Me.Position.Y -= Me.Engine.Bounds.Height
            End If
        End Sub
#End Region

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

        Public Function GetEntity(Of T As Entity)(distance As Double) As List(Of Entity)
            Dim result As New List(Of Entity)
            Dim quadrant As List(Of Entity) = Me.Engine.GetEntities(Me)
            If (quadrant.Count > 0) Then
                For Each e As Entity In quadrant
                    If (Me.Distance(e) <= Me.Sensor) Then
                        If (TypeOf e Is T) Then
                            result.Add(e)
                        End If
                    End If
                Next
            End If
            Return result
        End Function

#Region "Overriables"
        Public Overridable ReadOnly Property Size As Integer
            Get
                Return 10
            End Get
        End Property

        Public Overridable ReadOnly Property Sensor As Integer
            Get
                Return 50
            End Get
        End Property

        Public Overridable ReadOnly Property Attract As Double
            Get
                Return 0.001
            End Get
        End Property

        Public Overridable ReadOnly Property Reflect As Double
            Get
                Return 0.0005
            End Get
        End Property

        Public Overridable ReadOnly Property Avoidance As Double
            Get
                Return 0.005
            End Get
        End Property

        Public Overridable ReadOnly Property Alignment As Double
            Get
                Return 0.05
            End Get
        End Property

        Public Overridable ReadOnly Property Grouping As Double
            Get
                Return 0.009
            End Get
        End Property

        Public Overridable ReadOnly Property MaxSpeed As Double
            Get
                Return 6
            End Get
        End Property

        Public Overridable ReadOnly Property MinSpeed As Double
            Get
                Return 1
            End Get
        End Property

        Public Overridable ReadOnly Property TurnRate As Double
            Get
                Return 0.5
            End Get
        End Property

        Public Overridable ReadOnly Property TurnZone As Integer
            Get
                Return 40
            End Get
        End Property
#End Region

#Region "Properties"

        Public ReadOnly Property Angle As Single
            Get
                If (Double.IsNaN(Me.Velocity.X) Or Double.IsNaN(Me.Velocity.Y)) Then Return 0F
                If (Me.Velocity.X = 0 And Me.Velocity.Y = 0) Then Return 0F
                Dim a As Double = Math.Atan(Me.Velocity.Y / Me.Velocity.X) * 180 / Math.PI - 90
                If (Me.Velocity.X < 0) Then a += 180
                Return CSng(a)
            End Get
        End Property

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

        Public ReadOnly Property GetSpeed() As Double
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
                Dim quadrant As List(Of Entity) = Me.Engine.GetEntities(Me)
                If (quadrant.Count > 0) Then
                    For Each e As Entity In quadrant
                        If (e Is Me) Then Continue For
                        If (e.Type <> Me.Type) Then Continue For
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
                Dim quadrant As List(Of Entity) = Me.Engine.GetEntities(Me)
                If (quadrant.Count > 0) Then
                    For Each e As Entity In quadrant
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


#End Region

        Public Overrides Function ToString() As String
            Return String.Format("{0} [{1}]", Me.Name, Me.Position.Round)
        End Function
    End Class
End Namespace