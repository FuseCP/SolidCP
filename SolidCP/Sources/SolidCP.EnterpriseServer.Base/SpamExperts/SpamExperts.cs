using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class SpamExperts
    {


        string emailAddress;
        bool isPrimary;
        bool iSuserPrincipalName;


        public bool SEEnabled
        {
            get { return SEEnabled; }
            set { SEEnabled = value; }
        }

        public string schema
        {
            get { return this.schema; }
            set { this.schema = value; }
        }

        public string url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public string user
        {
            get { return user; }
            set { user = value; }
        }

        public string password
        {
            get { return password; }
            set { password = value; }
        }

        public string ErrorMailSubject
        {
            get { return ErrorMailSubject; }
            set { ErrorMailSubject = value; }
        }

        public string ErrorMailBody
        {
            get { return ErrorMailBody; }
            set { ErrorMailBody = "{0}.Error: {1}"; }
        }

    }
}
