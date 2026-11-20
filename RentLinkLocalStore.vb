Imports System.IO
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text.Json
Imports Npgsql
Imports NpgsqlTypes

' RentLink storage backed by PostgreSQL (replaces the old rentlink-data.json file).
' The public functions keep the same names and signatures as before, so
' RentLinkDataStore.vb and RentLinkWorkspace.vb do not need to change.
Public Module RentLinkLocalStore

    ' ===== EDIT THIS: your PostgreSQL connection =====
    ' You can also set a Windows environment variable called RENTLINK_DB with the
    ' same text, and that will be used instead (keeps the password out of the code).
    Private Const DefaultConnectionString As String = "Host=127.0.0.1;Port=5432;Database=rentlink;Username=postgres;Password=Tracy@123"
    ' ==================================================

    Private Const LocalAdministratorEmail As String = "rdenteh4@gmail.com"
    Private Const LocalAdministratorUsername As String = "Admin"
    Private Const PasswordIterations As Integer = 150000

    Private ReadOnly syncRoot As New Object()
    Private schemaReady As Boolean
    Private currentUserIsAdministrator As Boolean

    Public Property CurrentUserId As String = ""
    Public Property CurrentUserEmail As String = ""
    Public Property CurrentUserName As String = ""

    Public ReadOnly Property IsAdministrator As Boolean
        Get
            Return currentUserIsAdministrator
        End Get
    End Property

    Public ReadOnly Property AdministratorUid As String
        Get
            Return QueryDb(Function(connection)
                               Dim administrator = FindUser(connection, LocalAdministratorEmail)
                               Return If(administrator Is Nothing, "", administrator.UserId)
                           End Function)
        End Get
    End Property

    Public Sub SignOut()
        CurrentUserId = ""
        CurrentUserEmail = ""
        CurrentUserName = ""
        currentUserIsAdministrator = False
    End Sub

#Region "Accounts"

    Public Function Authenticate(email As String, password As String, register As Boolean, name As String, ByRef displayName As String, ByRef errorMessage As String) As Boolean
        displayName = ""
        errorMessage = ""
        Dim normalizedEmail = If(email, "").Trim().ToLowerInvariant()
        Dim isAdminLogin = Not register AndAlso String.Equals(normalizedEmail, LocalAdministratorUsername.ToLowerInvariant(), StringComparison.Ordinal)
        Dim lookupEmail = If(isAdminLogin, LocalAdministratorEmail, normalizedEmail)

        Dim account As LocalUser = QueryDb(Function(connection) FindUser(connection, lookupEmail))

        If register Then
            If account IsNot Nothing Then
                errorMessage = "An account with this email already exists. Please sign in instead."
                Return False
            End If

            Dim salt = RandomNumberGenerator.GetBytes(16)
            Dim newAccount As New LocalUser With {
                .UserId = Guid.NewGuid().ToString("N"),
                .Email = normalizedEmail,
                .Name = If(name, "").Trim(),
                .PasswordSalt = Convert.ToBase64String(salt),
                .PasswordHash = Convert.ToBase64String(HashPassword(password, salt)),
                .IsAdministrator = False
            }
            Try
                RunDb(Sub(connection) InsertUser(connection, newAccount))
            Catch ex As IOException When IsUniqueViolation(ex)
                errorMessage = "An account with this email already exists. Please sign in instead."
                Return False
            End Try
            account = newAccount
        Else
            If account Is Nothing OrElse (isAdminLogin AndAlso Not account.IsAdministrator) Then
                errorMessage = If(isAdminLogin, "The administrator username or password is incorrect.", "No account was found for this email. Register this account first.")
                Return False
            End If

            Try
                Dim salt = Convert.FromBase64String(account.PasswordSalt)
                Dim expectedHash = Convert.FromBase64String(account.PasswordHash)
                If Not CryptographicOperations.FixedTimeEquals(HashPassword(password, salt), expectedHash) Then
                    errorMessage = If(isAdminLogin, "The administrator username or password is incorrect.", "Email or password is incorrect. Check your details or register an account.")
                    Return False
                End If
            Catch ex As FormatException
                Throw New IOException("The account credentials are damaged. Register a new account or contact the administrator.", ex)
            End Try
        End If

        CurrentUserId = account.UserId
        CurrentUserEmail = account.Email
        currentUserIsAdministrator = account.IsAdministrator
        If isAdminLogin Then
            CurrentUserName = LocalAdministratorUsername
            displayName = LocalAdministratorUsername
        Else
            CurrentUserName = account.Name
            displayName = If(account.IsAdministrator, "Administrator", account.Name)
        End If
        Return True
    End Function

    Private Function HashPassword(password As String, salt As Byte()) As Byte()
        Return Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32)
    End Function

    Private Function FindUser(connection As NpgsqlConnection, email As String) As LocalUser
        Using command As New NpgsqlCommand("SELECT user_id, email, name, password_salt, password_hash, is_administrator FROM users WHERE lower(email) = lower(@email)", connection)
            command.Parameters.AddWithValue("email", email)
            Using reader = command.ExecuteReader()
                If Not reader.Read() Then Return Nothing
                Return New LocalUser With {
                    .UserId = reader.GetString(0),
                    .Email = reader.GetString(1),
                    .Name = reader.GetString(2),
                    .PasswordSalt = reader.GetString(3),
                    .PasswordHash = reader.GetString(4),
                    .IsAdministrator = reader.GetBoolean(5)
                }
            End Using
        End Using
    End Function

    Private Sub InsertUser(connection As NpgsqlConnection, account As LocalUser)
        Using command As New NpgsqlCommand("INSERT INTO users (user_id, email, name, password_salt, password_hash, is_administrator) VALUES (@id, @email, @name, @salt, @hash, @admin)", connection)
            command.Parameters.AddWithValue("id", account.UserId)
            command.Parameters.AddWithValue("email", account.Email)
            command.Parameters.AddWithValue("name", If(account.Name, ""))
            command.Parameters.AddWithValue("salt", account.PasswordSalt)
            command.Parameters.AddWithValue("hash", account.PasswordHash)
            command.Parameters.AddWithValue("admin", account.IsAdministrator)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Function IsUniqueViolation(ex As IOException) As Boolean
        Dim postgresError = TryCast(ex.InnerException, PostgresException)
        Return postgresError IsNot Nothing AndAlso postgresError.SqlState = PostgresErrorCodes.UniqueViolation
    End Function

#End Region

#Region "Listings"

    Private Const ListingColumns As String = "id, title, location, price, rent_price, purchase_price, rent_period, bedrooms, bathrooms, kind, mode, contact, communication, description, owner_name, owner_email, owner_uid, remote_id, status, available, views, enquiries, images"

    Private Const UpsertListingSql As String =
        "INSERT INTO listings (" & ListingColumns & ") VALUES (@id, @title, @location, @price, @rent_price, @purchase_price, @rent_period, @bedrooms, @bathrooms, @kind, @mode, @contact, @communication, @description, @owner_name, @owner_email, @owner_uid, @remote_id, @status, @available, @views, @enquiries, @images) " &
        "ON CONFLICT (remote_id) DO UPDATE SET id = EXCLUDED.id, title = EXCLUDED.title, location = EXCLUDED.location, price = EXCLUDED.price, rent_price = EXCLUDED.rent_price, purchase_price = EXCLUDED.purchase_price, rent_period = EXCLUDED.rent_period, bedrooms = EXCLUDED.bedrooms, bathrooms = EXCLUDED.bathrooms, kind = EXCLUDED.kind, mode = EXCLUDED.mode, contact = EXCLUDED.contact, communication = EXCLUDED.communication, description = EXCLUDED.description, owner_name = EXCLUDED.owner_name, owner_email = EXCLUDED.owner_email, owner_uid = EXCLUDED.owner_uid, status = EXCLUDED.status, available = EXCLUDED.available, views = EXCLUDED.views, enquiries = EXCLUDED.enquiries, images = EXCLUDED.images " &
        "WHERE @is_admin OR listings.owner_uid = @current_uid"

    Public Function LoadListings() As List(Of RentLinkWorkspace.PropertyListing)
        Dim isAdmin = IsAdministrator
        Dim uid = If(CurrentUserId, "")
        Return QueryDb(Function(connection)
                           Dim sql = "SELECT " & ListingColumns & " FROM listings" &
                                     If(isAdmin, "", " WHERE lower(status) = 'approved' OR owner_uid = @uid") & " ORDER BY seq"
                           Dim results As New List(Of RentLinkWorkspace.PropertyListing)()
                           Using command As New NpgsqlCommand(sql, connection)
                               If Not isAdmin Then command.Parameters.AddWithValue("uid", uid)
                               Using reader = command.ExecuteReader()
                                   While reader.Read()
                                       results.Add(ReadListing(reader))
                                   End While
                               End Using
                           End Using
                           Return results
                       End Function)
    End Function

    Public Sub SaveListings(listings As IEnumerable(Of RentLinkWorkspace.PropertyListing))
        If listings Is Nothing Then Return
        Dim isAdmin = IsAdministrator
        Dim uid = If(CurrentUserId, "")
        Dim items = listings.Where(Function(item) item IsNot Nothing).ToList()

        For Each item In items
            If String.IsNullOrWhiteSpace(item.RemoteId) Then item.RemoteId = Guid.NewGuid().ToString("N")
            If String.IsNullOrWhiteSpace(item.OwnerUid) Then item.OwnerUid = uid
            If Not isAdmin AndAlso Not String.Equals(item.OwnerUid, uid, StringComparison.Ordinal) Then
                Throw New IOException("RentLink blocked an attempt to save a property that does not belong to this account.")
            End If
            If item.Images Is Nothing Then item.Images = New List(Of String)()
            If item.Images.Count > 5 Then Throw New IOException("A property can have up to 5 photos.")
            For Each imagePath In item.Images
                If imagePath.StartsWith("firestore-photo:", StringComparison.Ordinal) Then Throw New IOException("This photo is still stored in Firebase. Re-select it from this computer before saving.")
                If Not imagePath.StartsWith(ImageRefPrefix, StringComparison.Ordinal) Then
                    If Not File.Exists(imagePath) Then Throw New IOException("A selected property photo could not be found: " & imagePath)
                    If New FileInfo(imagePath).Length > MaxImageBytes Then Throw New IOException("A photo is larger than 10 MB. Please choose a smaller one: " & Path.GetFileName(imagePath))
                End If
            Next
        Next

        Dim originalImages = items.ToDictionary(Function(item) item, Function(item) item.Images.ToList())
        Try
            RunDb(Sub(connection)
                      Using transaction = connection.BeginTransaction()
                          For Each item In items
                              item.Images = StoreImages(connection, transaction, item)
                              If UpsertListing(connection, transaction, item, isAdmin, uid) = 0 Then
                                  Throw New IOException("RentLink blocked an attempt to save a property that belongs to another account.")
                              End If
                              DeleteUnusedImages(connection, transaction, item)
                          Next
                          transaction.Commit()
                      End Using
                  End Sub)
        Catch
            For Each pair In originalImages
                pair.Key.Images = pair.Value
            Next
            Throw
        End Try
    End Sub

    Public Sub UpdateListing(listing As RentLinkWorkspace.PropertyListing)
        If listing Is Nothing OrElse String.IsNullOrWhiteSpace(listing.RemoteId) Then Throw New IOException("RentLink cannot update a property without its saved listing ID.")
        Dim isAdmin = IsAdministrator
        Dim uid = If(CurrentUserId, "")
        If listing.Images Is Nothing Then listing.Images = New List(Of String)()

        RunDb(Sub(connection)
                  Dim existingOwner As String = Nothing
                  Using lookup As New NpgsqlCommand("SELECT owner_uid FROM listings WHERE remote_id = @remote_id", connection)
                      lookup.Parameters.AddWithValue("remote_id", listing.RemoteId)
                      existingOwner = TryCast(lookup.ExecuteScalar(), String)
                  End Using
                  If existingOwner Is Nothing Then Throw New IOException("The property could not be found in the database.")
                  If Not isAdmin AndAlso Not String.Equals(existingOwner, uid, StringComparison.Ordinal) Then
                      Throw New IOException("RentLink blocked an attempt to update a property that does not belong to this account.")
                  End If
                  Dim originalImages = listing.Images.ToList()
                  Try
                      Using transaction = connection.BeginTransaction()
                          listing.Images = StoreImages(connection, transaction, listing)
                          UpsertListing(connection, transaction, listing, isAdmin, uid)
                          DeleteUnusedImages(connection, transaction, listing)
                          transaction.Commit()
                      End Using
                  Catch
                      listing.Images = originalImages
                      Throw
                  End Try
              End Sub)
    End Sub

    Private Const ImageRefPrefix As String = "db-image:"
    Private Const MaxImageBytes As Long = 10L * 1024L * 1024L

    ' Photos are stored inside PostgreSQL (table listing_images). The listing keeps a short
    ' reference like "db-image:abc123". When the app needs to show a photo, this copies it
    ' from the database into a small cache folder on this computer and returns that path.
    Public Function ResolveImagePath(imageReference As String) As String
        If String.IsNullOrWhiteSpace(imageReference) Then Return ""
        If imageReference.StartsWith("firestore-photo:", StringComparison.Ordinal) Then Return ""
        If Not imageReference.StartsWith(ImageRefPrefix, StringComparison.Ordinal) Then Return imageReference

        Dim imageId = imageReference.Substring(ImageRefPrefix.Length)
        If imageId.Length = 0 OrElse Not imageId.All(Function(c) Char.IsLetterOrDigit(c)) Then Return ""

        Try
            Dim cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RentLink", "image-cache")
            Directory.CreateDirectory(cacheFolder)
            Dim cached = Directory.GetFiles(cacheFolder, imageId & ".*")
            If cached.Length > 0 Then Return cached(0)

            Return QueryDb(Function(connection)
                               Using command As New NpgsqlCommand("SELECT file_name, content FROM listing_images WHERE image_id = @id", connection)
                                   command.Parameters.AddWithValue("id", imageId)
                                   Using reader = command.ExecuteReader()
                                       If Not reader.Read() Then Return ""
                                       Dim extension = Path.GetExtension(reader.GetString(0))
                                       If String.IsNullOrWhiteSpace(extension) Then extension = ".jpg"
                                       Dim target = Path.Combine(cacheFolder, imageId & extension)
                                       File.WriteAllBytes(target, DirectCast(reader(1), Byte()))
                                       Return target
                                   End Using
                               End Using
                           End Function)
        Catch ex As IOException
            Return ""
        Catch ex As UnauthorizedAccessException
            Return ""
        End Try
    End Function

    ' Copies any photo that is still a file on this computer into the database
    ' and returns the list of "db-image:" references to save on the listing.
    Private Function StoreImages(connection As NpgsqlConnection, transaction As NpgsqlTransaction, item As RentLinkWorkspace.PropertyListing) As List(Of String)
        Dim stored As New List(Of String)()
        For Each reference In If(item.Images, New List(Of String)())
            If String.IsNullOrWhiteSpace(reference) Then Continue For
            If reference.StartsWith(ImageRefPrefix, StringComparison.Ordinal) OrElse reference.StartsWith("firestore-photo:", StringComparison.Ordinal) Then
                stored.Add(reference)
                Continue For
            End If

            Dim imageId = Guid.NewGuid().ToString("N")
            Dim bytes = File.ReadAllBytes(reference)
            Using command As New NpgsqlCommand("INSERT INTO listing_images (image_id, listing_remote_id, file_name, content) VALUES (@id, @listing, @name, @content)", connection, transaction)
                command.Parameters.AddWithValue("id", imageId)
                command.Parameters.AddWithValue("listing", item.RemoteId)
                command.Parameters.AddWithValue("name", Path.GetFileName(reference))
                command.Parameters.Add("content", NpgsqlDbType.Bytea).Value = bytes
                command.ExecuteNonQuery()
            End Using
            stored.Add(ImageRefPrefix & imageId)
        Next
        Return stored
    End Function

    ' Removes photos from the database that the listing no longer uses.
    Private Sub DeleteUnusedImages(connection As NpgsqlConnection, transaction As NpgsqlTransaction, item As RentLinkWorkspace.PropertyListing)
        Dim keep = item.Images.Where(Function(r) r.StartsWith(ImageRefPrefix, StringComparison.Ordinal)).Select(Function(r) r.Substring(ImageRefPrefix.Length)).ToArray()
        Using command As New NpgsqlCommand("DELETE FROM listing_images WHERE listing_remote_id = @listing AND NOT (image_id = ANY(@keep))", connection, transaction)
            command.Parameters.AddWithValue("listing", item.RemoteId)
            command.Parameters.Add("keep", NpgsqlDbType.Array Or NpgsqlDbType.Text).Value = keep
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Function UpsertListing(connection As NpgsqlConnection, transaction As NpgsqlTransaction, item As RentLinkWorkspace.PropertyListing, isAdmin As Boolean, currentUid As String) As Integer
        Using command As New NpgsqlCommand(UpsertListingSql, connection, transaction)
            With command.Parameters
                .AddWithValue("id", item.Id)
                .AddWithValue("title", If(item.Title, ""))
                .AddWithValue("location", If(item.Location, ""))
                .AddWithValue("price", item.Price)
                .AddWithValue("rent_price", item.RentPrice)
                .AddWithValue("purchase_price", item.PurchasePrice)
                .AddWithValue("rent_period", If(item.RentPeriod, ""))
                .AddWithValue("bedrooms", item.Bedrooms)
                .AddWithValue("bathrooms", item.Bathrooms)
                .AddWithValue("kind", If(item.Kind, ""))
                .AddWithValue("mode", If(item.Mode, ""))
                .AddWithValue("contact", If(item.Contact, ""))
                .AddWithValue("communication", If(item.Communication, ""))
                .AddWithValue("description", If(item.Description, ""))
                .AddWithValue("owner_name", If(item.Owner, ""))
                .AddWithValue("owner_email", If(item.OwnerEmail, ""))
                .AddWithValue("owner_uid", If(item.OwnerUid, ""))
                .AddWithValue("remote_id", item.RemoteId)
                .AddWithValue("status", If(item.Status, ""))
                .AddWithValue("available", item.Available)
                .AddWithValue("views", item.Views)
                .AddWithValue("enquiries", item.Enquiries)
                .AddWithValue("images", JsonSerializer.Serialize(If(item.Images, New List(Of String)())))
                .AddWithValue("is_admin", isAdmin)
                .AddWithValue("current_uid", If(currentUid, ""))
            End With
            Return command.ExecuteNonQuery()
        End Using
    End Function

    Private Function ReadListing(reader As NpgsqlDataReader) As RentLinkWorkspace.PropertyListing
        Return New RentLinkWorkspace.PropertyListing With {
            .Id = reader.GetInt32(0),
            .Title = reader.GetString(1),
            .Location = reader.GetString(2),
            .Price = reader.GetDecimal(3),
            .RentPrice = reader.GetDecimal(4),
            .PurchasePrice = reader.GetDecimal(5),
            .RentPeriod = reader.GetString(6),
            .Bedrooms = reader.GetInt32(7),
            .Bathrooms = reader.GetInt32(8),
            .Kind = reader.GetString(9),
            .Mode = reader.GetString(10),
            .Contact = reader.GetString(11),
            .Communication = reader.GetString(12),
            .Description = reader.GetString(13),
            .Owner = reader.GetString(14),
            .OwnerEmail = reader.GetString(15),
            .OwnerUid = reader.GetString(16),
            .RemoteId = reader.GetString(17),
            .Status = reader.GetString(18),
            .Available = reader.GetBoolean(19),
            .Views = reader.GetInt32(20),
            .Enquiries = reader.GetInt32(21),
            .Images = ParseImages(reader.GetString(22))
        }
    End Function

    Private Function ParseImages(json As String) As List(Of String)
        If String.IsNullOrWhiteSpace(json) Then Return New List(Of String)()
        Try
            Return If(JsonSerializer.Deserialize(Of List(Of String))(json), New List(Of String)())
        Catch ex As JsonException
            Return New List(Of String)()
        End Try
    End Function

#End Region

#Region "Payment details"

    Public Function LoadListingPaymentDetails(listingRemoteId As String) As RentLinkListingPaymentDetails
        Return QueryDb(Function(connection)
                           Using command As New NpgsqlCommand("SELECT listing_remote_id, owner_uid, payment_option, mobile_money_network, mobile_money_account_name, mobile_money_number, bank_name, bank_account_name, bank_account_number FROM listing_payment_details WHERE listing_remote_id = @id", connection)
                               command.Parameters.AddWithValue("id", If(listingRemoteId, ""))
                               Using reader = command.ExecuteReader()
                                   If Not reader.Read() Then Return Nothing
                                   Return New RentLinkListingPaymentDetails With {
                                       .ListingRemoteId = reader.GetString(0),
                                       .OwnerUid = reader.GetString(1),
                                       .PaymentOption = reader.GetString(2),
                                       .MobileMoneyNetwork = reader.GetString(3),
                                       .MobileMoneyAccountName = reader.GetString(4),
                                       .MobileMoneyNumber = reader.GetString(5),
                                       .BankName = reader.GetString(6),
                                       .BankAccountName = reader.GetString(7),
                                       .BankAccountNumber = reader.GetString(8)
                                   }
                               End Using
                           End Using
                       End Function)
    End Function

    Public Sub SaveListingPaymentDetails(details As RentLinkListingPaymentDetails)
        If details Is Nothing OrElse String.IsNullOrWhiteSpace(details.ListingRemoteId) Then Throw New IOException("Payment details cannot be saved without a listing ID.")
        Dim isAdmin = IsAdministrator
        Dim uid = If(CurrentUserId, "")
        If Not isAdmin AndAlso Not String.Equals(details.OwnerUid, uid, StringComparison.Ordinal) Then Throw New IOException("RentLink blocked an attempt to save payment details for another account.")

        RunDb(Sub(connection)
                  Const sql As String = "INSERT INTO listing_payment_details (listing_remote_id, owner_uid, payment_option, mobile_money_network, mobile_money_account_name, mobile_money_number, bank_name, bank_account_name, bank_account_number) " &
                                        "VALUES (@id, @owner, @option, @mm_network, @mm_account, @mm_number, @bank_name, @bank_account_name, @bank_account_number) " &
                                        "ON CONFLICT (listing_remote_id) DO UPDATE SET owner_uid = EXCLUDED.owner_uid, payment_option = EXCLUDED.payment_option, mobile_money_network = EXCLUDED.mobile_money_network, mobile_money_account_name = EXCLUDED.mobile_money_account_name, mobile_money_number = EXCLUDED.mobile_money_number, bank_name = EXCLUDED.bank_name, bank_account_name = EXCLUDED.bank_account_name, bank_account_number = EXCLUDED.bank_account_number " &
                                        "WHERE @is_admin OR listing_payment_details.owner_uid = @current_uid"
                  Using command As New NpgsqlCommand(sql, connection)
                      With command.Parameters
                          .AddWithValue("id", details.ListingRemoteId)
                          .AddWithValue("owner", If(details.OwnerUid, ""))
                          .AddWithValue("option", If(details.PaymentOption, ""))
                          .AddWithValue("mm_network", If(details.MobileMoneyNetwork, ""))
                          .AddWithValue("mm_account", If(details.MobileMoneyAccountName, ""))
                          .AddWithValue("mm_number", If(details.MobileMoneyNumber, ""))
                          .AddWithValue("bank_name", If(details.BankName, ""))
                          .AddWithValue("bank_account_name", If(details.BankAccountName, ""))
                          .AddWithValue("bank_account_number", If(details.BankAccountNumber, ""))
                          .AddWithValue("is_admin", isAdmin)
                          .AddWithValue("current_uid", uid)
                      End With
                      If command.ExecuteNonQuery() = 0 Then Throw New IOException("RentLink blocked an attempt to save payment details for another account.")
                  End Using
              End Sub)
    End Sub

#End Region

#Region "Activity and purchases"

    Private Const ActivityColumns As String = "occurred_at, event_type, user_email, user_name, listing_id, listing_title, amount, notes, user_uid, listing_remote_id, listing_owner_uid, renter_phone, viewing_date"

    Public Function LoadActivity() As List(Of RentLinkActivityEvent)
        Dim isAdmin = IsAdministrator
        Dim uid = If(CurrentUserId, "")
        Return QueryDb(Function(connection)
                           Dim sql = "SELECT " & ActivityColumns & " FROM activity" &
                                     If(isAdmin, "", " WHERE user_uid = @uid OR listing_owner_uid = @uid") & " ORDER BY activity_id"
                           Dim results As New List(Of RentLinkActivityEvent)()
                           Using command As New NpgsqlCommand(sql, connection)
                               If Not isAdmin Then command.Parameters.AddWithValue("uid", uid)
                               Using reader = command.ExecuteReader()
                                   While reader.Read()
                                       Dim viewing As DateTime? = Nothing
                                       If Not reader.IsDBNull(12) Then viewing = reader.GetDateTime(12).ToLocalTime()
                                       results.Add(New RentLinkActivityEvent With {
                                           .Timestamp = reader.GetDateTime(0).ToLocalTime(),
                                           .EventType = reader.GetString(1),
                                           .UserEmail = reader.GetString(2),
                                           .UserName = reader.GetString(3),
                                           .ListingId = reader.GetInt32(4),
                                           .ListingTitle = reader.GetString(5),
                                           .Amount = reader.GetDecimal(6),
                                           .Notes = reader.GetString(7),
                                           .UserUid = reader.GetString(8),
                                           .ListingRemoteId = reader.GetString(9),
                                           .ListingOwnerUid = reader.GetString(10),
                                           .RenterPhone = reader.GetString(11),
                                           .ViewingDate = viewing
                                       })
                                   End While
                               End Using
                           End Using
                           Return results
                       End Function)
    End Function

    Public Sub RecordActivity(eventType As String, userEmail As String, userName As String, Optional listingId As Integer = 0, Optional listingTitle As String = "", Optional amount As Decimal = 0D, Optional notes As String = "", Optional listingRemoteId As String = "", Optional listingOwnerUid As String = "", Optional renterPhone As String = "", Optional viewingDate As DateTime? = Nothing)
        Dim uid = If(CurrentUserId, "")
        RunDb(Sub(connection) InsertActivity(connection, Nothing, eventType, userEmail, userName, listingId, listingTitle, amount, notes, uid, listingRemoteId, listingOwnerUid, renterPhone, viewingDate))
    End Sub

    Public Function HasCompletedPurchase(listingRemoteId As String) As Boolean
        Return QueryDb(Function(connection)
                           Using command As New NpgsqlCommand("SELECT 1 FROM purchased_listings WHERE listing_remote_id = @id", connection)
                               command.Parameters.AddWithValue("id", If(listingRemoteId, ""))
                               Return command.ExecuteScalar() IsNot Nothing
                           End Using
                       End Function)
    End Function

    Public Sub RecordPurchase(userEmail As String, userName As String, listingId As Integer, listingTitle As String, amount As Decimal, notes As String, listingRemoteId As String, listingOwnerUid As String)
        If String.IsNullOrWhiteSpace(listingRemoteId) Then Throw New IOException("This property is missing its listing ID. The purchase was not recorded.")
        Dim uid = If(CurrentUserId, "")
        RunDb(Sub(connection)
                  Using transaction = connection.BeginTransaction()
                      Using command As New NpgsqlCommand("INSERT INTO purchased_listings (listing_remote_id) VALUES (@id) ON CONFLICT (listing_remote_id) DO NOTHING", connection, transaction)
                          command.Parameters.AddWithValue("id", listingRemoteId)
                          If command.ExecuteNonQuery() = 0 Then
                              Throw New InvalidOperationException("This property has already been purchased and cannot be purchased again.")
                          End If
                      End Using
                      InsertActivity(connection, transaction, "SimulatedPurchaseCompleted", userEmail, userName, listingId, listingTitle, amount, notes, uid, listingRemoteId, listingOwnerUid, "", Nothing)
                      transaction.Commit()
                  End Using
              End Sub)
    End Sub

    Private Sub InsertActivity(connection As NpgsqlConnection, transaction As NpgsqlTransaction, eventType As String, userEmail As String, userName As String, listingId As Integer, listingTitle As String, amount As Decimal, notes As String, userUid As String, listingRemoteId As String, listingOwnerUid As String, renterPhone As String, viewingDate As DateTime?)
        Dim sql = "INSERT INTO activity (" & ActivityColumns & ") VALUES (@occurred, @event_type, @user_email, @user_name, @listing_id, @listing_title, @amount, @notes, @user_uid, @listing_remote_id, @listing_owner_uid, @renter_phone, @viewing_date)"
        Using command As New NpgsqlCommand(sql, connection, transaction)
            With command.Parameters
                .AddWithValue("occurred", DateTime.Now.ToUniversalTime())
                .AddWithValue("event_type", If(eventType, ""))
                .AddWithValue("user_email", If(userEmail, ""))
                .AddWithValue("user_name", If(userName, ""))
                .AddWithValue("listing_id", listingId)
                .AddWithValue("listing_title", If(listingTitle, ""))
                .AddWithValue("amount", amount)
                .AddWithValue("notes", If(notes, ""))
                .AddWithValue("user_uid", If(userUid, ""))
                .AddWithValue("listing_remote_id", If(listingRemoteId, ""))
                .AddWithValue("listing_owner_uid", If(listingOwnerUid, ""))
                .AddWithValue("renter_phone", If(renterPhone, ""))
            End With
            Dim viewingParameter = command.Parameters.Add("viewing_date", NpgsqlDbType.TimestampTz)
            If viewingDate.HasValue Then
                Dim value = viewingDate.Value
                If value.Kind = DateTimeKind.Unspecified Then value = DateTime.SpecifyKind(value, DateTimeKind.Local)
                viewingParameter.Value = value.ToUniversalTime()
            Else
                viewingParameter.Value = DBNull.Value
            End If
            command.ExecuteNonQuery()
        End Using
    End Sub

#End Region

#Region "Database plumbing"

    Private ReadOnly Property ConnectionString As String
        Get
            Dim fromEnvironment = Environment.GetEnvironmentVariable("RENTLINK_DB")
            If Not String.IsNullOrWhiteSpace(fromEnvironment) Then Return fromEnvironment
            Return DefaultConnectionString
        End Get
    End Property

    ' Opens a connection, makes sure the tables exist, runs the work, and turns any
    ' database error into an IOException (the rest of the app already handles IOException).
    Private Function QueryDb(Of T)(work As Func(Of NpgsqlConnection, T)) As T
        Try
            Using connection As New NpgsqlConnection(ConnectionString)
                connection.Open()
                EnsureSchema(connection)
                Return work(connection)
            End Using
        Catch ex As NpgsqlException
            Throw New IOException("RentLink coul%5d not use the PostgreSQL database: " & ex.Message, ex)
        End Try
    End Function

    Private Sub RunDb(work As Action(Of NpgsqlConnection))
        QueryDb(Of Boolean)(Function(connection)
                                work(connection)
                                Return True
                            End Function)
    End Sub

    Private Sub EnsureSchema(connection As NpgsqlConnection)
        SyncLock syncRoot
            If schemaReady Then Return

            Const ddl As String =
                "CREATE TABLE IF NOT EXISTS users (" &
                "  user_id text PRIMARY KEY," &
                "  email text NOT NULL," &
                "  name text NOT NULL DEFAULT ''," &
                "  password_salt text NOT NULL," &
                "  password_hash text NOT NULL," &
                "  is_administrator boolean NOT NULL DEFAULT FALSE," &
                "  created_at timestamptz NOT NULL DEFAULT now());" &
                "CREATE UNIQUE INDEX IF NOT EXISTS users_email_lower_idx ON users (lower(email));" &
                "CREATE TABLE IF NOT EXISTS listings (" &
                "  seq bigserial," &
                "  remote_id text PRIMARY KEY," &
                "  id integer NOT NULL DEFAULT 0," &
                "  title text NOT NULL DEFAULT ''," &
                "  location text NOT NULL DEFAULT ''," &
                "  price numeric(18,2) NOT NULL DEFAULT 0," &
                "  rent_price numeric(18,2) NOT NULL DEFAULT 0," &
                "  purchase_price numeric(18,2) NOT NULL DEFAULT 0," &
                "  rent_period text NOT NULL DEFAULT ''," &
                "  bedrooms integer NOT NULL DEFAULT 0," &
                "  bathrooms integer NOT NULL DEFAULT 0," &
                "  kind text NOT NULL DEFAULT ''," &
                "  mode text NOT NULL DEFAULT ''," &
                "  contact text NOT NULL DEFAULT ''," &
                "  communication text NOT NULL DEFAULT ''," &
                "  description text NOT NULL DEFAULT ''," &
                "  owner_name text NOT NULL DEFAULT ''," &
                "  owner_email text NOT NULL DEFAULT ''," &
                "  owner_uid text NOT NULL DEFAULT ''," &
                "  status text NOT NULL DEFAULT ''," &
                "  available boolean NOT NULL DEFAULT TRUE," &
                "  views integer NOT NULL DEFAULT 0," &
                "  enquiries integer NOT NULL DEFAULT 0," &
                "  images text NOT NULL DEFAULT '[]');" &
                "CREATE TABLE IF NOT EXISTS listing_payment_details (" &
                "  listing_remote_id text PRIMARY KEY," &
                "  owner_uid text NOT NULL DEFAULT ''," &
                "  payment_option text NOT NULL DEFAULT ''," &
                "  mobile_money_network text NOT NULL DEFAULT ''," &
                "  mobile_money_account_name text NOT NULL DEFAULT ''," &
                "  mobile_money_number text NOT NULL DEFAULT ''," &
                "  bank_name text NOT NULL DEFAULT ''," &
                "  bank_account_name text NOT NULL DEFAULT ''," &
                "  bank_account_number text NOT NULL DEFAULT '');" &
                "CREATE TABLE IF NOT EXISTS activity (" &
                "  activity_id bigserial PRIMARY KEY," &
                "  occurred_at timestamptz NOT NULL," &
                "  event_type text NOT NULL DEFAULT ''," &
                "  user_email text NOT NULL DEFAULT ''," &
                "  user_name text NOT NULL DEFAULT ''," &
                "  listing_id integer NOT NULL DEFAULT 0," &
                "  listing_title text NOT NULL DEFAULT ''," &
                "  amount numeric(18,2) NOT NULL DEFAULT 0," &
                "  notes text NOT NULL DEFAULT ''," &
                "  user_uid text NOT NULL DEFAULT ''," &
                "  listing_remote_id text NOT NULL DEFAULT ''," &
                "  listing_owner_uid text NOT NULL DEFAULT ''," &
                "  renter_phone text NOT NULL DEFAULT ''," &
                "  viewing_date timestamptz NULL);" &
                "CREATE TABLE IF NOT EXISTS purchased_listings (" &
                "  listing_remote_id text PRIMARY KEY," &
                "  purchased_at timestamptz NOT NULL DEFAULT now());" &
                "CREATE TABLE IF NOT EXISTS listing_images (" &
                "  image_id text PRIMARY KEY," &
                "  listing_remote_id text NOT NULL," &
                "  file_name text NOT NULL DEFAULT ''," &
                "  content bytea NOT NULL," &
                "  created_at timestamptz NOT NULL DEFAULT now());" &
                "CREATE INDEX IF NOT EXISTS listing_images_listing_idx ON listing_images (listing_remote_id);"

            Using command As New NpgsqlCommand(ddl, connection)
                command.ExecuteNonQuery()
            End Using

            SeedAdministrator(connection)
            schemaReady = True
        End SyncLock
    End Sub

    ' Creates the administrator account the first time the database is set up.
    ' Set the Windows environment variable RENTLINK_ADMIN_PASSWORD BEFORE the first run
    ' to choose your own admin password. If it is not set, the temporary first-run
    ' password below is used, so change it as soon as you can.
    Private Sub SeedAdministrator(connection As NpgsqlConnection)
        Dim existing = FindUser(connection, LocalAdministratorEmail)
        If existing IsNot Nothing Then
            If Not existing.IsAdministrator Then
                Using fix As New NpgsqlCommand("UPDATE users SET is_administrator = TRUE, name = @name WHERE user_id = @id", connection)
                    fix.Parameters.AddWithValue("name", LocalAdministratorUsername)
                    fix.Parameters.AddWithValue("id", existing.UserId)
                    fix.ExecuteNonQuery()
                End Using
            End If
            Return
        End If

        Dim initialPassword = Environment.GetEnvironmentVariable("RENTLINK_ADMIN_PASSWORD")
        If String.IsNullOrWhiteSpace(initialPassword) Then initialPassword = "admin@1"

        Dim salt = RandomNumberGenerator.GetBytes(16)
        InsertUser(connection, New LocalUser With {
            .UserId = Guid.NewGuid().ToString("N"),
            .Email = LocalAdministratorEmail,
            .Name = LocalAdministratorUsername,
            .PasswordSalt = Convert.ToBase64String(salt),
            .PasswordHash = Convert.ToBase64String(HashPassword(initialPassword, salt)),
            .IsAdministrator = True
        })
    End Sub

    Private Class LocalUser
        Public Property UserId As String
        Public Property Email As String
        Public Property Name As String
        Public Property PasswordSalt As String
        Public Property PasswordHash As String
        Public Property IsAdministrator As Boolean
    End Class

#End Region

End Module
