' Site1.master.vb
Imports SuperMarket.Entities

' تأكد من أن هذا الملف يرث من BasePage لتتمكن من استخدام الصلاحيات
Public Class Site1
    Inherits BaseMasterPage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' التحقق من أن المستخدم مسجل للدخول قبل إظهار أي شيء
        If CurrentUser IsNot Nothing Then
            ' استدعاء دالة التحكم في ظهور القوائم
            SetMenuVisibility()
        End If
    End Sub

    ''' <summary>
    ''' دالة مركزية للتحكم في ظهور وإخفاء القوائم الرئيسية بناءً على صلاحيات المستخدم.
    ''' </summary>
    Private Sub SetMenuVisibility()
        ' كل مستخدم (حتى الكاشير) يمكنه رؤية قائمة العمليات
        menuOperations.Visible = True

        ' قائمة البيانات الأساسية (الأصناف، العملاء، الموردين)
        ' تظهر إذا كان لدى المستخدم أي صلاحية لإدارة أي منها
        menuData.Visible = UserHasPermission("Manage_Products") OrElse
                           UserHasPermission("Manage_Customers") OrElse
                           UserHasPermission("Manage_Suppliers")

        ' قائمة المحاسبة
        menuAccounting.Visible = UserHasPermission("View_Financial_Reports") OrElse
                                 UserHasPermission("Manage_Chart_Of_Accounts")

        ' قائمة التقارير
        menuReports.Visible = UserHasPermission("View_Inventory_Reports") OrElse
                              UserHasPermission("View_Sales_Reports") ' صلاحيات مستقبلية

        ' قائمة الإعدادات (الأكثر حساسية)
        menuSettings.Visible = UserHasPermission("Manage_Users") OrElse
                               UserHasPermission("Manage_Roles_And_Permissions") OrElse
                               UserHasPermission("Manage_Accounting_Settings")
    End Sub

End Class