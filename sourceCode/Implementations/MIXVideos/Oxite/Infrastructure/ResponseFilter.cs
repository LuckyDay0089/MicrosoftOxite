﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web;

namespace Oxite.Infrastructure
{
    public class ResponseFilter : Stream
    {
        private readonly Stream responseStream;
        private readonly IList<ResponseInsert> inserts;
        private List<byte> buffer;
        private HttpContextBase context;

        public ResponseFilter(Stream responseStream, HttpContextBase context)
        {
            this.responseStream = responseStream;

            inserts = new List<ResponseInsert>(5);
            this.context = context;
        }

        public IList<ResponseInsert> Inserts
        {
            get { return inserts; }
        }

        public override bool CanRead
        {
            get { return responseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return responseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return responseStream.CanWrite; }
        }

        public override void Flush()
        {
            if (Inserts.Count > 0 && (this.context.AllErrors == null || this.context.AllErrors.Length == 0))
            {
                string xhtml = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count).Replace("&", "&amp;");
                XDocument doc = null;
                bool modifiedDoc = false;

                //TODO: (erikpo) Instead of try/catch switch this to be a site setting to force validation of output markup or not

                try
                {
                    doc = XDocument.Parse(xhtml, LoadOptions.PreserveWhitespace);

                    foreach (ResponseInsert insert in Inserts)
                        insert.Apply(doc, ref modifiedDoc);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#if DEBUG
                    throw ex;
#endif
                }

                if (modifiedDoc)
                    xhtml = doc.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
                buffer = new List<byte>(Encoding.UTF8.GetBytes(xhtml));
                responseStream.Write(buffer.ToArray(), 0, buffer.Count);
            }

            responseStream.Flush();
        }

        public override long Length
        {
            get { return responseStream.Length; }
        }

        public override long Position
        {
            get
            {
                return responseStream.Position;
            }
            set
            {
                responseStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            responseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.buffer == null)
                this.buffer = new List<byte>(buffer.Length);

            this.buffer.AddRange(buffer);
        }
    }
}
