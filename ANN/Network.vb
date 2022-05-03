
Namespace ANN
    <Serializable>
    Public Class Network
        Public Const MAXSAMPLES As Integer = 100
        Public Const MAXTARGET As Double = 0.005
        Public Property Settings As Settings
        Public Property Iterations As Integer
        Public Property Input As List(Of Neuron)
        Public Property Output As List(Of Neuron)
        Public Property Hidden As List(Of Hidden)
        <NonSerialized>
        Public Event Epoch(i As Integer, err As Double, target As Double)

        Sub New()
            Me.Iterations = 0
        End Sub

        Sub New(settings As Settings)
            MyClass.New
            Me.Settings = settings
            Me.Input = New List(Of Neuron)
            Me.Output = New List(Of Neuron)
            Me.Hidden = New List(Of Hidden)

            For i As Integer = 0 To Me.Settings.Input - 1
                Me.Input.Add(New Neuron(Me.Settings.Activation))
            Next
            For i As Integer = 0 To Me.Settings.Layers - 1
                Me.Hidden.Add(New Hidden)
                For j As Integer = 0 To Me.Settings.Hidden - 1
                    Me.Hidden(i).Add(New Neuron(Me.Settings.Activation, If(i = 0, Me.Input, Me.Hidden(i - 1))))
                Next
            Next
            For i As Integer = 0 To Me.Settings.Output - 1
                Me.Output.Add(New Neuron(Me.Settings.Activation, Me.Hidden(Me.Settings.Layers - 1)))
            Next
        End Sub

        Public Sub Train(data As List(Of Data), max As Integer)
            Dim condition As Double = 1.0
            Dim errors As New List(Of Double)
            For i As Integer = 0 To max - 1
                For Each item In data
                    Me.Forward(item.Input)
                    Me.Backward(item.Output)
                    errors.Add(Me.GetError(item.Output))
                    If (errors.Count >= Network.MAXSAMPLES) Then
                        errors.RemoveAt(0)
                    End If
                Next
                If (errors.Any) Then
                    condition = errors.Average()
                End If
                RaiseEvent Epoch(Me.Iterations, condition, MAXTARGET)
                Me.Iterations += 1
            Next
        End Sub

        Public Sub Train(data As List(Of Data), min As Double)
            Dim condition As Double = 1.0
            Dim errors As New List(Of Double)
            While condition > min AndAlso Me.Iterations < Integer.MaxValue
                For Each item In data
                    Me.Forward(item.Input)
                    Me.Backward(item.Output)
                    errors.Add(Me.GetError(item.Output))
                    If (errors.Count >= Network.MAXSAMPLES) Then
                        errors.RemoveAt(0)
                    End If
                Next
                If (errors.Any) Then
                    condition = errors.Average()
                End If
                RaiseEvent Epoch(Me.Iterations, condition, MAXTARGET)
                Me.Iterations += 1
            End While
        End Sub

        Public Function Compute(ParamArray inputs As Double()) As Double()
            Me.Forward(inputs)
            Return Me.Output.Select(Function(a) a.Value).ToArray()
        End Function

        Public Function GetError(ParamArray targets As Double()) As Double
            Dim index As Integer = 0
            Return Me.Output.Sum(Function(x) Me.ErrorSum(x, index, targets))
        End Function

        Private Sub Forward(ParamArray inputs As Double())
            Dim index As Integer = 0
            Me.Input.ForEach(Sub(x) Me.Forward(x, index, inputs))
            For Each item In Me.Hidden
                item.ForEach(Sub(x) x.GetValue())
            Next
            Me.Output.ForEach(Sub(x) x.GetValue())
        End Sub

        Private Sub Backward(ParamArray targets As Double())
            Dim index As Integer = 0
            Me.Output.ForEach(Sub(x) Me.Backward(x, index, targets))
            For Each item In Me.Hidden.AsEnumerable.Reverse
                item.ForEach(Sub(x) x.GetGradient())
                item.ForEach(Sub(x) x.Update(Me.Settings.LearnRate, Me.Settings.Momentum))
            Next
            Me.Output.ForEach(Sub(x) x.Update(Me.Settings.LearnRate, Me.Settings.Momentum))
        End Sub

        Private Sub Forward(n As Neuron, ByRef index As Integer, ParamArray inputs As Double())
            If (index >= inputs.Length) Then Throw New ArgumentException("index mismatch")
            n.Value = inputs(index)
            index += 1
        End Sub

        Private Sub Backward(n As Neuron, ByRef index As Integer, ParamArray inputs As Double())
            If (index >= inputs.Length) Then Throw New ArgumentException("index mismatch")
            n.GetGradient(inputs(index))
            index += 1
        End Sub

        Private Function ErrorSum(n As Neuron, ByRef index As Integer, ParamArray inputs As Double()) As Double
            If (index >= inputs.Length) Then Throw New ArgumentException("index mismatch")
            Dim value As Double = Math.Abs(n.GetError(inputs(index)))
            index += 1
            Return value
        End Function

    End Class
End Namespace