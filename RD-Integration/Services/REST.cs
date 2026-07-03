using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RD_INTEGRATION.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class REST : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly RD_Preference _preference;
        private readonly string _acumaticaBaseUrl;
        private readonly string _endpointName = "RetailDimension";
        private readonly string _endpointVersion = "20.200.001";
        private bool _loggedIn;
        private bool _loginFailed = false;

        public bool LoginFailed { get { return _loginFailed; } }

        public REST(RD_Preference preference)
        {
            try
            {
                _preference = preference;
                _acumaticaBaseUrl = preference.BaseURL.EndsWith("/") ? preference.BaseURL[0..^1] : preference.BaseURL;
                _endpointName = preference.EndpointName;
                _endpointVersion = preference.EndpointVersion;

                //specify to use TLS 1.2 as default connection
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var baseAddressUri = new Uri($"{_acumaticaBaseUrl}/entity/{_endpointName}/{_endpointVersion}/", UriKind.Absolute);
                var clientHandler = new HttpClientHandler { 
                    UseCookies = true, 
                    CookieContainer = new CookieContainer(),
                    SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                };

                _httpClient = new HttpClient(clientHandler)
                {
                    BaseAddress = baseAddressUri,
                    DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("text/json") }, ExpectContinue = false },
                    Timeout = new TimeSpan(0, 10, 0),
                };

                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"CompanyID={_preference.Tenant}; Locale=TimeZone=GMTP0800M&Culture=en-US");
                _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            }
            catch { throw;}
        }

        private async Task LoginAsync()
        {
            try
            {
                _loginFailed = false;

                if (!_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/auth/login", UriKind.Absolute);

                    var result = await _httpClient.PostAsJsonAsync(requestUri,
                        new
                        {
                            name = _preference.UserName,
                            password = _preference.Password,
                            company = _preference.Tenant,
                            branch = _preference.Branch ?? "",
                            locale = _preference.Locale ?? ""
                        });

                    _loggedIn = result.IsSuccessStatusCode;
                    _loginFailed = !_loggedIn;

                    //var request = new HttpRequestMessage (HttpMethod.Post, requestUri);

                    //request.Headers.Add("Cookie", $"CompanyID={_preference.Tenant}; Locale=TimeZone=GMTP0800M&Culture=en-US");

                    //request.Content = new StringContent(JsonConvert.SerializeObject(new
                    //{
                    //    name = _preference.UserName,
                    //    password = _preference.Password,
                    //    company = _preference.Tenant
                    //}), null, "application/json");

                    //var response = await _httpClient.SendAsync(request);
                    //_loggedIn = response.IsSuccessStatusCode;
                }
            }
            catch { }
        }

        private void Logout()
        {
            try
            {
                if (_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/auth/logout", UriKind.Absolute);
                    var result = _httpClient.PostAsync(requestUri,
                        new ByteArrayContent(new byte[0])); //Log out to Acumatica ERP

                    _loggedIn = false;
                }
            }
            catch { }
        }

        private async Task LogoutAsync()
        {
            try
            {
                if (_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/auth/logout", UriKind.Absolute);
                    var result = await _httpClient.PostAsync(requestUri,
                        new ByteArrayContent(new byte[0]));

                    _loggedIn = !result.IsSuccessStatusCode;
                }
            }
            catch { }
        }

        public async Task<string> GetAsync(string entityName, string parameters)
        {
            try
            {
                await LoginAsync();

                if (_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/{_endpointName}/{_endpointVersion}/{entityName}?{parameters}", UriKind.Absolute);
                    var response = await _httpClient.GetAsync(requestUri);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return result;
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(JsonConvert.DeserializeObject<AcumaticaInternalError>(result).ExceptionMessage);
                    else
                        throw new Exception($"API request failed ({(int)response.StatusCode} {response.StatusCode}): {result}");
                }
                else
                    throw new Exception("API Login failed! Please contact your system administrator.");
            }
            catch { throw; }
            finally { await LogoutAsync(); }
        }

        public async Task<string> GetUriAsync(string entityName, string parameters)
        {
            try
            {
                await LoginAsync();

                if (_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/{_endpointName}/{_endpointVersion}/{entityName}?{parameters}", UriKind.Absolute);

                    return requestUri.ToString();
                }
                else
                    throw new Exception("API Login failed! Please contact your system administrator.");
            }
            catch { throw; }
            finally { await LogoutAsync(); }
        }

        public async Task<string> PutAsync(string entityName, string jsonData, string parameters = "")
        {
            try
            {
                await LoginAsync();

                if (_loggedIn)
                {
                    var requestUri = new Uri($"{_acumaticaBaseUrl}/entity/{_endpointName}/{_endpointVersion}/{entityName}?{parameters}", UriKind.Absolute);
                    var response = await _httpClient.PutAsync(requestUri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                        return result;
                    else
                        throw new Exception(result);
                }
                else
                    throw new Exception("API Login failed! Please contact your system administrator.");
            }
            catch { throw; }
            finally { await LogoutAsync(); }
        }

        public void Dispose()
        {
            try
            {
                Logout();
                _httpClient.Dispose();
            }
            catch { }
        }
    }

    public class AcumaticaInternalError
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }

    public class AcumaticaUnprocessableError
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string Error { get; set; }
    }
}
