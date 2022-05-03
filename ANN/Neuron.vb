
Namespace ANN
    <Serializable> Public Class Neuron
        Public Property Bias As Double
        Public Property Delta As Double
        Public Property Gradient As Double
        Public Property Value As Double
        Public Property Input As List(Of Synapse)
        Public Property Output As List(Of Synapse)
        Public Property Activation As IAFunction
        Sub New()
        End Sub

        Sub New(af As IAFunction)
            Me.Activation = af
            Me.Input = New List(Of Synapse)
            Me.Output = New List(Of Synapse)
            Me.Bias = Randomizer.Double(-1, 1)
        End Sub

        Sub New(af As IAFunction, neurons As IEnumerable(Of Neuron))
            Me.New(af)
            Dim s As Synapse
            For Each n In neurons
                s = New Synapse(n, Me)
                n.Output.Add(s)
                Me.Input.Add(s)
            Next
        End Sub

        Public Overridable Function GetValue() As Double
            Me.Value = Me.Activation.Output(Input.Sum(Function(a) a.Weight * a.Input.Value) + Me.Bias)
            Return Me.Value
        End Function

        Public Function GetError(target As Double) As Double
            Return target - Me.Value
        End Function

        Public Function GetGradient(Optional target As Double? = Nothing) As Double
            If (target IsNot Nothing) Then
                Me.Gradient = Me.GetError(CDbl(target)) * Me.Activation.Derivative(Me.Value)
            Else
                Me.Gradient = Me.Output.Sum(Function(a) a.Output.Gradient * a.Weight) * Me.Activation.Derivative(Me.Value)
            End If
            Return Me.Gradient
        End Function

        Public Sub Update(l As Double, m As Double)
            Dim prev As Double = Me.Delta
            Me.Delta = l * Me.Gradient
            Me.Bias += Me.Delta + m * prev
            For Each s In Me.Input
                prev = s.Delta
                s.Delta = l * Me.Gradient * s.Input.Value
                s.Weight += s.Delta + m * prev
            Next
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} [B:{1} D:{2} G:{3}]", Me.Value, Me.Bias, Me.Delta, Me.Gradient)
        End Function
    End Class
End Namespace