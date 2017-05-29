using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using System.Drawing;
using System.IO;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for ThumbnailPhoto
    /// </summary>
    public class ThumbnailPhoto : IHttpHandler
    {
        public HttpContext Context = null;

        public int Param(string key)
        {
            string val = Context.Request.QueryString[key];
            if (val == null)
            {
                val = Context.Request.Form[key];
                if (val == null) return 0;
            }

            int res = 0;
            int.TryParse(val, out res);

            return res;
        }



        public void ProcessRequest(HttpContext context)
        {
            Context = context;

            int ItemID = Param("ItemID");
            int AccountID = Param("AccountID");

            BytesResult res = ES.Services.ExchangeServer.GetPicture(ItemID, AccountID);
            if ((res!=null)&&(res.IsSuccess) && (res.Value != null))
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(res.Value);
            }
            else
            {
                MemoryStream pictureStream = new MemoryStream();
                Bitmap emptyBmp = new Bitmap(1, 1);
                emptyBmp.Save(pictureStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(pictureStream.ToArray());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}