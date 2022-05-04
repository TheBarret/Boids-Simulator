Imports Simulator.Models

Namespace Entities
    Public Class Boid
        Inherits Entity
        Sub New(scene As Engine)
            MyBase.New(scene, EntityType.Boid, 0, 0)
        End Sub

        Public Overrides Sub Initialize(index As Integer)
            Me.Index = index
            Me.Enabled = True
            Me.Color = Color.Blue
            Me.Randomize()
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Boid"
            End Get
        End Property

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return 5
            End Get
        End Property

        Public Overrides ReadOnly Property Sensor As Integer
            Get
                Return 40
            End Get
        End Property

        Public Overrides ReadOnly Property Avoidance As Double
            Get
                Return 0.009
            End Get
        End Property

        Public Overrides ReadOnly Property Alignment As Double
            Get
                Return 0.1
            End Get
        End Property

        Public Overrides ReadOnly Property Grouping As Double
            Get
                Return 0.05
            End Get
        End Property

        Public Overrides ReadOnly Property MaxSpeed As Double
            Get
                Return 2
            End Get
        End Property

        Public Overrides ReadOnly Property MinSpeed As Double
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property TurnRate As Double
            Get
                Return 0.5
            End Get
        End Property

        Public Overrides ReadOnly Property TurnZone As Integer
            Get
                Return 40
            End Get
        End Property
    End Class
End Namespace