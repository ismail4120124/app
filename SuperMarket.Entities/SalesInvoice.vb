' في SuperMarket.Entities/SalesInvoice.vb

<Serializable()>
Public Class SalesInvoice
    ' --- الخصائص الأساسية (تطابق أعمدة قاعدة البيانات) ---
    Public Property InvoiceID As Long
    Public Property InvoiceDate As DateTime
    Public Property BranchID As Integer
    Public Property UserID As Integer
    Public Property CustomerID As Integer
    Public Property ShiftID As Integer?
    Public Property TotalAmount As Decimal
    Public Property TaxAmount As Decimal
    Public Property DiscountAmount As Decimal
    Public Property AmountPaid As Decimal
    Public Property PaymentMethod As String
    Public Property Notes As String

    ' --- الخصائص المحسوبة (للقراءة فقط) ---
    ''' <summary>
    ''' المبلغ الإجمالي بعد الخصم والضريبة. هذه الخاصية للقراءة فقط لأنها محسوبة في قاعدة البيانات.
    ''' </summary>
    Public ReadOnly Property FinalAmount As Decimal
        Get
            Return TotalAmount + TaxAmount - DiscountAmount
        End Get
    End Property

    ''' <summary>
    ''' المبلغ المتبقي على الفاتورة. هذه الخاصية للقراءة فقط لأنها محسوبة في قاعدة البيانات.
    ''' </summary>
    Public ReadOnly Property AmountRemaining As Decimal
        Get
            Return FinalAmount - AmountPaid
        End Get
    End Property

    ' --- الخصائص الإضافية (للعرض والطباعة فقط) ---
    Public Property BranchName As String
    Public Property UserName As String ' اسم الكاشير
    Public Property CustomerName As String

    ' --- قائمة التفاصيل ---
    Public Property Details As New List(Of SalesInvoiceDetail)()
End Class