Namespace ANN
    <Serializable> Public Class Settings
        Public Property Input As Integer
        Public Property Hidden As Integer
        Public Property Layers As Integer
        Public Property Output As Integer
        Public Property LearnRate As Double
        Public Property Momentum As Double
        Public Property DeepLearn As Boolean
        Public Property Activation As IAFunction
        Sub New()
        End Sub
        Sub New(input As Integer, hidden As Integer, layers As Integer, output As Integer)
            Me.DeepLearn = True
            Me.Input = input
            Me.Hidden = hidden
            Me.Layers = layers
            Me.Output = output
            Me.LearnRate = 0.3F
            Me.Momentum = 0.7F
            Me.Activation = New Sigmoid
        End Sub
    End Class
End Namespace