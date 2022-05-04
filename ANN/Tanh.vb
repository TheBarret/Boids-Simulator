
Namespace ANN
    <Serializable> Public Class Tanh
        Implements IAFunction

        Public Function Output(value As Double) As Double Implements IAFunction.Output
            If value < -Me.Clamp Then Return -Me.Clamp
            If value > Me.Clamp Then Return Me.Clamp
            Return Math.Tanh(value)
        End Function

        Public Function Derivative(value As Double) As Double Implements IAFunction.Derivative
            Return 1 - Math.Tanh(value) * 2
        End Function

        Public ReadOnly Property Name As String Implements IAFunction.Name
            Get
                Return "Tanh"
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IAFunction.Description
            Get
                Return "Logistic TanH Activation Function"
            End Get
        End Property

        Public ReadOnly Property Clamp As Double Implements IAFunction.Clamp
            Get
                Return 20.0F
            End Get
        End Property
    End Class
End Namespace
