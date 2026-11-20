Public Class RentLinkListingPaymentDetails
    Public Property ListingRemoteId As String
    Public Property OwnerUid As String
    Public Property PaymentOption As String
    Public Property MobileMoneyNetwork As String
    Public Property MobileMoneyAccountName As String
    Public Property MobileMoneyNumber As String
    Public Property BankName As String
    Public Property BankAccountName As String
    Public Property BankAccountNumber As String
End Class

Public Class RentLinkActivityEvent
    Public Property Timestamp As DateTime
    Public Property EventType As String
    Public Property UserEmail As String
    Public Property UserName As String
    Public Property ListingId As Integer
    Public Property ListingTitle As String
    Public Property Amount As Decimal
    Public Property Notes As String
    Public Property UserUid As String
    Public Property ListingRemoteId As String
    Public Property ListingOwnerUid As String
    Public Property RenterPhone As String
    Public Property ViewingDate As DateTime?
End Class

Public Module RentLinkDataStore
    Public Function LoadListings() As List(Of RentLinkWorkspace.PropertyListing)
        Return RentLinkLocalStore.LoadListings()
    End Function

    Public Sub SaveListings(listings As IEnumerable(Of RentLinkWorkspace.PropertyListing))
        RentLinkLocalStore.SaveListings(listings)
    End Sub

    Public Function LoadListingPaymentDetails(listingRemoteId As String) As RentLinkListingPaymentDetails
        Return RentLinkLocalStore.LoadListingPaymentDetails(listingRemoteId)
    End Function

    Public Sub SaveListingPaymentDetails(details As RentLinkListingPaymentDetails)
        RentLinkLocalStore.SaveListingPaymentDetails(details)
    End Sub

    Public Sub UpdateListing(listing As RentLinkWorkspace.PropertyListing)
        RentLinkLocalStore.UpdateListing(listing)
    End Sub

    Public Function LoadActivity() As List(Of RentLinkActivityEvent)
        Return RentLinkLocalStore.LoadActivity()
    End Function

    Public Sub RecordActivity(eventType As String, userEmail As String, userName As String, Optional listingId As Integer = 0, Optional listingTitle As String = "", Optional amount As Decimal = 0D, Optional notes As String = "", Optional listingRemoteId As String = "", Optional listingOwnerUid As String = "", Optional renterPhone As String = "", Optional viewingDate As DateTime? = Nothing)
        RentLinkLocalStore.RecordActivity(eventType, userEmail, userName, listingId, listingTitle, amount, notes, listingRemoteId, listingOwnerUid, renterPhone, viewingDate)
    End Sub

    Public Function HasCompletedPurchase(listingRemoteId As String) As Boolean
        Return RentLinkLocalStore.HasCompletedPurchase(listingRemoteId)
    End Function

    Public Sub RecordPurchase(userEmail As String, userName As String, listingId As Integer, listingTitle As String, amount As Decimal, notes As String, listingRemoteId As String, listingOwnerUid As String)
        RentLinkLocalStore.RecordPurchase(userEmail, userName, listingId, listingTitle, amount, notes, listingRemoteId, listingOwnerUid)
    End Sub
End Module
