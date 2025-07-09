' SuperMarket.BLL/BranchBLL.vb

Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class BranchBLL
    Private dal As New BranchDAL()

    ' استدعاء الدالة من طبقة DAL لجلب الفروع
    Public Function GetAllBranches() As List(Of Branch)
        Return dal.GetAllBranches()
    End Function

    ' استدعاء الدالة من طبقة DAL لإضافة فرع
    Public Sub AddBranch(ByVal branch As Branch)
        ' هنا يمكن إضافة منطق إضافي، مثلاً:
        If String.IsNullOrWhiteSpace(branch.BranchName) Then
            Throw New Exception("اسم الفرع مطلوب.")
        End If
        dal.AddBranch(branch)
    End Sub
End Class
