// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Reflection;

namespace CertEnrollInterop
{
    [ComImport, Guid("728AB307-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual), CoClass(typeof(object))]
    public interface CCspInformation : ICspInformation
    {
    }

    [ComImport, Guid("728AB308-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, CoClass(typeof(object)), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface CCspInformations : ICspInformations, IEnumerable
    {
    }

    public enum CERTENROLL_OBJECTID
    {
        XCN_OID_ANSI_X942 = 0x35,
        XCN_OID_ANSI_X942_DH = 0x36,
        XCN_OID_ANY_APPLICATION_POLICY = 0xd8,
        XCN_OID_ANY_CERT_POLICY = 180,
        XCN_OID_APPLICATION_CERT_POLICIES = 0xe5,
        XCN_OID_APPLICATION_POLICY_CONSTRAINTS = 0xe7,
        XCN_OID_APPLICATION_POLICY_MAPPINGS = 230,
        XCN_OID_ARCHIVED_KEY_ATTR = 0xe8,
        XCN_OID_ARCHIVED_KEY_CERT_HASH = 0xeb,
        XCN_OID_AUTHORITY_INFO_ACCESS = 0xcc,
        XCN_OID_AUTHORITY_KEY_IDENTIFIER = 0xa9,
        XCN_OID_AUTHORITY_KEY_IDENTIFIER2 = 0xb5,
        XCN_OID_AUTHORITY_REVOCATION_LIST = 0x9c,
        XCN_OID_AUTO_ENROLL_CTL_USAGE = 0xd9,
        XCN_OID_BACKGROUND_OTHER_LOGOTYPE = 0x147,
        XCN_OID_BASIC_CONSTRAINTS = 0xaf,
        XCN_OID_BASIC_CONSTRAINTS2 = 0xb2,
        XCN_OID_BIOMETRIC_EXT = 0xcd,
        XCN_OID_BUSINESS_CATEGORY = 0x85,
        XCN_OID_CA_CERTIFICATE = 0x9b,
        XCN_OID_CERT_EXTENSIONS = 0xcf,
        XCN_OID_CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID = 0x153,
        XCN_OID_CERT_KEY_IDENTIFIER_PROP_ID = 0x152,
        XCN_OID_CERT_MANIFOLD = 0xdb,
        XCN_OID_CERT_MD5_HASH_PROP_ID = 0x155,
        XCN_OID_CERT_POLICIES = 0xb3,
        XCN_OID_CERT_POLICIES_95 = 0xab,
        XCN_OID_CERT_POLICIES_95_QUALIFIER1 = 0x119,
        XCN_OID_CERT_PROP_ID_PREFIX = 0x151,
        XCN_OID_CERT_SUBJECT_NAME_MD5_HASH_PROP_ID = 340,
        XCN_OID_CERTIFICATE_REVOCATION_LIST = 0x9d,
        XCN_OID_CERTIFICATE_TEMPLATE = 0xe2,
        XCN_OID_CERTSRV_CA_VERSION = 220,
        XCN_OID_CERTSRV_CROSSCA_VERSION = 240,
        XCN_OID_CERTSRV_PREVIOUS_CERT_HASH = 0xdd,
        XCN_OID_CMC = 0x130,
        XCN_OID_CMC_ADD_ATTRIBUTES = 0x145,
        XCN_OID_CMC_ADD_EXTENSIONS = 0x138,
        XCN_OID_CMC_DATA_RETURN = 0x134,
        XCN_OID_CMC_DECRYPTED_POP = 0x13a,
        XCN_OID_CMC_ENCRYPTED_POP = 0x139,
        XCN_OID_CMC_GET_CERT = 0x13c,
        XCN_OID_CMC_GET_CRL = 0x13d,
        XCN_OID_CMC_ID_CONFIRM_CERT_ACCEPTANCE = 0x144,
        XCN_OID_CMC_ID_POP_LINK_RANDOM = 0x142,
        XCN_OID_CMC_ID_POP_LINK_WITNESS = 0x143,
        XCN_OID_CMC_IDENTIFICATION = 0x132,
        XCN_OID_CMC_IDENTITY_PROOF = 0x133,
        XCN_OID_CMC_LRA_POP_WITNESS = 0x13b,
        XCN_OID_CMC_QUERY_PENDING = 0x141,
        XCN_OID_CMC_RECIPIENT_NONCE = 0x137,
        XCN_OID_CMC_REG_INFO = 0x13f,
        XCN_OID_CMC_RESPONSE_INFO = 320,
        XCN_OID_CMC_REVOKE_REQUEST = 0x13e,
        XCN_OID_CMC_SENDER_NONCE = 310,
        XCN_OID_CMC_STATUS_INFO = 0x131,
        XCN_OID_CMC_TRANSACTION_ID = 0x135,
        XCN_OID_COMMON_NAME = 0x79,
        XCN_OID_COUNTRY_NAME = 0x7c,
        XCN_OID_CRL_DIST_POINTS = 0xbb,
        XCN_OID_CRL_NEXT_PUBLISH = 0xdf,
        XCN_OID_CRL_NUMBER = 0xbd,
        XCN_OID_CRL_REASON_CODE = 0xb9,
        XCN_OID_CRL_SELF_CDP = 0xe9,
        XCN_OID_CRL_VIRTUAL_BASE = 0xde,
        XCN_OID_CROSS_CERT_DIST_POINTS = 210,
        XCN_OID_CROSS_CERTIFICATE_PAIR = 0x9e,
        XCN_OID_CT_PKI_DATA = 0x12d,
        XCN_OID_CT_PKI_RESPONSE = 0x12e,
        XCN_OID_CTL = 0xd3,
        XCN_OID_DELTA_CRL_INDICATOR = 190,
        XCN_OID_DESCRIPTION = 0x83,
        XCN_OID_DESTINATION_INDICATOR = 0x91,
        XCN_OID_DEVICE_SERIAL_NUMBER = 0x7b,
        XCN_OID_DN_QUALIFIER = 0xa1,
        XCN_OID_DOMAIN_COMPONENT = 0xa2,
        XCN_OID_DRM = 0x111,
        XCN_OID_DRM_INDIVIDUALIZATION = 0x112,
        XCN_OID_DS = 0x3a,
        XCN_OID_DS_EMAIL_REPLICATION = 0xed,
        XCN_OID_DSALG = 0x3b,
        XCN_OID_DSALG_CRPT = 60,
        XCN_OID_DSALG_HASH = 0x3d,
        XCN_OID_DSALG_RSA = 0x3f,
        XCN_OID_DSALG_SIGN = 0x3e,
        XCN_OID_ECC_PUBLIC_KEY = 0x15d,
        XCN_OID_ECDSA_SHA1 = 0x162,
        XCN_OID_ECDSA_SPECIFIED = 0x162,
        XCN_OID_EFS_RECOVERY = 260,
        XCN_OID_EMBEDDED_NT_CRYPTO = 0x108,
        XCN_OID_ENCRYPTED_KEY_HASH = 0xef,
        XCN_OID_ENHANCED_KEY_USAGE = 0xbc,
        XCN_OID_ENROLL_CERTTYPE_EXTENSION = 0xda,
        XCN_OID_ENROLLMENT_AGENT = 0xc9,
        XCN_OID_ENROLLMENT_CSP_PROVIDER = 0xc7,
        XCN_OID_ENROLLMENT_NAME_VALUE_PAIR = 0xc6,
        XCN_OID_ENTERPRISE_OID_ROOT = 0xe3,
        XCN_OID_FACSIMILE_TELEPHONE_NUMBER = 0x8d,
        XCN_OID_FRESHEST_CRL = 0xc0,
        XCN_OID_GIVEN_NAME = 0x9f,
        XCN_OID_INFOSEC = 0x63,
        XCN_OID_INFOSEC_mosaicConfidentiality = 0x67,
        XCN_OID_INFOSEC_mosaicIntegrity = 0x69,
        XCN_OID_INFOSEC_mosaicKeyManagement = 0x6d,
        XCN_OID_INFOSEC_mosaicKMandSig = 0x6f,
        XCN_OID_INFOSEC_mosaicKMandUpdSig = 0x77,
        XCN_OID_INFOSEC_mosaicSignature = 0x65,
        XCN_OID_INFOSEC_mosaicTokenProtection = 0x6b,
        XCN_OID_INFOSEC_mosaicUpdatedInteg = 120,
        XCN_OID_INFOSEC_mosaicUpdatedSig = 0x76,
        XCN_OID_INFOSEC_sdnsConfidentiality = 0x66,
        XCN_OID_INFOSEC_sdnsIntegrity = 0x68,
        XCN_OID_INFOSEC_sdnsKeyManagement = 0x6c,
        XCN_OID_INFOSEC_sdnsKMandSig = 110,
        XCN_OID_INFOSEC_sdnsSignature = 100,
        XCN_OID_INFOSEC_sdnsTokenProtection = 0x6a,
        XCN_OID_INFOSEC_SuiteAConfidentiality = 0x71,
        XCN_OID_INFOSEC_SuiteAIntegrity = 0x72,
        XCN_OID_INFOSEC_SuiteAKeyManagement = 0x74,
        XCN_OID_INFOSEC_SuiteAKMandSig = 0x75,
        XCN_OID_INFOSEC_SuiteASignature = 0x70,
        XCN_OID_INFOSEC_SuiteATokenProtection = 0x73,
        XCN_OID_INITIALS = 160,
        XCN_OID_INTERNATIONAL_ISDN_NUMBER = 0x8f,
        XCN_OID_IPSEC_KP_IKE_INTERMEDIATE = 0xfe,
        XCN_OID_ISSUED_CERT_HASH = 0xec,
        XCN_OID_ISSUER_ALT_NAME = 0xae,
        XCN_OID_ISSUER_ALT_NAME2 = 0xb8,
        XCN_OID_ISSUING_DIST_POINT = 0xbf,
        XCN_OID_KEY_ATTRIBUTES = 170,
        XCN_OID_KEY_USAGE = 0xb0,
        XCN_OID_KEY_USAGE_RESTRICTION = 0xac,
        XCN_OID_KEYID_RDN = 0xa8,
        XCN_OID_KP_CA_EXCHANGE = 0xe0,
        XCN_OID_KP_CSP_SIGNATURE = 0x110,
        XCN_OID_KP_CTL_USAGE_SIGNING = 0xff,
        XCN_OID_KP_DOCUMENT_SIGNING = 0x10c,
        XCN_OID_KP_EFS = 0x103,
        XCN_OID_KP_KEY_RECOVERY = 0x10b,
        XCN_OID_KP_KEY_RECOVERY_AGENT = 0xe1,
        XCN_OID_KP_LIFETIME_SIGNING = 0x10d,
        XCN_OID_KP_MOBILE_DEVICE_SOFTWARE = 270,
        XCN_OID_KP_QUALIFIED_SUBORDINATION = 0x10a,
        XCN_OID_KP_SMART_DISPLAY = 0x10f,
        XCN_OID_KP_SMARTCARD_LOGON = 0x115,
        XCN_OID_KP_TIME_STAMP_SIGNING = 0x100,
        XCN_OID_LEGACY_POLICY_MAPPINGS = 0xc3,
        XCN_OID_LICENSE_SERVER = 0x114,
        XCN_OID_LICENSES = 0x113,
        XCN_OID_LOCAL_MACHINE_KEYSET = 0xa6,
        XCN_OID_LOCALITY_NAME = 0x7d,
        XCN_OID_LOGOTYPE_EXT = 0xce,
        XCN_OID_LOYALTY_OTHER_LOGOTYPE = 0x146,
        XCN_OID_MEMBER = 0x95,
        XCN_OID_NAME_CONSTRAINTS = 0xc1,
        XCN_OID_NETSCAPE = 0x121,
        XCN_OID_NETSCAPE_BASE_URL = 0x124,
        XCN_OID_NETSCAPE_CA_POLICY_URL = 0x128,
        XCN_OID_NETSCAPE_CA_REVOCATION_URL = 0x126,
        XCN_OID_NETSCAPE_CERT_EXTENSION = 290,
        XCN_OID_NETSCAPE_CERT_RENEWAL_URL = 0x127,
        XCN_OID_NETSCAPE_CERT_SEQUENCE = 300,
        XCN_OID_NETSCAPE_CERT_TYPE = 0x123,
        XCN_OID_NETSCAPE_COMMENT = 0x12a,
        XCN_OID_NETSCAPE_DATA_TYPE = 0x12b,
        XCN_OID_NETSCAPE_REVOCATION_URL = 0x125,
        XCN_OID_NETSCAPE_SSL_SERVER_NAME = 0x129,
        XCN_OID_NEXT_UPDATE_LOCATION = 0xd0,
        XCN_OID_NIST_sha256 = 0x159,
        XCN_OID_NIST_sha384 = 0x15a,
        XCN_OID_NIST_sha512 = 0x15b,
        XCN_OID_NONE = 0,
        XCN_OID_NT_PRINCIPAL_NAME = 0xd6,
        XCN_OID_NT5_CRYPTO = 0x106,
        XCN_OID_NTDS_REPLICATION = 0xf1,
        XCN_OID_OEM_WHQL_CRYPTO = 0x107,
        XCN_OID_OIW = 0x40,
        XCN_OID_OIWDIR = 0x5d,
        XCN_OID_OIWDIR_CRPT = 0x5e,
        XCN_OID_OIWDIR_HASH = 0x5f,
        XCN_OID_OIWDIR_md2 = 0x61,
        XCN_OID_OIWDIR_md2RSA = 0x62,
        XCN_OID_OIWDIR_SIGN = 0x60,
        XCN_OID_OIWSEC = 0x41,
        XCN_OID_OIWSEC_desCBC = 70,
        XCN_OID_OIWSEC_desCFB = 0x48,
        XCN_OID_OIWSEC_desECB = 0x45,
        XCN_OID_OIWSEC_desEDE = 80,
        XCN_OID_OIWSEC_desMAC = 0x49,
        XCN_OID_OIWSEC_desOFB = 0x47,
        XCN_OID_OIWSEC_dhCommMod = 0x4f,
        XCN_OID_OIWSEC_dsa = 0x4b,
        XCN_OID_OIWSEC_dsaComm = 0x53,
        XCN_OID_OIWSEC_dsaCommSHA = 0x54,
        XCN_OID_OIWSEC_dsaCommSHA1 = 0x5b,
        XCN_OID_OIWSEC_dsaSHA1 = 90,
        XCN_OID_OIWSEC_keyHashSeal = 0x56,
        XCN_OID_OIWSEC_md2RSASign = 0x57,
        XCN_OID_OIWSEC_md4RSA = 0x42,
        XCN_OID_OIWSEC_md4RSA2 = 0x44,
        XCN_OID_OIWSEC_md5RSA = 0x43,
        XCN_OID_OIWSEC_md5RSASign = 0x58,
        XCN_OID_OIWSEC_mdc2 = 0x52,
        XCN_OID_OIWSEC_mdc2RSA = 0x4d,
        XCN_OID_OIWSEC_rsaSign = 0x4a,
        XCN_OID_OIWSEC_rsaXchg = 0x55,
        XCN_OID_OIWSEC_sha = 0x51,
        XCN_OID_OIWSEC_sha1 = 0x59,
        XCN_OID_OIWSEC_sha1RSASign = 0x5c,
        XCN_OID_OIWSEC_shaDSA = 0x4c,
        XCN_OID_OIWSEC_shaRSA = 0x4e,
        XCN_OID_ORGANIZATION_NAME = 0x80,
        XCN_OID_ORGANIZATIONAL_UNIT_NAME = 0x81,
        XCN_OID_OS_VERSION = 200,
        XCN_OID_OWNER = 150,
        XCN_OID_PHYSICAL_DELIVERY_OFFICE_NAME = 0x89,
        XCN_OID_PKCS = 2,
        XCN_OID_PKCS_1 = 5,
        XCN_OID_PKCS_10 = 14,
        XCN_OID_PKCS_12 = 15,
        XCN_OID_PKCS_12_EXTENDED_ATTRIBUTES = 0xa7,
        XCN_OID_PKCS_12_FRIENDLY_NAME_ATTR = 0xa3,
        XCN_OID_PKCS_12_KEY_PROVIDER_NAME_ATTR = 0xa5,
        XCN_OID_PKCS_12_LOCAL_KEY_ID = 0xa4,
        XCN_OID_PKCS_2 = 6,
        XCN_OID_PKCS_3 = 7,
        XCN_OID_PKCS_4 = 8,
        XCN_OID_PKCS_5 = 9,
        XCN_OID_PKCS_6 = 10,
        XCN_OID_PKCS_7 = 11,
        XCN_OID_PKCS_7_DATA = 0x149,
        XCN_OID_PKCS_7_DIGESTED = 0x14d,
        XCN_OID_PKCS_7_ENCRYPTED = 0x14e,
        XCN_OID_PKCS_7_ENVELOPED = 0x14b,
        XCN_OID_PKCS_7_SIGNED = 330,
        XCN_OID_PKCS_7_SIGNEDANDENVELOPED = 0x14c,
        XCN_OID_PKCS_8 = 12,
        XCN_OID_PKCS_9 = 13,
        XCN_OID_PKCS_9_CONTENT_TYPE = 0x14f,
        XCN_OID_PKCS_9_MESSAGE_DIGEST = 0x150,
        XCN_OID_PKIX = 0xca,
        XCN_OID_PKIX_ACC_DESCR = 0x11a,
        XCN_OID_PKIX_CA_ISSUERS = 0x11c,
        XCN_OID_PKIX_KP = 0xf3,
        XCN_OID_PKIX_KP_CLIENT_AUTH = 0xf5,
        XCN_OID_PKIX_KP_CODE_SIGNING = 0xf6,
        XCN_OID_PKIX_KP_EMAIL_PROTECTION = 0xf7,
        XCN_OID_PKIX_KP_IPSEC_END_SYSTEM = 0xf8,
        XCN_OID_PKIX_KP_IPSEC_TUNNEL = 0xf9,
        XCN_OID_PKIX_KP_IPSEC_USER = 250,
        XCN_OID_PKIX_KP_OCSP_SIGNING = 0xfc,
        XCN_OID_PKIX_KP_SERVER_AUTH = 0xf4,
        XCN_OID_PKIX_KP_TIMESTAMP_SIGNING = 0xfb,
        XCN_OID_PKIX_NO_SIGNATURE = 0x12f,
        XCN_OID_PKIX_OCSP = 0x11b,
        XCN_OID_PKIX_OCSP_BASIC_SIGNED_RESPONSE = 0x148,
        XCN_OID_PKIX_OCSP_NOCHECK = 0xfd,
        XCN_OID_PKIX_PE = 0xcb,
        XCN_OID_PKIX_POLICY_QUALIFIER_CPS = 0x117,
        XCN_OID_PKIX_POLICY_QUALIFIER_USERNOTICE = 280,
        XCN_OID_POLICY_CONSTRAINTS = 0xc4,
        XCN_OID_POLICY_MAPPINGS = 0xc2,
        XCN_OID_POST_OFFICE_BOX = 0x88,
        XCN_OID_POSTAL_ADDRESS = 0x86,
        XCN_OID_POSTAL_CODE = 0x87,
        XCN_OID_PREFERRED_DELIVERY_METHOD = 0x92,
        XCN_OID_PRESENTATION_ADDRESS = 0x93,
        XCN_OID_PRIVATEKEY_USAGE_PERIOD = 0xb1,
        XCN_OID_PRODUCT_UPDATE = 0xd7,
        XCN_OID_RDN_DUMMY_SIGNER = 0xe4,
        XCN_OID_REASON_CODE_HOLD = 0xba,
        XCN_OID_REGISTERED_ADDRESS = 0x90,
        XCN_OID_REMOVE_CERTIFICATE = 0xd1,
        XCN_OID_RENEWAL_CERTIFICATE = 0xc5,
        XCN_OID_REQUEST_CLIENT_INFO = 0xee,
        XCN_OID_REQUIRE_CERT_CHAIN_POLICY = 0xea,
        XCN_OID_ROLE_OCCUPANT = 0x97,
        XCN_OID_ROOT_LIST_SIGNER = 0x109,
        XCN_OID_RSA = 1,
        XCN_OID_RSA_certExtensions = 0x27,
        XCN_OID_RSA_challengePwd = 0x24,
        XCN_OID_RSA_contentType = 0x20,
        XCN_OID_RSA_counterSign = 0x23,
        XCN_OID_RSA_data = 0x17,
        XCN_OID_RSA_DES_EDE3_CBC = 0x33,
        XCN_OID_RSA_DH = 0x16,
        XCN_OID_RSA_digestedData = 0x1b,
        XCN_OID_RSA_emailAddr = 30,
        XCN_OID_RSA_ENCRYPT = 4,
        XCN_OID_RSA_encryptedData = 0x1d,
        XCN_OID_RSA_envelopedData = 0x19,
        XCN_OID_RSA_extCertAttrs = 0x26,
        XCN_OID_RSA_HASH = 3,
        XCN_OID_RSA_hashedData = 0x1c,
        XCN_OID_RSA_MD2 = 0x2e,
        XCN_OID_RSA_MD2RSA = 0x11,
        XCN_OID_RSA_MD4 = 0x2f,
        XCN_OID_RSA_MD4RSA = 0x12,
        XCN_OID_RSA_MD5 = 0x30,
        XCN_OID_RSA_MD5RSA = 0x13,
        XCN_OID_RSA_messageDigest = 0x21,
        XCN_OID_RSA_MGF1 = 0x15c,
        XCN_OID_RSA_preferSignedData = 0x29,
        XCN_OID_RSA_RC2CBC = 0x31,
        XCN_OID_RSA_RC4 = 50,
        XCN_OID_RSA_RC5_CBCPad = 0x34,
        XCN_OID_RSA_RSA = 0x10,
        XCN_OID_RSA_SETOAEP_RSA = 0x15,
        XCN_OID_RSA_SHA1RSA = 20,
        XCN_OID_RSA_SHA256RSA = 0x156,
        XCN_OID_RSA_SHA384RSA = 0x157,
        XCN_OID_RSA_SHA512RSA = 0x158,
        XCN_OID_RSA_signedData = 0x18,
        XCN_OID_RSA_signEnvData = 0x1a,
        XCN_OID_RSA_signingTime = 0x22,
        XCN_OID_RSA_SMIMEalg = 0x2a,
        XCN_OID_RSA_SMIMEalgCMS3DESwrap = 0x2c,
        XCN_OID_RSA_SMIMEalgCMSRC2wrap = 0x2d,
        XCN_OID_RSA_SMIMEalgESDH = 0x2b,
        XCN_OID_RSA_SMIMECapabilities = 40,
        XCN_OID_RSA_SSA_PSS = 0x161,
        XCN_OID_RSA_unstructAddr = 0x25,
        XCN_OID_RSA_unstructName = 0x1f,
        XCN_OID_SEARCH_GUIDE = 0x84,
        XCN_OID_SEE_ALSO = 0x98,
        XCN_OID_SERIALIZED = 0xd5,
        XCN_OID_SERVER_GATED_CRYPTO = 0x101,
        XCN_OID_SGC_NETSCAPE = 0x102,
        XCN_OID_SORTED_CTL = 0xd4,
        XCN_OID_STATE_OR_PROVINCE_NAME = 0x7e,
        XCN_OID_STREET_ADDRESS = 0x7f,
        XCN_OID_SUBJECT_ALT_NAME = 0xad,
        XCN_OID_SUBJECT_ALT_NAME2 = 0xb7,
        XCN_OID_SUBJECT_DIR_ATTRS = 0xf2,
        XCN_OID_SUBJECT_KEY_IDENTIFIER = 0xb6,
        XCN_OID_SUPPORTED_APPLICATION_CONTEXT = 0x94,
        XCN_OID_SUR_NAME = 0x7a,
        XCN_OID_TELEPHONE_NUMBER = 0x8a,
        XCN_OID_TELETEXT_TERMINAL_IDENTIFIER = 140,
        XCN_OID_TELEX_NUMBER = 0x8b,
        XCN_OID_TITLE = 130,
        XCN_OID_USER_CERTIFICATE = 0x9a,
        XCN_OID_USER_PASSWORD = 0x99,
        XCN_OID_VERISIGN_BITSTRING_6_13 = 0x11f,
        XCN_OID_VERISIGN_ISS_STRONG_CRYPTO = 0x120,
        XCN_OID_VERISIGN_ONSITE_JURISDICTION_HASH = 0x11e,
        XCN_OID_VERISIGN_PRIVATE_6_9 = 0x11d,
        XCN_OID_WHQL_CRYPTO = 0x105,
        XCN_OID_X21_ADDRESS = 0x8e,
        XCN_OID_X957 = 0x37,
        XCN_OID_X957_DSA = 0x38,
        XCN_OID_X957_SHA1DSA = 0x39,
        XCN_OID_YESNO_TRUST_ATTR = 0x116
    }

    [ComImport, CoClass(typeof(object)), CompilerGenerated, Guid("728AB300-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface CObjectId : IObjectId
    {
    }

    [ComImport, CompilerGenerated, CoClass(typeof(object)), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB301-217D-11DA-B2A4-000E7BBB2B09")]
    public interface CObjectIds : IObjectIds, IEnumerable
    {
    }

    [ComImport, CoClass(typeof(object)), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB303-217D-11DA-B2A4-000E7BBB2B09")]
    public interface CX500DistinguishedName : IX500DistinguishedName
    {
    }

    [ComImport, CoClass(typeof(object)), Guid("728AB35B-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface CX509CertificateRequestPkcs10 : IX509CertificateRequestPkcs10V2, IX509CertificateRequestPkcs10, IX509CertificateRequest
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), CompilerGenerated, Guid("728AB350-217D-11DA-B2A4-000E7BBB2B09"), CoClass(typeof(object))]
    public interface CX509Enrollment : IX509Enrollment2, IX509Enrollment
    {
    }

    [ComImport, Guid("728AB30D-217D-11DA-B2A4-000E7BBB2B09"), CoClass(typeof(object)), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface CX509Extension : IX509Extension
    {
    }

    [ComImport, Guid("728AB310-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, CoClass(typeof(object)), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface CX509ExtensionEnhancedKeyUsage : IX509ExtensionEnhancedKeyUsage, IX509Extension
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB30F-217D-11DA-B2A4-000E7BBB2B09"), CoClass(typeof(object)), CompilerGenerated]
    public interface CX509ExtensionKeyUsage : IX509ExtensionKeyUsage, IX509Extension
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB30E-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, CoClass(typeof(object))]
    public interface CX509Extensions : IX509Extensions, IEnumerable
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), CompilerGenerated, Guid("728AB30C-217D-11DA-B2A4-000E7BBB2B09"), CoClass(typeof(object))]
    public interface CX509PrivateKey : IX509PrivateKey
    {
    }

    //[InterfaceType(ComInterfaceType.InterfaceIsDual)("728ab348-217d-11da-b2a4-000e7bbb2b09", "CERTENROLLLib.EncodingType"), CompilerGenerated]
    public enum EncodingType
    {
        XCN_CRYPT_STRING_ANY = 7,
        XCN_CRYPT_STRING_BASE64 = 1,
        XCN_CRYPT_STRING_BASE64_ANY = 6,
        XCN_CRYPT_STRING_BASE64HEADER = 0,
        XCN_CRYPT_STRING_BASE64REQUESTHEADER = 3,
        XCN_CRYPT_STRING_BASE64X509CRLHEADER = 9,
        XCN_CRYPT_STRING_BINARY = 2,
        XCN_CRYPT_STRING_HASHDATA = 0x10000000,
        XCN_CRYPT_STRING_HEX = 4,
        XCN_CRYPT_STRING_HEX_ANY = 8,
        XCN_CRYPT_STRING_HEXADDR = 10,
        XCN_CRYPT_STRING_HEXASCII = 5,
        XCN_CRYPT_STRING_HEXASCIIADDR = 11,
        XCN_CRYPT_STRING_HEXRAW = 12,
        XCN_CRYPT_STRING_NOCR = -2147483648,
        XCN_CRYPT_STRING_NOCRLF = 0x40000000,
        XCN_CRYPT_STRING_STRICT = 0x20000000
    }

    [ComImport, Guid("728AB307-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual), CompilerGenerated]
    public interface ICspInformation
    {
        [DispId(0x60020000)]
        void InitializeFromName([In, MarshalAs(UnmanagedType.BStr)] string strName);
    }

    [ComImport, Guid("728AB308-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, DefaultMember("ItemByIndex"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICspInformations : IEnumerable
    {
        void _VtblGap1_3();
        [DispId(2)]
        void Add([In, MarshalAs(UnmanagedType.Interface)] CCspInformation pVal);
    }

    public enum InstallResponseRestrictionFlags
    {
        AllowNone = 0,
        AllowNoOutstandingRequest = 1,
        AllowUntrustedCertificate = 2,
        AllowUntrustedRoot = 4
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB300-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated]
    public interface IObjectId
    {
        [DispId(0x60020000)]
        void InitializeFromName([In] CERTENROLL_OBJECTID Name);
    }

    [ComImport, DefaultMember("ItemByIndex"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB301-217D-11DA-B2A4-000E7BBB2B09")]
    public interface IObjectIds : IEnumerable
    {
        void _VtblGap1_3();
        [DispId(2)]
        void Add([In, MarshalAs(UnmanagedType.Interface)] CObjectId pVal);
    }

    [ComImport, CompilerGenerated, Guid("728AB303-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX500DistinguishedName
    {
        void _VtblGap1_1();
        [DispId(0x60020001)]
        void Encode([In, MarshalAs(UnmanagedType.BStr)] string strName, [In, Optional] X500NameFlags NameFlags);
    }

    [ComImport, Guid("728AB341-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509CertificateRequest
    {
    }

    [ComImport, Guid("728AB342-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual), CompilerGenerated]
    public interface IX509CertificateRequestPkcs10 : IX509CertificateRequest
    {
    }

    [ComImport, Guid("728AB35B-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509CertificateRequestPkcs10V2 : IX509CertificateRequestPkcs10, IX509CertificateRequest
    {
        void _VtblGap1_26();
        [DispId(0x60030001)]
        void InitializeFromPrivateKey([In] X509CertificateEnrollmentContext Context, [In, MarshalAs(UnmanagedType.Interface)] CX509PrivateKey pPrivateKey, [In, MarshalAs(UnmanagedType.BStr)] string strTemplateName);
        void _VtblGap2_11();
        CX500DistinguishedName Subject { [return: MarshalAs(UnmanagedType.Interface)] [DispId(0x6003000d)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [DispId(0x6003000d)] set; }
        void _VtblGap3_1();
        bool SmimeCapabilities { [DispId(0x60030010)] get; [param: In] [DispId(0x60030010)] set; }
        void _VtblGap4_4();
        CX509Extensions X509Extensions { [return: MarshalAs(UnmanagedType.Interface)] [DispId(0x60030016)] get; }
    }

    [ComImport, Guid("728AB346-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509Enrollment
    {
    }

    [ComImport, Guid("728AB350-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509Enrollment2 : IX509Enrollment
    {
        [DispId(0x60020000)]
        void Initialize([In] X509CertificateEnrollmentContext Context);
        void _VtblGap1_1();
        [DispId(0x60020002)]
        void InitializeFromRequest([In, MarshalAs(UnmanagedType.Interface)] IX509CertificateRequest pRequest);
        [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(0x60020003)]
        string CreateRequest([In, Optional] EncodingType Encoding);
        void _VtblGap2_1();
        [DispId(0x60020005)]
        void InstallResponse([In] InstallResponseRestrictionFlags Restrictions, [In, MarshalAs(UnmanagedType.BStr)] string strResponse, [In] EncodingType Encoding, [In, MarshalAs(UnmanagedType.BStr)] string strPassword);
        void _VtblGap3_11();
        string CertificateFriendlyName { [return: MarshalAs(UnmanagedType.BStr)] [DispId(0x60020011)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(0x60020011)] set; }
    }

    [ComImport, Guid("728AB30D-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated, InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509Extension
    {
    }

    [ComImport, CompilerGenerated, Guid("728AB310-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IX509ExtensionEnhancedKeyUsage : IX509Extension
    {
        void _VtblGap1_5();
        [DispId(0x60030000)]
        void InitializeEncode([In, MarshalAs(UnmanagedType.Interface)] CObjectIds pValue);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("728AB30F-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated]
    public interface IX509ExtensionKeyUsage : IX509Extension
    {
        void _VtblGap1_5();
        [DispId(0x60030000)]
        void InitializeEncode([In] X509KeyUsageFlags UsageFlags);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), DefaultMember("ItemByIndex"), Guid("728AB30E-217D-11DA-B2A4-000E7BBB2B09"), CompilerGenerated]
    public interface IX509Extensions : IEnumerable
    {
        void _VtblGap1_3();
        [DispId(2)]
        void Add([In, MarshalAs(UnmanagedType.Interface)] CX509Extension pVal);
    }

    [ComImport, Guid("728AB30C-217D-11DA-B2A4-000E7BBB2B09"), InterfaceType(ComInterfaceType.InterfaceIsDual), CompilerGenerated]
    public interface IX509PrivateKey
    {
        void _VtblGap1_1();
        [DispId(0x60020001)]
        void Create();
        void _VtblGap2_12();
        CCspInformations CspInformations { [return: MarshalAs(UnmanagedType.Interface)] [DispId(0x6002000e)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [DispId(0x6002000e)] set; }
        void _VtblGap3_10();
        X509KeySpec KeySpec { [DispId(0x6002001a)] get; [param: In] [DispId(0x6002001a)] set; }
        int Length { [DispId(0x6002001c)] get; [param: In] [DispId(0x6002001c)] set; }
        X509PrivateKeyExportFlags ExportPolicy { [DispId(0x6002001e)] get; [param: In] [DispId(0x6002001e)] set; }
        X509PrivateKeyUsageFlags KeyUsage { [DispId(0x60020020)] get; [param: In] [DispId(0x60020020)] set; }
        void _VtblGap4_2();
        bool MachineContext { [DispId(0x60020024)] get; [param: In] [DispId(0x60020024)] set; }
    }

    public enum X500NameFlags
    {
        XCN_CERT_NAME_STR_COMMA_FLAG = 0x4000000,
        XCN_CERT_NAME_STR_CRLF_FLAG = 0x8000000,
        XCN_CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG = 0x10000,
        XCN_CERT_NAME_STR_DISABLE_UTF8_DIR_STR_FLAG = 0x100000,
        XCN_CERT_NAME_STR_ENABLE_PUNYCODE_FLAG = 0x200000,
        XCN_CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG = 0x20000,
        XCN_CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG = 0x40000,
        XCN_CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG = 0x80000,
        XCN_CERT_NAME_STR_FORWARD_FLAG = 0x1000000,
        XCN_CERT_NAME_STR_NO_PLUS_FLAG = 0x20000000,
        XCN_CERT_NAME_STR_NO_QUOTING_FLAG = 0x10000000,
        XCN_CERT_NAME_STR_NONE = 0,
        XCN_CERT_NAME_STR_REVERSE_FLAG = 0x2000000,
        XCN_CERT_NAME_STR_SEMICOLON_FLAG = 0x40000000,
        XCN_CERT_OID_NAME_STR = 2,
        XCN_CERT_SIMPLE_NAME_STR = 1,
        XCN_CERT_X500_NAME_STR = 3,
        XCN_CERT_XML_NAME_STR = 4
    }

    public enum X509CertificateEnrollmentContext
    {
        ContextAdministratorForceMachine = 3,
        ContextMachine = 2,
        ContextUser = 1
    }

    public enum X509KeySpec
    {
        XCN_AT_NONE,
        XCN_AT_KEYEXCHANGE,
        XCN_AT_SIGNATURE
    }

    public enum X509KeyUsageFlags
    {
        XCN_CERT_CRL_SIGN_KEY_USAGE = 2,
        XCN_CERT_DATA_ENCIPHERMENT_KEY_USAGE = 0x10,
        XCN_CERT_DECIPHER_ONLY_KEY_USAGE = 0x8000,
        XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE = 0x80,
        XCN_CERT_ENCIPHER_ONLY_KEY_USAGE = 1,
        XCN_CERT_KEY_AGREEMENT_KEY_USAGE = 8,
        XCN_CERT_KEY_CERT_SIGN_KEY_USAGE = 4,
        XCN_CERT_KEY_ENCIPHERMENT_KEY_USAGE = 0x20,
        XCN_CERT_NO_KEY_USAGE = 0,
        XCN_CERT_NON_REPUDIATION_KEY_USAGE = 0x40,
        XCN_CERT_OFFLINE_CRL_SIGN_KEY_USAGE = 2
    }

    public enum X509PrivateKeyExportFlags
    {
        XCN_NCRYPT_ALLOW_ARCHIVING_FLAG = 4,
        XCN_NCRYPT_ALLOW_EXPORT_FLAG = 1,
        XCN_NCRYPT_ALLOW_EXPORT_NONE = 0,
        XCN_NCRYPT_ALLOW_PLAINTEXT_ARCHIVING_FLAG = 8,
        XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG = 2
    }

    public enum X509PrivateKeyUsageFlags
    {
        XCN_NCRYPT_ALLOW_ALL_USAGES = 0xffffff,
        XCN_NCRYPT_ALLOW_DECRYPT_FLAG = 1,
        XCN_NCRYPT_ALLOW_KEY_AGREEMENT_FLAG = 4,
        XCN_NCRYPT_ALLOW_SIGNING_FLAG = 2,
        XCN_NCRYPT_ALLOW_USAGES_NONE = 0
    }
}
