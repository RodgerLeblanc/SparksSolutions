using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RecaptchaVerify
{
    public class DefaultHttpRequestMessage : HttpRequestMessage
    {
        public DefaultHttpRequestMessage(DefaultHttpRequestMessageOptions options) : base(options.HttpMethod, options.RequestUri)
        {
            if (!String.IsNullOrEmpty(options.Token))
                Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            if (!String.IsNullOrEmpty(options.Body))
                Content = new StringContent(options.Body, System.Text.Encoding.UTF8, options.MediaType);
        }
    }

    public class DefaultHttpRequestMessageOptions
    {
        public HttpMethod HttpMethod { get; set; }
        public string RequestUri { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
        public string MediaType { get; set; } = "application/json";
    }
}