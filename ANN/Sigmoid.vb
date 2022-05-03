
Namespace ANN
    <Serializable> Public Class Sigmoid
        Implements IAFunction
        Sub New()
        End Sub

        Public Function Output(value As Double) As Double Implements IAFunction.Output
            If value < -Me.Clamp Then Return -Me.Clamp
            If value > Me.Clamp Then Return Me.Clamp
            Return 1.0 / (1.0 + Math.Exp(-value))
        End Function

        Public Function Derivative(value As Double) As Double Implements IAFunction.Derivative
            Return value * (1 - value)
        End Function

        Public ReadOnly Property Name As String Implements IAFunction.Name
            Get
                Return "Sigmoid"
            End Get
        End Property
        Public ReadOnly Property Description As String Implements IAFunction.Description
            Get
                Return "Logistic Sigmoid Activation Function"
            End Get
        End Property
        Public ReadOnly Property Clamp As Double Implements IAFunction.Clamp
            Get
                Return 45.0F
            End Get
        End Property
    End Class
End Namespace
