﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Simple.NExtLib.IO;

namespace Simple.OData.Client
{
    abstract class RequestRunner
    {
        public RequestRunner()
        {
        }

        public string Request(HttpWebRequest request)
        {
            using (var response = TryRequest(request))
            {
                return TryGetResponseBody(response);
            }
        }

        public HttpWebResponse TryRequest(HttpWebRequest request)
        {
            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                throw WebRequestException.CreateFromWebException(ex);
            }
        }

        public abstract IEnumerable<IDictionary<string, object>> FindEntries(HttpCommand command, bool scalarResult, bool setTotalCount, out int totalCount);
        public abstract IDictionary<string, object> GetEntry(HttpCommand command);
        public abstract IDictionary<string, object> InsertEntry(HttpCommand command, bool resultRequired);
        public abstract int UpdateEntry(HttpCommand command);
        public abstract int DeleteEntry(HttpCommand command);
        public abstract IEnumerable<IEnumerable<IEnumerable<KeyValuePair<string, object>>>> ExecuteFunction(HttpCommand command);

        private static string TryGetResponseBody(HttpWebResponse response)
        {
            if (response != null)
            {
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    return QuickIO.StreamToString(stream);
                }
            }

            return String.Empty;
        }
    }
}
