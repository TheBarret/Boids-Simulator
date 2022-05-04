
Namespace Models
    Public Class Shapes
        Public Shared ReadOnly Property Triangle(size As Integer) As Point()
            Get
                Dim points As New List(Of Point) From {New Point(0, 0),
                                                          New Point(-size, -1),
                                                          New Point(0, size + size),
                                                          New Point(size, -1),
                                                          New Point(0, 0)}
                Return points.ToArray
            End Get
        End Property
    End Class
End Namespace