
Namespace ANN
    <Serializable> Public Class Data
        Public Property Name As String
        Public Property Input As Double()
        Public Property Output As Double()
        Public Property Sample As String
        Sub New(input As Double(), output As Double(), Optional name As String = "")
            Me.Input = input
            Me.Output = output
            Me.Name = name
            Me.Sample = String.Empty
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} [{1}]", Me.Name, Me.Input.Sum / Me.Input.Count)
        End Function
    End Class
End Namespace