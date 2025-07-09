' SuperMarket.DAL/JournalVoucherDAL.vb
Imports System.Data.SqlClient
    Imports System.Configuration
    Imports SuperMarket.Entities

    Public Class JournalVoucherDAL
        Private ReadOnly _connectionString As String = ConfigurationManager.ConnectionStrings("SuperMarketDB").ConnectionString
        Public Function HasTransactions(ByVal accountId As Integer) As Boolean
            Using conn As New SqlConnection(_connectionString)
                ' نستخدم EXISTS لأنه الأسرع للتحقق من وجود سجلات فقط
                Dim query As String = "SELECT 1 FROM JournalVoucherDetails WHERE AccountID = @AccountID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@AccountID", accountId)
                conn.Open()
                ' ExecuteScalar سيُرجع 1 إذا وجد سجلات، و Nothing إذا لم يجد
                Return cmd.ExecuteScalar() IsNot Nothing
            End Using
        End Function
    ' في SuperMarket.DAL/JournalVoucherDAL.vb

    Public Function SaveJournalVoucher(ByVal jv As JournalVoucher) As Long
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()
            Try
                ' ==========================================================
                ' === 1. تعديل جملة INSERT لتشمل كل الأعمدة الاختيارية ===
                ' ==========================================================
                Dim headerQuery As String = "INSERT INTO JournalVouchers " &
                                        "(VoucherDate, Description, SalesInvoiceID, PurchaseInvoiceID, SalesReturnInvoiceID, StockTransferID) " &
                                        "VALUES (@Date, @Desc, @SalesID, @PurchaseID, @ReturnID, @TransferID); " &
                                        "SELECT SCOPE_IDENTITY();"

                Dim cmdHeader As New SqlCommand(headerQuery, conn, transaction)
                cmdHeader.Parameters.AddWithValue("@Date", jv.VoucherDate)
                cmdHeader.Parameters.AddWithValue("@Desc", jv.Description)

                ' ==========================================================
                ' === 2. تعديل الباراميترات لتمرير القيم الاختيارية بأمان ===
                ' ==========================================================
                ' نستخدم CType(..., Object) لضمان أن .NET يتعامل مع DBNull.Value بشكل صحيح
                cmdHeader.Parameters.AddWithValue("@SalesID", If(jv.SalesInvoiceID.HasValue, CType(jv.SalesInvoiceID.Value, Object), DBNull.Value))
                cmdHeader.Parameters.AddWithValue("@PurchaseID", If(jv.PurchaseInvoiceID.HasValue, CType(jv.PurchaseInvoiceID.Value, Object), DBNull.Value))
                cmdHeader.Parameters.AddWithValue("@ReturnID", If(jv.SalesReturnInvoiceID.HasValue, CType(jv.SalesReturnInvoiceID.Value, Object), DBNull.Value))
                cmdHeader.Parameters.AddWithValue("@TransferID", If(jv.StockTransferID.HasValue, CType(jv.StockTransferID.Value, Object), DBNull.Value))
                ' ==========================================================

                jv.VoucherID = Convert.ToInt64(cmdHeader.ExecuteScalar())

                ' --- حفظ تفاصيل القيد (يبقى كما هو) ---
                For Each detail As JournalVoucherDetail In jv.Details
                    Dim detailQuery As String = "INSERT INTO JournalVoucherDetails (VoucherID, AccountID, Debit, Credit, Description) VALUES (@VoucherID, @AccountID, @Debit, @Credit, @Desc)"
                    Dim cmdDetail As New SqlCommand(detailQuery, conn, transaction)
                    cmdDetail.Parameters.AddWithValue("@VoucherID", jv.VoucherID)
                    cmdDetail.Parameters.AddWithValue("@AccountID", detail.AccountID)
                    cmdDetail.Parameters.AddWithValue("@Debit", detail.Debit)
                    cmdDetail.Parameters.AddWithValue("@Credit", detail.Credit)
                    cmdDetail.Parameters.AddWithValue("@Desc", If(String.IsNullOrWhiteSpace(detail.Description), DBNull.Value, CType(detail.Description, Object)))
                    cmdDetail.ExecuteNonQuery()
                Next

                transaction.Commit()
                Return jv.VoucherID

            Catch ex As Exception
                transaction.Rollback()
                Throw ex
            End Try
        End Using
    End Function
    ' في نهاية كلاس JournalVoucherDAL.vb

    ''' <summary>
    ''' يجلب كشف حساب تفصيلي (دفتر أستاذ) لحساب معين خلال فترة زمنية.
    ''' </summary>
    Public Function GetLedgerForAccount(ByVal accountId As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of LedgerEntry)
        Dim ledgerEntries As New List(Of LedgerEntry)()

        Using conn As New SqlConnection(_connectionString)
            ' الاستعلام المجمع الذي جهزناه
            Dim query As String = "
            WITH OpeningBalance AS (
                SELECT ISNULL(SUM(Debit - Credit), 0) AS Balance
                FROM JournalVoucherDetails vd
                JOIN JournalVouchers vh ON vd.VoucherID = vh.VoucherID
                WHERE vd.AccountID = @AccountID AND vh.VoucherDate < @StartDate
            ),
            Movements AS (
                SELECT 
                    vh.VoucherDate,
                    vh.Description AS VoucherDescription,
                    vd.Description AS DetailDescription,
                    vd.Debit,
                    vd.Credit
                FROM JournalVoucherDetails vd
                JOIN JournalVouchers vh ON vd.VoucherID = vh.VoucherID
                WHERE vd.AccountID = @AccountID AND vh.VoucherDate BETWEEN @StartDate AND @EndDate
            )
            SELECT 
                VoucherDate, VoucherDescription, DetailDescription, Debit, Credit,
                (SELECT Balance FROM OpeningBalance) + SUM(Debit - Credit) OVER (ORDER BY VoucherDate, VoucherDescription, DetailDescription ROWS UNBOUNDED PRECEDING) AS Balance
            FROM Movements
            ORDER BY VoucherDate;"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@AccountID", accountId)
            cmd.Parameters.AddWithValue("@StartDate", startDate)
            cmd.Parameters.AddWithValue("@EndDate", endDate)

            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim entry As New LedgerEntry()
                    entry.VoucherDate = Convert.ToDateTime(reader("VoucherDate"))
                    entry.VoucherDescription = reader("VoucherDescription").ToString()
                    entry.DetailDescription = reader("DetailDescription").ToString()
                    entry.Debit = Convert.ToDecimal(reader("Debit"))
                    entry.Credit = Convert.ToDecimal(reader("Credit"))
                    entry.Balance = Convert.ToDecimal(reader("Balance"))
                    ledgerEntries.Add(entry)
                End While
            End Using
        End Using

        Return ledgerEntries
    End Function
    ' في نهاية كلاس JournalVoucherDAL.vb

    ''' <summary>
    ''' يجلب بيانات ميزان المراجعة لفترة زمنية محددة.
    ''' </summary>
    Public Function GetTrialBalance(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal showOnlyTransactional As Boolean) As List(Of TrialBalanceEntry)
        Dim trialBalanceEntries As New List(Of TrialBalanceEntry)()

        Using conn As New SqlConnection(_connectionString)
            ' الاستعلام المركزي لميزان المراجعة
            Dim query As String = "
            WITH AccountMovements AS (
                SELECT d.AccountID, SUM(d.Debit) AS TotalDebit, SUM(d.Credit) AS TotalCredit
                FROM JournalVoucherDetails d
                JOIN JournalVouchers h ON d.VoucherID = h.VoucherID
                WHERE h.VoucherDate BETWEEN @StartDate AND @EndDate
                GROUP BY d.AccountID
            ), 
            OpeningBalances AS (
                SELECT d.AccountID, SUM(d.Debit - d.Credit) AS OpeningBalance
                FROM JournalVoucherDetails d
                JOIN JournalVouchers h ON d.VoucherID = h.VoucherID
                WHERE h.VoucherDate < @StartDate
                GROUP BY d.AccountID
            )
            SELECT
                a.AccountNumber,
                a.AccountName,
                ISNULL(ob.OpeningBalance, 0) AS OpeningBalance,
                ISNULL(m.TotalDebit, 0) AS PeriodDebit,
                ISNULL(m.TotalCredit, 0) AS PeriodCredit,
                (ISNULL(ob.OpeningBalance, 0) + ISNULL(m.TotalDebit, 0) - ISNULL(m.TotalCredit, 0)) AS ClosingBalance
            FROM Accounts a
            LEFT JOIN AccountMovements m ON a.AccountID = m.AccountID
            LEFT JOIN OpeningBalances ob ON a.AccountID = ob.AccountID "

            ' إضافة شرط لعرض الحسابات التحليلية فقط إذا طلب المستخدم ذلك
            If showOnlyTransactional Then
                query &= " WHERE a.IsTransactional = 1 "
            End If

            query &= " ORDER BY a.AccountNumber;"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@StartDate", startDate)
            cmd.Parameters.AddWithValue("@EndDate", endDate)

            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim entry As New TrialBalanceEntry()
                    entry.AccountNumber = reader("AccountNumber").ToString()
                    entry.AccountName = reader("AccountName").ToString()
                    entry.OpeningBalance = Convert.ToDecimal(reader("OpeningBalance"))
                    entry.PeriodDebit = Convert.ToDecimal(reader("PeriodDebit"))
                    entry.PeriodCredit = Convert.ToDecimal(reader("PeriodCredit"))
                    entry.ClosingBalance = Convert.ToDecimal(reader("ClosingBalance"))
                    trialBalanceEntries.Add(entry)
                End While
            End Using
        End Using

        Return trialBalanceEntries
    End Function
End Class




