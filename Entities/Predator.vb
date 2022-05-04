Imports Simulator.Models

Namespace Entities
    Public Class Predator
        Inherits Entity
        Sub New(scene As Engine)
            MyBase.New(scene, EntityType.Pedator, 0, 0)
        End Sub

        Public Overrides Sub Initialize(index As Integer)
            Me.Index = index
            Me.Enabled = True
            Me.Color = Color.Red
            Me.Randomize()
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Predator"
            End Get
        End Property

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return 10
            End Get
        End Property

        Public Overrides ReadOnly Property Sensor As Integer
            Get
                Return 50
            End Get
        End Property

        Public Overrides ReadOnly Property Avoidance As Double
            Get
                Return 0.005
            End Get
        End Property

        Public Overrides ReadOnly Property Alignment As Double
            Get
                Return 0.05
            End Get
        End Property

        Public Overrides ReadOnly Property Grouping As Double
            Get
                Return 0.009
            End Get
        End Property

        Public Overrides ReadOnly Property MaxSpeed As Double
            Get
                Return 3
            End Get
        End Property

        Public Overrides ReadOnly Property MinSpeed As Double
            Get
                Return 1
            End Get
        End Property


    End Class
End Namespace