Imports Simulator.Models

Namespace Animals
    Public Class Omnivore
        Inherits Entity
        Sub New(scene As Scene)
            MyBase.New(scene, EntityType.Omnivore, 0, 0)
        End Sub

        Public Overrides Sub Initialize(index As Integer)
            Me.Index = index
            Me.Enabled = True
            Me.Color = Color.CornflowerBlue
            Me.Randomize()
        End Sub

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Omnivore"
            End Get
        End Property

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return 5
            End Get
        End Property

        Public Overrides ReadOnly Property Sensor As Integer
            Get
                Return 20
            End Get
        End Property
    End Class
End Namespace