
Namespace ANN
    <Serializable> Public Class Synapse
        Public Property Input As Neuron
        Public Property Output As Neuron
        Public Property Weight As Double
        Public Property Delta As Double
        Sub New()
        End Sub
        Sub New(input As Neuron, output As Neuron)
            Me.Input = input
            Me.Output = output
            Me.Weight = Randomizer.Double(-1, 1)
        End Sub
    End Class
End Namespace