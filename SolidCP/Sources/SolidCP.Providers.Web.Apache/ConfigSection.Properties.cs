using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SolidCP.Providers.Web.Apache
{

	public partial class ConfigSection
	{
		// Sections:

		// Enclose a group of directives that represent an extension of a base authentication provider and referenced by the specified alias
		// <AuthnProviderAlias baseProvider Alias> ... </AuthnProviderAlias>
		// <AuthnProviderAlias baseProvider Alias> ... </AuthnProviderAlias>

		// Enclose a group of directives that represent an extension of a base authorization provider and referenced by the specified alias
		// <AuthzProviderAlias baseProvider Alias Require-Parameters> ... </AuthzProviderAlias> 
		// <AuthzProviderAlias baseProvider Alias Require-Parameters> ... </AuthzProviderAlias> 

		// Enclose a group of directives that apply only to the named file-system directory, sub-directories, and their contents.
		// <Directory directory-path> ... </Directory>
		// <Directory directory-path> ... </Directory>

		// Enclose directives that apply to the contents of file-system directories matching a regular expression.
		// <DirectoryMatch regex> ... </DirectoryMatch>
		// <DirectoryMatch regex> ... </DirectoryMatch>

		// Contains directives that apply only if the condition of a previous <If> or <ElseIf> section is not satisfied by a request at runtime
		// <Else> ... </Else>
		// <Else> ... </Else>

		// Contains directives that apply only if a condition is satisfied by a request at runtime while the condition of a previous <If> or <ElseIf> section is not satisfied
		// <ElseIf expression> ... </ElseIf>
		// <ElseIf expression> ... </ElseIf>

		// Contains directives that apply to matched filenames
		// <Files filename> ... </Files>
		// <Files filename> ... </Files>

		// Contains directives that apply to regular-expression matched filenames
		// <FilesMatch regex> ... </FilesMatch>
		// <FilesMatch regex> ... </FilesMatch>

		// Contains directives that apply only if a condition is satisfied by a request at runtime
		// <If expression> ... </If>
		// <If expression> ... </If>

		// Encloses directives that will be processed only if a test is true at startup
		// <IfDefine [!]parameter-name> ...     </IfDefine>
		// <IfDefine[!] parameter-name> ...     </IfDefine>

		// Encloses directives that are processed conditional on the presence or absence of a specific directive
		// <IfDirective [!]directive-name> ...     </IfDirective>
		// <IfDirective[!] directive-name> ...     </IfDirective>

		// Encloses directives that will be processed only if file exists at startup
		// <IfFile [!]filename> ...     </IfFile>
		// <IfFile[!] filename> ...     </IfFile>

		// Encloses directives that are processed conditional on the presence or absence of a specific module
		// <IfModule [!]module-file|module-identifier> ...     </IfModule>
		// <IfModule[!] module-file|module-identifier> ...     </IfModule>

		// Encloses directives that are processed conditional on the presence or absence of a specific section directive
		// <IfSection [!]section-name> ...     </IfSection>
		// <IfSection[!] section-name> ...     </IfSection>

		// contains version dependent configuration
		// <IfVersion [[!]operator] version> ... </IfVersion>
		// <IfVersion[[!]operator] version> ... </IfVersion>

		// Restrict enclosed access controls to only certain HTTP methods
		// <Limit method [method] ... > ...     </Limit>
		// <Limit method[method] ... > ...     </Limit>

		// Restrict access controls to all HTTP methods except the named ones
		// <LimitExcept method [method] ... > ...     </LimitExcept>
		// <LimitExcept method[method] ... > ...     </LimitExcept>

		// Applies the enclosed directives only to matching URLs
		// <Location     URL-path|URL> ... </Location>
		// <Location URL-path|URL> ... </Location>

		// Applies the enclosed directives only to regular-expression matching URLs
		// <LocationMatch     regex> ... </LocationMatch>
		// <LocationMatch regex> ... </LocationMatch>

		// Define a configuration file macro
		// <Macro name [par1 .. parN]> ... </Macro>
		// <Macro name[par1..parN]> ... </Macro>

		// Container for directives applied to the same managed domains.
		// <MDomainSet dns-name [ other-dns-name... ]>...</MDomainSet>
		// <MDomainSet dns-name[other - dns - name... ]>...</MDomainSet>

		// Container for directives applied to proxied resources
		// <Proxy wildcard-url> ...</Proxy>
		// <Proxy wildcard-url> ...</Proxy>

		// Container for directives applied to regular-expression-matched proxied resources
		// <ProxyMatch regex> ...</ProxyMatch>
		// <ProxyMatch regex> ...</ProxyMatch>

		// Enclose a group of authorization directives of which none must fail and at least one must succeed for the enclosing directive to succeed.
		// <RequireAll> ... </RequireAll>
		// <RequireAll> ... </RequireAll>

		// Enclose a group of authorization directives of which one must succeed for the enclosing directive to succeed.
		// <RequireAny> ... </RequireAny>
		// <RequireAny> ... </RequireAny>

		// Enclose a group of authorization directives of which none must succeed for the enclosing directive to not fail.
		// <RequireNone> ... </RequireNone>
		// <RequireNone> ... </RequireNone>

		// Contains directives that apply only to a specific hostname or IP address
		// <VirtualHost addr[:port] [addr[:port]] ...> ... </VirtualHost>
		// <VirtualHost addr[:port] [addr[:port]] ...> ... </VirtualHost>


		// Properties:

		// Configures optimizations for a Protocol's Listener Sockets
		// AcceptFilter protocol accept_filter
		public string AcceptFilter { get { return this[nameof(AcceptFilter)]; } set { this[nameof(AcceptFilter)] = value; } }

		// Resources accept trailing pathname information
		// AcceptPathInfo On|Off|Default
		public string AcceptPathInfo { get { return this[nameof(AcceptPathInfo)]; } set { this[nameof(AcceptPathInfo)] = value; } }

		// Name of the distributed configuration file
		// AccessFileName filename [filename] ...
		public string AccessFileName { get { return this[nameof(AccessFileName)]; } set { this[nameof(AccessFileName)] = value; } }

		// Activates a CGI script for a particular handler or content-type
		// Action action-type cgi-script [virtual]
		public string Action { get { return this[nameof(Action)]; } set { this[nameof(Action)] = value; } }

		// Alternate text to display for a file, instead of an icon selected by filename
		// AddAlt string file [file] ...
		public string AddAlt { get { return this[nameof(AddAlt)]; } set { this[nameof(AddAlt)] = value; } }

		// Alternate text to display for a file instead of an icon selected by MIME-encoding
		// AddAltByEncoding string MIME-encoding [MIME-encoding] ...
		public string AddAltByEncoding { get { return this[nameof(AddAltByEncoding)]; } set { this[nameof(AddAltByEncoding)] = value; } }

		// Alternate text to display for a file, instead of an icon selected by MIME content-type
		// AddAltByType string MIME-type [MIME-type] ...
		public string AddAltByType { get { return this[nameof(AddAltByType)]; } set { this[nameof(AddAltByType)] = value; } }

		// Maps the given filename extensions to the specified content charset
		// AddCharset charset extension [extension] ...
		public string AddCharset { get { return this[nameof(AddCharset)]; } set { this[nameof(AddCharset)] = value; } }

		// Default charset parameter to be added when a response content-type is text/plain or text/html
		// AddDefaultCharset On|Off|charset
		public string AddDefaultCharset { get { return this[nameof(AddDefaultCharset)]; } set { this[nameof(AddDefaultCharset)] = value; } }

		// Description to display for a file
		// AddDescription string file [file] ...
		public string AddDescription { get { return this[nameof(AddDescription)]; } set { this[nameof(AddDescription)] = value; } }

		// Maps the given filename extensions to the specified encoding type
		// AddEncoding encoding extension [extension] ...
		public string AddEncoding { get { return this[nameof(AddEncoding)]; } set { this[nameof(AddEncoding)] = value; } }

		// Maps the filename extensions to the specified handler
		// AddHandler handler-name extension [extension] ...
		public string AddHandler { get { return this[nameof(AddHandler)]; } set { this[nameof(AddHandler)] = value; } }

		// Icon to display for a file selected by name
		// AddIcon icon name [name] ...
		public string AddIcon { get { return this[nameof(AddIcon)]; } set { this[nameof(AddIcon)] = value; } }

		// Icon to display next to files selected by MIME content-encoding
		// AddIconByEncoding icon MIME-encoding [MIME-encoding] ...
		public string AddIconByEncoding { get { return this[nameof(AddIconByEncoding)]; } set { this[nameof(AddIconByEncoding)] = value; } }

		// Icon to display next to files selected by MIME content-type
		// AddIconByType icon MIME-type [MIME-type] ...
		public string AddIconByType { get { return this[nameof(AddIconByType)]; } set { this[nameof(AddIconByType)] = value; } }

		// Maps filename extensions to the filters that will process client requests
		// AddInputFilter filter[;filter...] extension [extension] ...
		public string AddInputFilter { get { return this[nameof(AddInputFilter)]; } set { this[nameof(AddInputFilter)] = value; } }

		// Maps the given filename extension to the specified content language
		// AddLanguage language-tag extension [extension] ...
		public string AddLanguage { get { return this[nameof(AddLanguage)]; } set { this[nameof(AddLanguage)] = value; } }

		// Adds additional information to the module information displayed by the server-info handler
		// AddModuleInfo module-name string
		public string AddModuleInfo { get { return this[nameof(AddModuleInfo)]; } set { this[nameof(AddModuleInfo)] = value; } }

		// Maps filename extensions to the filters that will process responses from the server
		// AddOutputFilter filter[;filter...] extension [extension] ...
		public string AddOutputFilter { get { return this[nameof(AddOutputFilter)]; } set { this[nameof(AddOutputFilter)] = value; } }

		// assigns an output filter to a particular media-type
		// AddOutputFilterByType filter[;filter...] media-type [media-type] ...
		public string AddOutputFilterByType { get { return this[nameof(AddOutputFilterByType)]; } set { this[nameof(AddOutputFilterByType)] = value; } }

		// Maps the given filename extensions onto the specified content type
		// AddType media-type extension [extension] ...
		public string AddType { get { return this[nameof(AddType)]; } set { this[nameof(AddType)] = value; } }

		// Maps URLs to filesystem locations
		// Alias [URL-path] file-path|directory-path
		public string Alias { get { return this[nameof(Alias)]; } set { this[nameof(Alias)] = value; } }

		// Maps URLs to filesystem locations using regular expressions
		// AliasMatch regex file-path|directory-path
		public string AliasMatch { get { return this[nameof(AliasMatch)]; } set { this[nameof(AliasMatch)] = value; } }

		// Map the full path after the alias in a location.
		// AliasPreservePath OFF|ON
		public string AliasPreservePath { get { return this[nameof(AliasPreservePath)]; } set { this[nameof(AliasPreservePath)] = value; } }

		// Controls which hosts can access an area of the server
		// Allow from all|host|env=[!]env-variable [host|env=[!]env-variable] ...
		public string Allow { get { return this[nameof(Allow)]; } set { this[nameof(Allow)] = value; } }

		// Ports that are allowed to CONNECT through the proxy
		// AllowCONNECT port[-port] [port[-port]] ...
		public string AllowCONNECT { get { return this[nameof(AllowCONNECT)]; } set { this[nameof(AllowCONNECT)] = value; } }

		// Determines whether encoded path separators in URLs are allowed to be passed through
		// AllowEncodedSlashes On|Off|NoDecode
		public string AllowEncodedSlashes { get { return this[nameof(AllowEncodedSlashes)]; } set { this[nameof(AllowEncodedSlashes)] = value; } }

		// Restrict access to the listed HTTP methods
		// AllowMethods reset|HTTP-method [HTTP-method]...
		public string AllowMethods { get { return this[nameof(AllowMethods)]; } set { this[nameof(AllowMethods)] = value; } }

		// Types of directives that are allowed in .htaccess files
		// AllowOverride All|None|directive-type [directive-type] ...
		public string AllowOverride { get { return this[nameof(AllowOverride)]; } set { this[nameof(AllowOverride)] = value; } }

		// Individual directives that are allowed in .htaccess files
		// AllowOverrideList None|directive [directive-type] ...
		public string AllowOverrideList { get { return this[nameof(AllowOverrideList)]; } set { this[nameof(AllowOverrideList)] = value; } }

		// Specifies userIDs that are allowed access without password verification
		// Anonymous user [user] ...
		public string Anonymous { get { return this[nameof(Anonymous)]; } set { this[nameof(Anonymous)] = value; } }

		// Sets whether the password entered will be logged in the error log
		// Anonymous_LogEmail On|Off
		public string Anonymous_LogEmail { get { return this[nameof(Anonymous_LogEmail)]; } set { this[nameof(Anonymous_LogEmail)] = value; } }

		// Specifies whether blank passwords are allowed
		// Anonymous_MustGiveEmail On|Off
		public string Anonymous_MustGiveEmail { get { return this[nameof(Anonymous_MustGiveEmail)]; } set { this[nameof(Anonymous_MustGiveEmail)] = value; } }

		// Sets whether the userID field may be empty
		// Anonymous_NoUserID On|Off
		public string Anonymous_NoUserID { get { return this[nameof(Anonymous_NoUserID)]; } set { this[nameof(Anonymous_NoUserID)] = value; } }

		// Sets whether to check the password field for a correctly formatted email address
		// Anonymous_VerifyEmail On|Off
		public string Anonymous_VerifyEmail { get { return this[nameof(Anonymous_VerifyEmail)]; } set { this[nameof(Anonymous_VerifyEmail)] = value; } }

		// Limit concurrent connections per process
		// AsyncRequestWorkerFactor factor
		public string AsyncRequestWorkerFactor { get { return this[nameof(AsyncRequestWorkerFactor)]; } set { this[nameof(AsyncRequestWorkerFactor)] = value; } }

		// Sets whether authorization and authentication are passed to lower level modules
		// AuthBasicAuthoritative On|Off
		public string AuthBasicAuthoritative { get { return this[nameof(AuthBasicAuthoritative)]; } set { this[nameof(AuthBasicAuthoritative)] = value; } }

		// Fake basic authentication using the given expressions for username and password
		// AuthBasicFake off|username [password]
		public string AuthBasicFake { get { return this[nameof(AuthBasicFake)]; } set { this[nameof(AuthBasicFake)] = value; } }

		// Sets the authentication provider(s) for this location
		// AuthBasicProvider provider-name [provider-name] ...
		public string AuthBasicProvider { get { return this[nameof(AuthBasicProvider)]; } set { this[nameof(AuthBasicProvider)] = value; } }

		// Check passwords against the authentication providers as if Digest Authentication was in force instead of Basic Authentication. 
		// AuthBasicUseDigestAlgorithm MD5|Off
		public string AuthBasicUseDigestAlgorithm { get { return this[nameof(AuthBasicUseDigestAlgorithm)]; } set { this[nameof(AuthBasicUseDigestAlgorithm)] = value; } }

		// SQL query to look up a password for a user
		// AuthDBDUserPWQuery query
		public string AuthDBDUserPWQuery { get { return this[nameof(AuthDBDUserPWQuery)]; } set { this[nameof(AuthDBDUserPWQuery)] = value; } }

		// SQL query to look up a password hash for a user and realm. 
		// AuthDBDUserRealmQuery query
		public string AuthDBDUserRealmQuery { get { return this[nameof(AuthDBDUserRealmQuery)]; } set { this[nameof(AuthDBDUserRealmQuery)] = value; } }

		// Sets the name of the database file containing the list of user groups for authorization
		// AuthDBMGroupFile file-path
		public string AuthDBMGroupFile { get { return this[nameof(AuthDBMGroupFile)]; } set { this[nameof(AuthDBMGroupFile)] = value; } }

		// Sets the type of database file that is used to store passwords
		// AuthDBMType default|SDBM|GDBM|NDBM|DB
		public string AuthDBMType { get { return this[nameof(AuthDBMType)]; } set { this[nameof(AuthDBMType)] = value; } }

		// Sets the name of a database file containing the list of users and passwords for authentication
		// AuthDBMUserFile file-path
		public string AuthDBMUserFile { get { return this[nameof(AuthDBMUserFile)]; } set { this[nameof(AuthDBMUserFile)] = value; } }

		// Selects the algorithm used to calculate the challenge and response hashes in digest authentication
		// AuthDigestAlgorithm MD5|MD5-sess
		public string AuthDigestAlgorithm { get { return this[nameof(AuthDigestAlgorithm)]; } set { this[nameof(AuthDigestAlgorithm)] = value; } }

		// URIs that are in the same protection space for digest authentication
		// AuthDigestDomain URI [URI] ...
		public string AuthDigestDomain { get { return this[nameof(AuthDigestDomain)]; } set { this[nameof(AuthDigestDomain)] = value; } }

		// How long the server nonce is valid
		// AuthDigestNonceLifetime seconds
		public string AuthDigestNonceLifetime { get { return this[nameof(AuthDigestNonceLifetime)]; } set { this[nameof(AuthDigestNonceLifetime)] = value; } }

		// Sets the authentication provider(s) for this location
		// AuthDigestProvider provider-name [provider-name] ...
		public string AuthDigestProvider { get { return this[nameof(AuthDigestProvider)]; } set { this[nameof(AuthDigestProvider)] = value; } }

		// Determines the quality-of-protection to use in digest authentication
		// AuthDigestQop none|auth|auth-int [auth|auth-int]
		public string AuthDigestQop { get { return this[nameof(AuthDigestQop)]; } set { this[nameof(AuthDigestQop)] = value; } }

		// The amount of shared memory to allocate for keeping track of clients
		// AuthDigestShmemSize size
		public string AuthDigestShmemSize { get { return this[nameof(AuthDigestShmemSize)]; } set { this[nameof(AuthDigestShmemSize)] = value; } }

		// Sets whether authorization and authentication are passed to lower level modules
		// AuthFormAuthoritative On|Off
		public string AuthFormAuthoritative { get { return this[nameof(AuthFormAuthoritative)]; } set { this[nameof(AuthFormAuthoritative)] = value; } }

		// The name of a form field carrying the body of the request to attempt on successful login
		// AuthFormBody fieldname
		public string AuthFormBody { get { return this[nameof(AuthFormBody)]; } set { this[nameof(AuthFormBody)] = value; } }

		// Disable the CacheControl no-store header on the login page
		// AuthFormDisableNoStore On|Off
		public string AuthFormDisableNoStore { get { return this[nameof(AuthFormDisableNoStore)]; } set { this[nameof(AuthFormDisableNoStore)] = value; } }

		// Fake a Basic Authentication header
		// AuthFormFakeBasicAuth On|Off
		public string AuthFormFakeBasicAuth { get { return this[nameof(AuthFormFakeBasicAuth)]; } set { this[nameof(AuthFormFakeBasicAuth)] = value; } }

		// The name of a form field carrying a URL to redirect to on successful login
		// AuthFormLocation fieldname
		public string AuthFormLocation { get { return this[nameof(AuthFormLocation)]; } set { this[nameof(AuthFormLocation)] = value; } }

		// The URL of the page to be redirected to should login be required
		// AuthFormLoginRequiredLocation url
		public string AuthFormLoginRequiredLocation { get { return this[nameof(AuthFormLoginRequiredLocation)]; } set { this[nameof(AuthFormLoginRequiredLocation)] = value; } }

		// The URL of the page to be redirected to should login be successful
		// AuthFormLoginSuccessLocation url
		public string AuthFormLoginSuccessLocation { get { return this[nameof(AuthFormLoginSuccessLocation)]; } set { this[nameof(AuthFormLoginSuccessLocation)] = value; } }

		// The URL to redirect to after a user has logged out
		// AuthFormLogoutLocation uri
		public string AuthFormLogoutLocation { get { return this[nameof(AuthFormLogoutLocation)]; } set { this[nameof(AuthFormLogoutLocation)] = value; } }

		// The name of a form field carrying the method of the request to attempt on successful login
		// AuthFormMethod fieldname
		public string AuthFormMethod { get { return this[nameof(AuthFormMethod)]; } set { this[nameof(AuthFormMethod)] = value; } }

		// The name of a form field carrying the mimetype of the body of the request to attempt on successful login
		// AuthFormMimetype fieldname
		public string AuthFormMimetype { get { return this[nameof(AuthFormMimetype)]; } set { this[nameof(AuthFormMimetype)] = value; } }

		// The name of a form field carrying the login password
		// AuthFormPassword fieldname
		public string AuthFormPassword { get { return this[nameof(AuthFormPassword)]; } set { this[nameof(AuthFormPassword)] = value; } }

		// Sets the authentication provider(s) for this location
		// AuthFormProvider provider-name [provider-name] ...
		public string AuthFormProvider { get { return this[nameof(AuthFormProvider)]; } set { this[nameof(AuthFormProvider)] = value; } }

		// Bypass authentication checks for high traffic sites
		// AuthFormSitePassphrase secret
		public string AuthFormSitePassphrase { get { return this[nameof(AuthFormSitePassphrase)]; } set { this[nameof(AuthFormSitePassphrase)] = value; } }

		// The largest size of the form in bytes that will be parsed for the login details
		// AuthFormSize size
		public string AuthFormSize { get { return this[nameof(AuthFormSize)]; } set { this[nameof(AuthFormSize)] = value; } }

		// The name of a form field carrying the login username
		// AuthFormUsername fieldname
		public string AuthFormUsername { get { return this[nameof(AuthFormUsername)]; } set { this[nameof(AuthFormUsername)] = value; } }

		// Sets the name of a text file containing the list of user groups for authorization
		// AuthGroupFile file-path
		public string AuthGroupFile { get { return this[nameof(AuthGroupFile)]; } set { this[nameof(AuthGroupFile)] = value; } }

		// Specifies the prefix for environment variables set during authorization
		// AuthLDAPAuthorizePrefix prefix
		public string AuthLDAPAuthorizePrefix { get { return this[nameof(AuthLDAPAuthorizePrefix)]; } set { this[nameof(AuthLDAPAuthorizePrefix)] = value; } }

		// Determines if other authentication providers are used when a user can be mapped to a DN but the server cannot successfully bind with the user's credentials.
		// AuthLDAPBindAuthoritative off|on
		public string AuthLDAPBindAuthoritative { get { return this[nameof(AuthLDAPBindAuthoritative)]; } set { this[nameof(AuthLDAPBindAuthoritative)] = value; } }

		// Optional DN to use in binding to the LDAP server
		// AuthLDAPBindDN distinguished-name
		public string AuthLDAPBindDN { get { return this[nameof(AuthLDAPBindDN)]; } set { this[nameof(AuthLDAPBindDN)] = value; } }

		// Password used in conjunction with the bind DN
		// AuthLDAPBindPassword password
		public string AuthLDAPBindPassword { get { return this[nameof(AuthLDAPBindPassword)]; } set { this[nameof(AuthLDAPBindPassword)] = value; } }

		// Language to charset conversion configuration file
		// AuthLDAPCharsetConfig file-path
		public string AuthLDAPCharsetConfig { get { return this[nameof(AuthLDAPCharsetConfig)]; } set { this[nameof(AuthLDAPCharsetConfig)] = value; } }

		// Use the authenticated user's credentials to perform authorization comparisons
		// AuthLDAPCompareAsUser on|off
		public string AuthLDAPCompareAsUser { get { return this[nameof(AuthLDAPCompareAsUser)]; } set { this[nameof(AuthLDAPCompareAsUser)] = value; } }

		// Use the LDAP server to compare the DNs
		// AuthLDAPCompareDNOnServer on|off
		public string AuthLDAPCompareDNOnServer { get { return this[nameof(AuthLDAPCompareDNOnServer)]; } set { this[nameof(AuthLDAPCompareDNOnServer)] = value; } }

		// When will the module de-reference aliases
		// AuthLDAPDereferenceAliases never|searching|finding|always
		public string AuthLDAPDereferenceAliases { get { return this[nameof(AuthLDAPDereferenceAliases)]; } set { this[nameof(AuthLDAPDereferenceAliases)] = value; } }

		// LDAP attributes used to identify the user members of groups.
		// AuthLDAPGroupAttribute attribute
		public string AuthLDAPGroupAttribute { get { return this[nameof(AuthLDAPGroupAttribute)]; } set { this[nameof(AuthLDAPGroupAttribute)] = value; } }

		// Use the DN of the client username when checking for group membership
		// AuthLDAPGroupAttributeIsDN on|off
		public string AuthLDAPGroupAttributeIsDN { get { return this[nameof(AuthLDAPGroupAttributeIsDN)]; } set { this[nameof(AuthLDAPGroupAttributeIsDN)] = value; } }

		// Determines if the server does the initial DN lookup using the basic authentication users' own username, instead of anonymously or with hard-coded credentials for the server
		// AuthLDAPInitialBindAsUser off|on
		public string AuthLDAPInitialBindAsUser { get { return this[nameof(AuthLDAPInitialBindAsUser)]; } set { this[nameof(AuthLDAPInitialBindAsUser)] = value; } }

		// Specifies the transformation of the basic authentication username to be used when binding to the LDAP server to perform a DN lookup
		// AuthLDAPInitialBindPattern regex substitution
		public string AuthLDAPInitialBindPattern { get { return this[nameof(AuthLDAPInitialBindPattern)]; } set { this[nameof(AuthLDAPInitialBindPattern)] = value; } }

		// Specifies the maximum sub-group nesting depth that will be evaluated before the user search is discontinued.
		// AuthLDAPMaxSubGroupDepth Number
		public string AuthLDAPMaxSubGroupDepth { get { return this[nameof(AuthLDAPMaxSubGroupDepth)]; } set { this[nameof(AuthLDAPMaxSubGroupDepth)] = value; } }

		// Use the value of the attribute returned during the user query to set the REMOTE_USER environment variable
		// AuthLDAPRemoteUserAttribute uid
		public string AuthLDAPRemoteUserAttribute { get { return this[nameof(AuthLDAPRemoteUserAttribute)]; } set { this[nameof(AuthLDAPRemoteUserAttribute)] = value; } }

		// Use the DN of the client username to set the REMOTE_USER environment variable
		// AuthLDAPRemoteUserIsDN on|off
		public string AuthLDAPRemoteUserIsDN { get { return this[nameof(AuthLDAPRemoteUserIsDN)]; } set { this[nameof(AuthLDAPRemoteUserIsDN)] = value; } }

		// Use the authenticated user's credentials to perform authorization searches
		// AuthLDAPSearchAsUser on|off
		public string AuthLDAPSearchAsUser { get { return this[nameof(AuthLDAPSearchAsUser)]; } set { this[nameof(AuthLDAPSearchAsUser)] = value; } }

		// Specifies the attribute labels, one value per directive line, used to distinguish the members of the current group that are groups.
		// AuthLDAPSubGroupAttribute attribute
		public string AuthLDAPSubGroupAttribute { get { return this[nameof(AuthLDAPSubGroupAttribute)]; } set { this[nameof(AuthLDAPSubGroupAttribute)] = value; } }

		// Specifies which LDAP objectClass values identify directory objects that are groups during sub-group processing.
		// AuthLDAPSubGroupClass LdapObjectClass
		public string AuthLDAPSubGroupClass { get { return this[nameof(AuthLDAPSubGroupClass)]; } set { this[nameof(AuthLDAPSubGroupClass)] = value; } }

		// URL specifying the LDAP search parameters
		// AuthLDAPURL url [NONE|SSL|TLS|STARTTLS]
		public string AuthLDAPURL { get { return this[nameof(AuthLDAPURL)]; } set { this[nameof(AuthLDAPURL)] = value; } }

		// Controls the manner in which each configuration section's authorization logic is combined with that of preceding configuration sections.
		// AuthMerging Off | And | Or
		public string AuthMerging { get { return this[nameof(AuthMerging)]; } set { this[nameof(AuthMerging)] = value; } }

		// Authorization realm for use in HTTP authentication
		// AuthName auth-domain
		public string AuthName { get { return this[nameof(AuthName)]; } set { this[nameof(AuthName)] = value; } }

		// Specify a context string for use in the cache key
		// AuthnCacheContext directory|server|custom-string
		public string AuthnCacheContext { get { return this[nameof(AuthnCacheContext)]; } set { this[nameof(AuthnCacheContext)] = value; } }

		// Enable Authn caching configured anywhere
		// AuthnCacheEnable
		public string AuthnCacheEnable { get { return this[nameof(AuthnCacheEnable)]; } set { this[nameof(AuthnCacheEnable)] = value; } }

		// Specify which authn provider(s) to cache for
		// AuthnCacheProvideFor authn-provider [...]
		public string AuthnCacheProvideFor { get { return this[nameof(AuthnCacheProvideFor)]; } set { this[nameof(AuthnCacheProvideFor)] = value; } }

		// Select socache backend provider to use
		// AuthnCacheSOCache provider-name[:provider-args]
		public string AuthnCacheSOCache { get { return this[nameof(AuthnCacheSOCache)]; } set { this[nameof(AuthnCacheSOCache)] = value; } }

		// Set a timeout for cache entries
		// AuthnCacheTimeout timeout (seconds)
		public string AuthnCacheTimeout { get { return this[nameof(AuthnCacheTimeout)]; } set { this[nameof(AuthnCacheTimeout)] = value; } }

		// Enables a FastCGI application to handle the check_authn authentication hook.
		// AuthnzFcgiCheckAuthnProvider provider-name|None option ...
		public string AuthnzFcgiCheckAuthnProvider { get { return this[nameof(AuthnzFcgiCheckAuthnProvider)]; } set { this[nameof(AuthnzFcgiCheckAuthnProvider)] = value; } }

		// Defines a FastCGI application as a provider for authentication and/or authorization
		// AuthnzFcgiDefineProvider type provider-name backend-address
		public string AuthnzFcgiDefineProvider { get { return this[nameof(AuthnzFcgiDefineProvider)]; } set { this[nameof(AuthnzFcgiDefineProvider)] = value; } }

		// Type of user authentication
		// AuthType None|Basic|Digest|Form
		public string AuthType { get { return this[nameof(AuthType)]; } set { this[nameof(AuthType)] = value; } }

		// Sets the name of a text file containing the list of users and passwords for authentication
		// AuthUserFile file-path
		public string AuthUserFile { get { return this[nameof(AuthUserFile)]; } set { this[nameof(AuthUserFile)] = value; } }

		// Determines whether to redirect the Client to the Referring page on successful login or logout if a Referer request header is present
		// AuthzDBDLoginToReferer On|Off
		public string AuthzDBDLoginToReferer { get { return this[nameof(AuthzDBDLoginToReferer)]; } set { this[nameof(AuthzDBDLoginToReferer)] = value; } }

		// Specify the SQL Query for the required operation
		// AuthzDBDQuery query
		public string AuthzDBDQuery { get { return this[nameof(AuthzDBDQuery)]; } set { this[nameof(AuthzDBDQuery)] = value; } }

		// Specify a query to look up a login page for the user
		// AuthzDBDRedirectQuery query
		public string AuthzDBDRedirectQuery { get { return this[nameof(AuthzDBDRedirectQuery)]; } set { this[nameof(AuthzDBDRedirectQuery)] = value; } }

		// Sets the type of database file that is used to store list of user groups
		// AuthzDBMType default|SDBM|GDBM|NDBM|DB
		public string AuthzDBMType { get { return this[nameof(AuthzDBMType)]; } set { this[nameof(AuthzDBMType)] = value; } }

		// Send '403 FORBIDDEN' instead of '401 UNAUTHORIZED' if authentication succeeds but authorization fails 
		// AuthzSendForbiddenOnFailure On|Off
		public string AuthzSendForbiddenOnFailure { get { return this[nameof(AuthzSendForbiddenOnFailure)]; } set { this[nameof(AuthzSendForbiddenOnFailure)] = value; } }

		// Number of additional Balancers that can be added Post-configuration
		// BalancerGrowth #
		public string BalancerGrowth { get { return this[nameof(BalancerGrowth)]; } set { this[nameof(BalancerGrowth)] = value; } }

		// Inherit ProxyPassed Balancers/Workers from the main server
		// BalancerInherit On|Off
		public string BalancerInherit { get { return this[nameof(BalancerInherit)]; } set { this[nameof(BalancerInherit)] = value; } }

		// Add a member to a load balancing group
		// BalancerMember [balancerurl] url [key=value [key=value ...]]
		public string BalancerMember { get { return this[nameof(BalancerMember)]; } set { this[nameof(BalancerMember)] = value; } }

		// Attempt to persist changes made by the Balancer Manager across restarts.
		// BalancerPersist On|Off
		public string BalancerPersist { get { return this[nameof(BalancerPersist)]; } set { this[nameof(BalancerPersist)] = value; } }

		// How the outgoing ETag header should be modified during compression
		// BrotliAlterETag AddSuffix|NoChange|Remove
		public string BrotliAlterETag { get { return this[nameof(BrotliAlterETag)]; } set { this[nameof(BrotliAlterETag)] = value; } }

		// Maximum input block size
		// BrotliCompressionMaxInputBlock value
		public string BrotliCompressionMaxInputBlock { get { return this[nameof(BrotliCompressionMaxInputBlock)]; } set { this[nameof(BrotliCompressionMaxInputBlock)] = value; } }

		// Compression quality
		// BrotliCompressionQuality value
		public string BrotliCompressionQuality { get { return this[nameof(BrotliCompressionQuality)]; } set { this[nameof(BrotliCompressionQuality)] = value; } }

		// Brotli sliding compression window size
		// BrotliCompressionWindow value
		public string BrotliCompressionWindow { get { return this[nameof(BrotliCompressionWindow)]; } set { this[nameof(BrotliCompressionWindow)] = value; } }

		// Places the compression ratio in a note for logging
		// BrotliFilterNote [type] notename
		public string BrotliFilterNote { get { return this[nameof(BrotliFilterNote)]; } set { this[nameof(BrotliFilterNote)] = value; } }

		// Sets environment variables conditional on HTTP User-Agent 
		// BrowserMatch regex [!]env-variable[=value] [[!]env-variable[=value]] ...
		public string BrowserMatch { get { return this[nameof(BrowserMatch)]; } set { this[nameof(BrowserMatch)] = value; } }

		// Sets environment variables conditional on User-Agent without respect to case
		// BrowserMatchNoCase  regex [!]env-variable[=value] [[!]env-variable[=value]] ...
		public string BrowserMatchNoCase { get { return this[nameof(BrowserMatchNoCase)]; } set { this[nameof(BrowserMatchNoCase)] = value; } }

		// Buffer log entries in memory before writing to disk
		// BufferedLogs On|Off
		public string BufferedLogs { get { return this[nameof(BufferedLogs)]; } set { this[nameof(BufferedLogs)] = value; } }

		// Maximum size in bytes to buffer by the buffer filter
		// BufferSize integer
		public string BufferSize { get { return this[nameof(BufferSize)]; } set { this[nameof(BufferSize)] = value; } }

		// The default duration to cache a document when no expiry date is specified.
		// CacheDefaultExpire seconds
		public string CacheDefaultExpire { get { return this[nameof(CacheDefaultExpire)]; } set { this[nameof(CacheDefaultExpire)] = value; } }

		// Add an X-Cache-Detail header to the response.
		// CacheDetailHeader on|off
		public string CacheDetailHeader { get { return this[nameof(CacheDetailHeader)]; } set { this[nameof(CacheDetailHeader)] = value; } }

		// The number of characters in subdirectory names
		// CacheDirLength length
		public string CacheDirLength { get { return this[nameof(CacheDirLength)]; } set { this[nameof(CacheDirLength)] = value; } }

		// The number of levels of subdirectories in the cache.
		// CacheDirLevels levels
		public string CacheDirLevels { get { return this[nameof(CacheDirLevels)]; } set { this[nameof(CacheDirLevels)] = value; } }

		// Disable caching of specified URLs
		// CacheDisable url-string | on
		public string CacheDisable { get { return this[nameof(CacheDisable)]; } set { this[nameof(CacheDisable)] = value; } }

		// Enable caching of specified URLs using a specified storage manager
		// CacheEnable cache_type [url-string]
		public string CacheEnable { get { return this[nameof(CacheEnable)]; } set { this[nameof(CacheEnable)] = value; } }

		// Cache a list of file handles at startup time
		// CacheFile file-path [file-path] ...
		public string CacheFile { get { return this[nameof(CacheFile)]; } set { this[nameof(CacheFile)] = value; } }

		// Add an X-Cache header to the response.
		// CacheHeader on|off
		public string CacheHeader { get { return this[nameof(CacheHeader)]; } set { this[nameof(CacheHeader)] = value; } }

		// Ignore request to not serve cached content to client
		// CacheIgnoreCacheControl On|Off
		public string CacheIgnoreCacheControl { get { return this[nameof(CacheIgnoreCacheControl)]; } set { this[nameof(CacheIgnoreCacheControl)] = value; } }

		// Do not store the given HTTP header(s) in the cache. 
		// CacheIgnoreHeaders header-string [header-string] ...
		public string CacheIgnoreHeaders { get { return this[nameof(CacheIgnoreHeaders)]; } set { this[nameof(CacheIgnoreHeaders)] = value; } }

		// Ignore the fact that a response has no Last Modified header.
		// CacheIgnoreNoLastMod On|Off
		public string CacheIgnoreNoLastMod { get { return this[nameof(CacheIgnoreNoLastMod)]; } set { this[nameof(CacheIgnoreNoLastMod)] = value; } }

		// Ignore query string when caching
		// CacheIgnoreQueryString On|Off
		public string CacheIgnoreQueryString { get { return this[nameof(CacheIgnoreQueryString)]; } set { this[nameof(CacheIgnoreQueryString)] = value; } }

		// Ignore defined session identifiers encoded in the URL when caching 
		// CacheIgnoreURLSessionIdentifiers identifier [identifier] ...
		public string CacheIgnoreURLSessionIdentifiers { get { return this[nameof(CacheIgnoreURLSessionIdentifiers)]; } set { this[nameof(CacheIgnoreURLSessionIdentifiers)] = value; } }

		// Override the base URL of reverse proxied cache keys.
		// CacheKeyBaseURL URL
		public string CacheKeyBaseURL { get { return this[nameof(CacheKeyBaseURL)]; } set { this[nameof(CacheKeyBaseURL)] = value; } }

		// The factor used to compute an expiry date based on the LastModified date.
		// CacheLastModifiedFactor float
		public string CacheLastModifiedFactor { get { return this[nameof(CacheLastModifiedFactor)]; } set { this[nameof(CacheLastModifiedFactor)] = value; } }

		// Enable the thundering herd lock.
		// CacheLock on|off
		public string CacheLock { get { return this[nameof(CacheLock)]; } set { this[nameof(CacheLock)] = value; } }

		// Set the maximum possible age of a cache lock.
		// CacheLockMaxAge integer
		public string CacheLockMaxAge { get { return this[nameof(CacheLockMaxAge)]; } set { this[nameof(CacheLockMaxAge)] = value; } }

		// Set the lock path directory.
		// CacheLockPath directory
		public string CacheLockPath { get { return this[nameof(CacheLockPath)]; } set { this[nameof(CacheLockPath)] = value; } }

		// The maximum time in seconds to cache a document
		// CacheMaxExpire seconds
		public string CacheMaxExpire { get { return this[nameof(CacheMaxExpire)]; } set { this[nameof(CacheMaxExpire)] = value; } }

		// The maximum size (in bytes) of a document to be placed in the cache
		// CacheMaxFileSize bytes
		public string CacheMaxFileSize { get { return this[nameof(CacheMaxFileSize)]; } set { this[nameof(CacheMaxFileSize)] = value; } }

		// The minimum time in seconds to cache a document
		// CacheMinExpire seconds
		public string CacheMinExpire { get { return this[nameof(CacheMinExpire)]; } set { this[nameof(CacheMinExpire)] = value; } }

		// The minimum size (in bytes) of a document to be placed in the cache
		// CacheMinFileSize bytes
		public string CacheMinFileSize { get { return this[nameof(CacheMinFileSize)]; } set { this[nameof(CacheMinFileSize)] = value; } }

		// Allows content-negotiated documents to be cached by proxy servers
		// CacheNegotiatedDocs On|Off
		public string CacheNegotiatedDocs { get { return this[nameof(CacheNegotiatedDocs)]; } set { this[nameof(CacheNegotiatedDocs)] = value; } }

		// Run the cache from the quick handler.
		// CacheQuickHandler on|off
		public string CacheQuickHandler { get { return this[nameof(CacheQuickHandler)]; } set { this[nameof(CacheQuickHandler)] = value; } }

		// The minimum size (in bytes) of the document to read and be cached   before sending the data downstream
		// CacheReadSize bytes
		public string CacheReadSize { get { return this[nameof(CacheReadSize)]; } set { this[nameof(CacheReadSize)] = value; } }

		// The minimum time (in milliseconds) that should elapse while reading   before data is sent downstream
		// CacheReadTime milliseconds
		public string CacheReadTime { get { return this[nameof(CacheReadTime)]; } set { this[nameof(CacheReadTime)] = value; } }

		// The directory root under which cache files are stored
		// CacheRoot directory
		public string CacheRoot { get { return this[nameof(CacheRoot)]; } set { this[nameof(CacheRoot)] = value; } }

		// The shared object cache implementation to use
		// CacheSocache type[:args]
		public string CacheSocache { get { return this[nameof(CacheSocache)]; } set { this[nameof(CacheSocache)] = value; } }

		// The maximum size (in bytes) of an entry to be placed in the cache
		// CacheSocacheMaxSize bytes
		public string CacheSocacheMaxSize { get { return this[nameof(CacheSocacheMaxSize)]; } set { this[nameof(CacheSocacheMaxSize)] = value; } }

		// The maximum time (in seconds) for a document to be placed in the cache
		// CacheSocacheMaxTime seconds
		public string CacheSocacheMaxTime { get { return this[nameof(CacheSocacheMaxTime)]; } set { this[nameof(CacheSocacheMaxTime)] = value; } }

		// The minimum time (in seconds) for a document to be placed in the cache
		// CacheSocacheMinTime seconds
		public string CacheSocacheMinTime { get { return this[nameof(CacheSocacheMinTime)]; } set { this[nameof(CacheSocacheMinTime)] = value; } }

		// The minimum size (in bytes) of the document to read and be cached   before sending the data downstream
		// CacheSocacheReadSize bytes
		public string CacheSocacheReadSize { get { return this[nameof(CacheSocacheReadSize)]; } set { this[nameof(CacheSocacheReadSize)] = value; } }

		// The minimum time (in milliseconds) that should elapse while reading   before data is sent downstream
		// CacheSocacheReadTime milliseconds
		public string CacheSocacheReadTime { get { return this[nameof(CacheSocacheReadTime)]; } set { this[nameof(CacheSocacheReadTime)] = value; } }

		// Serve stale content in place of 5xx responses.
		// CacheStaleOnError on|off
		public string CacheStaleOnError { get { return this[nameof(CacheStaleOnError)]; } set { this[nameof(CacheStaleOnError)] = value; } }

		// Attempt to cache responses that the server reports as expired
		// CacheStoreExpired On|Off
		public string CacheStoreExpired { get { return this[nameof(CacheStoreExpired)]; } set { this[nameof(CacheStoreExpired)] = value; } }

		// Attempt to cache requests or responses that have been marked as no-store.
		// CacheStoreNoStore On|Off
		public string CacheStoreNoStore { get { return this[nameof(CacheStoreNoStore)]; } set { this[nameof(CacheStoreNoStore)] = value; } }

		// Attempt to cache responses that the server has marked as private
		// CacheStorePrivate On|Off
		public string CacheStorePrivate { get { return this[nameof(CacheStorePrivate)]; } set { this[nameof(CacheStorePrivate)] = value; } }

		// The length of time to wait for more output from the CGI program
		// CGIDScriptTimeout time[s|ms]
		public string CGIDScriptTimeout { get { return this[nameof(CGIDScriptTimeout)]; } set { this[nameof(CGIDScriptTimeout)] = value; } }

		// Technique for locating the interpreter for CGI scripts
		// CGIMapExtension cgi-path .extension
		public string CGIMapExtension { get { return this[nameof(CGIMapExtension)]; } set { this[nameof(CGIMapExtension)] = value; } }

		// Enables passing HTTP authorization headers to scripts as CGI variables
		// CGIPassAuth On|Off
		public string CGIPassAuth { get { return this[nameof(CGIPassAuth)]; } set { this[nameof(CGIPassAuth)] = value; } }

		// Controls how some CGI variables are set
		// CGIVar variable rule
		public string CGIVar { get { return this[nameof(CGIVar)]; } set { this[nameof(CGIVar)] = value; } }

		// Charset to translate into
		// CharsetDefault charset
		public string CharsetDefault { get { return this[nameof(CharsetDefault)]; } set { this[nameof(CharsetDefault)] = value; } }

		// Configures charset translation behavior
		// CharsetOptions option [option] ...
		public string CharsetOptions { get { return this[nameof(CharsetOptions)]; } set { this[nameof(CharsetOptions)] = value; } }

		// Source charset of files
		// CharsetSourceEnc charset
		public string CharsetSourceEnc { get { return this[nameof(CharsetSourceEnc)]; } set { this[nameof(CharsetSourceEnc)] = value; } }

		// Also match files with differing file name extensions.
		// CheckBasenameMatch on|off
		public string CheckBasenameMatch { get { return this[nameof(CheckBasenameMatch)]; } set { this[nameof(CheckBasenameMatch)] = value; } }

		// Limits the action of the speling module to case corrections
		// CheckCaseOnly on|off
		public string CheckCaseOnly { get { return this[nameof(CheckCaseOnly)]; } set { this[nameof(CheckCaseOnly)] = value; } }

		// Enables the spelling module
		// CheckSpelling on|off
		public string CheckSpelling { get { return this[nameof(CheckSpelling)]; } set { this[nameof(CheckSpelling)] = value; } }

		// Directory for apache to run chroot(8) after startup.
		// ChrootDir /path/to/directory
		public string ChrootDir { get { return this[nameof(ChrootDir)]; } set { this[nameof(ChrootDir)] = value; } }

		// Enables the generation of Content-MD5 HTTP Response headers
		// ContentDigest On|Off
		public string ContentDigest { get { return this[nameof(ContentDigest)]; } set { this[nameof(ContentDigest)] = value; } }

		// The domain to which the tracking cookie applies
		// CookieDomain domain
		public string CookieDomain { get { return this[nameof(CookieDomain)]; } set { this[nameof(CookieDomain)] = value; } }

		// Expiry time for the tracking cookie
		// CookieExpires expiry-period
		public string CookieExpires { get { return this[nameof(CookieExpires)]; } set { this[nameof(CookieExpires)] = value; } }

		// Adds the 'HTTPOnly' attribute to the cookie
		// CookieHTTPOnly on|off
		public string CookieHTTPOnly { get { return this[nameof(CookieHTTPOnly)]; } set { this[nameof(CookieHTTPOnly)] = value; } }

		// Name of the tracking cookie
		// CookieName token
		public string CookieName { get { return this[nameof(CookieName)]; } set { this[nameof(CookieName)] = value; } }

		// Adds the 'SameSite' attribute to the cookie
		// CookieSameSite None|Lax|Strict
		public string CookieSameSite { get { return this[nameof(CookieSameSite)]; } set { this[nameof(CookieSameSite)] = value; } }

		// Adds the 'Secure' attribute to the cookie
		// CookieSecure on|off
		public string CookieSecure { get { return this[nameof(CookieSecure)]; } set { this[nameof(CookieSecure)] = value; } }

		// Format of the cookie header field
		// CookieStyle     Netscape|Cookie|Cookie2|RFC2109|RFC2965
		public string CookieStyle { get { return this[nameof(CookieStyle)]; } set { this[nameof(CookieStyle)] = value; } }

		// Enables tracking cookie
		// CookieTracking on|off
		public string CookieTracking { get { return this[nameof(CookieTracking)]; } set { this[nameof(CookieTracking)] = value; } }

		// Directory where Apache HTTP Server attempts to switch before dumping core
		// CoreDumpDirectory directory
		public string CoreDumpDirectory { get { return this[nameof(CoreDumpDirectory)]; } set { this[nameof(CoreDumpDirectory)] = value; } }

		// Sets filename and format of log file
		// CustomLog  file|pipe format|nickname [env=[!]environment-variable| expr=expression]
		public string CustomLog { get { return this[nameof(CustomLog)]; } set { this[nameof(CustomLog)] = value; } }

		// Enable WebDAV HTTP methods
		// Dav On|Off|provider-name
		public string Dav { get { return this[nameof(Dav)]; } set { this[nameof(Dav)] = value; } }

		// Configure repository root path
		// DavBasePath root-path
		public string DavBasePath { get { return this[nameof(DavBasePath)]; } set { this[nameof(DavBasePath)] = value; } }

		// Allow PROPFIND, Depth: Infinity requests
		// DavDepthInfinity on|off
		public string DavDepthInfinity { get { return this[nameof(DavDepthInfinity)]; } set { this[nameof(DavDepthInfinity)] = value; } }

		// Location of the DAV lock database
		// DavGenericLockDB file-path
		public string DavGenericLockDB { get { return this[nameof(DavGenericLockDB)]; } set { this[nameof(DavGenericLockDB)] = value; } }

		// Location of the DAV lock database
		// DavLockDB file-path
		public string DavLockDB { get { return this[nameof(DavLockDB)]; } set { this[nameof(DavLockDB)] = value; } }

		// Enable lock discovery
		// DavLockDiscovery on|off
		public string DavLockDiscovery { get { return this[nameof(DavLockDiscovery)]; } set { this[nameof(DavLockDiscovery)] = value; } }

		// Minimum amount of time the server holds a lock on a DAV resource
		// DavMinTimeout seconds
		public string DavMinTimeout { get { return this[nameof(DavMinTimeout)]; } set { this[nameof(DavMinTimeout)] = value; } }

		// Keepalive time for idle connections
		// DBDExptime time-in-seconds
		public string DBDExptime { get { return this[nameof(DBDExptime)]; } set { this[nameof(DBDExptime)] = value; } }

		// Execute an SQL statement after connecting to a database
		// DBDInitSQL "SQL statement"
		public string DBDInitSQL { get { return this[nameof(DBDInitSQL)]; } set { this[nameof(DBDInitSQL)] = value; } }

		// Maximum sustained number of connections
		// DBDKeep number
		public string DBDKeep { get { return this[nameof(DBDKeep)]; } set { this[nameof(DBDKeep)] = value; } }

		// Maximum number of connections
		// DBDMax number
		public string DBDMax { get { return this[nameof(DBDMax)]; } set { this[nameof(DBDMax)] = value; } }

		// Minimum number of connections
		// DBDMin number
		public string DBDMin { get { return this[nameof(DBDMin)]; } set { this[nameof(DBDMin)] = value; } }

		// Parameters for database connection
		// DBDParams param1=value1[,param2=value2]
		public string DBDParams { get { return this[nameof(DBDParams)]; } set { this[nameof(DBDParams)] = value; } }

		// Whether to use persistent connections
		// DBDPersist On|Off
		public string DBDPersist { get { return this[nameof(DBDPersist)]; } set { this[nameof(DBDPersist)] = value; } }

		// Define an SQL prepared statement
		// DBDPrepareSQL "SQL statement" label
		public string DBDPrepareSQL { get { return this[nameof(DBDPrepareSQL)]; } set { this[nameof(DBDPrepareSQL)] = value; } }

		// Specify an SQL driver
		// DBDriver name
		public string DBDriver { get { return this[nameof(DBDriver)]; } set { this[nameof(DBDriver)] = value; } }

		// Icon to display for files when no specific icon is configured
		// DefaultIcon url-path
		public string DefaultIcon { get { return this[nameof(DefaultIcon)]; } set { this[nameof(DefaultIcon)] = value; } }

		// Defines a default language-tag to be sent in the Content-Language header field for all resources in the current context that have not been assigned a language-tag by some other means.
		// DefaultLanguage language-tag
		public string DefaultLanguage { get { return this[nameof(DefaultLanguage)]; } set { this[nameof(DefaultLanguage)] = value; } }

		// Base directory for the server run-time files
		// DefaultRuntimeDir directory-path
		public string DefaultRuntimeDir { get { return this[nameof(DefaultRuntimeDir)]; } set { this[nameof(DefaultRuntimeDir)] = value; } }

		// This directive has no effect other than to emit warnings if the value is not none. In prior versions, DefaultType would specify a default media type to assign to response content for which no other media type configuration could be found.
		// DefaultType media-type|none
		public string DefaultType { get { return this[nameof(DefaultType)]; } set { this[nameof(DefaultType)] = value; } }

		// Define a variable
		// Define parameter-name [parameter-value]
		public string Define { get { return this[nameof(Define)]; } set { this[nameof(Define)] = value; } }

		// How the outgoing ETag header should be modified during compression
		// DeflateAlterETag AddSuffix|NoChange|Remove
		public string DeflateAlterETag { get { return this[nameof(DeflateAlterETag)]; } set { this[nameof(DeflateAlterETag)] = value; } }

		// Fragment size to be compressed at one time by zlib
		// DeflateBufferSize value
		public string DeflateBufferSize { get { return this[nameof(DeflateBufferSize)]; } set { this[nameof(DeflateBufferSize)] = value; } }

		// How much compression do we apply to the output
		// DeflateCompressionLevel value
		public string DeflateCompressionLevel { get { return this[nameof(DeflateCompressionLevel)]; } set { this[nameof(DeflateCompressionLevel)] = value; } }

		// Places the compression ratio in a note for logging
		// DeflateFilterNote [type] notename
		public string DeflateFilterNote { get { return this[nameof(DeflateFilterNote)]; } set { this[nameof(DeflateFilterNote)] = value; } }

		// Maximum size of inflated request bodies
		// DeflateInflateLimitRequestBody value
		public string DeflateInflateLimitRequestBody { get { return this[nameof(DeflateInflateLimitRequestBody)]; } set { this[nameof(DeflateInflateLimitRequestBody)] = value; } }

		// Maximum number of times the inflation ratio for request bodies              can be crossed
		// DeflateInflateRatioBurst value
		public string DeflateInflateRatioBurst { get { return this[nameof(DeflateInflateRatioBurst)]; } set { this[nameof(DeflateInflateRatioBurst)] = value; } }

		// Maximum inflation ratio for request bodies
		// DeflateInflateRatioLimit value
		public string DeflateInflateRatioLimit { get { return this[nameof(DeflateInflateRatioLimit)]; } set { this[nameof(DeflateInflateRatioLimit)] = value; } }

		// How much memory should be used by zlib for compression
		// DeflateMemLevel value
		public string DeflateMemLevel { get { return this[nameof(DeflateMemLevel)]; } set { this[nameof(DeflateMemLevel)] = value; } }

		// Zlib compression window size
		// DeflateWindowSize value
		public string DeflateWindowSize { get { return this[nameof(DeflateWindowSize)]; } set { this[nameof(DeflateWindowSize)] = value; } }

		// Controls which hosts are denied access to the server
		// Deny from all|host|env=[!]env-variable [host|env=[!]env-variable] ...
		public string Deny { get { return this[nameof(Deny)]; } set { this[nameof(Deny)] = value; } }

		// Toggle how this module responds when another handler is configured
		// DirectoryCheckHandler On|Off
		public string DirectoryCheckHandler { get { return this[nameof(DirectoryCheckHandler)]; } set { this[nameof(DirectoryCheckHandler)] = value; } }

		// List of resources to look for when the client requests a directory
		// DirectoryIndex     disabled | local-url [local-url] ...
		public string DirectoryIndex { get { return this[nameof(DirectoryIndex)]; } set { this[nameof(DirectoryIndex)] = value; } }

		// Configures an external redirect for directory indexes. 
		// DirectoryIndexRedirect on | off | permanent | temp | seeother | 3xx-code 
		public string DirectoryIndexRedirect { get { return this[nameof(DirectoryIndexRedirect)]; } set { this[nameof(DirectoryIndexRedirect)] = value; } }

		// Toggle trailing slash redirects on or off
		// DirectorySlash On|Off
		public string DirectorySlash { get { return this[nameof(DirectorySlash)]; } set { this[nameof(DirectorySlash)] = value; } }

		// Directory that forms the main document tree visible from the web
		// DocumentRoot directory-path
		public string DocumentRoot { get { return this[nameof(DocumentRoot)]; } set { this[nameof(DocumentRoot)] = value; } }

		// Determines whether the privileges required by dtrace are enabled.
		// DTracePrivileges On|Off
		public string DTracePrivileges { get { return this[nameof(DTracePrivileges)]; } set { this[nameof(DTracePrivileges)] = value; } }

		// Dump all input data to the error log
		// DumpIOInput On|Off
		public string DumpIOInput { get { return this[nameof(DumpIOInput)]; } set { this[nameof(DumpIOInput)] = value; } }

		// Dump all output data to the error log
		// DumpIOOutput On|Off
		public string DumpIOOutput { get { return this[nameof(DumpIOOutput)]; } set { this[nameof(DumpIOOutput)] = value; } }

		// Enables a hook that runs exception handlers after a crash
		// EnableExceptionHook On|Off
		public string EnableExceptionHook { get { return this[nameof(EnableExceptionHook)]; } set { this[nameof(EnableExceptionHook)] = value; } }

		// Use memory-mapping to read files during delivery
		// EnableMMAP On|Off
		public string EnableMMAP { get { return this[nameof(EnableMMAP)]; } set { this[nameof(EnableMMAP)] = value; } }

		// Use the kernel sendfile support to deliver files to the client
		// EnableSendfile On|Off
		public string EnableSendfile { get { return this[nameof(EnableSendfile)]; } set { this[nameof(EnableSendfile)] = value; } }

		// Abort configuration parsing with a custom error message
		// Error message
		public string Error { get { return this[nameof(Error)]; } set { this[nameof(Error)] = value; } }

		// What the server will return to the client in case of an error
		// ErrorDocument error-code document
		public string ErrorDocument { get { return this[nameof(ErrorDocument)]; } set { this[nameof(ErrorDocument)] = value; } }

		// Location where the server will log errors
		// ErrorLog file-path|syslog[:[facility][:tag]]
		public string ErrorLog { get { return this[nameof(ErrorLog)]; } set { this[nameof(ErrorLog)] = value; } }

		// Format specification for error log entries
		// ErrorLogFormat [connection|request] format
		public string ErrorLogFormat { get { return this[nameof(ErrorLogFormat)]; } set { this[nameof(ErrorLogFormat)] = value; } }

		// Demonstration directive to illustrate the Apache module API
		// Example
		public string Example { get { return this[nameof(Example)]; } set { this[nameof(Example)] = value; } }

		// Enables generation of Expires headers
		// ExpiresActive On|Off
		public string ExpiresActive { get { return this[nameof(ExpiresActive)]; } set { this[nameof(ExpiresActive)] = value; } }

		// Value of the Expires header configured by MIME type
		// ExpiresByType MIME-type seconds
		public string ExpiresByType { get { return this[nameof(ExpiresByType)]; } set { this[nameof(ExpiresByType)] = value; } }

		// Default algorithm for calculating expiration time
		// ExpiresDefault seconds
		public string ExpiresDefault { get { return this[nameof(ExpiresDefault)]; } set { this[nameof(ExpiresDefault)] = value; } }

		// Keep track of extended status information for each request
		// ExtendedStatus On|Off
		public string ExtendedStatus { get { return this[nameof(ExtendedStatus)]; } set { this[nameof(ExtendedStatus)] = value; } }

		// Define an external filter
		// ExtFilterDefine filtername parameters
		public string ExtFilterDefine { get { return this[nameof(ExtFilterDefine)]; } set { this[nameof(ExtFilterDefine)] = value; } }

		// Configure mod_ext_filter options
		// ExtFilterOptions option [option] ...
		public string ExtFilterOptions { get { return this[nameof(ExtFilterOptions)]; } set { this[nameof(ExtFilterOptions)] = value; } }

		// Define a default URL for requests that don't map to a file
		// FallbackResource disabled | local-url
		public string FallbackResource { get { return this[nameof(FallbackResource)]; } set { this[nameof(FallbackResource)] = value; } }

		// File attributes used to create the ETag HTTP response header for static files
		// FileETag component ...
		public string FileETag { get { return this[nameof(FileETag)]; } set { this[nameof(FileETag)] = value; } }

		// Configure the filter chain
		// FilterChain [+=-@!]filter-name ...
		public string FilterChain { get { return this[nameof(FilterChain)]; } set { this[nameof(FilterChain)] = value; } }

		// Declare a smart filter
		// FilterDeclare filter-name [type]
		public string FilterDeclare { get { return this[nameof(FilterDeclare)]; } set { this[nameof(FilterDeclare)] = value; } }

		// Deal with correct HTTP protocol handling
		// FilterProtocol filter-name [provider-name]     proto-flags
		public string FilterProtocol { get { return this[nameof(FilterProtocol)]; } set { this[nameof(FilterProtocol)] = value; } }

		// Register a content filter
		// FilterProvider filter-name provider-name  expression
		public string FilterProvider { get { return this[nameof(FilterProvider)]; } set { this[nameof(FilterProvider)] = value; } }

		// Get debug/diagnostic information from     mod_filter
		// FilterTrace filter-name level
		public string FilterTrace { get { return this[nameof(FilterTrace)]; } set { this[nameof(FilterTrace)] = value; } }

		// Maximum number of pipelined responses above which they are flushed to the network
		// FlushMaxPipelined number
		public string FlushMaxPipelined { get { return this[nameof(FlushMaxPipelined)]; } set { this[nameof(FlushMaxPipelined)] = value; } }

		// Threshold above which pending data are flushed to the network
		// FlushMaxThreshold number-of-bytes
		public string FlushMaxThreshold { get { return this[nameof(FlushMaxThreshold)]; } set { this[nameof(FlushMaxThreshold)] = value; } }

		// Action to take if a single acceptable document is not found
		// ForceLanguagePriority None|Prefer|Fallback [Prefer|Fallback]
		public string ForceLanguagePriority { get { return this[nameof(ForceLanguagePriority)]; } set { this[nameof(ForceLanguagePriority)] = value; } }

		// Forces all matching files to be served with the specified media type in the HTTP Content-Type header field
		// ForceType media-type|None
		public string ForceType { get { return this[nameof(ForceType)]; } set { this[nameof(ForceType)] = value; } }

		// Sets filename of the forensic log
		// ForensicLog filename|pipe
		public string ForensicLog { get { return this[nameof(ForensicLog)]; } set { this[nameof(ForensicLog)] = value; } }

		// Sets filename and format of log file
		// GlobalLogfile|pipe format|nickname [env=[!]environment-variable| expr=expression]
		public string GlobalLogfile { get { return this[nameof(GlobalLogfile)]; } set { this[nameof(GlobalLogfile)] = value; } }

		// Directory to write gmon.out profiling data to.  
		// GprofDir /tmp/gprof/|/tmp/gprof/%
		public string GprofDir { get { return this[nameof(GprofDir)]; } set { this[nameof(GprofDir)] = value; } }

		// Specify a timeout after which a gracefully shutdown server will exit.
		// GracefulShutdownTimeout seconds
		public string GracefulShutdownTimeout { get { return this[nameof(GracefulShutdownTimeout)]; } set { this[nameof(GracefulShutdownTimeout)] = value; } }

		// Group under which the server will answer requests
		// Group unix-group
		public string Group { get { return this[nameof(Group)]; } set { this[nameof(Group)] = value; } }

		// Determine file handling in responses
		// H2CopyFiles on|off
		public string H2CopyFiles { get { return this[nameof(H2CopyFiles)]; } set { this[nameof(H2CopyFiles)] = value; } }

		// H2 Direct Protocol Switch
		// H2Direct on|off
		public string H2Direct { get { return this[nameof(H2Direct)]; } set { this[nameof(H2Direct)] = value; } }

		// Add a response header to be picked up in 103 Early Hints
		// H2EarlyHint name value
		public string H2EarlyHint { get { return this[nameof(H2EarlyHint)]; } set { this[nameof(H2EarlyHint)] = value; } }

		// Determine sending of 103 status codes
		// H2EarlyHints on|off
		public string H2EarlyHints { get { return this[nameof(H2EarlyHints)]; } set { this[nameof(H2EarlyHints)] = value; } }

		// Maximum bytes inside a single HTTP/2 DATA frame
		// H2MaxDataFrameLen n
		public string H2MaxDataFrameLen { get { return this[nameof(H2MaxDataFrameLen)]; } set { this[nameof(H2MaxDataFrameLen)] = value; } }

		// Maximum number of active streams per HTTP/2 session.
		// H2MaxSessionStreams n
		public string H2MaxSessionStreams { get { return this[nameof(H2MaxSessionStreams)]; } set { this[nameof(H2MaxSessionStreams)] = value; } }

		// Maximum number of seconds h2 workers remain idle until shut down.
		// H2MaxWorkerIdleSeconds n
		public string H2MaxWorkerIdleSeconds { get { return this[nameof(H2MaxWorkerIdleSeconds)]; } set { this[nameof(H2MaxWorkerIdleSeconds)] = value; } }

		// Maximum number of worker threads to use per child process.
		// H2MaxWorkers n
		public string H2MaxWorkers { get { return this[nameof(H2MaxWorkers)]; } set { this[nameof(H2MaxWorkers)] = value; } }

		// Minimal number of worker threads to use per child process.
		// H2MinWorkers n
		public string H2MinWorkers { get { return this[nameof(H2MinWorkers)]; } set { this[nameof(H2MinWorkers)] = value; } }

		// Require HTTP/2 connections to be "modern TLS" only
		// H2ModernTLSOnly on|off
		public string H2ModernTLSOnly { get { return this[nameof(H2ModernTLSOnly)]; } set { this[nameof(H2ModernTLSOnly)] = value; } }

		// Determine buffering behaviour of output
		// H2OutputBuffering on|off
		public string H2OutputBuffering { get { return this[nameof(H2OutputBuffering)]; } set { this[nameof(H2OutputBuffering)] = value; } }

		// Determine the range of padding bytes added to payload frames
		// H2Padding numbits
		public string H2Padding { get { return this[nameof(H2Padding)]; } set { this[nameof(H2Padding)] = value; } }

		// En-/Disable forward proxy requests via HTTP/2
		// H2ProxyRequests  on|off
		public string H2ProxyRequests { get { return this[nameof(H2ProxyRequests)]; } set { this[nameof(H2ProxyRequests)] = value; } }

		// H2 Server Push Switch
		// H2Push on|off
		public string H2Push { get { return this[nameof(H2Push)]; } set { this[nameof(H2Push)] = value; } }

		// H2 Server Push Diary Size
		// H2PushDiarySize n
		public string H2PushDiarySize { get { return this[nameof(H2PushDiarySize)]; } set { this[nameof(H2PushDiarySize)] = value; } }

		// H2 Server Push Priority
		// H2PushPriority mime-type [after|before|interleaved] [weight]
		public string H2PushPriority { get { return this[nameof(H2PushPriority)]; } set { this[nameof(H2PushPriority)] = value; } }

		// Declares resources for early pushing to the client
		// H2PushResource [add] path [critical]
		public string H2PushResource { get { return this[nameof(H2PushResource)]; } set { this[nameof(H2PushResource)] = value; } }

		// Serialize Request/Response Processing Switch
		// H2SerializeHeaders on|off
		public string H2SerializeHeaders { get { return this[nameof(H2SerializeHeaders)]; } set { this[nameof(H2SerializeHeaders)] = value; } }

		// Maximum amount of output data buffered per stream.
		// H2StreamMaxMemSize bytes
		public string H2StreamMaxMemSize { get { return this[nameof(H2StreamMaxMemSize)]; } set { this[nameof(H2StreamMaxMemSize)] = value; } }

		// Maximum time waiting when sending/receiving data to stream processing
		// H2StreamTimeout time-interval[s]
		public string H2StreamTimeout { get { return this[nameof(H2StreamTimeout)]; } set { this[nameof(H2StreamTimeout)] = value; } }

		// Configure the number of seconds of idle time on TLS before shrinking writes
		// H2TLSCoolDownSecs seconds
		public string H2TLSCoolDownSecs { get { return this[nameof(H2TLSCoolDownSecs)]; } set { this[nameof(H2TLSCoolDownSecs)] = value; } }

		// Configure the number of bytes on TLS connection before doing max writes
		// H2TLSWarmUpSize amount
		public string H2TLSWarmUpSize { get { return this[nameof(H2TLSWarmUpSize)]; } set { this[nameof(H2TLSWarmUpSize)] = value; } }

		// H2 Upgrade Protocol Switch
		// H2Upgrade on|off
		public string H2Upgrade { get { return this[nameof(H2Upgrade)]; } set { this[nameof(H2Upgrade)] = value; } }

		// En-/Disable WebSockets via HTTP/2
		// H2WebSockets  on|off
		public string H2WebSockets { get { return this[nameof(H2WebSockets)]; } set { this[nameof(H2WebSockets)] = value; } }

		// Size of Stream Window for upstream data.
		// H2WindowSize bytes
		public string H2WindowSize { get { return this[nameof(H2WindowSize)]; } set { this[nameof(H2WindowSize)] = value; } }

		// Configure HTTP response headers
		// Header [condition] add|append|echo|edit|edit*|merge|set|setifempty|unset|note header [[expr=]value [replacement] [early|env=[!]varname|expr=expression]] 
		public string Header { get { return this[nameof(Header)]; } set { this[nameof(Header)] = value; } }

		// Name of the file that will be inserted at the top of the index listing
		// HeaderName filename
		public string HeaderName { get { return this[nameof(HeaderName)]; } set { this[nameof(HeaderName)] = value; } }

		// Multicast address for heartbeat packets
		// HeartbeatAddress addr:port
		public string HeartbeatAddress { get { return this[nameof(HeartbeatAddress)]; } set { this[nameof(HeartbeatAddress)] = value; } }

		// multicast address to listen for incoming heartbeat requests 
		// HeartbeatListen addr:port
		public string HeartbeatListen { get { return this[nameof(HeartbeatListen)]; } set { this[nameof(HeartbeatListen)] = value; } }

		// Specifies the maximum number of servers that will be sending heartbeat requests to this server
		// HeartbeatMaxServers number-of-servers
		public string HeartbeatMaxServers { get { return this[nameof(HeartbeatMaxServers)]; } set { this[nameof(HeartbeatMaxServers)] = value; } }

		// Path to store heartbeat data when using flat-file storage
		// HeartbeatStorage file-path
		public string HeartbeatStorage { get { return this[nameof(HeartbeatStorage)]; } set { this[nameof(HeartbeatStorage)] = value; } }

		// Enables DNS lookups on client IP addresses
		// HostnameLookups On|Off|Double
		public string HostnameLookups { get { return this[nameof(HostnameLookups)]; } set { this[nameof(HostnameLookups)] = value; } }

		// Modify restrictions on HTTP Request Messages
		// HttpProtocolOptions [Strict|Unsafe] [RegisteredMethods|LenientMethods]  [Allow0.9|Require1.0]
		public string HttpProtocolOptions { get { return this[nameof(HttpProtocolOptions)]; } set { this[nameof(HttpProtocolOptions)] = value; } }

		// Enables logging of the RFC 1413 identity of the remote user
		// IdentityCheck On|Off
		public string IdentityCheck { get { return this[nameof(IdentityCheck)]; } set { this[nameof(IdentityCheck)] = value; } }

		// Determines the timeout duration for ident requests
		// IdentityCheckTimeout seconds
		public string IdentityCheckTimeout { get { return this[nameof(IdentityCheckTimeout)]; } set { this[nameof(IdentityCheckTimeout)] = value; } }

		// Default base for imagemap files
		// ImapBase map|referer|URL
		public string ImapBase { get { return this[nameof(ImapBase)]; } set { this[nameof(ImapBase)] = value; } }

		// Default action when an imagemap is called with coordinates that are not explicitly mapped
		// ImapDefault error|nocontent|map|referer|URL
		public string ImapDefault { get { return this[nameof(ImapDefault)]; } set { this[nameof(ImapDefault)] = value; } }

		// Action if no coordinates are given when calling an imagemap
		// ImapMenu none|formatted|semiformatted|unformatted
		public string ImapMenu { get { return this[nameof(ImapMenu)]; } set { this[nameof(ImapMenu)] = value; } }

		// Includes other configuration files from within the server configuration files
		// Include file-path|directory-path|wildcard
		public string Include { get { return this[nameof(Include)]; } set { this[nameof(Include)] = value; } }

		// Includes other configuration files from within the server configuration files
		// IncludeOptional file-path|directory-path|wildcard
		public string IncludeOptional { get { return this[nameof(IncludeOptional)]; } set { this[nameof(IncludeOptional)] = value; } }

		// Inserts text in the HEAD section of an index page.
		// IndexHeadInsert "markup ..."
		public string IndexHeadInsert { get { return this[nameof(IndexHeadInsert)]; } set { this[nameof(IndexHeadInsert)] = value; } }

		// Adds to the list of files to hide when listing a directory
		// IndexIgnore file [file] ...
		public string IndexIgnore { get { return this[nameof(IndexIgnore)]; } set { this[nameof(IndexIgnore)] = value; } }

		// Empties the list of files to hide when listing a directory
		// IndexIgnoreReset ON|OFF
		public string IndexIgnoreReset { get { return this[nameof(IndexIgnoreReset)]; } set { this[nameof(IndexIgnoreReset)] = value; } }

		// Various configuration settings for directory indexing
		// IndexOptions  [+|-]option [[+|-]option] ...
		public string IndexOptions { get { return this[nameof(IndexOptions)]; } set { this[nameof(IndexOptions)] = value; } }

		// Sets the default ordering of the directory index
		// IndexOrderDefault Ascending|Descending Name|Date|Size|Description
		public string IndexOrderDefault { get { return this[nameof(IndexOrderDefault)]; } set { this[nameof(IndexOrderDefault)] = value; } }

		// Adds a CSS stylesheet to the directory index
		// IndexStyleSheet url-path
		public string IndexStyleSheet { get { return this[nameof(IndexStyleSheet)]; } set { this[nameof(IndexStyleSheet)] = value; } }

		// Sed command to filter request data (typically POST data)
		// InputSed sed-command
		public string InputSed { get { return this[nameof(InputSed)]; } set { this[nameof(InputSed)] = value; } }

		// Record HSE_APPEND_LOG_PARAMETER requests from ISAPI extensions to the error log
		// ISAPIAppendLogToErrors on|off
		public string ISAPIAppendLogToErrors { get { return this[nameof(ISAPIAppendLogToErrors)]; } set { this[nameof(ISAPIAppendLogToErrors)] = value; } }

		// Record HSE_APPEND_LOG_PARAMETER requests from ISAPI extensions to the query field
		// ISAPIAppendLogToQuery on|off
		public string ISAPIAppendLogToQuery { get { return this[nameof(ISAPIAppendLogToQuery)]; } set { this[nameof(ISAPIAppendLogToQuery)] = value; } }

		// ISAPI .dll files to be loaded at startup
		// ISAPICacheFile file-path [file-path] ...
		public string ISAPICacheFile { get { return this[nameof(ISAPICacheFile)]; } set { this[nameof(ISAPICacheFile)] = value; } }

		// Fake asynchronous support for ISAPI callbacks
		// ISAPIFakeAsync on|off
		public string ISAPIFakeAsync { get { return this[nameof(ISAPIFakeAsync)]; } set { this[nameof(ISAPIFakeAsync)] = value; } }

		// Log unsupported feature requests from ISAPI extensions
		// ISAPILogNotSupported on|off
		public string ISAPILogNotSupported { get { return this[nameof(ISAPILogNotSupported)]; } set { this[nameof(ISAPILogNotSupported)] = value; } }

		// Size of the Read Ahead Buffer sent to ISAPI extensions
		// ISAPIReadAheadBuffer size
		public string ISAPIReadAheadBuffer { get { return this[nameof(ISAPIReadAheadBuffer)]; } set { this[nameof(ISAPIReadAheadBuffer)] = value; } }

		// Enables HTTP persistent connections
		// KeepAlive On|Off
		public string KeepAlive { get { return this[nameof(KeepAlive)]; } set { this[nameof(KeepAlive)] = value; } }

		// Amount of time the server will wait for subsequent requests on a persistent connection
		// KeepAliveTimeout num[ms]
		public string KeepAliveTimeout { get { return this[nameof(KeepAliveTimeout)]; } set { this[nameof(KeepAliveTimeout)] = value; } }

		// Keep the request body instead of discarding it up to the specified maximum size, for potential use by filters such as mod_include.
		// KeptBodySize maximum size in bytes
		public string KeptBodySize { get { return this[nameof(KeptBodySize)]; } set { this[nameof(KeptBodySize)] = value; } }

		// The precedence of language variants for cases where the client does not express a preference
		// LanguagePriority MIME-lang [MIME-lang] ...
		public string LanguagePriority { get { return this[nameof(LanguagePriority)]; } set { this[nameof(LanguagePriority)] = value; } }

		// Maximum number of entries in the primary LDAP cache
		// LDAPCacheEntries number
		public string LDAPCacheEntries { get { return this[nameof(LDAPCacheEntries)]; } set { this[nameof(LDAPCacheEntries)] = value; } }

		// Time that cached items remain valid
		// LDAPCacheTTL seconds
		public string LDAPCacheTTL { get { return this[nameof(LDAPCacheTTL)]; } set { this[nameof(LDAPCacheTTL)] = value; } }

		// Discard backend connections that have been sitting in the connection pool too long
		// LDAPConnectionPoolTTL n
		public string LDAPConnectionPoolTTL { get { return this[nameof(LDAPConnectionPoolTTL)]; } set { this[nameof(LDAPConnectionPoolTTL)] = value; } }

		// Specifies the socket connection timeout in seconds
		// LDAPConnectionTimeout seconds
		public string LDAPConnectionTimeout { get { return this[nameof(LDAPConnectionTimeout)]; } set { this[nameof(LDAPConnectionTimeout)] = value; } }

		// Enable debugging in the LDAP SDK
		// LDAPLibraryDebug 7
		public string LDAPLibraryDebug { get { return this[nameof(LDAPLibraryDebug)]; } set { this[nameof(LDAPLibraryDebug)] = value; } }

		// Number of entries used to cache LDAP compare operations
		// LDAPOpCacheEntries number
		public string LDAPOpCacheEntries { get { return this[nameof(LDAPOpCacheEntries)]; } set { this[nameof(LDAPOpCacheEntries)] = value; } }

		// Time that entries in the operation cache remain valid
		// LDAPOpCacheTTL seconds
		public string LDAPOpCacheTTL { get { return this[nameof(LDAPOpCacheTTL)]; } set { this[nameof(LDAPOpCacheTTL)] = value; } }

		// The maximum number of referral hops to chase before terminating an LDAP query.
		// LDAPReferralHopLimit number
		public string LDAPReferralHopLimit { get { return this[nameof(LDAPReferralHopLimit)]; } set { this[nameof(LDAPReferralHopLimit)] = value; } }

		// Enable referral chasing during queries to the LDAP server.
		// LDAPReferrals On|Off|default
		public string LDAPReferrals { get { return this[nameof(LDAPReferrals)]; } set { this[nameof(LDAPReferrals)] = value; } }

		// Configures the number of LDAP server retries.
		// LDAPRetries number-of-retries
		public string LDAPRetries { get { return this[nameof(LDAPRetries)]; } set { this[nameof(LDAPRetries)] = value; } }

		// Configures the delay between LDAP server retries.
		// LDAPRetryDelay seconds
		public string LDAPRetryDelay { get { return this[nameof(LDAPRetryDelay)]; } set { this[nameof(LDAPRetryDelay)] = value; } }

		// Sets the shared memory cache file
		// LDAPSharedCacheFile directory-path/filename
		public string LDAPSharedCacheFile { get { return this[nameof(LDAPSharedCacheFile)]; } set { this[nameof(LDAPSharedCacheFile)] = value; } }

		// Size in bytes of the shared-memory cache
		// LDAPSharedCacheSize bytes
		public string LDAPSharedCacheSize { get { return this[nameof(LDAPSharedCacheSize)]; } set { this[nameof(LDAPSharedCacheSize)] = value; } }

		// Specifies the timeout for LDAP search and bind operations, in seconds
		// LDAPTimeout seconds
		public string LDAPTimeout { get { return this[nameof(LDAPTimeout)]; } set { this[nameof(LDAPTimeout)] = value; } }

		// Sets the file containing or nickname referring to a per connection client certificate. Not all LDAP toolkits support per connection client certificates.
		// LDAPTrustedClientCert type directory-path/filename/nickname [password]
		public string LDAPTrustedClientCert { get { return this[nameof(LDAPTrustedClientCert)]; } set { this[nameof(LDAPTrustedClientCert)] = value; } }

		// Sets the file or database containing global trusted Certificate Authority or global client certificates
		// LDAPTrustedGlobalCert type directory-path/filename [password]
		public string LDAPTrustedGlobalCert { get { return this[nameof(LDAPTrustedGlobalCert)]; } set { this[nameof(LDAPTrustedGlobalCert)] = value; } }

		// Specifies the SSL/TLS mode to be used when connecting to an LDAP server.
		// LDAPTrustedMode type
		public string LDAPTrustedMode { get { return this[nameof(LDAPTrustedMode)]; } set { this[nameof(LDAPTrustedMode)] = value; } }

		// Force server certificate verification
		// LDAPVerifyServerCert On|Off
		public string LDAPVerifyServerCert { get { return this[nameof(LDAPVerifyServerCert)]; } set { this[nameof(LDAPVerifyServerCert)] = value; } }

		// Determine maximum number of internal redirects and nested subrequests
		// LimitInternalRecursion number [number]
		public string LimitInternalRecursion { get { return this[nameof(LimitInternalRecursion)]; } set { this[nameof(LimitInternalRecursion)] = value; } }

		// Restricts the total size of the HTTP request body sent from the client
		// LimitRequestBody bytes
		public string LimitRequestBody { get { return this[nameof(LimitRequestBody)]; } set { this[nameof(LimitRequestBody)] = value; } }

		// Limits the number of HTTP request header fields that will be accepted from the client
		// LimitRequestFields number
		public string LimitRequestFields { get { return this[nameof(LimitRequestFields)]; } set { this[nameof(LimitRequestFields)] = value; } }

		// Limits the size of the HTTP request header allowed from the client
		// LimitRequestFieldSize bytes
		public string LimitRequestFieldSize { get { return this[nameof(LimitRequestFieldSize)]; } set { this[nameof(LimitRequestFieldSize)] = value; } }

		// Limit the size of the HTTP request line that will be accepted from the client
		// LimitRequestLine bytes
		public string LimitRequestLine { get { return this[nameof(LimitRequestLine)]; } set { this[nameof(LimitRequestLine)] = value; } }

		// Limits the size of an XML-based request body
		// LimitXMLRequestBody bytes
		public string LimitXMLRequestBody { get { return this[nameof(LimitXMLRequestBody)]; } set { this[nameof(LimitXMLRequestBody)] = value; } }

		// IP addresses and ports that the server listens to
		// Listen [IP-address:]portnumber [protocol]
		public string Listen { get { return this[nameof(Listen)]; } set { this[nameof(Listen)] = value; } }

		// Maximum length of the queue of pending connections
		// ListenBackLog backlog
		public string ListenBackLog { get { return this[nameof(ListenBackLog)]; } set { this[nameof(ListenBackLog)] = value; } }

		// Ratio between the number of CPU cores (online) and the number of listeners' buckets
		// ListenCoresBucketsRatio ratio
		public string ListenCoresBucketsRatio { get { return this[nameof(ListenCoresBucketsRatio)]; } set { this[nameof(ListenCoresBucketsRatio)] = value; } }

		// Link in the named object file or library
		// LoadFile filename [filename] ...
		public string LoadFile { get { return this[nameof(LoadFile)]; } set { this[nameof(LoadFile)] = value; } }

		// Links in the object file or library, and adds to the list of active modules
		// LoadModule module filename
		public string LoadModule { get { return this[nameof(LoadModule)]; } set { this[nameof(LoadModule)] = value; } }

		// Describes a format for use in a log file
		// LogFormat format|nickname [nickname]
		public string LogFormat { get { return this[nameof(LogFormat)]; } set { this[nameof(LogFormat)] = value; } }

		// Enable tracking of time to first byte (TTFB)
		// LogIOTrackTTFB ON|OFF
		public string LogIOTrackTTFB { get { return this[nameof(LogIOTrackTTFB)]; } set { this[nameof(LogIOTrackTTFB)] = value; } }

		// Controls the verbosity of the ErrorLog
		// LogLevel [module:]level     [module:level] ... 
		public string LogLevel { get { return this[nameof(LogLevel)]; } set { this[nameof(LogLevel)] = value; } }

		// Log user-defined message to error log 
		// LogMessage message [hook=hook] [expr=expression] 
		public string LogMessage { get { return this[nameof(LogMessage)]; } set { this[nameof(LogMessage)] = value; } }

		// Plug an authorization provider function into mod_authz_core
		// LuaAuthzProvider provider_name /path/to/lua/script.lua function_name
		public string LuaAuthzProvider { get { return this[nameof(LuaAuthzProvider)]; } set { this[nameof(LuaAuthzProvider)] = value; } }

		// Configure the compiled code cache.
		// LuaCodeCache stat|forever|never
		public string LuaCodeCache { get { return this[nameof(LuaCodeCache)]; } set { this[nameof(LuaCodeCache)] = value; } }

		// Provide a hook for the access_checker phase of request processing
		// LuaHookAccessChecker  /path/to/lua/script.lua  hook_function_name [early|late]
		public string LuaHookAccessChecker { get { return this[nameof(LuaHookAccessChecker)]; } set { this[nameof(LuaHookAccessChecker)] = value; } }

		// Provide a hook for the auth_checker phase of request processing
		// LuaHookAuthChecker  /path/to/lua/script.lua hook_function_name [early|late]
		public string LuaHookAuthChecker { get { return this[nameof(LuaHookAuthChecker)]; } set { this[nameof(LuaHookAuthChecker)] = value; } }

		// Provide a hook for the check_user_id phase of request processing
		// LuaHookCheckUserID  /path/to/lua/script.lua hook_function_name [early|late]
		public string LuaHookCheckUserID { get { return this[nameof(LuaHookCheckUserID)]; } set { this[nameof(LuaHookCheckUserID)] = value; } }

		// Provide a hook for the fixups phase of a request processing
		// LuaHookFixups  /path/to/lua/script.lua hook_function_name
		public string LuaHookFixups { get { return this[nameof(LuaHookFixups)]; } set { this[nameof(LuaHookFixups)] = value; } }

		// Provide a hook for the insert_filter phase of request processing
		// LuaHookInsertFilter  /path/to/lua/script.lua hook_function_name
		public string LuaHookInsertFilter { get { return this[nameof(LuaHookInsertFilter)]; } set { this[nameof(LuaHookInsertFilter)] = value; } }

		// Provide a hook for the access log phase of a request processing
		// LuaHookLog  /path/to/lua/script.lua log_function_name
		public string LuaHookLog { get { return this[nameof(LuaHookLog)]; } set { this[nameof(LuaHookLog)] = value; } }

		// Provide a hook for the map_to_storage phase of request processing
		// LuaHookMapToStorage  /path/to/lua/script.lua hook_function_name
		public string LuaHookMapToStorage { get { return this[nameof(LuaHookMapToStorage)]; } set { this[nameof(LuaHookMapToStorage)] = value; } }

		// Provide a hook for the pre_translate phase of a request processing
		// LuaHookPreTranslate  /path/to/lua/script.lua hook_function_name
		public string LuaHookPreTranslate { get { return this[nameof(LuaHookPreTranslate)]; } set { this[nameof(LuaHookPreTranslate)] = value; } }

		// Provide a hook for the translate name phase of request processing
		// LuaHookTranslateName  /path/to/lua/script.lua  hook_function_name [early|late]
		public string LuaHookTranslateName { get { return this[nameof(LuaHookTranslateName)]; } set { this[nameof(LuaHookTranslateName)] = value; } }

		// Provide a hook for the type_checker phase of request processing
		// LuaHookTypeChecker  /path/to/lua/script.lua hook_function_name
		public string LuaHookTypeChecker { get { return this[nameof(LuaHookTypeChecker)]; } set { this[nameof(LuaHookTypeChecker)] = value; } }

		// Controls how parent configuration sections are merged into children
		// LuaInherit none|parent-first|parent-last
		public string LuaInherit { get { return this[nameof(LuaInherit)]; } set { this[nameof(LuaInherit)] = value; } }

		// Provide a Lua function for content input filtering
		// LuaInputFilter filter_name /path/to/lua/script.lua function_name
		public string LuaInputFilter { get { return this[nameof(LuaInputFilter)]; } set { this[nameof(LuaInputFilter)] = value; } }

		// Map a path to a lua handler
		// LuaMapHandler uri-pattern /path/to/lua/script.lua [function-name]
		public string LuaMapHandler { get { return this[nameof(LuaMapHandler)]; } set { this[nameof(LuaMapHandler)] = value; } }

		// Provide a Lua function for content output filtering
		// LuaOutputFilter filter_name /path/to/lua/script.lua function_name
		public string LuaOutputFilter { get { return this[nameof(LuaOutputFilter)]; } set { this[nameof(LuaOutputFilter)] = value; } }

		// Add a directory to lua's package.cpath
		// LuaPackageCPath /path/to/include/?.soa
		public string LuaPackageCPath { get { return this[nameof(LuaPackageCPath)]; } set { this[nameof(LuaPackageCPath)] = value; } }

		// Add a directory to lua's package.path
		// LuaPackagePath /path/to/include/?.lua
		public string LuaPackagePath { get { return this[nameof(LuaPackagePath)]; } set { this[nameof(LuaPackagePath)] = value; } }

		// Provide a hook for the quick handler of request processing
		// LuaQuickHandler /path/to/script.lua hook_function_name
		public string LuaQuickHandler { get { return this[nameof(LuaQuickHandler)]; } set { this[nameof(LuaQuickHandler)] = value; } }

		// Specify the base path for resolving relative paths for mod_lua directives
		// LuaRoot /path/to/a/directory
		public string LuaRoot { get { return this[nameof(LuaRoot)]; } set { this[nameof(LuaRoot)] = value; } }

		// One of once, request, conn, thread -- default is once
		// LuaScope once|request|conn|thread|server [min] [max]
		public string LuaScope { get { return this[nameof(LuaScope)]; } set { this[nameof(LuaScope)] = value; } }

		// Limit on the number of connections that an individual child server will handle during its life
		// MaxConnectionsPerChild number
		public string MaxConnectionsPerChild { get { return this[nameof(MaxConnectionsPerChild)]; } set { this[nameof(MaxConnectionsPerChild)] = value; } }

		// Number of requests allowed on a persistent connection
		// MaxKeepAliveRequests number
		public string MaxKeepAliveRequests { get { return this[nameof(MaxKeepAliveRequests)]; } set { this[nameof(MaxKeepAliveRequests)] = value; } }

		// Maximum amount of memory that the main allocator is allowed to hold without calling free()
		// MaxMemFree KBytes
		public string MaxMemFree { get { return this[nameof(MaxMemFree)]; } set { this[nameof(MaxMemFree)] = value; } }

		// Number of overlapping ranges (eg: 100-200,150-300) allowed before returning the complete resource
		// MaxRangeOverlaps default | unlimited | none | number-of-ranges
		public string MaxRangeOverlaps { get { return this[nameof(MaxRangeOverlaps)]; } set { this[nameof(MaxRangeOverlaps)] = value; } }

		// Number of range reversals (eg: 100-200,50-70) allowed before returning the complete resource
		// MaxRangeReversals default | unlimited | none | number-of-ranges
		public string MaxRangeReversals { get { return this[nameof(MaxRangeReversals)]; } set { this[nameof(MaxRangeReversals)] = value; } }

		// Number of ranges allowed before returning the complete resource 
		// MaxRanges default | unlimited | none | number-of-ranges
		public string MaxRanges { get { return this[nameof(MaxRanges)]; } set { this[nameof(MaxRanges)] = value; } }

		// Maximum number of connections that will be processed simultaneously
		// MaxRequestWorkers number
		public string MaxRequestWorkers { get { return this[nameof(MaxRequestWorkers)]; } set { this[nameof(MaxRequestWorkers)] = value; } }

		// Maximum number of idle child server processes
		// MaxSpareServers number
		public string MaxSpareServers { get { return this[nameof(MaxSpareServers)]; } set { this[nameof(MaxSpareServers)] = value; } }

		// Maximum number of idle threads
		// MaxSpareThreads number
		public string MaxSpareThreads { get { return this[nameof(MaxSpareThreads)]; } set { this[nameof(MaxSpareThreads)] = value; } }

		// Set the maximum number of worker threads
		// MaxThreads number
		public string MaxThreads { get { return this[nameof(MaxThreads)]; } set { this[nameof(MaxThreads)] = value; } }

		// -
		// MDActivationDelay duration
		public string MDActivationDelay { get { return this[nameof(MDActivationDelay)]; } set { this[nameof(MDActivationDelay)] = value; } }

		// Control if base server may be managed or only virtual hosts.
		// MDBaseServer on|off
		public string MDBaseServer { get { return this[nameof(MDBaseServer)]; } set { this[nameof(MDBaseServer)] = value; } }

		// Type of ACME challenge used to prove domain ownership.
		// MDCAChallenges name [ name ... ]
		public string MDCAChallenges { get { return this[nameof(MDCAChallenges)]; } set { this[nameof(MDCAChallenges)] = value; } }

		// You confirm that you accepted the Terms of Service of the Certificate Authority.
		// MDCertificateAgreement accepted
		public string MDCertificateAgreement { get { return this[nameof(MDCertificateAgreement)]; } set { this[nameof(MDCertificateAgreement)] = value; } }

		// The URL(s) of the ACME Certificate Authority to use.
		// MDCertificateAuthority url
		public string MDCertificateAuthority { get { return this[nameof(MDCertificateAuthority)]; } set { this[nameof(MDCertificateAuthority)] = value; } }

		// -
		// MDCertificateCheck name url
		public string MDCertificateCheck { get { return this[nameof(MDCertificateCheck)]; } set { this[nameof(MDCertificateCheck)] = value; } }

		// Specify a static certificate file for the MD.
		// MDCertificateFile path-to-pem-file
		public string MDCertificateFile { get { return this[nameof(MDCertificateFile)]; } set { this[nameof(MDCertificateFile)] = value; } }

		// Specify a static private key for for the static cerrtificate.
		// MDCertificateKeyFile path-to-file
		public string MDCertificateKeyFile { get { return this[nameof(MDCertificateKeyFile)]; } set { this[nameof(MDCertificateKeyFile)] = value; } }

		// The URL of a certificate log monitor.
		// MDCertificateMonitor name url
		public string MDCertificateMonitor { get { return this[nameof(MDCertificateMonitor)]; } set { this[nameof(MDCertificateMonitor)] = value; } }

		// The protocol to use with the Certificate Authority.
		// MDCertificateProtocol protocol
		public string MDCertificateProtocol { get { return this[nameof(MDCertificateProtocol)]; } set { this[nameof(MDCertificateProtocol)] = value; } }

		// Exposes public certificate information in JSON.
		// MDCertificateStatus on|off
		public string MDCertificateStatus { get { return this[nameof(MDCertificateStatus)]; } set { this[nameof(MDCertificateStatus)] = value; } }

		// -
		// MDChallengeDns01 path-to-command
		public string MDChallengeDns01 { get { return this[nameof(MDChallengeDns01)]; } set { this[nameof(MDChallengeDns01)] = value; } }

		// -
		// MDChallengeDns01Version 1|2
		public string MDChallengeDns01Version { get { return this[nameof(MDChallengeDns01Version)]; } set { this[nameof(MDChallengeDns01Version)] = value; } }

		// -
		// MDContactEmail address
		public string MDContactEmail { get { return this[nameof(MDContactEmail)]; } set { this[nameof(MDContactEmail)] = value; } }

		// former name of MDRenewMode.
		// MDDriveMode always|auto|manual
		public string MDDriveMode { get { return this[nameof(MDDriveMode)]; } set { this[nameof(MDDriveMode)] = value; } }

		// -
		// MDExternalAccountBinding key-id hmac-64 | none | file
		public string MDExternalAccountBinding { get { return this[nameof(MDExternalAccountBinding)]; } set { this[nameof(MDExternalAccountBinding)] = value; } }

		// Define a proxy for outgoing connections.
		// MDHttpProxy url
		public string MDHttpProxy { get { return this[nameof(MDHttpProxy)]; } set { this[nameof(MDHttpProxy)] = value; } }

		// -
		// MDMatchNames all|servernames
		public string MDMatchNames { get { return this[nameof(MDMatchNames)]; } set { this[nameof(MDMatchNames)] = value; } }

		// Additional hostname for the managed domain.
		// MDMember hostname
		public string MDMember { get { return this[nameof(MDMember)]; } set { this[nameof(MDMember)] = value; } }

		// Control if the alias domain names are automatically added.
		// MDMembers auto|manual
		public string MDMembers { get { return this[nameof(MDMembers)]; } set { this[nameof(MDMembers)] = value; } }

		// Handle events for Manage Domains
		// MDMessageCmd path-to-cmd optional-args
		public string MDMessageCmd { get { return this[nameof(MDMessageCmd)]; } set { this[nameof(MDMessageCmd)] = value; } }

		// Control if new certificates carry the OCSP Must Staple flag.
		// MDMustStaple on|off
		public string MDMustStaple { get { return this[nameof(MDMustStaple)]; } set { this[nameof(MDMustStaple)] = value; } }

		// Run a program when a Managed Domain is ready.
		// MDNotifyCmd path [ args ]
		public string MDNotifyCmd { get { return this[nameof(MDNotifyCmd)]; } set { this[nameof(MDNotifyCmd)] = value; } }

		// Define list of domain names that belong to one group.
		// MDomain dns-name [ other-dns-name... ] [auto|manual]
		public string MDomain { get { return this[nameof(MDomain)]; } set { this[nameof(MDomain)] = value; } }

		// Map external to internal ports for domain ownership verification.
		// MDPortMap map1 [ map2 ]
		public string MDPortMap { get { return this[nameof(MDPortMap)]; } set { this[nameof(MDPortMap)] = value; } }

		// Set type and size of the private keys generated.
		// MDPrivateKeys type [ params... ]
		public string MDPrivateKeys { get { return this[nameof(MDPrivateKeys)]; } set { this[nameof(MDPrivateKeys)] = value; } }

		// Controls if certificates shall be renewed.
		// MDRenewMode always|auto|manual
		public string MDRenewMode { get { return this[nameof(MDRenewMode)]; } set { this[nameof(MDRenewMode)] = value; } }

		// Control when a certificate will be renewed.
		// MDRenewWindow duration
		public string MDRenewWindow { get { return this[nameof(MDRenewWindow)]; } set { this[nameof(MDRenewWindow)] = value; } }

		// Redirects http: traffic to https: for Managed Domains.
		// MDRequireHttps off|temporary|permanent
		public string MDRequireHttps { get { return this[nameof(MDRequireHttps)]; } set { this[nameof(MDRequireHttps)] = value; } }

		// -
		// MDRetryDelay duration
		public string MDRetryDelay { get { return this[nameof(MDRetryDelay)]; } set { this[nameof(MDRetryDelay)] = value; } }

		// -
		// MDRetryFailover number
		public string MDRetryFailover { get { return this[nameof(MDRetryFailover)]; } set { this[nameof(MDRetryFailover)] = value; } }

		// Control if Managed Domain information is added to server-status.
		// MDServerStatus on|off
		public string MDServerStatus { get { return this[nameof(MDServerStatus)]; } set { this[nameof(MDServerStatus)] = value; } }

		// Enable stapling for certificates not managed by mod_md.
		// MDStapleOthers on|off
		public string MDStapleOthers { get { return this[nameof(MDStapleOthers)]; } set { this[nameof(MDStapleOthers)] = value; } }

		// Enable stapling for all or a particular MDomain.
		// MDStapling on|off
		public string MDStapling { get { return this[nameof(MDStapling)]; } set { this[nameof(MDStapling)] = value; } }

		// Controls when old responses should be removed.
		// MDStaplingKeepResponse duration
		public string MDStaplingKeepResponse { get { return this[nameof(MDStaplingKeepResponse)]; } set { this[nameof(MDStaplingKeepResponse)] = value; } }

		// Control when the stapling responses will be renewed.
		// MDStaplingRenewWindow duration
		public string MDStaplingRenewWindow { get { return this[nameof(MDStaplingRenewWindow)]; } set { this[nameof(MDStaplingRenewWindow)] = value; } }

		// Path on the local file system to store the Managed Domains data.
		// MDStoreDir path
		public string MDStoreDir { get { return this[nameof(MDStoreDir)]; } set { this[nameof(MDStoreDir)] = value; } }

		// -
		// MDStoreLocks on|off|duration
		public string MDStoreLocks { get { return this[nameof(MDStoreLocks)]; } set { this[nameof(MDStoreLocks)] = value; } }

		// Define the time window when you want to be warned about an expiring certificate.
		// MDWarnWindow duration
		public string MDWarnWindow { get { return this[nameof(MDWarnWindow)]; } set { this[nameof(MDWarnWindow)] = value; } }

		// Keepalive time for idle connections
		// MemcacheConnTTL num[units]
		public string MemcacheConnTTL { get { return this[nameof(MemcacheConnTTL)]; } set { this[nameof(MemcacheConnTTL)] = value; } }

		// Controls whether the server merges consecutive slashes in URLs. 
		// MergeSlashes ON|OFF
		public string MergeSlashes { get { return this[nameof(MergeSlashes)]; } set { this[nameof(MergeSlashes)] = value; } }

		// Determines whether trailers are merged into headers
		// MergeTrailers [on|off]
		public string MergeTrailers { get { return this[nameof(MergeTrailers)]; } set { this[nameof(MergeTrailers)] = value; } }

		// Name of the directory to find CERN-style meta information files
		// MetaDir directory
		public string MetaDir { get { return this[nameof(MetaDir)]; } set { this[nameof(MetaDir)] = value; } }

		// Activates CERN meta-file processing
		// MetaFiles on|off
		public string MetaFiles { get { return this[nameof(MetaFiles)]; } set { this[nameof(MetaFiles)] = value; } }

		// File name suffix for the file containing CERN-style meta information
		// MetaSuffix suffix
		public string MetaSuffix { get { return this[nameof(MetaSuffix)]; } set { this[nameof(MetaSuffix)] = value; } }

		// Enable MIME-type determination based on file contents using the specified magic file
		// MimeMagicFile file-path
		public string MimeMagicFile { get { return this[nameof(MimeMagicFile)]; } set { this[nameof(MimeMagicFile)] = value; } }

		// Minimum number of idle child server processes
		// MinSpareServers number
		public string MinSpareServers { get { return this[nameof(MinSpareServers)]; } set { this[nameof(MinSpareServers)] = value; } }

		// Minimum number of idle threads available to handle request spikes
		// MinSpareThreads number
		public string MinSpareThreads { get { return this[nameof(MinSpareThreads)]; } set { this[nameof(MinSpareThreads)] = value; } }

		// Map a list of files into memory at startup time
		// MMapFile file-path [file-path] ...
		public string MMapFile { get { return this[nameof(MMapFile)]; } set { this[nameof(MMapFile)] = value; } }

		// Modem standard to simulate
		// ModemStandard V.21|V.26bis|V.32|V.34|V.92
		public string ModemStandard { get { return this[nameof(ModemStandard)]; } set { this[nameof(ModemStandard)] = value; } }

		// Tells mod_mime to treat path_info components as part of the filename
		// ModMimeUsePathInfo On|Off
		public string ModMimeUsePathInfo { get { return this[nameof(ModMimeUsePathInfo)]; } set { this[nameof(ModMimeUsePathInfo)] = value; } }

		// The types of files that will be included when searching for a matching file with MultiViews
		// MultiviewsMatch Any|NegotiatedOnly|Filters|Handlers [Handlers|Filters]
		public string MultiviewsMatch { get { return this[nameof(MultiviewsMatch)]; } set { this[nameof(MultiviewsMatch)] = value; } }

		// Configures mutex mechanism and lock file directory for all or specified mutexes
		// Mutex mechanism [default|mutex-name] ... [OmitPID]
		public string Mutex { get { return this[nameof(Mutex)]; } set { this[nameof(Mutex)] = value; } }

		// DEPRECATED: Designates an IP address for name-virtual hosting
		// NameVirtualHost addr[:port]
		public string NameVirtualHost { get { return this[nameof(NameVirtualHost)]; } set { this[nameof(NameVirtualHost)] = value; } }

		// Hosts, domains, or networks that will be connected to directly
		// NoProxy host [host] ...
		public string NoProxy { get { return this[nameof(NoProxy)]; } set { this[nameof(NoProxy)] = value; } }

		// List of additional client certificates
		// NWSSLTrustedCerts filename [filename] ...
		public string NWSSLTrustedCerts { get { return this[nameof(NWSSLTrustedCerts)]; } set { this[nameof(NWSSLTrustedCerts)] = value; } }

		// Allows a connection to be upgraded to an SSL connection upon request
		// NWSSLUpgradeable [IP-address:]portnumber
		public string NWSSLUpgradeable { get { return this[nameof(NWSSLUpgradeable)]; } set { this[nameof(NWSSLUpgradeable)] = value; } }

		// Configures what features are available in a particular directory
		// Options [+|-]option [[+|-]option] ...
		public string Options { get { return this[nameof(Options)]; } set { this[nameof(Options)] = value; } }

		// Controls the default access state and the order in which Allow and Deny are evaluated.
		//  Order ordering
		public string Order { get { return this[nameof(Order)]; } set { this[nameof(Order)] = value; } }

		// Sed command for filtering response content
		// OutputSed sed-command
		public string OutputSed { get { return this[nameof(OutputSed)]; } set { this[nameof(OutputSed)] = value; } }

		// Passes environment variables from the shell
		// PassEnv env-variable [env-variable] ...
		public string PassEnv { get { return this[nameof(PassEnv)]; } set { this[nameof(PassEnv)] = value; } }

		// File where the server records the process ID of the daemon
		// PidFile filename
		public string PidFile { get { return this[nameof(PidFile)]; } set { this[nameof(PidFile)] = value; } }

		// Trade off processing speed and efficiency vs security against malicious privileges-aware code.
		// PrivilegesMode FAST|SECURE|SELECTIVE
		public string PrivilegesMode { get { return this[nameof(PrivilegesMode)]; } set { this[nameof(PrivilegesMode)] = value; } }

		// Protocol for a listening socket
		// Protocol protocol
		public string Protocol { get { return this[nameof(Protocol)]; } set { this[nameof(Protocol)] = value; } }

		// Turn the echo server on or off
		// ProtocolEcho On|Off
		public string ProtocolEcho { get { return this[nameof(ProtocolEcho)]; } set { this[nameof(ProtocolEcho)] = value; } }

		// Protocols available for a server/virtual host
		// Protocols protocol ...
		public string Protocols { get { return this[nameof(Protocols)]; } set { this[nameof(Protocols)] = value; } }

		// Determines if order of Protocols determines precedence during negotiation
		// ProtocolsHonorOrder On|Off
		public string ProtocolsHonorOrder { get { return this[nameof(ProtocolsHonorOrder)]; } set { this[nameof(ProtocolsHonorOrder)] = value; } }

		// Forward 100-continue expectation to the origin server
		// Proxy100Continue Off|On
		public string Proxy100Continue { get { return this[nameof(Proxy100Continue)]; } set { this[nameof(Proxy100Continue)] = value; } }

		// Add proxy information in X-Forwarded-* headers
		// ProxyAddHeaders Off|On
		public string ProxyAddHeaders { get { return this[nameof(ProxyAddHeaders)]; } set { this[nameof(ProxyAddHeaders)] = value; } }

		// Determines how to handle bad header lines in a response
		// ProxyBadHeader IsError|Ignore|StartBody
		public string ProxyBadHeader { get { return this[nameof(ProxyBadHeader)]; } set { this[nameof(ProxyBadHeader)] = value; } }

		// Words, hosts, or domains that are banned from being proxied
		// ProxyBlock *|word|host|domain [word|host|domain] ...
		public string ProxyBlock { get { return this[nameof(ProxyBlock)]; } set { this[nameof(ProxyBlock)] = value; } }

		// Default domain name for proxied requests
		// ProxyDomain Domain
		public string ProxyDomain { get { return this[nameof(ProxyDomain)]; } set { this[nameof(ProxyDomain)] = value; } }

		// Override error pages for proxied content
		// ProxyErrorOverride Off|On [code ...]
		public string ProxyErrorOverride { get { return this[nameof(ProxyErrorOverride)]; } set { this[nameof(ProxyErrorOverride)] = value; } }

		// Pathname to DBM file.
		// ProxyExpressDBMFile pathname
		public string ProxyExpressDBMFile { get { return this[nameof(ProxyExpressDBMFile)]; } set { this[nameof(ProxyExpressDBMFile)] = value; } }

		// DBM type of file.
		// ProxyExpressDBMType type
		public string ProxyExpressDBMType { get { return this[nameof(ProxyExpressDBMType)]; } set { this[nameof(ProxyExpressDBMType)] = value; } }

		// Enable the module functionality.
		// ProxyExpressEnable on|off
		public string ProxyExpressEnable { get { return this[nameof(ProxyExpressEnable)]; } set { this[nameof(ProxyExpressEnable)] = value; } }

		// Specify the type of backend FastCGI application
		// ProxyFCGIBackendType FPM|GENERIC
		public string ProxyFCGIBackendType { get { return this[nameof(ProxyFCGIBackendType)]; } set { this[nameof(ProxyFCGIBackendType)] = value; } }

		// Allow variables sent to FastCGI servers to be fixed up
		// ProxyFCGISetEnvIf conditional-expression     [!]environment-variable-name     [value-expression]
		public string ProxyFCGISetEnvIf { get { return this[nameof(ProxyFCGISetEnvIf)]; } set { this[nameof(ProxyFCGISetEnvIf)] = value; } }

		// Define the character set for proxied FTP listings
		// ProxyFtpDirCharset character_set
		public string ProxyFtpDirCharset { get { return this[nameof(ProxyFtpDirCharset)]; } set { this[nameof(ProxyFtpDirCharset)] = value; } }

		// Whether wildcards in requested filenames are escaped when sent to the FTP server
		// ProxyFtpEscapeWildcards on|off
		public string ProxyFtpEscapeWildcards { get { return this[nameof(ProxyFtpEscapeWildcards)]; } set { this[nameof(ProxyFtpEscapeWildcards)] = value; } }

		// Whether wildcards in requested filenames trigger a file listing
		// ProxyFtpListOnWildcard on|off
		public string ProxyFtpListOnWildcard { get { return this[nameof(ProxyFtpListOnWildcard)]; } set { this[nameof(ProxyFtpListOnWildcard)] = value; } }

		// Creates a named condition expression to use to determine health of the backend based on its response
		// ProxyHCExpr name {ap_expr expression}
		public string ProxyHCExpr { get { return this[nameof(ProxyHCExpr)]; } set { this[nameof(ProxyHCExpr)] = value; } }

		// Creates a named template for setting various health check parameters
		// ProxyHCTemplate name parameter=setting [...]
		public string ProxyHCTemplate { get { return this[nameof(ProxyHCTemplate)]; } set { this[nameof(ProxyHCTemplate)] = value; } }

		// Sets the total server-wide size of the threadpool used for the health check workers
		// ProxyHCTPsize size
		public string ProxyHCTPsize { get { return this[nameof(ProxyHCTPsize)]; } set { this[nameof(ProxyHCTPsize)] = value; } }

		// Sets the buffer size increment for buffering inline scripts and stylesheets.
		// ProxyHTMLBufSize bytes
		public string ProxyHTMLBufSize { get { return this[nameof(ProxyHTMLBufSize)]; } set { this[nameof(ProxyHTMLBufSize)] = value; } }

		// Specify a charset for mod_proxy_html output.
		// ProxyHTMLCharsetOut Charset | *
		public string ProxyHTMLCharsetOut { get { return this[nameof(ProxyHTMLCharsetOut)]; } set { this[nameof(ProxyHTMLCharsetOut)] = value; } }

		// Sets an HTML or XHTML document type declaration.
		// ProxyHTMLDocType HTML|XHTML [Legacy] OR ProxyHTMLDocType fpi [SGML|XML]
		public string ProxyHTMLDocType { get { return this[nameof(ProxyHTMLDocType)]; } set { this[nameof(ProxyHTMLDocType)] = value; } }

		// Turns the proxy_html filter on or off.
		// ProxyHTMLEnable On|Off
		public string ProxyHTMLEnable { get { return this[nameof(ProxyHTMLEnable)]; } set { this[nameof(ProxyHTMLEnable)] = value; } }

		// Specify attributes to treat as scripting events.
		// ProxyHTMLEvents attribute [attribute ...]
		public string ProxyHTMLEvents { get { return this[nameof(ProxyHTMLEvents)]; } set { this[nameof(ProxyHTMLEvents)] = value; } }

		// Determines whether to fix links in inline scripts, stylesheets, and scripting events.
		// ProxyHTMLExtended On|Off
		public string ProxyHTMLExtended { get { return this[nameof(ProxyHTMLExtended)]; } set { this[nameof(ProxyHTMLExtended)] = value; } }

		// Fixes for simple HTML errors.
		// ProxyHTMLFixups [lowercase] [dospath] [reset]
		public string ProxyHTMLFixups { get { return this[nameof(ProxyHTMLFixups)]; } set { this[nameof(ProxyHTMLFixups)] = value; } }

		// Enables per-request interpolation of ProxyHTMLURLMap rules.
		// ProxyHTMLInterp On|Off
		public string ProxyHTMLInterp { get { return this[nameof(ProxyHTMLInterp)]; } set { this[nameof(ProxyHTMLInterp)] = value; } }

		// Specify HTML elements that have URL attributes to be rewritten.
		// ProxyHTMLLinks element attribute [attribute2 ...]
		public string ProxyHTMLLinks { get { return this[nameof(ProxyHTMLLinks)]; } set { this[nameof(ProxyHTMLLinks)] = value; } }

		// Turns on or off extra pre-parsing of metadata in HTML <head> sections.
		// ProxyHTMLMeta On|Off
		public string ProxyHTMLMeta { get { return this[nameof(ProxyHTMLMeta)]; } set { this[nameof(ProxyHTMLMeta)] = value; } }

		// Determines whether to strip HTML comments.
		// ProxyHTMLStripComments On|Off
		public string ProxyHTMLStripComments { get { return this[nameof(ProxyHTMLStripComments)]; } set { this[nameof(ProxyHTMLStripComments)] = value; } }

		// Defines a rule to rewrite HTML links
		// ProxyHTMLURLMap from-pattern to-pattern [flags] [cond]
		public string ProxyHTMLURLMap { get { return this[nameof(ProxyHTMLURLMap)]; } set { this[nameof(ProxyHTMLURLMap)] = value; } }

		// Determine size of internal data throughput buffer
		// ProxyIOBufferSize bytes
		public string ProxyIOBufferSize { get { return this[nameof(ProxyIOBufferSize)]; } set { this[nameof(ProxyIOBufferSize)] = value; } }

		// Maximum number of proxies that a request can be forwarded through
		// ProxyMaxForwards number
		public string ProxyMaxForwards { get { return this[nameof(ProxyMaxForwards)]; } set { this[nameof(ProxyMaxForwards)] = value; } }

		// Maps remote servers into the local server URL-space
		// ProxyPass [path] !|url [key=value   [key=value ...]] [nocanon] [interpolate] [noquery]
		public string ProxyPass { get { return this[nameof(ProxyPass)]; } set { this[nameof(ProxyPass)] = value; } }

		// Inherit ProxyPass directives defined from the main server
		// ProxyPassInherit On|Off
		public string ProxyPassInherit { get { return this[nameof(ProxyPassInherit)]; } set { this[nameof(ProxyPassInherit)] = value; } }

		// Enable Environment Variable interpolation in Reverse Proxy configurations
		// ProxyPassInterpolateEnv On|Off
		public string ProxyPassInterpolateEnv { get { return this[nameof(ProxyPassInterpolateEnv)]; } set { this[nameof(ProxyPassInterpolateEnv)] = value; } }

		// Maps remote servers into the local server URL-space using regular expressions
		// ProxyPassMatch [regex] !|url [key=value 	[key=value ...]]
		public string ProxyPassMatch { get { return this[nameof(ProxyPassMatch)]; } set { this[nameof(ProxyPassMatch)] = value; } }

		// Adjusts the URL in HTTP response headers sent from a reverse proxied server
		// ProxyPassReverse [path] url [interpolate]
		public string ProxyPassReverse { get { return this[nameof(ProxyPassReverse)]; } set { this[nameof(ProxyPassReverse)] = value; } }

		// Adjusts the Domain string in Set-Cookie headers from a reverse-proxied server
		// ProxyPassReverseCookieDomain internal-domain public-domain [interpolate]
		public string ProxyPassReverseCookieDomain { get { return this[nameof(ProxyPassReverseCookieDomain)]; } set { this[nameof(ProxyPassReverseCookieDomain)] = value; } }

		// Adjusts the Path string in Set-Cookie headers from a reverse-proxied server
		// ProxyPassReverseCookiePath internal-path public-path [interpolate]
		public string ProxyPassReverseCookiePath { get { return this[nameof(ProxyPassReverseCookiePath)]; } set { this[nameof(ProxyPassReverseCookiePath)] = value; } }

		// Use incoming Host HTTP request header for proxy request
		// ProxyPreserveHost On|Off
		public string ProxyPreserveHost { get { return this[nameof(ProxyPreserveHost)]; } set { this[nameof(ProxyPreserveHost)] = value; } }

		// Network buffer size for proxied HTTP and FTP connections
		// ProxyReceiveBufferSize bytes
		public string ProxyReceiveBufferSize { get { return this[nameof(ProxyReceiveBufferSize)]; } set { this[nameof(ProxyReceiveBufferSize)] = value; } }

		// Remote proxy used to handle certain requests
		// ProxyRemote match remote-server [username:password]
		public string ProxyRemote { get { return this[nameof(ProxyRemote)]; } set { this[nameof(ProxyRemote)] = value; } }

		// Remote proxy used to handle requests matched by regular expressions
		// ProxyRemoteMatch regex remote-server
		public string ProxyRemoteMatch { get { return this[nameof(ProxyRemoteMatch)]; } set { this[nameof(ProxyRemoteMatch)] = value; } }

		// Enables forward (standard) proxy requests
		// ProxyRequests On|Off
		public string ProxyRequests { get { return this[nameof(ProxyRequests)]; } set { this[nameof(ProxyRequests)] = value; } }

		// Enable or disable internal redirect responses from the backend
		// ProxySCGIInternalRedirect On|Off|Headername
		public string ProxySCGIInternalRedirect { get { return this[nameof(ProxySCGIInternalRedirect)]; } set { this[nameof(ProxySCGIInternalRedirect)] = value; } }

		// Enable evaluation of X-Sendfile pseudo response header
		// ProxySCGISendfile On|Off|Headername
		public string ProxySCGISendfile { get { return this[nameof(ProxySCGISendfile)]; } set { this[nameof(ProxySCGISendfile)] = value; } }

		// Set various Proxy balancer or member parameters
		// ProxySet url key=value [key=value ...]
		public string ProxySet { get { return this[nameof(ProxySet)]; } set { this[nameof(ProxySet)] = value; } }

		// Set local IP address for outgoing proxy connections
		// ProxySourceAddress address
		public string ProxySourceAddress { get { return this[nameof(ProxySourceAddress)]; } set { this[nameof(ProxySourceAddress)] = value; } }

		// Show Proxy LoadBalancer status in mod_status
		// ProxyStatus Off|On|Full
		public string ProxyStatus { get { return this[nameof(ProxyStatus)]; } set { this[nameof(ProxyStatus)] = value; } }

		// Network timeout for proxied requests
		// ProxyTimeout seconds
		public string ProxyTimeout { get { return this[nameof(ProxyTimeout)]; } set { this[nameof(ProxyTimeout)] = value; } }

		// Information provided in the Via HTTP response header for proxied requests
		// ProxyVia On|Off|Full|Block
		public string ProxyVia { get { return this[nameof(ProxyVia)]; } set { this[nameof(ProxyVia)] = value; } }

		// Instructs this module to let mod_proxy_http handle the request
		// ProxyWebsocketFallbackToProxyHttp On|Off
		public string ProxyWebsocketFallbackToProxyHttp { get { return this[nameof(ProxyWebsocketFallbackToProxyHttp)]; } set { this[nameof(ProxyWebsocketFallbackToProxyHttp)] = value; } }

		// Controls whether the REDIRECT_URL environment variable is fully qualified
		// QualifyRedirectURL On|Off
		public string QualifyRedirectURL { get { return this[nameof(QualifyRedirectURL)]; } set { this[nameof(QualifyRedirectURL)] = value; } }

		// Size of the buffers used to read data
		// ReadBufferSize bytes
		public string ReadBufferSize { get { return this[nameof(ReadBufferSize)]; } set { this[nameof(ReadBufferSize)] = value; } }

		// Name of the file that will be inserted at the end of the index listing
		// ReadmeName filename
		public string ReadmeName { get { return this[nameof(ReadmeName)]; } set { this[nameof(ReadmeName)] = value; } }

		// TCP receive buffer size
		// ReceiveBufferSize bytes
		public string ReceiveBufferSize { get { return this[nameof(ReceiveBufferSize)]; } set { this[nameof(ReceiveBufferSize)] = value; } }

		// Sends an external redirect asking the client to fetch a different URL
		// Redirect [status] [URL-path] URL
		public string Redirect { get { return this[nameof(Redirect)]; } set { this[nameof(Redirect)] = value; } }

		// Sends an external redirect based on a regular expression match of the current URL
		// RedirectMatch [status] regex URL
		public string RedirectMatch { get { return this[nameof(RedirectMatch)]; } set { this[nameof(RedirectMatch)] = value; } }

		// Sends an external permanent redirect asking the client to fetch a different URL
		// RedirectPermanent URL-path URL
		public string RedirectPermanent { get { return this[nameof(RedirectPermanent)]; } set { this[nameof(RedirectPermanent)] = value; } }

		// Allows relative redirect targets.
		// RedirectRelative On|Off
		public string RedirectRelative { get { return this[nameof(RedirectRelative)]; } set { this[nameof(RedirectRelative)] = value; } }

		// Sends an external temporary redirect asking the client to fetch a different URL
		// RedirectTemp URL-path URL
		public string RedirectTemp { get { return this[nameof(RedirectTemp)]; } set { this[nameof(RedirectTemp)] = value; } }

		// TTL used for the connection pool with the Redis server(s)
		// RedisConnPoolTTL num[units]
		public string RedisConnPoolTTL { get { return this[nameof(RedisConnPoolTTL)]; } set { this[nameof(RedisConnPoolTTL)] = value; } }

		// R/W timeout used for the connection with the Redis server(s)
		// RedisTimeout num[units]
		public string RedisTimeout { get { return this[nameof(RedisTimeout)]; } set { this[nameof(RedisTimeout)] = value; } }

		// Reflect an input header to the output headers
		// ReflectorHeader inputheader [outputheader]
		public string ReflectorHeader { get { return this[nameof(ReflectorHeader)]; } set { this[nameof(ReflectorHeader)] = value; } }

		// Allow to configure global/default options for regexes
		// RegexDefaultOptions [none] [+|-]option [[+|-]option] ...
		public string RegexDefaultOptions { get { return this[nameof(RegexDefaultOptions)]; } set { this[nameof(RegexDefaultOptions)] = value; } }

		// Register non-standard HTTP methods
		// RegisterHttpMethod method [method [...]]
		public string RegisterHttpMethod { get { return this[nameof(RegisterHttpMethod)]; } set { this[nameof(RegisterHttpMethod)] = value; } }

		// Declare the header field which should be parsed for useragent IP addresses
		// RemoteIPHeader header-field
		public string RemoteIPHeader { get { return this[nameof(RemoteIPHeader)]; } set { this[nameof(RemoteIPHeader)] = value; } }

		// Declare client intranet IP addresses trusted to present the RemoteIPHeader value
		// RemoteIPInternalProxy proxy-ip|proxy-ip/subnet|hostname ...
		public string RemoteIPInternalProxy { get { return this[nameof(RemoteIPInternalProxy)]; } set { this[nameof(RemoteIPInternalProxy)] = value; } }

		// Declare client intranet IP addresses trusted to present the RemoteIPHeader value
		// RemoteIPInternalProxyList filename
		public string RemoteIPInternalProxyList { get { return this[nameof(RemoteIPInternalProxyList)]; } set { this[nameof(RemoteIPInternalProxyList)] = value; } }

		// Declare the header field which will record all intermediate IP addresses
		// RemoteIPProxiesHeader HeaderFieldName
		public string RemoteIPProxiesHeader { get { return this[nameof(RemoteIPProxiesHeader)]; } set { this[nameof(RemoteIPProxiesHeader)] = value; } }

		// Enable or disable PROXY protocol handling
		// RemoteIPProxyProtocol On|Off
		public string RemoteIPProxyProtocol { get { return this[nameof(RemoteIPProxyProtocol)]; } set { this[nameof(RemoteIPProxyProtocol)] = value; } }

		// Disable processing of PROXY header for certain hosts or networks
		// RemoteIPProxyProtocolExceptions host|range [host|range] [host|range]
		public string RemoteIPProxyProtocolExceptions { get { return this[nameof(RemoteIPProxyProtocolExceptions)]; } set { this[nameof(RemoteIPProxyProtocolExceptions)] = value; } }

		// Declare client intranet IP addresses trusted to present the RemoteIPHeader value
		// RemoteIPTrustedProxy proxy-ip|proxy-ip/subnet|hostname ...
		public string RemoteIPTrustedProxy { get { return this[nameof(RemoteIPTrustedProxy)]; } set { this[nameof(RemoteIPTrustedProxy)] = value; } }

		// Declare client intranet IP addresses trusted to present the RemoteIPHeader value
		// RemoteIPTrustedProxyList filename
		public string RemoteIPTrustedProxyList { get { return this[nameof(RemoteIPTrustedProxyList)]; } set { this[nameof(RemoteIPTrustedProxyList)] = value; } }

		// Removes any character set associations for a set of file extensions
		// RemoveCharset extension [extension] ...
		public string RemoveCharset { get { return this[nameof(RemoveCharset)]; } set { this[nameof(RemoveCharset)] = value; } }

		// Removes any content encoding associations for a set of file extensions
		// RemoveEncoding extension [extension] ...
		public string RemoveEncoding { get { return this[nameof(RemoveEncoding)]; } set { this[nameof(RemoveEncoding)] = value; } }

		// Removes any handler associations for a set of file extensions
		// RemoveHandler extension [extension] ...
		public string RemoveHandler { get { return this[nameof(RemoveHandler)]; } set { this[nameof(RemoveHandler)] = value; } }

		// Removes any input filter associations for a set of file extensions
		// RemoveInputFilter extension [extension] ...
		public string RemoveInputFilter { get { return this[nameof(RemoveInputFilter)]; } set { this[nameof(RemoveInputFilter)] = value; } }

		// Removes any language associations for a set of file extensions
		// RemoveLanguage extension [extension] ...
		public string RemoveLanguage { get { return this[nameof(RemoveLanguage)]; } set { this[nameof(RemoveLanguage)] = value; } }

		// Removes any output filter associations for a set of file extensions
		// RemoveOutputFilter extension [extension] ...
		public string RemoveOutputFilter { get { return this[nameof(RemoveOutputFilter)]; } set { this[nameof(RemoveOutputFilter)] = value; } }

		// Removes any content type associations for a set of file extensions
		// RemoveType extension [extension] ...
		public string RemoveType { get { return this[nameof(RemoveType)]; } set { this[nameof(RemoveType)] = value; } }

		// Configure HTTP request headers
		// RequestHeader add|append|edit|edit*|merge|set|setifempty|unset header [[expr=]value [replacement] [early|env=[!]varname|expr=expression]] 
		public string RequestHeader { get { return this[nameof(RequestHeader)]; } set { this[nameof(RequestHeader)] = value; } }

		// Set timeout values for completing the TLS handshake, receiving the request headers and/or body from client. 
		// RequestReadTimeout [handshake=timeout[-maxtimeout][,MinRate=rate] [header=timeout[-maxtimeout][,MinRate=rate] [body=timeout[-maxtimeout][,MinRate=rate] 
		public string RequestReadTimeout { get { return this[nameof(RequestReadTimeout)]; } set { this[nameof(RequestReadTimeout)] = value; } }

		// Tests whether an authenticated user is authorized by an authorization provider.
		// Require [not] entity-name     [entity-name] ...
		public string Require { get { return this[nameof(Require)]; } set { this[nameof(Require)] = value; } }

		// Sets the base URL for per-directory rewrites
		// RewriteBase URL-path
		public string RewriteBase { get { return this[nameof(RewriteBase)]; } set { this[nameof(RewriteBase)] = value; } }

		// Defines a condition under which rewriting will take place 
		//  RewriteCond TestString CondPattern [flags]
		public string RewriteCond { get { return this[nameof(RewriteCond)]; } set { this[nameof(RewriteCond)] = value; } }

		// Enables or disables runtime rewriting engine
		// RewriteEngine on|off
		public string RewriteEngine { get { return this[nameof(RewriteEngine)]; } set { this[nameof(RewriteEngine)] = value; } }

		// Defines a mapping function for key-lookup
		// RewriteMap MapName MapType:MapSource [MapTypeOptions]
		public string RewriteMap { get { return this[nameof(RewriteMap)]; } set { this[nameof(RewriteMap)] = value; } }

		// Sets some special options for the rewrite engine
		// RewriteOptions Options
		public string RewriteOptions { get { return this[nameof(RewriteOptions)]; } set { this[nameof(RewriteOptions)] = value; } }

		// Defines rules for the rewriting engine
		// RewriteRule       Pattern Substitution [flags]
		public string RewriteRule { get { return this[nameof(RewriteRule)]; } set { this[nameof(RewriteRule)] = value; } }

		// Limits the CPU consumption of processes launched by Apache httpd children
		// RLimitCPU seconds|max [seconds|max]
		public string RLimitCPU { get { return this[nameof(RLimitCPU)]; } set { this[nameof(RLimitCPU)] = value; } }

		// Limits the memory consumption of processes launched by Apache httpd children
		// RLimitMEM bytes|max [bytes|max]
		public string RLimitMEM { get { return this[nameof(RLimitMEM)]; } set { this[nameof(RLimitMEM)] = value; } }

		// Limits the number of processes that can be launched by processes launched by Apache httpd children
		// RLimitNPROC number|max [number|max]
		public string RLimitNPROC { get { return this[nameof(RLimitNPROC)]; } set { this[nameof(RLimitNPROC)] = value; } }

		// Interaction between host-level access control and user authentication
		// Satisfy Any|All
		public string Satisfy { get { return this[nameof(Satisfy)]; } set { this[nameof(Satisfy)] = value; } }

		// Location of the file used to store coordination data for the child processes
		// ScoreBoardFile file-path
		public string ScoreBoardFile { get { return this[nameof(ScoreBoardFile)]; } set { this[nameof(ScoreBoardFile)] = value; } }

		// Activates a CGI script for a particular request method.
		// Script method cgi-script
		public string Script { get { return this[nameof(Script)]; } set { this[nameof(Script)] = value; } }

		// Maps a URL to a filesystem location and designates the target as a CGI script
		// ScriptAlias [URL-path] file-path|directory-path
		public string ScriptAlias { get { return this[nameof(ScriptAlias)]; } set { this[nameof(ScriptAlias)] = value; } }

		// Maps a URL to a filesystem location using a regular expression and designates the target as a CGI script
		// ScriptAliasMatch regex file-path|directory-path
		public string ScriptAliasMatch { get { return this[nameof(ScriptAliasMatch)]; } set { this[nameof(ScriptAliasMatch)] = value; } }

		// Technique for locating the interpreter for CGI scripts
		// ScriptInterpreterSource Registry|Registry-Strict|Script
		public string ScriptInterpreterSource { get { return this[nameof(ScriptInterpreterSource)]; } set { this[nameof(ScriptInterpreterSource)] = value; } }

		// Location of the CGI script error logfile
		// ScriptLog file-path
		public string ScriptLog { get { return this[nameof(ScriptLog)]; } set { this[nameof(ScriptLog)] = value; } }

		// Maximum amount of PUT or POST requests that will be recorded in the scriptlog
		// ScriptLogBuffer bytes
		public string ScriptLogBuffer { get { return this[nameof(ScriptLogBuffer)]; } set { this[nameof(ScriptLogBuffer)] = value; } }

		// Size limit of the CGI script logfile
		// ScriptLogLength bytes
		public string ScriptLogLength { get { return this[nameof(ScriptLogLength)]; } set { this[nameof(ScriptLogLength)] = value; } }

		// The filename prefix of the socket to use for communication with the cgi daemon
		// ScriptSock file-path
		public string ScriptSock { get { return this[nameof(ScriptSock)]; } set { this[nameof(ScriptSock)] = value; } }

		// Enables SSL encryption for the specified port
		// SecureListen [IP-address:]portnumber Certificate-Name [MUTUAL]
		public string SecureListen { get { return this[nameof(SecureListen)]; } set { this[nameof(SecureListen)] = value; } }

		// Determine if mod_status displays the first 63 characters of a request or the last 63, assuming the request itself is greater than 63 chars.
		// SeeRequestTail On|Off
		public string SeeRequestTail { get { return this[nameof(SeeRequestTail)]; } set { this[nameof(SeeRequestTail)] = value; } }

		// TCP buffer size
		// SendBufferSize bytes
		public string SendBufferSize { get { return this[nameof(SendBufferSize)]; } set { this[nameof(SendBufferSize)] = value; } }

		// Email address that the server includes in error messages sent to the client
		// ServerAdmin email-address|URL
		public string ServerAdmin { get { return this[nameof(ServerAdmin)]; } set { this[nameof(ServerAdmin)] = value; } }

		// Alternate names for a host used when matching requests to name-virtual hosts
		// ServerAlias hostname [hostname] ...
		public string ServerAlias { get { return this[nameof(ServerAlias)]; } set { this[nameof(ServerAlias)] = value; } }

		// Upper limit on configurable number of processes
		// ServerLimit number
		public string ServerLimit { get { return this[nameof(ServerLimit)]; } set { this[nameof(ServerLimit)] = value; } }

		// Hostname and port that the server uses to identify itself
		// ServerName [scheme://]domain-name|ip-address[:port]
		public string ServerName { get { return this[nameof(ServerName)]; } set { this[nameof(ServerName)] = value; } }

		// Legacy URL pathname for a name-based virtual host that is accessed by an incompatible browser
		// ServerPath URL-path
		public string ServerPath { get { return this[nameof(ServerPath)]; } set { this[nameof(ServerPath)] = value; } }

		// Base directory for the server installation
		// ServerRoot directory-path
		public string ServerRoot { get { return this[nameof(ServerRoot)]; } set { this[nameof(ServerRoot)] = value; } }

		// Configures the footer on server-generated documents
		// ServerSignature On|Off|EMail
		public string ServerSignature { get { return this[nameof(ServerSignature)]; } set { this[nameof(ServerSignature)] = value; } }

		// Configures the Server HTTP response header
		// ServerTokens Major|Minor|Min[imal]|Prod[uctOnly]|OS|Full
		public string ServerTokens { get { return this[nameof(ServerTokens)]; } set { this[nameof(ServerTokens)] = value; } }

		// Enables a session for the current directory or location
		// Session On|Off
		public string Session { get { return this[nameof(Session)]; } set { this[nameof(Session)] = value; } }

		// Name and attributes for the RFC2109 cookie storing the session
		// SessionCookieName name attributes
		public string SessionCookieName { get { return this[nameof(SessionCookieName)]; } set { this[nameof(SessionCookieName)] = value; } }

		// Name and attributes for the RFC2965 cookie storing the session
		// SessionCookieName2 name attributes
		public string SessionCookieName2 { get { return this[nameof(SessionCookieName2)]; } set { this[nameof(SessionCookieName2)] = value; } }

		// Control for whether session cookies should be removed from incoming HTTP headers
		// SessionCookieRemove On|Off
		public string SessionCookieRemove { get { return this[nameof(SessionCookieRemove)]; } set { this[nameof(SessionCookieRemove)] = value; } }

		// The crypto cipher to be used to encrypt the session
		// SessionCryptoCipher name
		public string SessionCryptoCipher { get { return this[nameof(SessionCryptoCipher)]; } set { this[nameof(SessionCryptoCipher)] = value; } }

		// The crypto driver to be used to encrypt the session
		// SessionCryptoDriver name [param[=value]]
		public string SessionCryptoDriver { get { return this[nameof(SessionCryptoDriver)]; } set { this[nameof(SessionCryptoDriver)] = value; } }

		// The key used to encrypt the session
		// SessionCryptoPassphrase secret [ secret ... ] 
		public string SessionCryptoPassphrase { get { return this[nameof(SessionCryptoPassphrase)]; } set { this[nameof(SessionCryptoPassphrase)] = value; } }

		// File containing keys used to encrypt the session
		// SessionCryptoPassphraseFile filename
		public string SessionCryptoPassphraseFile { get { return this[nameof(SessionCryptoPassphraseFile)]; } set { this[nameof(SessionCryptoPassphraseFile)] = value; } }

		// Name and attributes for the RFC2109 cookie storing the session ID
		// SessionDBDCookieName name attributes
		public string SessionDBDCookieName { get { return this[nameof(SessionDBDCookieName)]; } set { this[nameof(SessionDBDCookieName)] = value; } }

		// Name and attributes for the RFC2965 cookie storing the session ID
		// SessionDBDCookieName2 name attributes
		public string SessionDBDCookieName2 { get { return this[nameof(SessionDBDCookieName2)]; } set { this[nameof(SessionDBDCookieName2)] = value; } }

		// Control for whether session ID cookies should be removed from incoming HTTP headers
		// SessionDBDCookieRemove On|Off
		public string SessionDBDCookieRemove { get { return this[nameof(SessionDBDCookieRemove)]; } set { this[nameof(SessionDBDCookieRemove)] = value; } }

		// The SQL query to use to remove sessions from the database
		// SessionDBDDeleteLabel label
		public string SessionDBDDeleteLabel { get { return this[nameof(SessionDBDDeleteLabel)]; } set { this[nameof(SessionDBDDeleteLabel)] = value; } }

		// The SQL query to use to insert sessions into the database
		// SessionDBDInsertLabel label
		public string SessionDBDInsertLabel { get { return this[nameof(SessionDBDInsertLabel)]; } set { this[nameof(SessionDBDInsertLabel)] = value; } }

		// Enable a per user session
		// SessionDBDPerUser On|Off
		public string SessionDBDPerUser { get { return this[nameof(SessionDBDPerUser)]; } set { this[nameof(SessionDBDPerUser)] = value; } }

		// The SQL query to use to select sessions from the database
		// SessionDBDSelectLabel label
		public string SessionDBDSelectLabel { get { return this[nameof(SessionDBDSelectLabel)]; } set { this[nameof(SessionDBDSelectLabel)] = value; } }

		// The SQL query to use to update existing sessions in the database
		// SessionDBDUpdateLabel label
		public string SessionDBDUpdateLabel { get { return this[nameof(SessionDBDUpdateLabel)]; } set { this[nameof(SessionDBDUpdateLabel)] = value; } }

		// Control whether the contents of the session are written to the HTTP_SESSION environment variable
		// SessionEnv On|Off
		public string SessionEnv { get { return this[nameof(SessionEnv)]; } set { this[nameof(SessionEnv)] = value; } }

		// Define URL prefixes for which a session is ignored
		// SessionExclude path
		public string SessionExclude { get { return this[nameof(SessionExclude)]; } set { this[nameof(SessionExclude)] = value; } }

		// Define the number of seconds a session's expiry may change without the session being updated
		// SessionExpiryUpdateInterval interval
		public string SessionExpiryUpdateInterval { get { return this[nameof(SessionExpiryUpdateInterval)]; } set { this[nameof(SessionExpiryUpdateInterval)] = value; } }

		// Import session updates from a given HTTP response header
		// SessionHeader header
		public string SessionHeader { get { return this[nameof(SessionHeader)]; } set { this[nameof(SessionHeader)] = value; } }

		// Define URL prefixes for which a session is valid
		// SessionInclude path
		public string SessionInclude { get { return this[nameof(SessionInclude)]; } set { this[nameof(SessionInclude)] = value; } }

		// Define a maximum age in seconds for a session
		// SessionMaxAge maxage
		public string SessionMaxAge { get { return this[nameof(SessionMaxAge)]; } set { this[nameof(SessionMaxAge)] = value; } }

		// Sets environment variables
		// SetEnv env-variable [value]
		public string SetEnv { get { return this[nameof(SetEnv)]; } set { this[nameof(SetEnv)] = value; } }

		// Sets environment variables based on attributes of the request 
		// SetEnvIf attribute regex [!]env-variable[=value] [[!]env-variable[=value]] ...
		public string SetEnvIf { get { return this[nameof(SetEnvIf)]; } set { this[nameof(SetEnvIf)] = value; } }

		// Sets environment variables based on an ap_expr expression
		// SetEnvIfExpr expr [!]env-variable[=value] [[!]env-variable[=value]] ...
		public string SetEnvIfExpr { get { return this[nameof(SetEnvIfExpr)]; } set { this[nameof(SetEnvIfExpr)] = value; } }

		// Sets environment variables based on attributes of the request without respect to case
		// SetEnvIfNoCase attribute regex [!]env-variable[=value] [[!]env-variable[=value]] ...
		public string SetEnvIfNoCase { get { return this[nameof(SetEnvIfNoCase)]; } set { this[nameof(SetEnvIfNoCase)] = value; } }

		// Forces all matching files to be processed by a handler
		// SetHandler handler-name|none|expression
		public string SetHandler { get { return this[nameof(SetHandler)]; } set { this[nameof(SetHandler)] = value; } }

		// Sets the filters that will process client requests and POST input
		// SetInputFilter filter[;filter...]
		public string SetInputFilter { get { return this[nameof(SetInputFilter)]; } set { this[nameof(SetInputFilter)] = value; } }

		// Sets the filters that will process responses from the server
		// SetOutputFilter filter[;filter...]
		public string SetOutputFilter { get { return this[nameof(SetOutputFilter)]; } set { this[nameof(SetOutputFilter)] = value; } }

		// String that ends an include element
		// SSIEndTag tag
		public string SSIEndTag { get { return this[nameof(SSIEndTag)]; } set { this[nameof(SSIEndTag)] = value; } }

		// Error message displayed when there is an SSI error
		// SSIErrorMsg message
		public string SSIErrorMsg { get { return this[nameof(SSIErrorMsg)]; } set { this[nameof(SSIErrorMsg)] = value; } }

		// Controls whether ETags are generated by the server.
		// SSIETag on|off
		public string SSIETag { get { return this[nameof(SSIETag)]; } set { this[nameof(SSIETag)] = value; } }

		// Controls whether Last-Modified headers are generated by the server.
		// SSILastModified on|off
		public string SSILastModified { get { return this[nameof(SSILastModified)]; } set { this[nameof(SSILastModified)] = value; } }

		// Enable compatibility mode for conditional expressions.
		// SSILegacyExprParser on|off
		public string SSILegacyExprParser { get { return this[nameof(SSILegacyExprParser)]; } set { this[nameof(SSILegacyExprParser)] = value; } }

		// String that starts an include element
		// SSIStartTag tag
		public string SSIStartTag { get { return this[nameof(SSIStartTag)]; } set { this[nameof(SSIStartTag)] = value; } }

		// Configures the format in which date strings are displayed
		// SSITimeFormat formatstring
		public string SSITimeFormat { get { return this[nameof(SSITimeFormat)]; } set { this[nameof(SSITimeFormat)] = value; } }

		// String displayed when an unset variable is echoed
		// SSIUndefinedEcho string
		public string SSIUndefinedEcho { get { return this[nameof(SSIUndefinedEcho)]; } set { this[nameof(SSIUndefinedEcho)] = value; } }

		// File of concatenated PEM-encoded CA Certificates for Client Auth
		// SSLCACertificateFile file-path
		public string SSLCACertificateFile { get { return this[nameof(SSLCACertificateFile)]; } set { this[nameof(SSLCACertificateFile)] = value; } }

		// Directory of PEM-encoded CA Certificates for Client Auth
		// SSLCACertificatePath directory-path
		public string SSLCACertificatePath { get { return this[nameof(SSLCACertificatePath)]; } set { this[nameof(SSLCACertificatePath)] = value; } }

		// File of concatenated PEM-encoded CA Certificates for defining acceptable CA names
		// SSLCADNRequestFile file-path
		public string SSLCADNRequestFile { get { return this[nameof(SSLCADNRequestFile)]; } set { this[nameof(SSLCADNRequestFile)] = value; } }

		// Directory of PEM-encoded CA Certificates for defining acceptable CA names
		// SSLCADNRequestPath directory-path
		public string SSLCADNRequestPath { get { return this[nameof(SSLCADNRequestPath)]; } set { this[nameof(SSLCADNRequestPath)] = value; } }

		// Enable CRL-based revocation checking
		// SSLCARevocationCheck chain|leaf|none [flags ...]
		public string SSLCARevocationCheck { get { return this[nameof(SSLCARevocationCheck)]; } set { this[nameof(SSLCARevocationCheck)] = value; } }

		// File of concatenated PEM-encoded CA CRLs for Client Auth
		// SSLCARevocationFile file-path
		public string SSLCARevocationFile { get { return this[nameof(SSLCARevocationFile)]; } set { this[nameof(SSLCARevocationFile)] = value; } }

		// Directory of PEM-encoded CA CRLs for Client Auth
		// SSLCARevocationPath directory-path
		public string SSLCARevocationPath { get { return this[nameof(SSLCARevocationPath)]; } set { this[nameof(SSLCARevocationPath)] = value; } }

		// File of PEM-encoded Server CA Certificates
		// SSLCertificateChainFile file-path
		public string SSLCertificateChainFile { get { return this[nameof(SSLCertificateChainFile)]; } set { this[nameof(SSLCertificateChainFile)] = value; } }

		// Server PEM-encoded X.509 certificate data file or token identifier
		// SSLCertificateFile file-path|certid
		public string SSLCertificateFile { get { return this[nameof(SSLCertificateFile)]; } set { this[nameof(SSLCertificateFile)] = value; } }

		// Server PEM-encoded private key file
		// SSLCertificateKeyFile file-path|keyid
		public string SSLCertificateKeyFile { get { return this[nameof(SSLCertificateKeyFile)]; } set { this[nameof(SSLCertificateKeyFile)] = value; } }

		// Cipher Suite available for negotiation in SSL handshake
		// SSLCipherSuite [protocol] cipher-spec
		public string SSLCipherSuite { get { return this[nameof(SSLCipherSuite)]; } set { this[nameof(SSLCipherSuite)] = value; } }

		// Enable compression on the SSL level
		// SSLCompression on|off
		public string SSLCompression { get { return this[nameof(SSLCompression)]; } set { this[nameof(SSLCompression)] = value; } }

		// Enable use of a cryptographic hardware accelerator
		// SSLCryptoDevice engine
		public string SSLCryptoDevice { get { return this[nameof(SSLCryptoDevice)]; } set { this[nameof(SSLCryptoDevice)] = value; } }

		// SSL Engine Operation Switch
		// SSLEngine on|off|optional
		public string SSLEngine { get { return this[nameof(SSLEngine)]; } set { this[nameof(SSLEngine)] = value; } }

		// SSL FIPS mode Switch
		// SSLFIPS on|off
		public string SSLFIPS { get { return this[nameof(SSLFIPS)]; } set { this[nameof(SSLFIPS)] = value; } }

		// Option to prefer the server's cipher preference order
		// SSLHonorCipherOrder on|off
		public string SSLHonorCipherOrder { get { return this[nameof(SSLHonorCipherOrder)]; } set { this[nameof(SSLHonorCipherOrder)] = value; } }

		// Option to enable support for insecure renegotiation
		// SSLInsecureRenegotiation on|off
		public string SSLInsecureRenegotiation { get { return this[nameof(SSLInsecureRenegotiation)]; } set { this[nameof(SSLInsecureRenegotiation)] = value; } }

		// Set the default responder URI for OCSP validation
		// SSLOCSPDefaultResponder uri
		public string SSLOCSPDefaultResponder { get { return this[nameof(SSLOCSPDefaultResponder)]; } set { this[nameof(SSLOCSPDefaultResponder)] = value; } }

		// Enable OCSP validation of the client certificate chain
		// SSLOCSPEnable on|leaf|off
		public string SSLOCSPEnable { get { return this[nameof(SSLOCSPEnable)]; } set { this[nameof(SSLOCSPEnable)] = value; } }

		// skip the OCSP responder certificates verification
		// SSLOCSPNoverify on|off
		public string SSLOCSPNoverify { get { return this[nameof(SSLOCSPNoverify)]; } set { this[nameof(SSLOCSPNoverify)] = value; } }

		// Force use of the default responder URI for OCSP validation
		// SSLOCSPOverrideResponder on|off
		public string SSLOCSPOverrideResponder { get { return this[nameof(SSLOCSPOverrideResponder)]; } set { this[nameof(SSLOCSPOverrideResponder)] = value; } }

		// Proxy URL to use for OCSP requests
		// SSLOCSPProxyURL url
		public string SSLOCSPProxyURL { get { return this[nameof(SSLOCSPProxyURL)]; } set { this[nameof(SSLOCSPProxyURL)] = value; } }

		// Set of trusted PEM encoded OCSP responder certificates
		// SSLOCSPResponderCertificateFile file
		public string SSLOCSPResponderCertificateFile { get { return this[nameof(SSLOCSPResponderCertificateFile)]; } set { this[nameof(SSLOCSPResponderCertificateFile)] = value; } }

		// Timeout for OCSP queries
		// SSLOCSPResponderTimeout seconds
		public string SSLOCSPResponderTimeout { get { return this[nameof(SSLOCSPResponderTimeout)]; } set { this[nameof(SSLOCSPResponderTimeout)] = value; } }

		// Maximum allowable age for OCSP responses
		// SSLOCSPResponseMaxAge seconds
		public string SSLOCSPResponseMaxAge { get { return this[nameof(SSLOCSPResponseMaxAge)]; } set { this[nameof(SSLOCSPResponseMaxAge)] = value; } }

		// Maximum allowable time skew for OCSP response validation
		// SSLOCSPResponseTimeSkew seconds
		public string SSLOCSPResponseTimeSkew { get { return this[nameof(SSLOCSPResponseTimeSkew)]; } set { this[nameof(SSLOCSPResponseTimeSkew)] = value; } }

		// Use a nonce within OCSP queries
		// SSLOCSPUseRequestNonce on|off
		public string SSLOCSPUseRequestNonce { get { return this[nameof(SSLOCSPUseRequestNonce)]; } set { this[nameof(SSLOCSPUseRequestNonce)] = value; } }

		// Configure OpenSSL parameters through its SSL_CONF API
		// SSLOpenSSLConfCmd command-name command-value
		public string SSLOpenSSLConfCmd { get { return this[nameof(SSLOpenSSLConfCmd)]; } set { this[nameof(SSLOpenSSLConfCmd)] = value; } }

		// Configure various SSL engine run-time options
		// SSLOptions [+|-]option ...
		public string SSLOptions { get { return this[nameof(SSLOptions)]; } set { this[nameof(SSLOptions)] = value; } }

		// Type of pass phrase dialog for encrypted private keys
		// SSLPassPhraseDialog type
		public string SSLPassPhraseDialog { get { return this[nameof(SSLPassPhraseDialog)]; } set { this[nameof(SSLPassPhraseDialog)] = value; } }

		// Configure usable SSL/TLS protocol versions
		// SSLProtocol [+|-]protocol ...
		public string SSLProtocol { get { return this[nameof(SSLProtocol)]; } set { this[nameof(SSLProtocol)] = value; } }

		// File of concatenated PEM-encoded CA Certificates for Remote Server Auth
		// SSLProxyCACertificateFile file-path
		public string SSLProxyCACertificateFile { get { return this[nameof(SSLProxyCACertificateFile)]; } set { this[nameof(SSLProxyCACertificateFile)] = value; } }

		// Directory of PEM-encoded CA Certificates for Remote Server Auth
		// SSLProxyCACertificatePath directory-path
		public string SSLProxyCACertificatePath { get { return this[nameof(SSLProxyCACertificatePath)]; } set { this[nameof(SSLProxyCACertificatePath)] = value; } }

		// Enable CRL-based revocation checking for Remote Server Auth
		// SSLProxyCARevocationCheck chain|leaf|none
		public string SSLProxyCARevocationCheck { get { return this[nameof(SSLProxyCARevocationCheck)]; } set { this[nameof(SSLProxyCARevocationCheck)] = value; } }

		// File of concatenated PEM-encoded CA CRLs for Remote Server Auth
		// SSLProxyCARevocationFile file-path
		public string SSLProxyCARevocationFile { get { return this[nameof(SSLProxyCARevocationFile)]; } set { this[nameof(SSLProxyCARevocationFile)] = value; } }

		// Directory of PEM-encoded CA CRLs for Remote Server Auth
		// SSLProxyCARevocationPath directory-path
		public string SSLProxyCARevocationPath { get { return this[nameof(SSLProxyCARevocationPath)]; } set { this[nameof(SSLProxyCARevocationPath)] = value; } }

		// Whether to check the remote server certificate's CN field 
		// SSLProxyCheckPeerCN on|off
		public string SSLProxyCheckPeerCN { get { return this[nameof(SSLProxyCheckPeerCN)]; } set { this[nameof(SSLProxyCheckPeerCN)] = value; } }

		// Whether to check if remote server certificate is expired 
		// SSLProxyCheckPeerExpire on|off
		public string SSLProxyCheckPeerExpire { get { return this[nameof(SSLProxyCheckPeerExpire)]; } set { this[nameof(SSLProxyCheckPeerExpire)] = value; } }

		// Configure host name checking for remote server certificates 
		// SSLProxyCheckPeerName on|off
		public string SSLProxyCheckPeerName { get { return this[nameof(SSLProxyCheckPeerName)]; } set { this[nameof(SSLProxyCheckPeerName)] = value; } }

		// Cipher Suite available for negotiation in SSL proxy handshake
		// SSLProxyCipherSuite [protocol] cipher-spec
		public string SSLProxyCipherSuite { get { return this[nameof(SSLProxyCipherSuite)]; } set { this[nameof(SSLProxyCipherSuite)] = value; } }

		// SSL Proxy Engine Operation Switch
		// SSLProxyEngine on|off
		public string SSLProxyEngine { get { return this[nameof(SSLProxyEngine)]; } set { this[nameof(SSLProxyEngine)] = value; } }

		// File of concatenated PEM-encoded CA certificates to be used by the proxy for choosing a certificate
		// SSLProxyMachineCertificateChainFile filename
		public string SSLProxyMachineCertificateChainFile { get { return this[nameof(SSLProxyMachineCertificateChainFile)]; } set { this[nameof(SSLProxyMachineCertificateChainFile)] = value; } }

		// File of concatenated PEM-encoded client certificates and keys to be used by the proxy
		// SSLProxyMachineCertificateFile filename
		public string SSLProxyMachineCertificateFile { get { return this[nameof(SSLProxyMachineCertificateFile)]; } set { this[nameof(SSLProxyMachineCertificateFile)] = value; } }

		// Directory of PEM-encoded client certificates and keys to be used by the proxy
		// SSLProxyMachineCertificatePath directory
		public string SSLProxyMachineCertificatePath { get { return this[nameof(SSLProxyMachineCertificatePath)]; } set { this[nameof(SSLProxyMachineCertificatePath)] = value; } }

		// Configure usable SSL protocol flavors for proxy usage
		// SSLProxyProtocol [+|-]protocol ...
		public string SSLProxyProtocol { get { return this[nameof(SSLProxyProtocol)]; } set { this[nameof(SSLProxyProtocol)] = value; } }

		// Type of remote server Certificate verification
		// SSLProxyVerify level
		public string SSLProxyVerify { get { return this[nameof(SSLProxyVerify)]; } set { this[nameof(SSLProxyVerify)] = value; } }

		// Maximum depth of CA Certificates in Remote Server Certificate verification
		// SSLProxyVerifyDepth number
		public string SSLProxyVerifyDepth { get { return this[nameof(SSLProxyVerifyDepth)]; } set { this[nameof(SSLProxyVerifyDepth)] = value; } }

		// Pseudo Random Number Generator (PRNG) seeding source
		// SSLRandomSeed context source [bytes]
		public string SSLRandomSeed { get { return this[nameof(SSLRandomSeed)]; } set { this[nameof(SSLRandomSeed)] = value; } }

		// Set the size for the SSL renegotiation buffer
		// SSLRenegBufferSize bytes
		public string SSLRenegBufferSize { get { return this[nameof(SSLRenegBufferSize)]; } set { this[nameof(SSLRenegBufferSize)] = value; } }

		// Allow access only when an arbitrarily complex boolean expression is true
		// SSLRequire expression
		public string SSLRequire { get { return this[nameof(SSLRequire)]; } set { this[nameof(SSLRequire)] = value; } }

		// Deny access when SSL is not used for the HTTP request
		// SSLRequireSSL
		public string SSLRequireSSL { get { return this[nameof(SSLRequireSSL)]; } set { this[nameof(SSLRequireSSL)] = value; } }

		// Type of the global/inter-process SSL Session Cache
		// SSLSessionCache type
		public string SSLSessionCache { get { return this[nameof(SSLSessionCache)]; } set { this[nameof(SSLSessionCache)] = value; } }

		// Number of seconds before an SSL session expires in the Session Cache
		// SSLSessionCacheTimeout seconds
		public string SSLSessionCacheTimeout { get { return this[nameof(SSLSessionCacheTimeout)]; } set { this[nameof(SSLSessionCacheTimeout)] = value; } }

		// Persistent encryption/decryption key for TLS session tickets
		// SSLSessionTicketKeyFile file-path
		public string SSLSessionTicketKeyFile { get { return this[nameof(SSLSessionTicketKeyFile)]; } set { this[nameof(SSLSessionTicketKeyFile)] = value; } }

		// Enable or disable use of TLS session tickets
		// SSLSessionTickets on|off
		public string SSLSessionTickets { get { return this[nameof(SSLSessionTickets)]; } set { this[nameof(SSLSessionTickets)] = value; } }

		// SRP unknown user seed
		// SSLSRPUnknownUserSeed secret-string
		public string SSLSRPUnknownUserSeed { get { return this[nameof(SSLSRPUnknownUserSeed)]; } set { this[nameof(SSLSRPUnknownUserSeed)] = value; } }

		// Path to SRP verifier file
		// SSLSRPVerifierFile file-path
		public string SSLSRPVerifierFile { get { return this[nameof(SSLSRPVerifierFile)]; } set { this[nameof(SSLSRPVerifierFile)] = value; } }

		// Configures the OCSP stapling cache
		// SSLStaplingCache type
		public string SSLStaplingCache { get { return this[nameof(SSLStaplingCache)]; } set { this[nameof(SSLStaplingCache)] = value; } }

		// Number of seconds before expiring invalid responses in the OCSP stapling cache
		// SSLStaplingErrorCacheTimeout seconds
		public string SSLStaplingErrorCacheTimeout { get { return this[nameof(SSLStaplingErrorCacheTimeout)]; } set { this[nameof(SSLStaplingErrorCacheTimeout)] = value; } }

		// Synthesize "tryLater" responses for failed OCSP stapling queries
		// SSLStaplingFakeTryLater on|off
		public string SSLStaplingFakeTryLater { get { return this[nameof(SSLStaplingFakeTryLater)]; } set { this[nameof(SSLStaplingFakeTryLater)] = value; } }

		// Override the OCSP responder URI specified in the certificate's AIA extension
		// SSLStaplingForceURL uri
		public string SSLStaplingForceURL { get { return this[nameof(SSLStaplingForceURL)]; } set { this[nameof(SSLStaplingForceURL)] = value; } }

		// Timeout for OCSP stapling queries
		// SSLStaplingResponderTimeout seconds
		public string SSLStaplingResponderTimeout { get { return this[nameof(SSLStaplingResponderTimeout)]; } set { this[nameof(SSLStaplingResponderTimeout)] = value; } }

		// Maximum allowable age for OCSP stapling responses
		// SSLStaplingResponseMaxAge seconds
		public string SSLStaplingResponseMaxAge { get { return this[nameof(SSLStaplingResponseMaxAge)]; } set { this[nameof(SSLStaplingResponseMaxAge)] = value; } }

		// Maximum allowable time skew for OCSP stapling response validation
		// SSLStaplingResponseTimeSkew seconds
		public string SSLStaplingResponseTimeSkew { get { return this[nameof(SSLStaplingResponseTimeSkew)]; } set { this[nameof(SSLStaplingResponseTimeSkew)] = value; } }

		// Pass stapling related OCSP errors on to client
		// SSLStaplingReturnResponderErrors on|off
		public string SSLStaplingReturnResponderErrors { get { return this[nameof(SSLStaplingReturnResponderErrors)]; } set { this[nameof(SSLStaplingReturnResponderErrors)] = value; } }

		// Number of seconds before expiring responses in the OCSP stapling cache
		// SSLStaplingStandardCacheTimeout seconds
		public string SSLStaplingStandardCacheTimeout { get { return this[nameof(SSLStaplingStandardCacheTimeout)]; } set { this[nameof(SSLStaplingStandardCacheTimeout)] = value; } }

		// Whether to allow non-SNI clients to access a name-based virtual host. 
		// SSLStrictSNIVHostCheck on|off
		public string SSLStrictSNIVHostCheck { get { return this[nameof(SSLStrictSNIVHostCheck)]; } set { this[nameof(SSLStrictSNIVHostCheck)] = value; } }

		// Variable name to determine user name
		// SSLUserName varname
		public string SSLUserName { get { return this[nameof(SSLUserName)]; } set { this[nameof(SSLUserName)] = value; } }

		// Enable stapling of OCSP responses in the TLS handshake
		// SSLUseStapling on|off
		public string SSLUseStapling { get { return this[nameof(SSLUseStapling)]; } set { this[nameof(SSLUseStapling)] = value; } }

		// Type of Client Certificate verification
		// SSLVerifyClient level
		public string SSLVerifyClient { get { return this[nameof(SSLVerifyClient)]; } set { this[nameof(SSLVerifyClient)] = value; } }

		// Maximum depth of CA Certificates in Client Certificate verification
		// SSLVerifyDepth number
		public string SSLVerifyDepth { get { return this[nameof(SSLVerifyDepth)]; } set { this[nameof(SSLVerifyDepth)] = value; } }

		// Number of child server processes created at startup
		// StartServers number
		public string StartServers { get { return this[nameof(StartServers)]; } set { this[nameof(StartServers)] = value; } }

		// Number of threads created on startup
		// StartThreads number
		public string StartThreads { get { return this[nameof(StartThreads)]; } set { this[nameof(StartThreads)] = value; } }

		// Controls whether the server requires the requested hostname be listed enumerated in the virtual host handling the request
		// StrictHostCheck ON|OFF
		public string StrictHostCheck { get { return this[nameof(StrictHostCheck)]; } set { this[nameof(StrictHostCheck)] = value; } }

		// Pattern to filter the response content
		// Substitute s/pattern/substitution/[infq]
		public string Substitute { get { return this[nameof(Substitute)]; } set { this[nameof(Substitute)] = value; } }

		// Change the merge order of inherited patterns
		// SubstituteInheritBefore on|off
		public string SubstituteInheritBefore { get { return this[nameof(SubstituteInheritBefore)]; } set { this[nameof(SubstituteInheritBefore)] = value; } }

		// Set the maximum line size
		// SubstituteMaxLineLength bytes(b|B|k|K|m|M|g|G)
		public string SubstituteMaxLineLength { get { return this[nameof(SubstituteMaxLineLength)]; } set { this[nameof(SubstituteMaxLineLength)] = value; } }

		// Enable or disable the suEXEC feature
		// Suexec On|Off
		public string Suexec { get { return this[nameof(Suexec)]; } set { this[nameof(Suexec)] = value; } }

		// User and group for CGI programs to run as
		// SuexecUserGroup User Group
		public string SuexecUserGroup { get { return this[nameof(SuexecUserGroup)]; } set { this[nameof(SuexecUserGroup)] = value; } }

		// Sets the upper limit on the configurable number of threads per child process
		// ThreadLimit number
		public string ThreadLimit { get { return this[nameof(ThreadLimit)]; } set { this[nameof(ThreadLimit)] = value; } }

		// Number of threads created by each child process
		// ThreadsPerChild number
		public string ThreadsPerChild { get { return this[nameof(ThreadsPerChild)]; } set { this[nameof(ThreadsPerChild)] = value; } }

		// The size in bytes of the stack used by threads handling client connections
		// ThreadStackSize size
		public string ThreadStackSize { get { return this[nameof(ThreadStackSize)]; } set { this[nameof(ThreadStackSize)] = value; } }

		// Amount of time the server will wait for certain events before failing a request
		// TimeOut seconds
		public string TimeOut { get { return this[nameof(TimeOut)]; } set { this[nameof(TimeOut)] = value; } }

		// adds a certificate and key (PEM encoded) to a server/virtual host.
		// TLSCertificate cert_file [key_file]
		public string TLSCertificate { get { return this[nameof(TLSCertificate)]; } set { this[nameof(TLSCertificate)] = value; } }

		// defines ciphers that are preferred.
		// TLSCiphersPrefer cipher(-list)
		public string TLSCiphersPrefer { get { return this[nameof(TLSCiphersPrefer)]; } set { this[nameof(TLSCiphersPrefer)] = value; } }

		// defines ciphers that are not to be used.
		// TLSCiphersSuppress cipher(-list)
		public string TLSCiphersSuppress { get { return this[nameof(TLSCiphersSuppress)]; } set { this[nameof(TLSCiphersSuppress)] = value; } }

		// defines on which address+port the module shall handle incoming connections.
		// TLSEngine [address:]port
		public string TLSEngine { get { return this[nameof(TLSEngine)]; } set { this[nameof(TLSEngine)] = value; } }

		// determines if the order of ciphers supported by the client is honored
		// TLSHonorClientOrder on|off
		public string TLSHonorClientOrder { get { return this[nameof(TLSHonorClientOrder)]; } set { this[nameof(TLSHonorClientOrder)] = value; } }

		// enables SSL variables for requests.
		// TLSOptions [+|-]option
		public string TLSOptions { get { return this[nameof(TLSOptions)]; } set { this[nameof(TLSOptions)] = value; } }

		// specifies the minimum version of the TLS protocol to use.
		// TLSProtocol version+
		public string TLSProtocol { get { return this[nameof(TLSProtocol)]; } set { this[nameof(TLSProtocol)] = value; } }

		// sets the root certificates to validate the backend server with.
		// TLSProxyCA file.pem
		public string TLSProxyCA { get { return this[nameof(TLSProxyCA)]; } set { this[nameof(TLSProxyCA)] = value; } }

		// defines ciphers that are preferred for a proxy connection.
		// TLSProxyCiphersPrefer cipher(-list)
		public string TLSProxyCiphersPrefer { get { return this[nameof(TLSProxyCiphersPrefer)]; } set { this[nameof(TLSProxyCiphersPrefer)] = value; } }

		// defines ciphers that are not to be used for a proxy connection.
		// TLSProxyCiphersSuppress cipher(-list)
		public string TLSProxyCiphersSuppress { get { return this[nameof(TLSProxyCiphersSuppress)]; } set { this[nameof(TLSProxyCiphersSuppress)] = value; } }

		// enables TLS for backend connections.
		// TLSProxyEngine on|off
		public string TLSProxyEngine { get { return this[nameof(TLSProxyEngine)]; } set { this[nameof(TLSProxyEngine)] = value; } }

		// adds a certificate and key file (PEM encoded) to a proxy setup.
		// TLSProxyMachineCertificate cert_file [key_file]
		public string TLSProxyMachineCertificate { get { return this[nameof(TLSProxyMachineCertificate)]; } set { this[nameof(TLSProxyMachineCertificate)] = value; } }

		// specifies the minimum version of the TLS protocol to use in proxy connections.
		// TLSProxyProtocol version+
		public string TLSProxyProtocol { get { return this[nameof(TLSProxyProtocol)]; } set { this[nameof(TLSProxyProtocol)] = value; } }

		// specifies the cache for TLS session resumption.
		// TLSSessionCache cache-spec
		public string TLSSessionCache { get { return this[nameof(TLSSessionCache)]; } set { this[nameof(TLSSessionCache)] = value; } }

		// enforces exact matches of client server indicators (SNI) against host names.
		// TLSStrictSNI on|off
		public string TLSStrictSNI { get { return this[nameof(TLSStrictSNI)]; } set { this[nameof(TLSStrictSNI)] = value; } }

		// Determines the behavior on TRACE requests
		// TraceEnable [on|off|extended]
		public string TraceEnable { get { return this[nameof(TraceEnable)]; } set { this[nameof(TraceEnable)] = value; } }

		// Specify location of a log file
		// TransferLog file|pipe
		public string TransferLog { get { return this[nameof(TransferLog)]; } set { this[nameof(TransferLog)] = value; } }

		// The location of the mime.types file
		// TypesConfig file-path
		public string TypesConfig { get { return this[nameof(TypesConfig)]; } set { this[nameof(TypesConfig)] = value; } }

		// Undefine the existence of a variable
		// UnDefine parameter-name
		public string UnDefine { get { return this[nameof(UnDefine)]; } set { this[nameof(UnDefine)] = value; } }

		// Undefine a macro
		// UndefMacro name
		public string UndefMacro { get { return this[nameof(UndefMacro)]; } set { this[nameof(UndefMacro)] = value; } }

		// Removes variables from the environment
		// UnsetEnv env-variable [env-variable] ...
		public string UnsetEnv { get { return this[nameof(UnsetEnv)]; } set { this[nameof(UnsetEnv)] = value; } }

		// Use a macro
		// Use name [value1 ... valueN] 
		public string Use { get { return this[nameof(Use)]; } set { this[nameof(Use)] = value; } }

		// Configures how the server determines its own name and port
		// UseCanonicalName On|Off|DNS
		public string UseCanonicalName { get { return this[nameof(UseCanonicalName)]; } set { this[nameof(UseCanonicalName)] = value; } }

		// Configures how the server determines its own port
		// UseCanonicalPhysicalPort On|Off
		public string UseCanonicalPhysicalPort { get { return this[nameof(UseCanonicalPhysicalPort)]; } set { this[nameof(UseCanonicalPhysicalPort)] = value; } }

		// The userid under which the server will answer requests
		// User unix-userid
		public string User { get { return this[nameof(User)]; } set { this[nameof(User)] = value; } }

		// Location of the user-specific directories
		// UserDir directory-filename [directory-filename] ...
		public string UserDir { get { return this[nameof(UserDir)]; } set { this[nameof(UserDir)] = value; } }

		// Determines whether the virtualhost can run subprocesses, and the privileges available to subprocesses.
		// VHostCGIMode On|Off|Secure
		public string VHostCGIMode { get { return this[nameof(VHostCGIMode)]; } set { this[nameof(VHostCGIMode)] = value; } }

		// Assign arbitrary privileges to subprocesses created by a virtual host.
		// VHostCGIPrivs [+-]?privilege-name [[+-]?privilege-name] ...
		public string VHostCGIPrivs { get { return this[nameof(VHostCGIPrivs)]; } set { this[nameof(VHostCGIPrivs)] = value; } }

		// Sets the Group ID under which a virtual host runs.
		// VHostGroup unix-groupid
		public string VHostGroup { get { return this[nameof(VHostGroup)]; } set { this[nameof(VHostGroup)] = value; } }

		// Assign arbitrary privileges to a virtual host.
		// VHostPrivs [+-]?privilege-name [[+-]?privilege-name] ...
		public string VHostPrivs { get { return this[nameof(VHostPrivs)]; } set { this[nameof(VHostPrivs)] = value; } }

		// Determines whether the server runs with enhanced security for the virtualhost.
		// VHostSecure On|Off
		public string VHostSecure { get { return this[nameof(VHostSecure)]; } set { this[nameof(VHostSecure)] = value; } }

		// Sets the User ID under which a virtual host runs.
		// VHostUser unix-userid
		public string VHostUser { get { return this[nameof(VHostUser)]; } set { this[nameof(VHostUser)] = value; } }

		// Dynamically configure the location of the document root for a given virtual host
		// VirtualDocumentRoot interpolated-directory|none
		public string VirtualDocumentRoot { get { return this[nameof(VirtualDocumentRoot)]; } set { this[nameof(VirtualDocumentRoot)] = value; } }

		// Dynamically configure the location of the document root for a given virtual host
		// VirtualDocumentRootIP interpolated-directory|none
		public string VirtualDocumentRootIP { get { return this[nameof(VirtualDocumentRootIP)]; } set { this[nameof(VirtualDocumentRootIP)] = value; } }

		// Dynamically configure the location of the CGI directory for a given virtual host
		// VirtualScriptAlias interpolated-directory|none
		public string VirtualScriptAlias { get { return this[nameof(VirtualScriptAlias)]; } set { this[nameof(VirtualScriptAlias)] = value; } }

		// Dynamically configure the location of the CGI directory for a given virtual host
		// VirtualScriptAliasIP interpolated-directory|none
		public string VirtualScriptAliasIP { get { return this[nameof(VirtualScriptAliasIP)]; } set { this[nameof(VirtualScriptAliasIP)] = value; } }

		// Watchdog interval in seconds
		// WatchdogInterval time-interval[s]
		public string WatchdogInterval { get { return this[nameof(WatchdogInterval)]; } set { this[nameof(WatchdogInterval)] = value; } }

		// Parse SSI directives in files with the execute bit set
		// XBitHack on|off|full
		public string XBitHack { get { return this[nameof(XBitHack)]; } set { this[nameof(XBitHack)] = value; } }

		// Recognise Aliases for encoding values
		// xml2EncAlias charset alias [alias ...]
		public string xml2EncAlias { get { return this[nameof(xml2EncAlias)]; } set { this[nameof(xml2EncAlias)] = value; } }

		// Sets a default encoding to assume when absolutely no information can be automatically detected
		// xml2EncDefault name
		public string xml2EncDefault { get { return this[nameof(xml2EncDefault)]; } set { this[nameof(xml2EncDefault)] = value; } }

		// Advise the parser to skip leading junk.
		// xml2StartParse element [element ...]
		public string xml2StartParse { get { return this[nameof(xml2StartParse)]; } set { this[nameof(xml2StartParse)] = value; } }

	}
}
