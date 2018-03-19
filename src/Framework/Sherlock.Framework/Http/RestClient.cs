using Microsoft.Net.Http.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public class RestClient : IDisposable
    {
        private readonly HttpClient _client;
        private Version _requestedApiVersion;
        private static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
        private const string UserAgent = "Sherlock-rest";

        public RestClient(RestClientConfiguration configuration, Version requestedApiVersion = null)
        {
            Configuration = configuration;
            _requestedApiVersion = requestedApiVersion;
            JsonSerializer = new HttpJsonSerializer();

            ManagedHandler handler;
            var uri = Configuration.EndpointBaseUri;
            switch (uri.Scheme.ToLowerInvariant())
            {
                case "http":
                    var builder = new UriBuilder(uri)
                    {
                        Scheme = configuration.Credentials.IsTlsCredentials() ? "https" : "http"
                    };
                    uri = builder.Uri;
                    handler = new ManagedHandler();
                    break;

                case "https":
                    handler = new ManagedHandler();
                    break;

                case "unix":
                    var pipeString = uri.LocalPath;
                    handler = new ManagedHandler(async (string host, int port, CancellationToken cancellationToken) =>
                    {
                        var sock = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                        await sock.ConnectAsync(new UnixDomainSocketEndPoint(pipeString));
                        return sock;
                    });
                    uri = new UriBuilder("http", uri.Segments.Last()).Uri;
                    break;
                default:
                    throw new Exception($"Unsupported url scheme '{configuration.EndpointBaseUri.Scheme}'");
            }

            this.EndpointBaseUri = uri;

            _client = new HttpClient(Configuration.Credentials.GetHandler(handler), true);
            _client.Timeout = InfiniteTimeout;
            _client.Timeout = Configuration.Timeout;
        }

        public Uri EndpointBaseUri { get; }

        public HttpJsonSerializer JsonSerializer { get; }

        public RestClientConfiguration Configuration { get; }

        public async Task<T> MakeRequestJsonAsync<T>(
            HttpMethod method,
            string path,
            String queryString = null,
            IRequestContent body = null,
            IDictionary<string, string> headers = null,
            TimeSpan? timeout = null,
            CancellationToken? token = null,
            IEnumerable<RestResponseErrorHandlingDelegate> errorHandlers = null)
        {
            var response =  await this.MakeRequestAsync(method, path, queryString, body, headers, timeout, token, errorHandlers);
            return this.JsonSerializer.DeserializeObject<T>(response.Body);
        }

        public async Task<RestResponse> MakeRequestAsync(
            HttpMethod method,
            string path,
            String queryString = null,
            IRequestContent body = null,
            IDictionary<string, string> headers = null,
            TimeSpan? timeout = null,
            CancellationToken? token = null,
            IEnumerable<RestResponseErrorHandlingDelegate> errorHandlers = null)
        {
            var response = await this.MakeRequestCoreAsync(method, path, queryString, body, timeout, token, HttpCompletionOption.ResponseContentRead, headers).ConfigureAwait(false);
            using (response)
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                HandleIfErrorResponse(response.StatusCode, responseBody, errorHandlers);

                return new RestResponse(response.StatusCode, responseBody);
            }
        }

        public async Task<Stream> MakeRequestForStreamAsync(
            HttpMethod method,
            string path,
            String queryString = null,
            IRequestContent body = null,
            IDictionary<string, string> headers = null,
            TimeSpan? timeout = null,
            CancellationToken? token = null,
             IEnumerable<RestResponseErrorHandlingDelegate> errorHandlers = null)
        {
            var response = await this.MakeRequestCoreAsync(method, path, queryString, body, timeout, token, HttpCompletionOption.ResponseHeadersRead, headers).ConfigureAwait(false);

            HandleIfErrorResponse(response.StatusCode, null, errorHandlers);

            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public async Task<RestStreamedResponse> MakeRequestForStreamedResponseAsync(
            HttpMethod method,
            string path,
            String queryString = null,
            CancellationToken? cancellationToken = null,
            IEnumerable<RestResponseErrorHandlingDelegate> errorHandlers = null)
        {
            var response = await MakeRequestCoreAsync(method, path, queryString, null, InfiniteTimeout,cancellationToken, HttpCompletionOption.ResponseHeadersRead, null);
            HandleIfErrorResponse(response.StatusCode, null, errorHandlers);

            var body = await response.Content.ReadAsStreamAsync();

            return new RestStreamedResponse(response.StatusCode, body, response.Headers);
        }

        public async Task<WriteClosableStream> MakeRequestForHijackedStreamAsync(
           HttpMethod method,
           string path,
           String queryString = null,
           IRequestContent body = null,
           IDictionary<string, string> headers = null,
           TimeSpan? timeout = null,
           CancellationToken? cancellationToken = null,
           IEnumerable<RestResponseErrorHandlingDelegate> errorHandlers = null)
        {
            var response = await MakeRequestCoreAsync(method, path, queryString, body, timeout, cancellationToken, HttpCompletionOption.ResponseHeadersRead, headers).ConfigureAwait(false);

            HandleIfErrorResponse(response.StatusCode, null, errorHandlers);

            var content = response.Content as HttpConnectionResponseContent;
            if (content == null)
            {
                throw new NotSupportedException("message handler does not support hijacked streams");
            }

            return content.HijackStream();
        }


        private Task<HttpResponseMessage> MakeRequestCoreAsync(
           HttpMethod method,
           string path,
           String queryString = null,
           IRequestContent data = null,
           TimeSpan? timeout = null,
           CancellationToken? cancellationToken = null,
           HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
           IDictionary<string, string> headers = null)
        {
            var request = PrepareRequest(method, path, queryString, headers, data);

            CancellationToken token;
            if (timeout != InfiniteTimeout)
            {
                token = cancellationToken ?? CancellationToken.None;
                var timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
                timeoutTokenSource.CancelAfter(timeout ?? this.Configuration.Timeout);
                token = timeoutTokenSource.Token;
            }

            return _client.SendAsync(request, completionOption, token).ContinueWith(t=>
            {
                try
                {
                    return t.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException cex)
                {
                    String body = null;
                    if (data != null)
                    {
                        body = data.GetContent()?.ReadAsStringAsync()?.GetAwaiter().GetResult();
                    }
                    throw new SherlockRestTimeoutException($@"在 {(timeout ?? this.Configuration.Timeout).TotalSeconds.ToString()} 秒内没有收到对 docker api （uri: {request.RequestUri?.ToString().IfNullOrWhiteSpace("null")}, data:{body.IfNullOrWhiteSpace("null")}）调用的响应。", cex);
                }
                catch (Exception ex)
                {
                    String body = null;
                    if (data != null)
                    {
                        body = data.GetContent()?.ReadAsStringAsync()?.GetAwaiter().GetResult();
                    }
                    throw new SherlockRestException(HttpStatusCode.BadRequest, $@"请求 docker api（uri: {request.RequestUri?.ToString().IfNullOrWhiteSpace("null")}, data:{body.IfNullOrWhiteSpace("null")}）发生错误。", ex);
                }
            });
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string responseBody, IEnumerable<RestResponseErrorHandlingDelegate> handlers)
        {
            // If no customer handlers just default the response.
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    handler(statusCode, responseBody);
                }
            }

            // No custom handler was fired. Default the response for generic success/failures.
            if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
            {
                throw new SherlockRestException(statusCode, responseBody);
            }
        }

        protected HttpRequestMessage PrepareRequest(HttpMethod method, string path, String queryString, IDictionary<string, string> headers, IRequestContent data)
        {
            if (string.IsNullOrEmpty("path"))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var request = new HttpRequestMessage(method, RestUtility.BuildUri(this.EndpointBaseUri, this._requestedApiVersion, path, queryString));

            request.Headers.Add("User-Agent", UserAgent);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (data != null)
            {
                var requestContent = data.GetContent(); // make the call only once.
                request.Content = requestContent;
            }

            return request;
        }

        public void Dispose()
        {
            _client.Dispose();
            Configuration.Dispose();
        }
    }

    public delegate void RestResponseErrorHandlingDelegate(HttpStatusCode statusCode, string responseBody);

}
