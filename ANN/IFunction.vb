Namespace ANN
    Public Interface IAFunction
        Function Output(x As Double) As Double
        Function Derivative(value As Double) As Double
        ReadOnly Property Name As String
        ReadOnly Property Description As String
        ReadOnly Property Clamp As Double
    End Interface
End Namespace