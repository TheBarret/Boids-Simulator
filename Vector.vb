

Public Class Vector
    Public Property X As Double
    Public Property Y As Double

    Sub New()
        Me.X = 0
        Me.Y = 0
    End Sub

    Sub New(x As Double, y As Double)
        Me.X = x
        Me.Y = y
    End Sub

    Public Function Length() As Double
        Return Math.Sqrt(X * X + Y * Y)
    End Function

    Public Function Normalize() As Vector
        Dim len As Double = Me.Length()
        Return New Vector(X / len, Y / len)
    End Function

    Public Function Distance(v1 As Vector) As Double
        Dim dx As Double = v1.X - Me.X
        Dim dy As Double = v1.Y - Me.Y
        Return Math.Sqrt(dx * dx + dy * dy)
    End Function

    Public Function SqrtMagnitude() As Double
        Return Math.Sqrt(X * X + Y * Y)
    End Function

    Public ReadOnly Property ToPointF() As PointF
        Get
            Return New PointF(CSng(X), CSng(Y))
        End Get
    End Property

    Public ReadOnly Property Round As Vector
        Get
            Return New Vector(Math.Round(Me.X), Math.Round(Me.Y))
        End Get
    End Property

    Public Shared Operator +(ByVal v1 As Vector, ByVal v2 As Vector) As Vector
        Return New Vector(v1.X + v2.X, v1.Y + v2.Y)
    End Operator

    Public Shared Operator -(ByVal v1 As Vector, ByVal v2 As Vector) As Vector
        Return New Vector(v1.X - v2.X, v1.Y - v2.Y)
    End Operator

    Public Shared Operator *(ByVal v1 As Vector, ByVal v2 As Vector) As Vector
        Return New Vector(v1.X * v2.X, v1.Y * v2.Y)
    End Operator

    Public Shared Operator /(ByVal v1 As Vector, ByVal v2 As Double) As Vector
        Return New Vector(v1.X / v2, v1.Y / v2)
    End Operator

    Public Shared ReadOnly Property Zero As Vector
        Get
            Return New Vector(0, 0)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return String.Format("[{0},{1}]", Me.X, Me.Y)
    End Function
End Class
