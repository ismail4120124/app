' SuperMarket.BLL/StockTransferBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class StockTransferBLL
    Private dal As New StockTransferDAL()

    ''' <summary>
    ''' يتحقق من صحة بيانات أمر التحويل ثم يقوم بحفظه.
    ''' </summary>
    ''' <param name="transfer">كائن أمر التحويل المراد حفظه.</param>
    ''' <returns>رقم أمر التحويل الجديد بعد الحفظ.</returns>
    Public Function SaveStockTransfer(ByVal transfer As StockTransfer) As Integer
        ' ==========================================================
        ' === 1. تطبيق قواعد العمل والتحقق من صحة البيانات ===
        ' ==========================================================

        ' التحقق من وجود أصناف في أمر التحويل
        If transfer.Details Is Nothing OrElse transfer.Details.Count = 0 Then
            Throw New Exception("لا يمكن حفظ أمر تحويل فارغ. يرجى إضافة صنف واحد على الأقل.")
        End If

        ' التحقق من أن الفرع المصدر والمستلم صالحان
        If transfer.SourceBranchID <= 0 OrElse transfer.DestinationBranchID <= 0 Then
            Throw New Exception("يجب اختيار فرع مصدر وفرع مستلم.")
        End If

        ' التحقق من أن الفرع المصدر والمستلم مختلفان
        If transfer.SourceBranchID = transfer.DestinationBranchID Then
            Throw New Exception("لا يمكن التحويل من وإلى نفس الفرع.")
        End If

        ' التحقق من وجود المستخدم
        If transfer.UserID <= 0 Then
            Throw New Exception("بيانات المستخدم غير صالحة.")
        End If

        ' التحقق من أن كل الكميات المحولة أكبر من صفر
        For Each item As StockTransferDetail In transfer.Details
            If item.Quantity <= 0 Then
                Throw New Exception("كمية التحويل للصنف '" & item.ProductName & "' يجب أن تكون أكبر من صفر.")
            End If
        Next

        ' ==========================================================
        ' === 2. استدعاء طبقة DAL لتنفيذ الحفظ الفعلي ===
        ' ==========================================================

        ' لا توجد عمليات حسابية هنا، فقط تمرير الكائن بعد التحقق منه
        Return dal.SaveStockTransfer(transfer)
    End Function

End Class