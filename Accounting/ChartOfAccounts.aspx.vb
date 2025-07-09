' ChartOfAccounts.aspx.vb
Imports SuperMarket.BLL
Imports SuperMarket.Entities

Public Class ChartOfAccounts
    Inherits BasePage

    Private bll As New AccountBLL()
    Private allAccounts As List(Of Account) ' لتخزين كل الحسابات في متغير واحد لتجنب استدعاء قاعدة البيانات مرارًا

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' جلب كل الحسابات مرة واحدة عند تحميل الصفحة
        allAccounts = bll.GetAllAccounts()
        If Not IsPostBack Then
            PopulateTreeView()
        End If
    End Sub

    ' ==========================================================
    ' === بناء شجرة الحسابات (TreeView) ===
    ' ==========================================================
    Private Sub PopulateTreeView()
        TreeViewAccounts.Nodes.Clear()
        ' ابدأ ببناء الشجرة من الحسابات الرئيسية (التي ليس لها أب)
        Dim rootAccounts = allAccounts.Where(Function(acc) Not acc.ParentAccountID.HasValue).ToList()
        For Each rootAcc As Account In rootAccounts
            Dim rootNode As New TreeNode(rootAcc.AccountName, rootAcc.AccountID.ToString())
            TreeViewAccounts.Nodes.Add(rootNode)
            ' استدعاء دالة متكررة لإضافة الأبناء
            AddChildNodes(rootNode, rootAcc.AccountID)
        Next
    End Sub

    Private Sub AddChildNodes(ByVal parentNode As TreeNode, ByVal parentId As Integer)
        ' ابحث عن كل الحسابات التي هذا الحساب هو الأب لها
        Dim childAccounts = allAccounts.Where(Function(acc) acc.ParentAccountID.HasValue AndAlso acc.ParentAccountID.Value = parentId).ToList()
        For Each childAcc As Account In childAccounts
            Dim childNode As New TreeNode(childAcc.AccountName, childAcc.AccountID.ToString())
            parentNode.ChildNodes.Add(childNode)
            ' استدعاء الدالة نفسها لإضافة أبناء الأبناء (Recursion)
            AddChildNodes(childNode, childAcc.AccountID)
        Next
    End Sub

    ' ==========================================================
    ' === التعامل مع تفاعل المستخدم ===
    ' ==========================================================
    Protected Sub TreeViewAccounts_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeViewAccounts.SelectedNodeChanged
        ' عندما يضغط المستخدم على حساب في الشجرة، نجهز النموذج لإضافة "ابن" لهذا الحساب
        Dim selectedNode As TreeNode = TreeViewAccounts.SelectedNode
        Dim selectedAccountId As Integer = Convert.ToInt32(selectedNode.Value)

        Dim selectedAccount = allAccounts.FirstOrDefault(Function(acc) acc.AccountID = selectedAccountId)

        If selectedAccount IsNot Nothing Then
            ' التحقق مما إذا كان الحساب المختار تحليليًا أم لا
            If selectedAccount.IsTransactional Then
                ' إذا كان الحساب تحليليًا، لا يمكن إضافة أبناء له.
                ' بدلاً من ذلك، سنعرض تفاصيله للتعديل.
                lblFormTitle.Text = "تعديل حساب: " & selectedAccount.AccountName
                txtAccountNumber.Text = selectedAccount.AccountNumber
                txtAccountName.Text = selectedAccount.AccountName
                ddlAccountType.SelectedValue = selectedAccount.AccountType
                chkIsTransactional.Checked = selectedAccount.IsTransactional
                btnDeleteAccount.Visible = True
            Else
                ' إذا كان الحساب رئيسيًا، نجهز النموذج لإضافة حساب فرعي تحته.
                lblFormTitle.Text = "إضافة حساب فرعي جديد"
                txtAccountNumber.Text = "" ' تفريغ الحقول للحساب الجديد
                txtAccountName.Text = ""
                chkIsTransactional.Checked = False
                btnDeleteAccount.Visible = False ' لا يمكن حذف الأب عند إضافة ابن
            End If

            ' في كلتا الحالتين، نحدد الحساب الأب
            hdnSelectedAccountID.Value = selectedAccount.AccountID.ToString()
            txtParentAccount.Text = selectedAccount.AccountName & " (" & selectedAccount.AccountNumber & ")"

            ' نوع الحساب الفرعي يكون نفس نوع الأب افتراضيًا
            ddlAccountType.SelectedValue = selectedAccount.AccountType

            lblMessage.Text = ""
        End If
    End Sub



    ' في ChartOfAccounts.aspx.vb

    Protected Sub btnSaveAccount_Click(sender As Object, e As EventArgs) Handles btnSaveAccount.Click
        Try
            Dim acc As New Account()
            acc.AccountNumber = txtAccountNumber.Text.Trim()
            acc.AccountName = txtAccountName.Text.Trim()
            acc.AccountType = ddlAccountType.SelectedValue
            acc.IsTransactional = chkIsTransactional.Checked

            ' في حالة التعديل، hdnSelectedAccountID يحتوي على ID الحساب نفسه
            ' في حالة الإضافة، يحتوي على ID الحساب الأب (أو 0)
            Dim selectedId As Integer = 0
            Integer.TryParse(hdnSelectedAccountID.Value, selectedId)

            If lblFormTitle.Text.StartsWith("تعديل") Then
                ' --- وضع التعديل ---
                If selectedId <= 0 Then Throw New Exception("خطأ: لم يتم تحديد الحساب المراد تعديله.")
                acc.AccountID = selectedId

                ' === تفعيل السطر التالي ===
                bll.UpdateAccount(acc)
                lblMessage.Text = "تم تعديل الحساب بنجاح."

            Else
                ' --- وضع الإضافة (يبقى كما هو) ---
                Dim parentId As Integer = selectedId
                If parentId > 0 Then
                    Dim parentAccount = allAccounts.FirstOrDefault(Function(a) a.AccountID = parentId)
                    If parentAccount Is Nothing Then Throw New Exception("الحساب الأب المحدد غير موجود.")
                    acc.ParentAccountID = parentId
                    acc.AccountLevel = parentAccount.AccountLevel + 1
                Else
                    acc.ParentAccountID = Nothing
                    acc.AccountLevel = 0
                End If
                bll.AddAccount(acc)
                lblMessage.Text = "تم إضافة الحساب بنجاح."
            End If

            ' --- إعادة تحميل وتحديث الواجهة ---
            allAccounts = bll.GetAllAccounts()
            PopulateTreeView()
            ResetForm()
            lblMessage.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblMessage.Text = "خطأ في الحفظ: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' في ChartOfAccounts.aspx.vb

    Protected Sub btnDeleteAccount_Click(sender As Object, e As EventArgs) Handles btnDeleteAccount.Click
        Try
            Dim accountIdToDelete As Integer = 0
            Integer.TryParse(hdnSelectedAccountID.Value, accountIdToDelete)

            If accountIdToDelete <= 0 Then
                Throw New Exception("الرجاء تحديد الحساب المراد حذفه من الشجرة أولاً.")
            End If

            ' استدعاء BLL لتنفيذ عملية الحذف مع التحققات
            bll.DeleteAccount(accountIdToDelete)

            ' عرض رسالة نجاح وإعادة تحميل الواجهة
            lblMessage.Text = "تم حذف الحساب بنجاح."
            lblMessage.ForeColor = System.Drawing.Color.Green

            allAccounts = bll.GetAllAccounts()
            PopulateTreeView()
            ResetForm()

        Catch ex As Exception
            ' عرض أي خطأ ناتج عن فشل التحقق أو الحذف
            lblMessage.Text = "فشل الحذف: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' في ChartOfAccounts.aspx.vb

    Protected Sub btnNewMainAccount_Click(sender As Object, e As EventArgs) Handles btnNewMainAccount.Click
        ResetForm()
        ' إلغاء تحديد أي عقدة في الشجرة
        If TreeViewAccounts.SelectedNode IsNot Nothing Then
            TreeViewAccounts.SelectedNode.Selected = False
        End If
    End Sub

    Private Sub ResetForm()
        lblFormTitle.Text = "إضافة حساب رئيسي جديد"
        hdnSelectedAccountID.Value = "0"
        txtAccountNumber.Text = ""
        txtAccountName.Text = ""
        txtParentAccount.Text = "-- لا يوجد أب (حساب رئيسي) --"
        chkIsTransactional.Checked = False
        chkIsTransactional.Enabled = True
        btnDeleteAccount.Visible = False
        lblMessage.Text = ""
        ddlAccountType.SelectedIndex = 0
    End Sub

End Class