' SuperMarket.BLL/SettingsBLL.vb
Imports SuperMarket.DAL
Imports SuperMarket.Entities

Public Class SettingsBLL
    Private dal As New SettingsDAL()

    Public Function GetAccountingSettings() As AccountingSettings1
        Return dal.GetAccountingSettings()
    End Function

    Public Sub SaveAccountingSettings(ByVal settings As AccountingSettings1)
        ' يمكنك إضافة أي تحقق هنا إذا لزم الأمر
        dal.SaveAccountingSettings(settings)
    End Sub
End Class